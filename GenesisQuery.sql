 USE MASTER
 GO

 IF NOT EXISTS (
     SELECT [name]
     FROM sys.databases
     WHERE [name] = N'BangazonWorkforce'
 )
 CREATE DATABASE BangazonWorkforce
 GO

 USE BangazonWorkforce
 GO

-- DELETE FROM OrderProduct;
-- DELETE FROM ComputerEmployee;
-- DELETE FROM EmployeeTraining;
-- DELETE FROM Employee;
-- DELETE FROM TrainingProgram;
-- DELETE FROM Computer;
-- DELETE FROM Department;
-- DELETE FROM [Order];
-- DELETE FROM PaymentType;
-- DELETE FROM Product;
-- DELETE FROM ProductType;
-- DELETE FROM Customer;


-- ALTER TABLE Employee DROP CONSTRAINT [FK_EmployeeDepartment];
-- ALTER TABLE ComputerEmployee DROP CONSTRAINT [FK_ComputerEmployee_Employee];
-- ALTER TABLE ComputerEmployee DROP CONSTRAINT [FK_ComputerEmployee_Computer];
-- ALTER TABLE EmployeeTraining DROP CONSTRAINT [FK_EmployeeTraining_Employee];
-- ALTER TABLE EmployeeTraining DROP CONSTRAINT [FK_EmployeeTraining_Training];
-- ALTER TABLE Product DROP CONSTRAINT [FK_Product_ProductType];
-- ALTER TABLE Product DROP CONSTRAINT [FK_Product_Customer];
-- ALTER TABLE PaymentType DROP CONSTRAINT [FK_PaymentType_Customer];
-- ALTER TABLE [Order] DROP CONSTRAINT [FK_Order_Customer];
-- ALTER TABLE [Order] DROP CONSTRAINT [FK_Order_Payment];
-- ALTER TABLE OrderProduct DROP CONSTRAINT [FK_OrderProduct_Product];
-- ALTER TABLE OrderProduct DROP CONSTRAINT [FK_OrderProduct_Order];


-- DROP TABLE IF EXISTS OrderProduct;
-- DROP TABLE IF EXISTS ComputerEmployee;
-- DROP TABLE IF EXISTS EmployeeTraining;
-- DROP TABLE IF EXISTS Employee;
-- DROP TABLE IF EXISTS TrainingProgram;
-- DROP TABLE IF EXISTS Computer;
-- DROP TABLE IF EXISTS Department;
-- DROP TABLE IF EXISTS [Order];
-- DROP TABLE IF EXISTS PaymentType;
-- DROP TABLE IF EXISTS Product;
-- DROP TABLE IF EXISTS ProductType;
-- DROP TABLE IF EXISTS Customer;


CREATE TABLE Department (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(55) NOT NULL,
	Budget 	INTEGER NOT NULL
);

CREATE TABLE Employee (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL,
	DepartmentId INTEGER NOT NULL,
	IsSuperVisor BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_EmployeeDepartment FOREIGN KEY(DepartmentId) REFERENCES Department(Id)
);

CREATE TABLE Computer (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	PurchaseDate DATETIME NOT NULL,
	DecomissionDate DATETIME,
	Make VARCHAR(55) NOT NULL,
	Manufacturer VARCHAR(55) NOT NULL
);

CREATE TABLE ComputerEmployee (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INTEGER NOT NULL,
	ComputerId INTEGER NOT NULL,
	AssignDate DATETIME NOT NULL,
	UnassignDate DATETIME,
    CONSTRAINT FK_ComputerEmployee_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_ComputerEmployee_Computer FOREIGN KEY(ComputerId) REFERENCES Computer(Id)
);


CREATE TABLE TrainingProgram (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	MaxAttendees INTEGER NOT NULL
);

CREATE TABLE EmployeeTraining (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INTEGER NOT NULL,
	TrainingProgramId INTEGER NOT NULL,
    CONSTRAINT FK_EmployeeTraining_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_EmployeeTraining_Training FOREIGN KEY(TrainingProgramId) REFERENCES TrainingProgram(Id)
);

CREATE TABLE ProductType (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(55) NOT NULL
);

CREATE TABLE Customer (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL,
	CreationDate DATETIME NOT NULL,
	LastActiveDate DATETIME NOT NULL
);

CREATE TABLE Product (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	ProductTypeId INTEGER NOT NULL,
	CustomerId INTEGER NOT NULL,
	Price MONEY NOT NULL,
	Title VARCHAR(255) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	Quantity INTEGER NOT NULL,
    CONSTRAINT FK_Product_ProductType FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id),
    CONSTRAINT FK_Product_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);


CREATE TABLE PaymentType (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	AcctNumber VARCHAR(55) NOT NULL,
	[Name] VARCHAR(55) NOT NULL,
	CustomerId INTEGER NOT NULL,
    CONSTRAINT FK_PaymentType_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);

CREATE TABLE [Order] (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	CustomerId INTEGER NOT NULL,
	PaymentTypeId INTEGER,
    CONSTRAINT FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id),
    CONSTRAINT FK_Order_Payment FOREIGN KEY(PaymentTypeId) REFERENCES PaymentType(Id)
);

CREATE TABLE OrderProduct (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	OrderId INTEGER NOT NULL,
	ProductId INTEGER NOT NULL,
    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY(ProductId) REFERENCES Product(Id),
    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY(OrderId) REFERENCES [Order](Id)
);
ALTER TABLE Employee ADD StartDate datetime, EndDate datetime;
ALTER TABLE [Order] ADD Status varchar(55);
ALTER TABLE Computer ADD CurrentEmployeeId int, CONSTRAINT Fk_Computer_Employee FOREIGN KEY(CurrentEmployeeId) REFERENCES Employee(Id);

INSERT INTO Department (Name, Budget) VALUES ('Accounting', 10000);
INSERT INTO Department (Name, Budget) VALUES ('Human Resources', 2000);
INSERT INTO Department (Name, Budget) VALUES ('IT', 5000);
INSERT INTO Department (Name, Budget) VALUES ('Management', 80000);
INSERT INTO Department (Name, Budget) VALUES ('Shipping', 7000);
INSERT INTO Department (Name, Budget) VALUES ('Customer Service', 3000);
INSERT INTO Department (Name, Budget) VALUES ('Legal', 500);

INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor, StartDate, EndDate) VALUES ('Matt', 'Ross', 1, 'false', '2019-08-08', null);
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor, StartDate, EndDate) VALUES ('Maggie', 'Johnson', 3, 'false', '2019-08-08', null);
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor, StartDate, EndDate) VALUES ('Haroon', 'Iqbal', 5, 'false', '2019-08-08', null);
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor, StartDate, EndDate) VALUES ('Ellie', 'Ash', 2, 'true', '2019-08-08', null);
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor, StartDate, EndDate) VALUES ('Bryan', 'Nilsen', 4, 'true', '2019-08-08', null);
INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor, StartDate, EndDate) VALUES ('Adam', 'Shaeffer', 3, 'true', '2019-08-08', null);

<<<<<<< HEAD
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES (2019-04-04, null, 'Mac', 'Apple', 2);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES (2019-04-04, null, 'PC', 'Dell', 4);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES (2019-04-04, null, 'Mac', 'Apple', 3);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES (2019-04-04, null, 'PC', 'Dell', 1);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES (2019-04-04, 2019-06-06, 'PC', 'HP', null);

INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (2, 1, 2019-04-04, null);
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (4, 2, 2019-04-04, null);
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (3, 3, 2019-04-04, null);
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (1, 4, 2019-04-04, null);
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (5, 5, 2019-04-04, 2019-06-06);
=======
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES ('2019-04-04', null, 'Mac', 'Apple', 2);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES ('2019-04-04', null, 'PC', 'Dell', 4);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES ('2019-04-04', null, 'Mac', 'Apple', 3);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES ('2019-04-04', null, 'PC', 'Dell', 1);
INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer, CurrentEmployeeID) VALUES ('2019-04-04', '2019-06-06', 'PC', 'HP', null);

INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (2, 1, '2019-04-04', null);
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (4, 2, '2019-04-04', null);
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (3, 3, '2019-04-04', null);
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (1, 4, '2019-04-04', null);
INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (5, 5, '2019-04-04', '2019-06-06');
>>>>>>> origin

INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Joe', 'Snyder', '2019-07-07', '2019-10-10');
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Michael', 'Stiles', '2019-07-07', '2019-10-10');
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Bennett', 'Foster', '2019-07-07', '2019-10-10');
INSERT INTO Customer (FirstName, LastName, CreationDate, LastActiveDate) VALUES ('Sarah', 'Fleming', '2016-07-07', '2017-10-10');

INSERT INTO PaymentType (AcctNumber, Name,	CustomerId) VALUES (0000123, 'Visa', 1);
INSERT INTO PaymentType (AcctNumber, Name,	CustomerId) VALUES (0000234, 'Mastercard', 2);
INSERT INTO PaymentType (AcctNumber, Name,	CustomerId) VALUES (0000567, 'Paypal', 3);
INSERT INTO PaymentType (AcctNumber, Name,	CustomerId) VALUES (0000890, 'Amex', 1);

INSERT INTO ProductType (Name) VALUES ('Food');
INSERT INTO ProductType (Name) VALUES ('Electronics');
INSERT INTO ProductType (Name) VALUES ('Books');

INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (1, 1, 19.99, 'Candy Corn', 'Delicious', 100);
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (2, 2, 39.99, 'Headphones', 'Loud', 200);
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (3, 3, 20.99, 'Moby Dick', 'Long', 300);
INSERT INTO Product (ProductTypeId, CustomerId, Price, Title, Description, Quantity) VALUES (1, 4, 15.99, 'Chocolate', 'Yum', 400);

INSERT INTO [Order] (CustomerId, PaymentTypeId, Status) VALUES (1, 3, 'In Progress');
INSERT INTO [Order] (CustomerId, PaymentTypeId, Status) VALUES (2, 3, 'Complete');
INSERT INTO [Order] (CustomerId, PaymentTypeId, Status) VALUES (3, 3, 'Cancelled');
INSERT INTO [Order] (CustomerId, PaymentTypeId, Status) VALUES (1, 1, 'Shipped');
INSERT INTO [Order] (CustomerId, PaymentTypeId, Status) VALUES (2, 2, 'Complete');
INSERT INTO [Order] (CustomerId, PaymentTypeId, Status) VALUES (2, 1, 'In Progress');

INSERT INTO OrderProduct (OrderId, ProductId) VALUES (1, 1);
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (2, 2);
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (3, 3);
INSERT INTO OrderProduct (OrderId, ProductId) VALUES (4, 2);

INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees) VALUES ('AWS For Enterprise', '2020-01-01', '2020-01-04', 100);
INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees) VALUES ('DevOps For Dummues', '2020-01-01', '2020-01-04', 100);
INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees) VALUES ('Security? Sure!', '2020-01-01', '2020-01-04', 100);
INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees) VALUES ('Harrassment', '2019-01-01', '2019-05-04', 100);
INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees) VALUES ('Customer Service', '2019-01-01', '2019-12-31', 5);
INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees) VALUES ('PingPong Skillz', '2019-01-01', '2020-01-04', 30);


INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 1);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 2);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (1, 3);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (2, 1);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (2, 2);
INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId) VALUES (2, 3); 


SELECT Name, StartDate, EndDate, MaxAttendees FROM TrainingProgram