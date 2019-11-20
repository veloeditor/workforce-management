using BangazonWorkforceMVC.Models.ViewModels;
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
                        cmd.CommandText = @"SELECT  
		                                        e.FirstName, e.LastName, e.DepartmentId,
		                                        d.Name AS DEPARTMENT_NAME,
		                                        COUNT(*) AS EMP_Count
	                                        FROM EmployeeTraining et
	                                        LEFT JOIN Employee e ON e.Id = et.EmployeeId
	                                        LEFT JOIN Department d ON d.Id = e.DepartmentId
	                                        WHERE d.Id = @id
	                                        GROUP BY e.FirstName, e.LastName, e.DepartmentId,
		                                        d.Name";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        var reader = cmd.ExecuteReader();

                        var TPs = new List<EmployeeTrainingsCount>();
                        while (reader.Read())
                        {
                            TPs.Add(
                                new EmployeeTrainingsCount
                                {
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                                    TrainingsCount = reader.GetInt32(reader.GetOrdinal("TrainingsCount")),
                                });
                        }

                        reader.Close();
                        return Ok(TPs);
                    }
                }
            }
        }
    }
}
}
