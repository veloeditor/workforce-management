using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration; 
using BangazonWorkforceMVC.Models;
using BangazonWorkforceMVC.Models.ViewModels;



namespace BangazonWorkforceMVC.Controllers
{
    public class DepartmentsController : Controller
    {
        private string _connectionString;
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public DepartmentsController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // GET: Department
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT d.id, d.Name, d.Budget                                             
                                          FROM Department d
                                      ORDER BY d.Name, d.Budget";
                    var reader = cmd.ExecuteReader();
                    var departments = new List<Department>();
                    while (reader.Read())
                    {
                        departments.Add(
                            new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                            });
                    }

                    reader.Close();
                    return View(departments);
                }
            }
        }

        // GET: Students/Details/5
         public ActionResult Details(int id)
         {
             return View();
         }
         


        // GET: Department/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            try
            {

                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @" INSERT INTO Department (Name, Budget)
                                                VALUES (@name, @budget);";
                        cmd.Parameters.Add(new SqlParameter("@name", department.Name));
                        cmd.Parameters.Add(new SqlParameter("@budget", department.Budget));
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

        private List<Employee> GetAllEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName
                                  FROM Employee";
                    var reader = cmd.ExecuteReader();

                    var employees = new List<Employee>();
                    while (reader.Read())
                    {
                        employees.Add(
                                new Employee()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                }
                            );
                    }

                    reader.Close();

                    return employees;
                }
            }
        }
    }
}