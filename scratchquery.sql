select * from Computer where id = 42

SELECT c.Id
FROM Computer c
LEFT JOIN ComputerEmployee ce ON ce.ComputerId = c.Id
WHERE ce.Id IS NULL

select * from ComputerEmployee