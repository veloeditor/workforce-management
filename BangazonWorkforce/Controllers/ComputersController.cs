using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BangazonWorkforceMVC.Models;
using BangazonWorkforceMVC.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkforceMVC.Controllers
{
    public class ComputersController : Controller
    {
        private readonly IConfiguration _config;

        public ComputersController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Computers
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                            c.PurchaseDate, 
                                            IsNull(c.DecomissionDate, '') AS DecomissionDate, 
                                            c.Make,            
                                            c.Manufacturer,
                                            isNull(e.FirstName + ' ' +
                                            e.LastName, '') AS ComputerEmployee,
                                            e.Id AS EmployeeId,
                                            c.Id AS ComputerId
                                       FROM ComputerEmployee ce
                                       LEFT JOIN Employee e ON e.Id = ce.EmployeeId
                                       FULL OUTER JOIN Computer c ON c.Id = ce.ComputerId";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<ComputerEmployeeViewModel> computersEmployees = new List<ComputerEmployeeViewModel>();

                    while (reader.Read())
                    {
                        ComputerEmployeeViewModel ComputerEmployee = new ComputerEmployeeViewModel();
                        Computer aComputer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee anEmployee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("ComputerEmployee")),
                            };
                            ComputerEmployee.Employee = anEmployee;
                        }
                        ComputerEmployee.Computer = aComputer;
                        computersEmployees.Add(ComputerEmployee);
                    };
                    reader.Close();
                    return View(computersEmployees);
                }
            }
        }

        // GET: Computers/Details/5
        public ActionResult Details(int id)
        {
            var computer = GetComputerById(id);
            return View(computer);
        }

        // GET: Computers/Create
        public ActionResult Create()
        {
            var viewModel = new ComputerCreateViewModel();
            var employees = GetAllEmployees();
            var selectItems = employees.ToList();

            viewModel.Employees = selectItems;
            return View(viewModel);

        }

        // POST: Computers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ComputerCreateViewModel computerEmployee)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Computer
                                                (Make, Manufacturer, PurchaseDate, DecomissionDate)
                                            OUTPUT INSERTED.Id
                                            VALUES
                                                (@make, @manufacturer, @purchaseDate, null);";
                        cmd.Parameters.Add(new SqlParameter("@make", computerEmployee.Computer.Make));
                        cmd.Parameters.Add(new SqlParameter("@manufacturer", computerEmployee.Computer.Manufacturer));
                        cmd.Parameters.Add(new SqlParameter("@purchaseDate", computerEmployee.Computer.PurchaseDate));

                        computerEmployee.Computer.Id = (int)await cmd.ExecuteScalarAsync();

                        cmd.CommandText = @"INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate)
                                            VALUES (@employeeId, @computerId, GETDATE(), null)";

                        cmd.Parameters.Add(new SqlParameter("@computerId", computerEmployee.Computer.Id));
                        cmd.Parameters.Add(new SqlParameter("@employeeId", computerEmployee.Employee.Id));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Computers/Delete/5
        public ActionResult Delete(int id)
        {
            var computer = GetComputerById(id);
            return View(computer);
        }

        // POST: Computers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Computer computer)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT Id, EmployeeId, ComputerId, AssignDate, UnassignDate FROM ComputerEmployee WHERE ComputerId = @id;";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            reader.Close();
                            return Ok("You are not allowed to delete computers that have been assigned");
                        }
                        else
                        {
                            reader.Close();
                            cmd.CommandText = @"DELETE FROM Computer WHERE Id = @id";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Computer GetComputerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            Id,
                                            PurchaseDate, 
                                            IsNull(DecomissionDate, '') AS DecomissionDate, 
                                            Make,            
                                            Manufacturer 
                                       FROM Computer
                                       WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Computer computer = null;

                    if (reader.Read())
                    {
                        computer = new Computer()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                        };

                    }
                    reader.Close();
                    return computer;
                }
            }
        }

        private List<Employee> GetAllEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            Id, FirstName, LastName, DepartmentId, IsSupervisor
                                       FROM Employee";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Employee> employees = new List<Employee>();

                    while (reader.Read())
                    {
                        Employee employee = new Employee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                        };
                        employees.Add(employee);
                    }
                    reader.Close();
                    return employees;
                }
            }
        }
    }
}