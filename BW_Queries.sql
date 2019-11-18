SELECT tp.Id, tp.Name as ProgramName, tp.StartDate, tp.EndDate, tp.MaxAttendees, e.FirstName + ' ' + e.LastName as EmployeesEnrolled
                                          FROM TrainingProgram tp LEFT JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id 
                                     LEFT JOIN Employee e ON et.EmployeeId = e.Id
                                         WHERE StartDate > GetDate()

INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 10);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (2, 11);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (3, 12);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (4, 13);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (5, 15);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 16);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 18);