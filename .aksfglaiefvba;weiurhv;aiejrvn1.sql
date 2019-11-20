/*
SELECT et.EmployeeId, et.TrainingProgramId, d.Id AS DeptId,
	COUNT(et.EmployeeId) AS TrainCount
FROM EmployeeTraining et
INNER JOIN Employee e on e.id = et.EmployeeId
LEFT JOIN Department d on e.DepartmentId = d.Id
                                           
WHERE e.DepartmentId = 1
GROUP BY et.TrainingProgramId, e.DepartmentId, et.EmployeeId, d.Id
ORDER BY COUNT(*)
*/

SELECT COUNT(e.Id) AS EmployeeCount, d.Id AS DepartmentId
FROM EmployeeTraining et
LEFT JOIN Employee e ON e.Id = et.EmployeeId
LEFT JOIN Department d ON e.DepartmentId = d.Id
GROUP BY d.Id
--Where d.Id = 1

select  
		e.FirstName, e.LastName, e.DepartmentId,
		d.Name AS DEPARTMENT_NAME,
		COUNT(*) AS EMP_Count
	FROM EmployeeTraining et
	LEFT JOIN Employee e ON e.Id = et.EmployeeId
	LEFT JOIN Department d ON d.Id = e.DepartmentId
	WHERE d.Id = 1
	GROUP BY e.FirstName, e.LastName, e.DepartmentId,
		d.Name
	

