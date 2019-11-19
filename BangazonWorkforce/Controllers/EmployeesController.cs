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
    public class EmployeesController : Controller
    {
        private readonly IConfiguration _config;

        public EmployeesController(IConfiguration config)
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
        // GET: Employees
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                SELECT e.Id, e.FirstName, e.LastName, d.Name AS DepartmentName
                                FROM Employee e
                                LEFT JOIN Department d on d.Id = e.DepartmentId
                                ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Department = new Department()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("DepartmentName")),
                            }
                            
                        };

                        employees.Add(employee);
                    }

                    reader.Close();

                    return View(employees);
                }
            }
        }

        // GET: Employees/Details/5
        public ActionResult Details(int id)
        {
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                      SELECT DISTINCT e.Id AS EmployeeId, e.FirstName, e.LastName,
                        d.Name AS DepartmentName, d.Id AS DepartmentId, ISNULL(c.Id, '') AS ComputerId,
                        ISNULL(c.Manufacturer, 'N/A') AS Manufacturer, ISNULL(c.Make, 'N/A') AS Make,
						ISNULL(tp.Name, 'N/A') AS ProgramName, ISNULL(tp.Id, '') AS TrainingProgramId
                        FROM Employee e
                        LEFT JOIN Department d ON d.Id = e.DepartmentId
                        LEFT JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id
                        LEFT JOIN Computer c on c.Id = ce.ComputerId
						LEFT JOIN EmployeeTraining et ON et.EmployeeId = e.Id
						LEFT JOIN TrainingProgram tp ON tp.Id = et.TrainingProgramId
                        WHERE ce.UnassignDate IS NULL AND e.Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Employee> employee = new Dictionary<int, Employee>();

                        while (reader.Read())
                        {
                            int employeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                            if (!employee.ContainsKey(employeeId))
                            {
                                Employee newEmployee = new Employee
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Department = new Department()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                        Name = reader.GetString(reader.GetOrdinal("DepartmentName")),
                                    },
                                    Computer = new Computer()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                        Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                                        Make = reader.GetString(reader.GetOrdinal("Make")),
                                    },
                                };
                                employee.Add(employeeId, newEmployee);
                            }
                            Employee fromDictionary = employee[employeeId];

                            if (!reader.IsDBNull(reader.GetOrdinal("TrainingProgramId")))
                            {
                                TrainingProgram trainingProgram = new TrainingProgram
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                                    Name = reader.GetString(reader.GetOrdinal("ProgramName"))
                                };
                                fromDictionary.ProgramList.Add(trainingProgram);
                            }
                        }
                        reader.Close();

                        return View(employee.Values.First());
                    }
                }
            }
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            var viewModel = new EmployeeCreateViewModel();
            var departments = GetAllDepartments();
            var selectItems = departments
                .Select(department => new SelectListItem
                {
                    Text = department.Name,
                    Value = department.Id.ToString()
                })
                .ToList();

            selectItems.Insert(0, new SelectListItem
            {
                Text = "Choose department...",
                Value = "department"
            });
            viewModel.Departments = selectItems;
            return View(viewModel);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeCreateViewModel model)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Employee
                ( FirstName, LastName, IsSupervisor, DepartmentId)
                VALUES
                ( @firstName, @lastName, @isSupervisor, @departmentId)";
                        cmd.Parameters.Add(new SqlParameter("@firstName", model.employee.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", model.employee.LastName));
                        cmd.Parameters.Add(new SqlParameter("@isSupervisor", model.employee.IsSupervisor));
                        cmd.Parameters.Add(new SqlParameter("@departmentId", model.employee.DepartmentId));
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

        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = new EmployeeEditViewModel()
            {
                employee = GetEmployeeById(id),
                departments = GetAllDepartments(),
                computers = GetAllComputers(),
                computersEmployees = GetAllComputerEmployees()

            };
            return View(viewModel);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, EmployeeEditViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Employee
                                            SET LastName = @lastName,
                                                DepartmentId = @departmentId
                                            WHERE Id = @id;
                                            UPDATE ComputerEmployee
                                            SET ComputerId = @computerId";

                        cmd.Parameters.Add(new SqlParameter("@lastName", viewModel.employee.LastName));
                        cmd.Parameters.Add(new SqlParameter("@departmentId", viewModel.employee.DepartmentId));
                        cmd.Parameters.Add(new SqlParameter("@computerId", viewModel.ComputerEmployee.ComputerId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                      
                        
                        int rowsaffected = cmd.ExecuteNonQuery();



                        if (rowsaffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        throw new Exception("no rows affected");

                    }

                }
            }
            catch
            {
                
                return View();
            }

        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employees/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Employee GetEmployeeById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.Id, e.FirstName, e.LastName, e.DepartmentId, ce.ComputerId
                                        FROM Employee e
                                        JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id
                                        WHERE e.Id = @id AND ce.UnassignDate IS NULL";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    var reader = cmd.ExecuteReader();

                    Employee employee = null;
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("Firstname")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            ComputerEmployee = new ComputerEmployee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ComputerId = reader.GetInt32(reader.GetOrdinal("ComputerId"))
                            }
                        };
                    }
                    reader.Close();
                    return employee;
                }
            }
        }

        private List<Department> GetAllDepartments()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name AS DepartmentName FROM Department";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Department> departments = new List<Department>();
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("DepartmentName")),
                        });
                    }

                    reader.Close();

                    return departments;
                }
            }
        }

        private List<Computer> GetAllComputers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Manufacturer, Make from Computer";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Computer> computers = new List<Computer>();
                    while (reader.Read())
                    {
                        computers.Add(new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                            Make = reader.GetString(reader.GetOrdinal("Make"))
                        });
                    }

                    reader.Close();

                    return computers;
                }
            }
        }
        private List<ComputerEmployee> GetAllComputerEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, EmployeeId, ComputerId FROM ComputerEmployee";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<ComputerEmployee> computersEmployees = new List<ComputerEmployee>();
                    while (reader.Read())
                    {
                        computersEmployees.Add(new ComputerEmployee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                            ComputerId = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                            
                        });
                    }

                    reader.Close();

                    return computersEmployees;
                }
            }
        }
    }
}