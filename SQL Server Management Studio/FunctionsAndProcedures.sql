USE SoftUni

GO 
--01. Employees with Salary Above 35000
CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000
              AS
		   BEGIN
				SELECT FirstName
					  ,LastName
				  FROM Employees
				 WHERE Salary >35000
		     END

EXECUTE dbo.usp_GetEmployeesSalaryAbove35000

GO
--2. Employees with Salary Above Number
CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber (@minSalary DECIMAL(18,4))
              AS 
			  BEGIN
			  		SELECT FirstName
					       ,LastName
				      FROM Employees
				     WHERE Salary >=@minSalary
			  END

EXECUTE dbo.usp_GetEmployeesSalaryAboveNumber 48100

GO
--3.Town Names Starting With

CREATE PROCEDURE usp_GetTownsStartingWith (@startWith VARCHAR(20))
              AS 
		   BEGIN
		        SELECT Name
				  FROM Towns
				WHERE LOWER(LEFT([Name], LEN(@startWith))) = LOWER(@startWith)
			 END

EXECUTE dbo.usp_GetTownsStartingWith b

GO
--4. Employees from Town

CREATE PROCEDURE usp_GetEmployeesFromTown (@townName VARCHAR(30))
                 AS 
				 BEGIN
							SELECT e.FirstName,e.LastName
							FROM Employees AS e
							JOIN Addresses AS a 
							   ON e.AddressID = a.AddressID
							JOIN Towns AS t
							   ON a.TownID = t.TownID
							WHERE t.Name = @townName
				 END
EXECUTE dbo.usp_GetEmployeesFromTown Sofia

GO

--5. Salary Level Function
CREATE FUNCTION ufn_GetSalaryLevel (@salary DECIMAL(18,4))
 RETURNS VARCHAR(10)
                 AS 
			  BEGIN
			       DECLARE @salaryLevel VARCHAR(10)

				   IF @salary <30000
				   SET @salaryLevel = 'Low'
				   ELSE IF (@salary BETWEEN	30000 AND 50000)
				   SET @salaryLevel = 'Average'
				   ELSE 
				   SET @salaryLevel = 'High'

					RETURN @salaryLevel
			  END
--6.Employees by Salary Level
GO
CREATE PROCEDURE usp_EmployeesBySalaryLevel @salaryLevel VARCHAR(10)
              AS
           BEGIN
                    SELECT FirstName,
                           LastName
                      FROM Employees
                     WHERE dbo.ufn_GetSalaryLevel(Salary) = @salaryLevel
             END

EXECUTE dbo.usp_EmployeesBySalaryLevel 'High'

--7. Define Function
GO
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(50), @word VARCHAR(50))
RETURNS BIT  
             AS
			 BEGIN
			      DECLARE @wordIndex INT = 1
				  WHILE (@wordIndex <= LEN(@word))
				  BEGIN
				        DECLARE @currentCharacter CHAR = SUBSTRING(@word, @wordIndex,1)
						IF CHARINDEX(@currentCharacter, @setOfLetters) = 0
						BEGIN 
						     RETURN 0;
						END
						SET @wordIndex+=1
				  END
			 RETURN 1;
		END
SELECT dbo.ufn_IsWordComprised('oistmiahf', 'Sofia') AS 'asdd'
GO
--8. Delete Employees and Departments
CREATE OR ALTER PROC usp_DeleteEmployeesFromDepartment (@departmentId INT) 
		 AS
				 DECLARE  @employeesToDelete TABLE ([Id] INT)
	
			 INSERT INTO @employeesToDelete
				  SELECT EmployeeID
				    FROM Employees
				   WHERE DepartmentID = @departmentId

			 DELETE FROM EmployeesProjects
			       WHERE EmployeeID IN (SELECT * FROM @employeesToDelete)

			ALTER TABLE Departments
			ALTER COLUMN ManagerID INT

				  UPDATE Departments
					 SET ManagerID = NULL
				   WHERE ManagerID IN (SELECT * FROM @employeesToDelete)

				  UPDATE Employees
				     SET ManagerID = NULL
				   WHERE ManagerID IN (SELECT * FROM @employeesToDelete)

			 DELETE FROM Employees
				   WHERE DepartmentID = @departmentId

			 DELETE FROM Departments
				   WHERE DepartmentID = @departmentId

				  SELECT 
						COUNT(*) AS [Count] 
					FROM Employees
				   WHERE DepartmentID = @departmentId
--09. Find Full Name
USE Bank
GO
CREATE PROCEDURE usp_GetHoldersFullName
              AS
			  BEGIN
			           SELECT CONCAT(FirstName,' ', LastName) AS 'Full Name'
					 FROM AccountHolders
			  END

EXECUTE dbo.usp_GetHoldersFullName

GO
--10.People with Balance Higher Than

CREATE OR ALTER PROCEDURE usp_GetHoldersWithBalanceHigherThan (@threshold MONEY)
              AS 
			BEGIN
					SELECT ah.FirstName AS 'First Name',
				    	   ah.LastName AS 'Last Name'
				   	 FROM AccountHolders as ah
					JOIN Accounts as a 
					ON ah.Id = a.AccountHolderId
				  GROUP BY ah.FirstName, ah.LastName
                  HAVING SUM(a.Balance) > @threshold
                ORDER BY ah.FirstName, ah.LastName
			END

EXECUTE dbo.usp_GetHoldersWithBalanceHigherThan 2500

--11.Future Value Function
GO
CREATE FUNCTION ufn_CalculateFutureValue (@sum DECIMAL(10,2), @rate FLOAT, @years INT)
  RETURNS DECIMAL (12,4) 
               AS
			BEGIN
			       DECLARE @FutureValue DECIMAL(12,4)
				       SET	@FutureValue = @sum*POWER((1+@rate),@years)
					RETURN @FutureValue
			  END

--12. Calculating Interest
CREATE PROCEDURE usp_CalculateFutureValueForAccount  (@accountId INT, @interestRate FLOAT)
               AS 
			BEGIN
			     SELECT a.Id AS 'Account Id',
				        ah.FirstName,
						ah.LastName,
						a.Balance,
						dbo.ufn_CalculateFutureValue (a.Balance, @interestRate, 5) AS [Balance in  5 years]
						FROM AccountHolders AS ah
						JOIN Accounts AS a 
						     ON ah.Id =a.AccountHolderId
						WHERE a.Id = @accountId
			  END
--13.*Scalar Function: Cash in User Games Odd Row
CREATE FUNCTION ufn_CashInUsersGames(@gameName NVARCHAR(50))
  RETURNS TABLE
             AS
         RETURN
                (
                    SELECT SUM([Cash])
                        AS [SumCash]
                      FROM (
                                SELECT [g].[Name],
                                       [ug].[Cash],
                                       ROW_NUMBER() OVER(ORDER BY [ug].[Cash] DESC)
                                    AS [RowNumber]
                                  FROM [UsersGames]
                                    AS [ug]
                            INNER JOIN [Games]
                                    AS [g]
                                    ON [ug].[GameId] = [g].[Id]
                                 WHERE [g].[Name] = @gameName
                           ) 
                        AS [RankingSubQuery]
                     WHERE [RowNumber] % 2 <> 0
                )
SELECT*FROM dbo.ufn_CashInUsersGames ('Love in a mist')
			