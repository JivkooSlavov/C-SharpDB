CREATE DATABASE Boardgames
USE Boardgames


--1.
CREATE TABLE Categories (
             Id INT PRIMARY KEY IDENTITY,
			 [Name] VARCHAR(50) NOT NULL
)
CREATE TABLE Addresses (
             Id INT PRIMARY KEY IDENTITY,
			 StreetName NVARCHAR(100) NOT NULL,
			 StreetNumber INT NOT NULL,
			 Town VARCHAR(30) NOT NULL,
			 Country VARCHAR(50) NOT NULL,
			 ZIP INT NOT NULL
)
CREATE TABLE Publishers (
             Id INT PRIMARY KEY IDENTITY,
		     [Name] VARCHAR(30) UNIQUE NOT NULL,
			 AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL,
			 Website NVARCHAR(40) NOT NULL,
			 Phone NVARCHAR(20) NOT NULL
)
CREATE TABLE PlayersRanges (
             Id INT PRIMARY KEY IDENTITY,
		     PlayersMin INT NOT NULL,
			 PlayersMax INT NOT NULL
)
CREATE TABLE Boardgames (
             Id INT PRIMARY KEY IDENTITY,
		     [Name] VARCHAR(30) NOT NULL,
			 YearPublished INT NOT NULL,
			 Rating DECIMAL(10,2) NOT NULL,
			 CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
			 PublisherId INT FOREIGN KEY REFERENCES Publishers(Id) NOT NULL,
			 PlayersRangeId INT FOREIGN KEY REFERENCES Publishers(Id) NOT NULL
)
CREATE TABLE Creators (
             Id INT PRIMARY KEY IDENTITY,
		     FirstName NVARCHAR(30) NOT NULL,
			 LastName NVARCHAR(30) NOT NULL,
			 Email NVARCHAR(30) NOT NULL
)
CREATE TABLE CreatorsBoardgames (
                CreatorId INT FOREIGN KEY REFERENCES Creators([Id])NOT NULL,
                BoardgameId INT FOREIGN KEY REFERENCES Boardgames([Id]) NOT NULL,
                PRIMARY KEY(CreatorId, BoardgameId)
)

--2.
INSERT INTO Boardgames
            ([Name], YearPublished, Rating, CategoryId,PublisherId,PlayersRangeId)
     VALUES 
            ('Deep Blue',2019,5.67,1,15,7),
			('Paris',2016,9.78,7,1,5),
			('Catan: Starfarers',2021,9.87,7,13,6),
			('Bleeding Kansas',2020,3.25,3,7,4),
			('One Small Step',2019,5.75,5,9,2)

INSERT INTO Publishers
            ([Name], AddressId, Website, Phone)
     VALUES 
            ('Agman Games',5,'www.agmangames.com','+16546135542'),
			('Amethyst Games',7,'www.amethystgames.com','+15558889992'),
			('BattleBooks',13,'www.battlebooks.com','+12345678907')

--3.
UPDATE PlayersRanges 
   SET PlayersMax +=1
   WHERE Id = 1
UPDATE Boardgames 
   SET [Name] = [Name] +'V2'
   WHERE YearPublished >='2020'

--4.
DELETE FROM CreatorsBoardgames WHERE BoardgameId IN (1,16,31,47)
DELETE FROM Boardgames WHERE PublisherId IN (1,16)
DELETE FROM Publishers WHERE AddressId IN (5)
DELETE FROM Addresses WHERE SUBSTRING(Town, 1, 1) = 'L'

--5.
SELECT [Name],[Rating]
FROM Boardgames
ORDER BY YearPublished, 
         [Name] DESC

--6.
SELECT b.Id,b.[Name],b.YearPublished,c.[Name] AS CategoryName
FROM Boardgames AS b 
JOIN Categories AS c ON b.CategoryId = c.Id
WHERE c.Name IN ('Strategy Games','Wargames')
ORDER BY b.YearPublished DESC

--7.
SELECT c.Id, CONCAT(c.FirstName,' ',c.LastName) AS CreatorName,
c.Email
FROM Creators AS c
LEFT JOIN CreatorsBoardgames AS cb ON cb.CreatorId = c.Id
WHERE cb.BoardgameId IS NULL

--8.
SELECT TOP(5) b.[Name],b.Rating,c.[Name] AS CategoryName
FROM Boardgames AS b
JOIN Categories AS c ON b.CategoryId = c.Id
JOIN PlayersRanges AS pr ON pr.Id = b.PlayersRangeId
WHERE b.Rating > 7.00 AND b.[Name] LIKE '%a%' OR b.Rating > 7.50 AND (pr.PlayersMax-pr.PlayersMin=3 OR pr.PlayersMax-pr.PlayersMin=4)
ORDER BY b.[Name],
         b.Rating DESC

--9.
SELECT CONCAT(c.FirstName,' ',c.LastName) AS FullName,
c.Email,MAX(b.Rating) AS Rating
FROM Creators AS c
JOIN CreatorsBoardgames AS cb ON cb.CreatorId = c.Id
JOIN Boardgames AS b ON b.Id = cb.BoardgameId
WHERE c.Email LIKE '%.com'
GROUP BY CONCAT(c.FirstName,' ',c.LastName),
c.Email

--10
SELECT c.LastName,CEILING(AVG(b.Rating)) AS AverageRating,p.[Name] AS PublisherName
FROM Creators AS c
JOIN CreatorsBoardgames AS cb ON cb.CreatorId = c.Id
JOIN Boardgames AS b ON b.Id = cb.BoardgameId
JOIN Publishers AS p ON b.PublisherId = p.Id
WHERE p.[Name] = 'Stonemaier Games'
GROUP BY c.LastName,p.[Name]
ORDER BY AVG(b.Rating) DESC

--11
GO
CREATE OR ALTER FUNCTION udf_CreatorWithBoardgames(@name VARCHAR(30)) 
    RETURNS INT
	         AS
			 BEGIN
			 DECLARE @countOfBoardgames  INT;
                    SET @countOfBoardgames = (
                                                        SELECT COUNT(*)
                                                          FROM Creators AS c
														  JOIN CreatorsBoardgames AS cb ON cb.CreatorId = c.Id
														  JOIN Boardgames AS b ON b.Id = cb.BoardgameId
														  WHERE c.FirstName = @name
                                                    )
 
                    RETURN @countOfBoardgames;        
		    END
SELECT dbo.udf_CreatorWithBoardgames('Bruno')

--12.
GO
CREATE OR ALTER PROCEDURE usp_SearchByCategory(@category VARCHAR(50))
              AS 
		   BEGIN 
		        SELECT b.[Name],b.YearPublished,b.Rating,c.[Name] AS CategoryName,p.[Name] AS PublisherName,
				CONCAT(pr.PlayersMin,' ', 'people') AS MinPlayers,
				CONCAT(pr.PlayersMax,' ', 'people') AS MaxPlayers
				FROM Categories AS c
				JOIN Boardgames AS b ON c.Id = b.CategoryId
				JOIN PlayersRanges AS pr ON b.PlayersRangeId = pr.Id
				JOIN Publishers AS p ON b.PublisherId = p.Id
				WHERE c.Name = @category
				ORDER BY p.[Name],
				         b.YearPublished DESC
		     END 
EXEC usp_SearchByCategory 'Wargames'
