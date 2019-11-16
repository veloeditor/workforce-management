using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BangazonWorkforceMVC.Models;
using BangazonWorkforceMVC.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkforceMVC.Controllers
{
    public class TrainingProgramsController : Controller
    {
        private readonly IConfiguration _config;

        public TrainingProgramsController(IConfiguration config)
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

        // GET: Get all TrainingPrograms
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, StartDate, EndDate, MaxAttendees FROM TrainingProgram
                                         WHERE StartDate > GetDate()";
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    List<TrainingProgram> trainingPrograms = new List<TrainingProgram>();

                    while (reader.Read())
                    {
                        TrainingProgram trainingProgram = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"))   

                        };
                        trainingPrograms.Add(trainingProgram);
                    }
                    reader.Close();
                    return View(trainingPrograms);
                }
            }
            
        }

        // GET: TrainingPrograms/Details/5
        public ActionResult Details(int id)
        {
           using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT tp.Id, tp.Name as ProgramName, tp.StartDate, tp.EndDate, tp.MaxAttendees, e.Id as EmployeeId, 
                                               e.FirstName + ' ' + e.LastName as EmployeesEnrolled
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id 
                                     LEFT JOIN Employee e ON et.EmployeeId = e.Id
                                         WHERE StartDate > GetDate()";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    TrainingProgram trainingProgram = null;
                  

                    if (reader.Read())
                    {
                        trainingProgram = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("ProgramName")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                            EmployeeList = new List<Employee>(),
                            Employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("EmployeesEnrolled"))
                            }
                        };
                        reader.Close();
                        return View(trainingProgram);
                        
                    }


                }
            }

            //Get All employees
            return View();
        }

        // GET: TrainingPrograms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainingPrograms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TrainingProgram newTrainingProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees) VALUES(@name, @startDate, @endDate, @maxAttendees)";
                    cmd.Parameters.Add(new SqlParameter("@name", newTrainingProgram.Name));
                    cmd.Parameters.Add(new SqlParameter("@startDate", newTrainingProgram.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@endDate", newTrainingProgram.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@maxAttendees", newTrainingProgram.MaxAttendees));

                    cmd.ExecuteNonQuery();
                    return RedirectToAction(nameof(Index));
                }
            }
        }
       

        // GET: TrainingPrograms/Edit/5
        public ActionResult Edit(int id)
        {
            TrainingProgram trainingProgram = GetTrainingProgramById(id);
            return View(trainingProgram);
        }

        // POST: TrainingPrograms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TrainingProgram updatedTrainingProgram)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE TrainingProgram
                                               SET Name = @name, StartDate = @startDate, EndDate = @endDate, MaxAttendees = @maxAttendees
                                             WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@name", updatedTrainingProgram.Name));
                        cmd.Parameters.Add(new SqlParameter("@startDate", updatedTrainingProgram.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", updatedTrainingProgram.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@maxAttendees", updatedTrainingProgram.MaxAttendees));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TrainingPrograms/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainingPrograms/Delete/5
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

        private TrainingProgram GetTrainingProgramById (int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT tp.Id, tp.Name as ProgramName, tp.StartDate, tp.EndDate, tp.MaxAttendees, e.Id as EmployeeId, 
                                               e.FirstName + ' ' + e.LastName as EmployeesEnrolled
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id 
                                     LEFT JOIN Employee e ON et.EmployeeId = e.Id
                                         WHERE StartDate > GetDate()";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    TrainingProgram trainingProgram = null;


                    if (reader.Read())
                    {
                        trainingProgram = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("ProgramName")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                            EmployeeList = new List<Employee>(),
                            Employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("EmployeesEnrolled"))
                            }
                        };
                        

                    }
                    reader.Close();
                        return trainingProgram;
                }
            }
        }
    }
}