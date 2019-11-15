SELECT e.Id, e.FirstName, e.LastName, d.Name AS 'DepartmentName'
FROM Employee e
LEFT JOIN Department d on d.Id = e.DepartmentId