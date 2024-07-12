CREATE DATABASE NationalTouristSitesOfBulgaria
USE NationalTouristSitesOfBulgaria

--1.
CREATE TABLE Categories (
				[Id] INT PRIMARY KEY IDENTITY,
                [Name] VARCHAR(50) NOT NULL
)
CREATE TABLE Locations (
				[Id] INT PRIMARY KEY IDENTITY,
                [Name] VARCHAR(50) NOT NULL,
				Municipality VARCHAR(50),
				Province VARCHAR(50)
)
CREATE TABLE Sites (
				[Id] INT PRIMARY KEY IDENTITY,
                [Name] VARCHAR(100) NOT NULL,
				LocationId INT FOREIGN KEY REFERENCES Locations(Id) NOT NULL,
				CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
				Establishment VARCHAR(15)
)
CREATE TABLE Tourists (
				[Id] INT PRIMARY KEY IDENTITY,
                [Name] VARCHAR(50) NOT NULL,
				Age INT CHECK(Age BETWEEN 0 AND 120) NOT NULL,
				PhoneNumber VARCHAR(20) NOT NULL,
				Nationality VARCHAR(30) NOT NULL,
				Reward VARCHAR(20)
)
CREATE TABLE SitesTourists
             (
                TouristId INT FOREIGN KEY REFERENCES Tourists([Id]) NOT NULL,
                SiteId INT FOREIGN KEY REFERENCES Sites([Id]) NOT NULL,
                PRIMARY KEY(TouristId, SiteId)
             )

CREATE TABLE BonusPrizes (
				[Id] INT PRIMARY KEY IDENTITY,
                [Name] VARCHAR(50) NOT NULL
)
CREATE TABLE TouristsBonusPrizes
             (
                TouristId INT FOREIGN KEY REFERENCES Tourists([Id]) NOT NULL ,
                BonusPrizeId INT FOREIGN KEY REFERENCES BonusPrizes([Id]) NOT NULL,
                PRIMARY KEY(TouristId, BonusPrizeId)
             )

--2.
INSERT INTO Tourists
            ([Name], Age, PhoneNumber, Nationality, Reward)
     VALUES
				 ('Borislava Kazakova',52,'+359896354244','Bulgaria',NULL),
			   ('Peter Bosh',48,'+447911844141','UK',NULL),
			   ('Martin Smith',29,'+353863818592','Ireland','Bronze badge'),
				('Svilen Dobrev',49,'+359986584786','Bulgaria','Silver badge'),
			   ('Kremena Popova',38,'+359893298604','Bulgaria',NULL)

INSERT INTO Sites
            ([Name], LocationId, CategoryId,Establishment)
     VALUES
				 ('Ustra fortress',90,7,'X'),
			   ('Karlanovo Pyramids',65,7,NULL),
			   ('The Tomb of Tsar Sevt',63,8,'V BC'),
				('Sinite Kamani Natural Park',17,1,NULL),
			   ('St. Petka of Bulgaria – Rupite',92,6,'1994')

--3.
UPDATE Sites
   SET Establishment = 'not defined'
 WHERE Establishment IS NULL

--4.
DELETE FROM TouristsBonusPrizes
WHERE BonusPrizeId = ( SELECT Id
                              FROM BonusPrizes
                             WHERE Name = 'Sleeping bag')
 DELETE 
 FROM BonusPrizes
  WHERE Name = 'Sleeping bag'
--5.
SELECT[Name],Age,PhoneNumber,Nationality
FROM Tourists
ORDER BY Nationality,
          Age DESC,
		  Name

--6.
SELECT s.Name AS [Site],
l.[Name] AS [Location],
s.Establishment,
c.[Name] AS Category
FROM Sites AS s
JOIN Categories AS c ON c.Id = s.CategoryId
JOIN Locations AS l ON l.Id = s.LocationId
ORDER BY    c.[Name] DESC,
			l.[Name],
		    s.[Name]

--7.
SELECT l.Province,
l.Municipality,l.[Name] AS [Location],
COUNT(l.Municipality) AS CountOfSites
FROM Locations AS l
JOIN Sites AS s ON s.LocationId = l.Id
WHERE l.Province = 'Sofia'
GROUP BY l.Province,
l.Municipality,l.[Name]
ORDER BY CountOfSites DESC,
[Location]

--8.
SELECT s.[Name] AS [Site],
l.[Name] AS [Location],
l.Municipality,
l.Province,
s.Establishment
FROM Sites AS s
JOIN Locations AS l ON l.Id = s.LocationId
WHERE LEFT(l.[Name], 1) NOT LIKE '[B,M,D]'
AND s.Establishment LIKE '%BC'
ORDER BY s.[Name]

--9.
SELECT
t.[Name],
t.Age,
t.PhoneNumber,
t.Nationality,
    CASE
    WHEN b.[Name] IS NULL THEN '(no bonus prize)'
    ELSE b.[Name]
END AS Reward
FROM Tourists AS t
LEFT JOIN TouristsBonusPrizes AS tb  ON t.Id = tb.TouristId
LEFT JOIN BonusPrizes AS b ON b.Id = tb.BonusPrizeId
ORDER BY t.[Name]

--10.
SELECT 
SUBSTRING(t.Name, CHARINDEX(' ', t.Name) + 1, LEN(t.Name)) AS 'LastName', 
t.Nationality, t.Age, t.PhoneNumber 
FROM Tourists AS t
JOIN SitesTourists AS st ON t.Id = st.TouristId
JOIN Sites AS s ON s.Id = st.SiteId
JOIN Categories AS c ON c.Id = s.CategoryId
WHERE c.[Name] = 'History and archaeology'
GROUP BY t.[Name],t.Nationality, t.Age, t.PhoneNumber 
ORDER BY LastName
--11.
GO
CREATE FUNCTION udf_GetTouristsCountOnATouristSite (@Site VARCHAR(100))
RETURNS INT
             AS
			 BEGIN
			 DECLARE @countOfTourist INT;
                    SET @countOfTourist =(SELECT
				 COUNT(s.[Name]) FROM Sites AS s
				 JOIN SitesTourists AS st ON st.SiteId = s.Id
				 JOIN Tourists AS t ON t.Id = st.TouristId
				 WHERE s.[Name] = @Site)
			     
          RETURN @countOfTourist
		     END
		    

--12.
GO
CREATE PROC usp_AnnualRewardLottery(@TouristName VARCHAR(50))
AS
BEGIN
IF (SELECT COUNT(s.Id) FROM Sites AS s
			JOIN SitesTourists AS st ON s.Id = st.SiteId
			JOIN Tourists AS t ON st.TouristId = t.Id
			WHERE t.Name = @TouristName) >= 100
	BEGIN 
			UPDATE Tourists
			SET	Reward = 'Gold badge'
			WHERE Name = @TouristName
	END
ELSE IF (SELECT COUNT(s.Id) FROM Sites AS s
			JOIN SitesTourists AS st ON s.Id = st.SiteId
			JOIN Tourists AS t ON st.TouristId = t.Id
			WHERE t.Name = @TouristName) >= 50
	BEGIN 
			UPDATE Tourists
			SET	Reward = 'Silver badge'
			WHERE Name = @TouristName
	END
ELSE IF (SELECT COUNT(s.Id) FROM Sites AS s
			JOIN SitesTourists AS st ON s.Id = st.SiteId
			JOIN Tourists AS t ON st.TouristId = t.Id
			WHERE t.Name = @TouristName) >= 25
	BEGIN 
			UPDATE Tourists
			SET	Reward = 'Bronze badge'
			WHERE Name = @TouristName
	END
SELECT Name, Reward FROM Tourists
WHERE Name = @TouristName
END

