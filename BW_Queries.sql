SELECT tp.Id, tp.Name as ProgramName, tp.StartDate, tp.EndDate, tp.MaxAttendees, e.FirstName + ' ' + e.LastName as EmployeesEnrolled
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id 
                                     LEFT JOIN Employee e ON et.EmployeeId = e.Id
                                         WHERE StartDate > GetDate()