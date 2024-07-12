USE [Gringotts]
GO
--01. Records’ Count
SELECT COUNT(*) AS [Count]
  FROM WizzardDeposits
--02. Longest Magic Wand
SELECT MAX(MagicWandSize) as LongestMagicWand
  FROM WizzardDeposits
--03.Longest Magic Wand per Deposit Groups
SELECT DepositGroup,
       MAX(MagicWandSize) 
	   AS LongestMagicWand
  FROM WizzardDeposits
  GROUP BY DepositGroup
  
--04.Smallest Deposit Group Per Magic Wand Size

SELECT TOP(2)
        DepositGroup
  FROM WizzardDeposits
  GROUP BY DepositGroup
  ORDER BY AVG(MagicWandSize)
--05.Deposits Sum
SELECT
        DepositGroup, 
		SUM(DepositAmount) AS TotalSum
  FROM WizzardDeposits
  GROUP BY DepositGroup
--06.Deposits Sum for Ollivander Family
SELECT
        DepositGroup, 
		SUM(DepositAmount) AS TotalSum
  FROM WizzardDeposits
  WHERE MagicWandCreator = 'Ollivander family'
  GROUP BY DepositGroup
--7. Deposits Filter
SELECT
        DepositGroup, 
		SUM(DepositAmount) AS TotalSum
  FROM WizzardDeposits
  WHERE MagicWandCreator = 'Ollivander family'
  GROUP BY DepositGroup
  HAVING SUM(DepositAmount) <150000
  ORDER BY TotalSum DESC

--8.Deposit Charge
SELECT DepositGroup,MagicWandCreator,DepositCharge AS MinDepositCharge
FROM WizzardDeposits
ORDER BY MagicWandCreator,
         DepositGroup
--9.Age Groups

SELECT [AgeGroup],
        COUNT(*)
      AS [WizardCount]
    FROM (
              SELECT 
                     CASE
                          WHEN [Age] BETWEEN 0 AND 10 THEN '[0-10]'
                          WHEN [Age] BETWEEN 11 AND 20 THEN '[11-20]'
                          WHEN [Age] BETWEEN 21 AND 30 THEN '[21-30]'
                          WHEN [Age] BETWEEN 31 AND 40 THEN '[31-40]'
                          WHEN [Age] BETWEEN 41 AND 50 THEN '[41-50]'
                          WHEN [Age] BETWEEN 51 AND 60 THEN '[51-60]'
                          ELSE '[61+]'
                      END
                  AS [AgeGroup]
                FROM [WizzardDeposits]
         )
      AS [AgeGroupSubquery]
GROUP BY [AgeGroup]

--10. First Letter
SELECT * FROM (
                     SELECT DISTINCT LEFT (FirstName,1) AS FirstLetter FROM WizzardDeposits
					 WHERE DepositGroup = 'Troll Chest') AS FirstLetters
GROUP BY FirstLetter
ORDER BY FirstLetter

--11. Average Interest 
SELECT DepositGroup,IsDepositExpired,
       AVG(DepositInterest) AS AverageInterest
   FROM WizzardDeposits
   WHERE DepositStartDate > '01-01-1985'
GROUP BY DepositGroup,IsDepositExpired
ORDER BY DepositGroup DESC,
         IsDepositExpired
--12. *Rich Wizard, Poor Wizard
SELECT SUM(Difference) AS SumDifference
    FROM ( 
	SELECT FirstName AS 'Host Wizard',
       DepositAmount AS 'Host Wizard Deposit',
	   LEAD(FirstName) OVER  (ORDER BY Id) AS 'Guest Wizard',
	   LEAD(DepositAmount) OVER  (ORDER BY Id) AS 'Guest Wizard Deposit',
	   DepositAmount - LEAD(DepositAmount) OVER  (ORDER BY Id) AS 'Difference'
       FROM WizzardDeposits
	) AS Subquery



GO
 
USE [SoftUni]
 
GO

--13. Departments Total Salaries
SELECT*FROM Employees

SELECT DepartmentID,
       SUM(Salary) AS TotalSalary
  FROM Employees
GROUP BY DepartmentID

--14. Employees Minimum Salaries
SELECT DepartmentID,
       MIN (Salary) AS MinimumSalary
  FROM Employees
  WHERE DepartmentID IN (2,5,7) AND HireDate > '01-01-2000'
GROUP BY DepartmentID

--15. Employees Average Salaries
SELECT * 
  INTO SelectedEmployees
  FROM Employees
 WHERE Salary > 30000

 DELETE 
   FROM SelectedEmployees
WHERE ManagerID = 42

UPDATE SelectedEmployees
  SET Salary +=5000
WHERE DepartmentID = 1

SELECT DepartmentID,
      AVG(Salary) AS AverageSalary
  FROM SelectedEmployees
GROUP BY DepartmentID

--16.Employees Maximum Salaries
SELECT DepartmentID,
      MAX(Salary) AS MaxSalary
  FROM Employees
GROUP BY DepartmentID
HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000

SELECT DepartmentID, MaxSalary
FROM (
    SELECT DepartmentID, MAX(Salary) AS MaxSalary
    FROM Employees
    GROUP BY DepartmentID
) AS Subquery
WHERE MaxSalary NOT BETWEEN 30000 AND 70000


--17. Employees Count Salaries
SELECT COUNT(*) AS Count 
       FROM Employees
WHERE ManagerID IS NULL

--18.3rd Highest Salary
SELECT DepartmentID,Salary AS ThirdHighestSalary
FROM (SELECT DepartmentID,
     Salary,
	 DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS SalaryRank
  FROM Employees
GROUP BY DepartmentID,Salary) AS Sub
WHERE SalaryRank = 3



  WITH RankedSalaries AS(
		SELECT 
			DepartmentID,
			Salary,
			DENSE_RANK() OVER(PARTITION BY DepartmentId ORDER BY Salary DESC) AS [Rank]
		FROM Employees
		GROUP BY DepartmentID, Salary)
SELECT 
	   DepartmentID,
	   Salary
  FROM RankedSalaries
 WHERE [Rank] = 3


 --19.Salary Challenge
SELECT 
TOP (10) [e].[FirstName],
         [e].[LastName],
         [e].[DepartmentID]
    FROM [Employees]
      AS [e]
   WHERE [e].[Salary] > (
                              SELECT AVG([Salary])
                                  AS [AverageSalary]
                                FROM [Employees]
                                  AS [eSub]
                               WHERE [eSub].[DepartmentID] = [e].[DepartmentID]
                            GROUP BY [DepartmentID]
                         )
ORDER BY [e].[DepartmentID]

SELECT*FROM Employees