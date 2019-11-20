using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Controllers.API
{
        [Route("api/[controller]")]
        [ApiController]
    public class TrainingProgramReportAPIController : ControllerBase
    {
            private string _connectionString;
            private SqlConnection Connection
            {
                get
                {
                    return new SqlConnection(_connectionString);
                }
            }

            public TrainingProgramReportAPIController(IConfiguration config)
            {
                _connectionString = config.GetConnectionString("DefaultConnection");
            }

            [HttpGet("{id:int}")]
            public async Task<IActionResult> Get(int id)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT et.EmployeeId, et.TrainingProgramId
                                               COUNT(et.TrainingProgramId) AS TrainingsCount
                                          fROM EmployeeTraining
                                               LEFT JOIN Employee e on e.id = et.EmployeeId
                                               LEFT JOIN Department d on e.DepartmentId = d.Id
                                           
                                         WHERE e.DepartmentId = @id
WE STOPPED HERE

                                      GROUP BY s.id, s.FirstName, s.LastName, s.SlackHandle";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        var reader = cmd.ExecuteReader();

                        var students = new List<StudentExerciseCount>();
                        while (reader.Read())
                        {
                            students.Add(
                                new StudentExerciseCount
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                                    LastName = reader.GetString(reader.GetOrdinal("lastname")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("slackhandle")),
                                    ExerciseCount = reader.GetInt32(reader.GetOrdinal("ExerciseCount")),
                                });
                        }

                        reader.Close();
                        return Ok(students);
                    }
                }
            }
        }
    }
}
}
