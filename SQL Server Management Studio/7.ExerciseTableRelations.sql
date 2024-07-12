CREATE DATABASE [TableRelations]

GO

USE [TableRelations]

GO
--1.One-To-One Relationship
CREATE TABLE [Passports](
 [PassportID] INT PRIMARY KEY IDENTITY (101,1),
 [PassportNumber] VARCHAR(20) NOT NULL
)

INSERT INTO [Passports]([PassportNumber])
      VALUES ('N34FG21B'),
	         ('K65LO4R7'),
			 ('K65LO4R7')

CREATE TABLE [Persons](
    [PersonID] INT PRIMARY KEY IDENTITY,
	[FirstName] VARCHAR(50) NOT NULL,
	[Salary] DECIMAL (7,2),
	[PassportID] INT FOREIGN KEY REFERENCES [Passports]([PassportID]) UNIQUE NOT NULL
)
INSERT INTO [Persons]([FirstName],[Salary],[PassportID])
      VALUES ('Roberto',43300.00,102),
	         ('Tom',56100.00,103),
			 ('Yana',60200.00,101)


--2.One-To-Many Relationship

CREATE TABLE [Manufacturers](
    [ManufacturerID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(20) NOT NULL,
	[EstablishedOn] DATETIME2
)

INSERT INTO [Manufacturers]([Name],[EstablishedOn])
       VALUES ('BMW', '07/03/1916'),
	          ('Tesla','01/01/2003'),
			  ('Lada','01/05/1966')

CREATE TABLE [Models](
    [ModelID] INT PRIMARY KEY IDENTITY (101,1),
	[Name] VARCHAR(20) NOT NULL,
	[ManufacturerID] INT FOREIGN KEY REFERENCES [Manufacturers]([ManufacturerID])
)
INSERT INTO [Models] ([Name],[ManufacturerID])
      VALUES ('X1',1),
	         ('i6',1),
			 ('Model S',2),
			 ('Model X',2),
			 ('Model 3',2),
			 ('Nova',3)
--3.Many-To-Many Relationship
CREATE TABLE [Students](
    [StudentID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR (20) NOT NULL
)
CREATE TABLE [Exams](
    [ExamID] INT PRIMARY KEY IDENTITY(101,1),
	[Name] VARCHAR (20) NOT NULL
)
CREATE TABLE [StudentsExams](
    [StudentID] INT FOREIGN KEY REFERENCES [Students]([StudentID]),
	[ExamID] INT FOREIGN KEY REFERENCES [Exams]([ExamID]),
	PRIMARY KEY([StudentID],[ExamID])
)

--4.Self-Referencing

CREATE TABLE [Teachers](
       [TeacherID] INT PRIMARY KEY IDENTITY(101,1),
	   [Name] VARCHAR(50) NOT NULL,
	   [ManagerID] INT FOREIGN KEY REFERENCES [Teachers]([TeacherID])
)

INSERT INTO [Teachers]([Name],[ManagerID])
       VALUES ('John',NULL),
	          ('Maya',106),
			  ('Silvia',106),
			  ('Ted',105),
			  ('Mark',101),
			  ('Greta',101)
SELECT*FROM [Teachers]

CREATE DATABASE [OnlineStoreDatabase]
USE [OnlineStoreDatabase]

--5.Online Store Database
CREATE TABLE [ItemTypes](
       [ItemTypeID] INT PRIMARY KEY IDENTITY,
	   [Name] VARCHAR(50) NOT NULL,
)
CREATE TABLE [Cities](
       [CityID] INT PRIMARY KEY IDENTITY,
	   [Name] VARCHAR(50) NOT NULL,
)
CREATE TABLE [Items](
       [ItemID] INT PRIMARY KEY IDENTITY,
	   [Name] VARCHAR(50) NOT NULL,
	   [ItemTypeID] INT FOREIGN KEY REFERENCES [ItemTypes]([ItemTypeID])
)
CREATE TABLE [Customers](
       [CustomerID] INT PRIMARY KEY IDENTITY,
	   [Name] VARCHAR(50) NOT NULL,
	   [Birthday] DATETIME2,
	   [CityID] INT FOREIGN KEY REFERENCES [Cities]([CityID])
)
CREATE TABLE [Orders](
       [OrderID] INT PRIMARY KEY IDENTITY,
	   [CustomerID] INT FOREIGN KEY REFERENCES [Customers]([CustomerID])
)
CREATE TABLE [OrderItems](
       [OrderID] INT FOREIGN KEY REFERENCES [Orders]([OrderID]),
	   [ItemID] INT FOREIGN KEY REFERENCES [Items]([ItemID]),
	   PRIMARY KEY ([OrderID],[ItemID])
)

--6.University Database
CREATE DATABASE [UniversityDatabase]
USE [UniversityDatabase]

CREATE TABLE [Majors](
       [MajorID] INT PRIMARY KEY IDENTITY,
	   [Name] VARCHAR(50) NOT NULL,
)
CREATE TABLE [Subjects](
       [SubjectID] INT PRIMARY KEY IDENTITY,
	   [SubjectName] VARCHAR(50) NOT NULL,
)
CREATE TABLE [Students](
       [StudentsID] INT PRIMARY KEY IDENTITY,
	   [StudentNumber] INT NOT NULL,
	   [StudentName] VARCHAR(50) NOT NULL,
	   [MajorID] INT FOREIGN KEY REFERENCES [Majors]([MajorID])
)
CREATE TABLE [Payments](
       [PaymentID] INT PRIMARY KEY IDENTITY,
	   [PaymentDate] DATETIME2 NOT NULL,
	   [PaymentAmount] DECIMAL (9,2) NOT NULL,
	   [StudentID] INT FOREIGN KEY REFERENCES [Students]([StudentsID])
)
CREATE TABLE [Agenda](
	   [StudentID] INT FOREIGN KEY REFERENCES [Students]([StudentsID]),
	   [SubjectID] INT FOREIGN KEY REFERENCES [Subjects]([SubjectID])
	   PRIMARY KEY([StudentID],[SubjectID])
)
--7.
USE [Geography]
SELECT*FROM [Peaks]

SELECT*FROM [Mountains]

--9.*Peaks in Rila

SELECT  [m].[MountainRange],
        [p].[PeakName],
		[p].[Elevation]
     FROM [Peaks]
       AS [p]
LEFT JOIN [Mountains]
       AS [m]
       ON [p].MountainId = [m].Id
	WHERE [m].MountainRange = 'Rila'
	ORDER BY [p].Elevation DESC