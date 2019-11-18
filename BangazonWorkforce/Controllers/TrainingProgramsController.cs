﻿using System;
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
                               ORDER BY (CASE WHEN StartDate > GetDate() THEN 1 ELSE 0 END) DESC, StartDate ASC";
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
                    cmd.CommandText = @"SELECT tp.Id as TrainingProgramId, tp.Name as ProgramName, tp.StartDate, tp.EndDate, tp.MaxAttendees, e.Id as EmployeeId, 
                                               e.FirstName + ' ' + e.LastName as EmployeesEnrolled
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id 
                                     LEFT JOIN Employee e ON et.EmployeeId = e.Id
                                         WHERE tp.Id = @id AND StartDate > GetDate()";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, TrainingProgram> trainingProgram = new Dictionary<int, TrainingProgram>();

                    while (reader.Read())
                    {
                        int trainingProgramId = reader.GetInt32(reader.GetOrdinal("TrainingProgramId"));
                        if (!trainingProgram.ContainsKey(trainingProgramId))
                        {
                            TrainingProgram newTrainingProgram = new TrainingProgram
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                                Name = reader.GetString(reader.GetOrdinal("ProgramName")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"))
                            };
                            trainingProgram.Add(trainingProgramId, newTrainingProgram);
                        }
                        TrainingProgram fromDictionary = trainingProgram[trainingProgramId];

                        if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("EmployeesEnrolled"))

                            };
                            fromDictionary.EmployeeList.Add(employee);
                        }
                    }
                    reader.Close();
                    return View(trainingProgram.Values.First());
                }
            }

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
            TrainingProgram trainingProgram = GetTrainingProgramById(id);
            return View(trainingProgram);
        }

        // POST: TrainingPrograms/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using(SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM EmployeeTraining WHERE TrainingProgramId = @id;
                                            DELETE FROM TrainingProgram WHERE id = @id";
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

        //Method to return a Training Program by ID but ONLY if it is a future program
        private TrainingProgram GetTrainingProgramById (int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT tp.Id as TrainingProgramId, tp.Name as ProgramName, tp.StartDate, tp.EndDate, tp.MaxAttendees, e.Id as EmployeeId, 
                                               e.FirstName + ' ' + e.LastName as EmployeesEnrolled
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id 
                                     LEFT JOIN Employee e ON et.EmployeeId = e.Id
                                         WHERE tp.Id = @id AND StartDate > GetDate()";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, TrainingProgram> trainingProgram = new Dictionary<int, TrainingProgram>();

                    while (reader.Read())
                    {
                        int trainingProgramId = reader.GetInt32(reader.GetOrdinal("TrainingProgramId"));
                        if (!trainingProgram.ContainsKey(trainingProgramId))
                        {
                            TrainingProgram newTrainingProgram = new TrainingProgram
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                                Name = reader.GetString(reader.GetOrdinal("ProgramName")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"))
                            };
                            trainingProgram.Add(trainingProgramId, newTrainingProgram);
                        }
                        TrainingProgram fromDictionary = trainingProgram[trainingProgramId];

                        if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("EmployeesEnrolled"))

                            };
                            fromDictionary.EmployeeList.Add(employee);
                        }
                    }
                    reader.Close();
                    return trainingProgram.Values.First();

                }
                    
            }
        }
    }
}