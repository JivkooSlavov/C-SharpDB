USE [SoftUni]
GO


--01.Employee Address
SELECT TOP(5)
      e.EmployeeID,
	  e.JobTitle,
	  e.AddressID,
	  a.AddressText
FROM Employees AS e
JOIN Addresses AS a
	ON e.AddressID = a.AddressID
ORDER BY e.AddressID

--02.Addresses with Towns

SELECT TOP(50)
      e.FirstName,
	  e.LastName,
	  t.Name AS Town,
	  a.AddressText
  FROM Employees AS e
  JOIN Addresses AS a
	ON e.AddressID = a.AddressID
  JOIN Towns AS t
    ON a.TownID = t.TownID
ORDER BY e.FirstName,e.LastName

--3.Sales Employees
SELECT e.EmployeeID,
       e.FirstName,
	   e.LastName,
	   d.Name AS DepartmentName
FROM Employees AS e
JOIN Departments AS d
  ON e.DepartmentID = d.DepartmentID
  WHERE d.Name = 'Sales'

--4.Employee Departments

SELECT TOP(5)
       e.EmployeeID,
       e.FirstName,
	   e.Salary,
	   d.Name AS DepartmentName
FROM Employees AS e
JOIN Departments AS d
  ON e.DepartmentID = d.DepartmentID
  WHERE e.Salary > 15000
  ORDER BY d.DepartmentID

--05.Employees Without Projects

SELECT TOP(3)
        e.EmployeeID,
        e.FirstName
     FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep
       ON e.EmployeeID = ep.EmployeeID
    WHERE ep.ProjectID IS NULL
 ORDER BY e.EmployeeID

 --6.Employees Hired After
 SELECT
       e.FirstName,
	   e.LastName,
	   e.HireDate,
	   d.Name AS DeptName
FROM Employees AS e
JOIN Departments AS d
  ON e.DepartmentID = d.DepartmentID
  WHERE e.HireDate > '1.1.1999' AND d.Name IN ('Sales','Finance')
  ORDER BY e.HireDate

  --7.Employees With Project
SELECT TOP(5)
       e.EmployeeID,
	   e.FirstName,
	   p.Name AS ProjectName
FROM Employees AS e
JOIN EmployeesProjects AS ep
  ON e.EmployeeID = ep.EmployeeID
JOIN Projects AS p
  ON ep.ProjectID = p.ProjectID
 WHERE p.StartDate > '2002.08.13' AND p.EndDate IS NULL
ORDER BY e.EmployeeID

--08.Employee 24
SELECT TOP(5)
       e.EmployeeID,
	   e.FirstName,
      CASE
      WHEN YEAR(p.[StartDate]) >= 2005 THEN NULL
      ELSE p.[Name]
      END
	  AS ProjectName
      FROM Employees AS e
JOIN EmployeesProjects AS ep
        ON e.EmployeeID = ep.EmployeeID
JOIN Projects AS p
        ON ep.ProjectID = p.ProjectID
     WHERE e.EmployeeID = 24 

--09.Employee Manager

SELECT 
         e.EmployeeID,
		 e.FirstName,
		 e.ManagerID,
		 m.FirstName AS ManagerName
      FROM  Employees AS e
      JOIN Employees AS m
       ON e.ManagerID = m.EmployeeID
WHERE e.ManagerID IN (3,7)
ORDER BY e.EmployeeID 

--10.Employees Summary

SELECT TOP(50)   e.EmployeeID,
		 CONCAT(e.FirstName,' ', e.Lastname) AS EmployeeName,
		 CONCAT(m.FirstName,' ', m.Lastname)  AS ManagerName,
		 d.Name AS DepartmentName
      FROM  Employees AS e
      JOIN Employees AS m
       ON e.ManagerID = m.EmployeeID
      JOIN Departments AS d
	   ON e.DepartmentID = d.DepartmentID
ORDER BY e.EmployeeID

--11.Min Average Salary

    WITH AverageSalaries AS(
  SELECT AVG(Salary) AS Average
    FROM Employees	
GROUP BY DepartmentID)
  SELECT MIN(Average) AS MinAverageSalary
    FROM AverageSalaries

--12.Highest Peaks in Bulgaria
USE Geography

SELECT
   mc.CountryCode,
   m.MountainRange,
   p.PeakName,
   p.Elevation
  FROM Countries AS c
  JOIN MountainsCountries AS mc
   ON c.CountryCode = mc.CountryCode
  JOIN Mountains AS m
  ON mc.MountainId = m.Id
  JOIN Peaks AS p
  ON m.Id = p.MountainId
  WHERE c.CountryName = 'Bulgaria' AND p.Elevation >2835
  ORDER BY p.Elevation DESC

  --13.Count Mountain Ranges

SELECT  CountryCode,
        COUNT(MountainId)
		AS MountainRanges
	    FROM MountainsCountries
   WHERE CountryCode IN (   SELECT CountryCode
                              FROM Countries
                            	WHERE CountryName IN ('United States','Russia ','Bulgaria')
						)
   GROUP BY CountryCode

--14. Countries With or Without Rivers

SELECT TOP(5)
          co.CountryName,
		  r.RiverName
           FROM Countries AS co
	  LEFT JOIN Continents AS c
	         ON co.ContinentCode = c.ContinentCode
	  LEFT JOIN CountriesRivers AS cr
	         ON co.CountryCode = cr.CountryCode
	  LEFT JOIN Rivers AS r
	         ON cr.RiverId = r.Id
WHERE c.ContinentName = 'Africa'
ORDER BY CountryName

--15.Continents and Currencies
SELECT [ContinentCode],
       [CurrencyCode],
       [CurrencyUsage]
  FROM (
            SELECT *,
                   DENSE_RANK() OVER (PARTITION BY [ContinentCode] ORDER BY [CurrencyUsage] DESC)
                AS [CurrencyRank]
              FROM (
                        SELECT [ContinentCode],
                               [CurrencyCode],
                               COUNT(*)
                            AS [CurrencyUsage]
                          FROM [Countries]
                      GROUP BY [ContinentCode], [CurrencyCode]
                        HAVING COUNT(*) > 1
                   )
                AS [CurrencyUsageSubquery]
       )
    AS [CurrencyRankingSubquery]
 WHERE [CurrencyRank] = 1
 
 SELECT*FROM Countries
--16.Highest Peak and Longest River by Country

   SELECT COUNT(c.CountryCode) AS Count
	       FROM Countries AS c
	  LEFT JOIN MountainsCountries AS mc
	         ON c.CountryCode = mc.CountryCode
	WHERE mc.MountainId IS NULL

--17.Highest Peak and Longest River by Country
SELECT TOP(5)
        c.CountryName,
		MAX(p.Elevation) AS HighestPeakElevation,
		MAX(r.[Length]) AS LongestRiverLength
     FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
       ON c.CountryCode = mc.CountryCode
LEFT JOIN Mountains AS m
       ON mc.MountainId = m.Id
LEFT JOIN Peaks AS p
       ON m.Id = p.MountainId
LEFT JOIN CountriesRivers AS cr
       ON c.CountryCode = cr.CountryCode
LEFT JOIN Rivers as r
       ON cr.RiverId = r.Id
GROUP BY c.CountryName
ORDER BY HighestPeakElevation DESC,
         LongestRiverLength DESC,
		 c.CountryName





--18 Highest Peak Name and Elevation by Country
 SELECT 
 TOP (5) [CountryName]
      AS [Country],
         ISNULL([PeakName], '(no highest peak)')
      AS [Highest Peak Name],
         ISNULL([Elevation], 0)
      AS [Highest Peak Elevation],
         ISNULL([MountainRange], '(no mountain)')
      AS [Mountain]
    FROM (
               SELECT [c].[CountryName],
                      [p].[PeakName],
                      [p].[Elevation],
                      [m].[MountainRange],
                      DENSE_RANK() OVER(PARTITION BY [c].[CountryName] ORDER BY [p].[Elevation] DESC)
                   AS [PeakRank]
                 FROM [Countries]
                   AS [c]
            LEFT JOIN [MountainsCountries]
                   AS [mc]
                   ON [mc].[CountryCode] = [c].[CountryCode]
            LEFT JOIN [Mountains]
                   AS [m]
                   ON [mc].[MountainId] = [m].[Id]
            LEFT JOIN [Peaks]
                   AS [p]
                   ON [p].[MountainId] = [m].[Id]
         ) 
      AS [PeakRankingSubquery]
   WHERE [PeakRank] = 1
ORDER BY [Country],
         [Highest Peak Name]