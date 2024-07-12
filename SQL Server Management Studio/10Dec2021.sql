CREATE DATABASE Airport
USE Airport
GO

--1.
CREATE TABLE Passengers
             (
                Id INT PRIMARY KEY IDENTITY,
                FullName VARCHAR(100) NOT NULL,
                Email VARCHAR(50) NOT NULL
             )
CREATE TABLE Pilots
             (
                Id INT PRIMARY KEY IDENTITY,
                FirstName VARCHAR(30) UNIQUE NOT NULL,
                LastName VARCHAR(30) UNIQUE NOT NULL,
				Age TinyInt CHECK(Age BETWEEN 21 AND 62) NOT NULL,
				Rating FLOAT CHECK(Rating BETWEEN 0.0 AND 10.0)
             )
CREATE TABLE AircraftTypes
             (
                Id INT PRIMARY KEY IDENTITY,
                TypeName VARCHAR(30) UNIQUE NOT NULL
             )
CREATE TABLE Aircraft
             (
                Id INT PRIMARY KEY IDENTITY,
                Manufacturer VARCHAR(25) NOT NULL,
				Model VARCHAR(30) NOT NULL,
				[Year] INT NOT NULL,
				FlightHours INT,
				Condition CHAR NOT NULL,
				TypeId INT FOREIGN KEY REFERENCES AircraftTypes(Id) NOT NULL
             )
CREATE TABLE PilotsAircraft
             (
                AircraftId INT FOREIGN KEY REFERENCES Aircraft([Id]),
                PilotId INT FOREIGN KEY REFERENCES Pilots([Id]),
                PRIMARY KEY(AircraftId, PilotId)
             )
CREATE TABLE Airports
             (
                Id INT PRIMARY KEY IDENTITY,
                AirportName VARCHAR(70) UNIQUE NOT NULL,
                Country VARCHAR(100) UNIQUE NOT NULL
             )
CREATE TABLE FlightDestinations
             (
                Id INT PRIMARY KEY IDENTITY,
                AirportId INT FOREIGN KEY REFERENCES Airports([Id]) NOT NULL,
				[Start] DATETIME NOT NULL,
				AircraftId INT FOREIGN KEY REFERENCES Aircraft([Id]) NOT NULL,
				PassengerId INT FOREIGN KEY REFERENCES Passengers([Id]) NOT NULL,
				TicketPrice DECIMAL(18,2) DEFAULT 15 NOT NULL
             )
--2.
INSERT INTO Passengers (FullName,Email)
SELECT 
  CONCAT(FirstName, ' ', LastName),
  CONCAT(FirstName, LastName, '@gmail.com')
 FROM Pilots WHERE Id BETWEEN 5 AND 15


--3.
UPDATE Aircraft
    SET Condition = 'A'
	WHERE Condition IN ('B','C') AND (FlightHours IS NULL OR FlightHours<=100) AND YEAR >=2013

--4.
DELETE FROM Passengers 
WHERE LEN(FullName)<=10

--5.
SELECT Manufacturer,Model,FlightHours,Condition
FROM Aircraft
ORDER BY FlightHours DESC

--6.
SELECT p.FirstName,p.LastName,a.Manufacturer,a.Model,a.FlightHours
FROM Aircraft AS a
JOIN PilotsAircraft AS pa ON a.Id = pa.AircraftId
JOIN Pilots AS p ON p.Id = pa.PilotId
WHERE FlightHours<=304
ORDER BY a.FlightHours DESC,
p.FirstName

--7.
SELECT TOP(20)
fd.Id AS DestinationId,
fd.[Start],p.FullName,
a.AirportName,fd.TicketPrice
FROM FlightDestinations AS fd
JOIN Airports AS a ON fd.AirportId = a.Id
JOIN Passengers AS p ON fd.PassengerId = p.Id
WHERE DAY(fd.[Start]) % 2 =0
ORDER BY fd.TicketPrice DESC,
a.AirportName

--8.
SELECT a.Id AS AircraftId,
a.Manufacturer,
a.FlightHours,
COUNT(fd.Id) AS FlightDestinationsCount,
ROUND(AVG(fd.TicketPrice),2) AS AvgPrice
FROM Aircraft AS a
JOIN FlightDestinations AS fd ON a.Id = fd.AircraftId
GROUP BY a.Id,a.Manufacturer,a.FlightHours
HAVING COUNT(fd.Id) >=2
ORDER BY FlightDestinationsCount DESC,
AircraftId

--9.
SELECT p.FullName,
COUNT(a.Id) AS CountOfAircraft,
SUM(fd.TicketPrice) AS TotalPayed
FROM Passengers AS p
JOIN FlightDestinations AS fd ON p.Id = fd.PassengerId
JOIN Aircraft AS a ON a.Id = fd.AircraftId
WHERE SUBSTRING(p.FullName, 2,1) = 'a'
GROUP BY p.FullName
HAVING COUNT(a.Id) >1
ORDER BY p.FullName


--10.
SELECT a.AirportName,
fd.[Start] AS DayTime,
fd.TicketPrice,
p.FullName,
ac.Manufacturer,
ac.Model
FROM FlightDestinations AS fd
JOIN Airports AS a ON fd.AirportId= a.Id
JOIN Passengers AS p ON fd.PassengerId = p.Id
JOIN Aircraft AS ac ON fd.AircraftId = ac.Id
WHERE CAST(fd.[Start] AS TIME) BETWEEN '06:00' AND '20:00'
AND fd.TicketPrice >2500
ORDER BY ac.Model

--11.
GO
CREATE FUNCTION udf_FlightDestinationsByEmail(@email VARCHAR(50)) 
    RETURNS INT
             AS
          BEGIN
		  DECLARE @countOfPassanger  INT;
                    SET @countOfPassanger = (
                                            SELECT [Id]
                                              FROM Passengers
                                             WHERE Email = @email
                                        );
 
                    DECLARE @countOfFlight INT;
                    SET @countOfFlight = (
                                                        SELECT COUNT(*)
                                                          FROM FlightDestinations
                                                         WHERE PassengerId = @countOfPassanger
                                                    );
 
                    RETURN @countOfFlight;    
		    END
SELECT dbo.udf_FlightDestinationsByEmail ('PierretteDunmuir@gmail.com')
--12.
GO
CREATE PROCEDURE usp_SearchByAirportName (@airportName VARCHAR(70))
              AS
           BEGIN
							   SELECT 
									a.AirportName,
									p.FullName,
									CASE 
										WHEN fd.TicketPrice <=400 THEN 'Low'
										WHEN fd.TicketPrice <=1500 THEN 'Medium' 
										ELSE 'High'
										END AS LevelOfTickerPrice,
									ac.Manufacturer,
									ac.Condition,
									act.TypeName
									FROM FlightDestinations AS fd
									JOIN Airports AS a ON fd.AirportId= a.Id
									JOIN Passengers AS p ON fd.PassengerId = p.Id
									JOIN Aircraft AS ac ON fd.AircraftId = ac.Id
									JOIN AircraftTypes AS act ON ac.TypeId = act.Id
									WHERE a.AirportName = @airportName
									ORDER BY ac.Manufacturer,
												  p.FullName
		     END

EXEC usp_SearchByAirportName 'Sir Seretse Khama International Airport'



      