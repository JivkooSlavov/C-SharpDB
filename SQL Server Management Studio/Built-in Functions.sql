SELECT*FROM Employees
USE [Employees]
GO
--1
  SELECT [FirstName],
         [LastName]
	FROM Employees
WHERE LEFT ([FirstName], 2) = 'Sa'

--1.Method with WildCard
  SELECT [FirstName],
         [LastName]
	FROM Employees
WHERE [FirstName] LIKE 'Sa%'

--2.Method with WildCard
  SELECT [FirstName],
         [LastName]
	FROM Employees
WHERE [LastName] LIKE '%ei%'

--2 
  SELECT [FirstName],
         [LastName]
	FROM Employees
WHERE CHARINDEX('ei', [LastName]) > 0

--3.
SELECT [FirstName]
  FROM [Employees]
 WHERE [DepartmentID] in (3,10) AND YEAR([HireDate]) BETWEEN 1995 AND 2005

 --4.  
  SELECT [FirstName],
         [LastName]
	FROM Employees
WHERE CHARINDEX('engineer', JobTitle) = 0

--4.Method with WildCard
  SELECT [FirstName],
         [LastName]
	FROM Employees
WHERE [JobTitle] NOT LIKE '%engineer%'

--5.
SELECT
      [Name]
FROM [Towns]
WHERE LEN([Name]) IN (5,6)
ORDER BY [Name]

--6.
SELECT* 
  FROM [Towns]
 WHERE LEFT([Name], 1) IN ('M','K','B', 'E')
ORDER BY [Name]

--6. Method with WildCard
SELECT* 
  FROM [Towns]
 WHERE [Name] LIKE '[MKBE]%'
ORDER BY [Name]

--7.Method with WildCard
SELECT* 
  FROM [Towns]
 WHERE [Name] LIKE '[^RBD]%'
ORDER BY [Name]

--7.Method with WildCard
SELECT* 
  FROM [Towns]
 WHERE LEFT([Name],1) NOT IN ('R','B','D')
ORDER BY [Name]


--8.
USE [SoftUni]
GO
CREATE VIEW V_EmployeesHiredAfter2000
AS 
SELECT [FirstName], [LastName]
FROM [Employees]
WHERE YEAR([HireDate]) > 2000

GO
--9.
SELECT [FirstName],[LastName]
 FROM [Employees]
WHERE LEN([LastName]) = 5

--10.
  SELECT 
         [EmployeeID], 
		 [FirstName],
		 [LastName],
		 [Salary],
		 DENSE_RANK() OVER (PARTITION BY [Salary] ORDER BY [EmployeeID])
	  AS [Rank]
    FROM [Employees]
   WHERE [Salary] BETWEEN 10000 AND 50000
ORDER BY [Salary] DESC


--11.
  SELECT *
  FROM (
  SELECT [EmployeeID], 
		 [FirstName],
		 [LastName],
		 [Salary],
		 DENSE_RANK() OVER (PARTITION BY [Salary] ORDER BY [EmployeeID])
	  AS [Rank]
    FROM [Employees]
   WHERE [Salary] BETWEEN 10000 AND 50000)
      AS [RankingSubquery]
WHERE [Rank] =2
ORDER BY [Salary] DESC


GO
USE [Geography]
--12.
SELECT*FROM [Countries]

SELECT [CountryName]
    AS [Country Name],
	   [IsoCode] 
	AS [ISO Code]
  FROM [Countries]
 WHERE [CountryName] LIKE '%a%a%a%'
 ORDER BY [IsoCode]

 --13.
  SELECT [p].[PeakName],
         [r].[RiverName],
		 LOWER(CONCAT(SUBSTRING([p].[PeakName], 1 , LEN([p].[PeakName]) -1), [r].[RiverName])) AS [Mix]
	FROM [Peaks]
	  AS [p],
	     [Rivers]
	  AS [r]
   WHERE RIGHT(LOWER([p].[PeakName]), 1) = LEFT(LOWER([r].[RiverName]), 1)
ORDER BY Mix

--14.
USE [Diablo]

SELECT*FROM [Games]

SELECT TOP(50)
       [Name],
	   FORMAT([Start], 'yyyy-MM-dd') AS [Start]
  FROM [Games]
WHERE YEAR([Start]) IN (2011,2012)
ORDER BY [Start],[Name]

--15.
SELECT*FROM [Users]

SELECT
       [Username],
	   SUBSTRING([Email], CHARINDEX('@', [Email])+1, LEN([Email])-CHARINDEX('@', [Email])+1) AS [Email Provider]
  FROM [Users]
ORDER BY [Email Provider],[Username]

--15...
SELECT
       [Username],
	   SUBSTRING([Email], CHARINDEX('@', [Email])+1, LEN([Email])) AS [Email Provider]
  FROM [Users]
ORDER BY [Email Provider],[Username]

--16.
SELECT
       [Username],
	   [IpAddress]
  FROM [Users]
WHERE [IpAddress] LIKE '___.1%.%.___'
ORDER BY [Username]

--17.
SELECT*FROM [Games]

SELECT 
      [Name] AS [Game],
CASE 
      WHEN DATEPART(hh,[Start]) BETWEEN 0 AND 11 THEN 'Morning'
	  WHEN DATEPART(hh,[Start]) BETWEEN 12 AND 17 THEN 'Afternoon '
      WHEN DATEPART(hh,[Start]) BETWEEN 18 AND 23 THEN 'Evening'
END AS [Part of the Day],
CASE 
      WHEN [Duration] <=3 THEN 'Extra Short'
	  WHEN [Duration] BETWEEN 4 AND 6 THEN 'Short'
	  WHEN [Duration] >6 THEN 'Long'
	  ELSE 'Extra Long'
END AS [Duration]
      FROM [Games]
ORDER BY [Game],[Duration],[Part of the Day]

-- 18. Orders Table

SELECT ProductName,
		OrderDate,
		DATEADD(day, 3, OrderDate) AS [Pay Due],
		DATEADD(month, 1, OrderDate) AS [Deliver Due]
FROM Orders

-- 19.People Table

CREATE TABLE People(
			 Id INT PRIMARY KEY IDENTITY,
			 [Name] VARCHAR(50) NOT NULL,
			 [Birthdate] DATETIME2 NOT NULL
)

INSERT INTO People ([Name], Birthdate)
	 VALUES ('Victor', '2000-12-07'),
			('Steven', '1992-09-10'),
			('Stephen', '1910-09-19'),
			('John', '2010-01-06')
	
SELECT [Name],
	   DATEDIFF(year, Birthdate, GETDATE()) AS [Age in Years],	
	   DATEDIFF(month, Birthdate, GETDATE()) AS [Age in Months],
	   DATEDIFF(day, Birthdate, GETDATE()) AS [Age in Days],
	   DATEDIFF(minute, Birthdate, GETDATE()) AS [Age in Minutes]
  FROM People

