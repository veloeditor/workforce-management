SELECT * FROM ComputerEmployee

SELECT e.Id, e.FirstName, e.LastName, d.Name, ce.ComputerId
FROM Employee e
LEFT JOIN Department d ON d.Id = e.DepartmentId
LEFT JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id
WHERE e.Id = 1