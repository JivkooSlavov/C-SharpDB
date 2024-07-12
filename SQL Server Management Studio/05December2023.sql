CREATE DATABASE RailwaysDb

GO

USE RailwaysDb
GO
CREATE TABLE Passengers (
              Id INT PRIMARY KEY IDENTITY,
			  [Name] NVARCHAR(80) NOT NULL   
            )
CREATE TABLE Towns (
                   Id INT PRIMARY KEY IDENTITY,
				   [Name] VARCHAR(30) NOT NULL
           )
CREATE TABLE RailwayStations (
                   Id INT PRIMARY KEY IDENTITY,
				   [Name] VARCHAR(50) NOT NULL,
				   TownId INT FOREIGN KEY REFERENCES Towns(Id) NOT NULL
           )
CREATE TABLE Trains (
                   Id INT PRIMARY KEY IDENTITY,
				   HourOfDeparture VARCHAR(5) NOT NULL,
				   HourOfArrival VARCHAR(5) NOT NULL,
				   DepartureTownId INT FOREIGN KEY REFERENCES Towns(Id) NOT NULL,
				   ArrivalTownId INT FOREIGN KEY REFERENCES Towns(Id) NOT NULL
           )
CREATE TABLE TrainsRailwayStations (
                   TrainId INT FOREIGN KEY REFERENCES Trains(Id),
				   RailwayStationId INT FOREIGN KEY REFERENCES RailwayStations(Id),
				   PRIMARY KEY (TrainId,RailwayStationId)
           )
CREATE TABLE MaintenanceRecords (
                   Id INT PRIMARY KEY IDENTITY,
				   DateOfMaintenance DATE NOT NULL,
				   Details VARCHAR(2000) NOT NULL,
				   TrainId  INT FOREIGN KEY REFERENCES Trains(Id) NOT NULL
				   )
CREATE TABLE Tickets (
                   Id INT PRIMARY KEY IDENTITY,
				   Price DECIMAL NOT NULL,
				   DateOfDeparture DATE NOT NULL,
				   DateOfArrival DATE NOT NULL,
			       TrainId  INT FOREIGN KEY REFERENCES Trains(Id) NOT NULL,
		           PassengerId  INT FOREIGN KEY REFERENCES Passengers(Id) NOT NULL
				   )
GO
--02. Insert
INSERT INTO Trains 
                 (HourOfDeparture,HourOfArrival,DepartureTownId,ArrivalTownId)
     VALUES ('07:00','19:00',1,3),
	        ('08:30','20:30',5,6),
			('09:00','21:00',4,8),
			('06:45','03:55',27,7),
			('10:15','12:15',15,5)

INSERT INTO TrainsRailwayStations 
                 (TrainId,RailwayStationId)
     VALUES (36,1),
	        (36,4),
			(36,31),
			(36,57),
			(36,7),
			(37,13),
			(37,54),
			(37,60),
			(37,16),
			(38,10),
			(38,50),
			(38,52),
			(38,22),
			(39,68),
			(39,3),
			(39,31),
			(39,19),
			(40,41),
			(40,7),
			(40,52),
			(40,13)


INSERT INTO Tickets 
                 (Price,DateOfDeparture,DateOfArrival,TrainId,PassengerId)
     VALUES (90.00,'2023-12-01','2023-12-01',36,1),
	        (115.00,'2023-08-02','2023-08-02',37,2),
			(160.00,'2023-08-03','2023-08-03',38,3),
			(255.00,'2023-09-01','2023-09-02',39,21),
			(95.00,'2023-09-02','2023-09-03',40,22)
--03.
UPDATE Tickets
SET DateOfDeparture = DATEADD(DAY, 7, DateOfDeparture),
DateOfArrival = DATEADD(DAY, 7, DateOfArrival)
WHERE DateOfDeparture > '2023-10-31';
--4.
--Problem 04: Delete

DELETE FROM Tickets 
WHERE TrainId IN (
    SELECT Id 
    FROM Trains 
    WHERE DepartureTownId = (SELECT Id FROM Towns WHERE Name = 'Berlin')
);

DELETE FROM MaintenanceRecords 
WHERE TrainId IN (
    SELECT Id 
    FROM Trains 
    WHERE DepartureTownId = (SELECT Id FROM Towns WHERE Name = 'Berlin')
);

DELETE FROM TrainsRailwayStations 
WHERE TrainId IN (
    SELECT Id 
    FROM Trains 
    WHERE DepartureTownId = (SELECT Id FROM Towns WHERE Name = 'Berlin')
);

DELETE FROM Trains 
WHERE DepartureTownId = (SELECT Id FROM Towns WHERE Name = 'Berlin');

--5.

SELECT DateOfDeparture,Price FROM Tickets
ORDER BY Price,
         DateOfDeparture DESC


--6.
SELECT p.Name AS PassengerName,
       t.Price AS TicketPrice,
	   DateOfDeparture,
	   t.TrainId AS TrainID
 FROM Tickets AS t
JOIN Passengers AS p
ON t.PassengerId = p.Id
ORDER BY TicketPrice DESC,
          PassengerName

--7.
SELECT t.[Name] AS Town,
	   rs.[Name] AS 'Railway Station'
FROM 
    RailwayStations rs
    LEFT JOIN TrainsRailwayStations trs ON rs.Id = trs.RailwayStationId
    INNER JOIN Towns t ON rs.TownId = t.Id
WHERE 
    trs.TrainId IS NULL
ORDER BY 
    t.[Name] ASC, rs.[Name] ASC;

--8.
SELECT TOP(3) t.Id AS TrainId,
       t.HourOfDeparture,
	   ti.Price AS TicketPrice,
	   dt.Name AS Destination
FROM Trains t
JOIN Tickets ti ON t.Id = ti.TrainId
JOIN Towns dT ON t.ArrivalTownId = dT.Id
WHERE t.HourOfDeparture LIKE '08:__'
AND ti.Price > 50.00
ORDER BY ti.Price ASC;

--9.
SELECT Tw.Name, COUNT(Ti.Id)
FROM Tickets AS Ti
JOIN Trains AS Tr ON Tr.Id = Ti.TrainId
JOIN Towns AS Tw ON Tw.Id = Tr.ArrivalTownId
WHERE Ti.Price > 76.99
GROUP BY Tw.Name
ORDER BY Tw.Name
.
--10

SELECT tr.Id AS TrainID,
t.Name AS DepartureTown,
m.Details
FROM MaintenanceRecords AS m
JOIN Trains AS tr 
ON m.TrainId = tr.Id
JOIN Towns AS t
ON t.Id = tr.DepartureTownId
WHERE m.Details LIKE '%inspection%'
ORDER BY tr.Id

--11.
CREATE OR ALTER FUNCTION udf_TownsWithTrains(@name VARCHAR(30))
RETURNS INT 
             AS
			 BEGIN
			      DECLARE @townId INT; 
				  SET @townId = ( 
				                        SELECT Id FROM Towns
										WHERE Name = @name
				                       )
				  
                    DECLARE @countOftrains INT;
                    SET @countOftrains = (
                                                        SELECT COUNT(*)
                                                          FROM Trains
                                                         WHERE DepartureTownId = @townId OR ArrivalTownId = @townId
                                                    );
			 RETURN @countOftrains
			 END
--12.
GO
CREATE PROCEDURE usp_SearchByTown(@townName VARCHAR(30)) 
              AS
		   BEGIN
		         SELECT p.Name PassengerName,
			  t.DateOfDeparture,
			  tr.HourOfDeparture
			  FROM Passengers AS p
			  JOIN Tickets AS t
			  ON p.Id = t.PassengerId
			  JOIN Trains AS tr
			  ON t.TrainId = tr.Id
			  JOIN Towns AS tw
			  ON tr.ArrivalTownId = tw.Id
			  WHERE tw.Name = @townName
			  ORDER BY t.DateOfDeparture DESC,
			           PassengerName
		     END
EXEC usp_SearchByTown 'Berlin'

			

GO
