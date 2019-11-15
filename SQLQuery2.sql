SELECT * FROM EmployeeTraining
SELECT * FROM Computer

SELECT e.Id, e.FirstName, e.LastName,
                        d.Name,
                        ISNULL(c.Manufacturer, 'N/A'), ISNULL(c.Make, 'N/A')
                        FROM Employee e
                        LEFT JOIN Department d ON d.Id = e.DepartmentId
                        LEFT JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id
                        LEFT JOIN Computer c on c.Id = ce.ComputerId
                        WHERE e.Id = 18