

SELECT e.Id, e.FirstName, e.LastName, e.DepartmentId, ce.ComputerId, c.Id, c.DecomissionDate, c.Make, c.Manufacturer
FROM Employee e
LEFT JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id
LEFT JOIN Computer c ON c.Id = ce.ComputerId;

