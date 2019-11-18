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
                        WHERE ce.UnassignDate IS NULL
						