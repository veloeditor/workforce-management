

											--UPDATE Employee
                                            --SET LastName = 'TEST',
                                               -- DepartmentId = 2
                                           -- WHERE Id = 10;
                                            --UPDATE ComputerEmployee
                                            --SET ComputerId = 4
                                           

SELECT e.Id, e.FirstName, e.LastName, e.DepartmentId, ce.ComputerId
                                        FROM Employee e
                                        JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id
                                        WHERE e.Id = 10 AND ce.UnassignDate IS NULL										

