SELECT d.Id as DepartmentId, d.Budget as Budget, d.Name as Department,
                                      COUNT(e.Id) as TotalEmployees
                                          FROM Department d Left JOIN Employee e on e.DepartmentId = d.Id
                                          GROUP BY d.Id, d.Name, d.Budget