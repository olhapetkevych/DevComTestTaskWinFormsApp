CREATE DATABASE devcomtaskdb_1
GO
USE devcomtaskdb_1
GO
CREATE TABLE Users
(
	UserID INT IDENTITY PRIMARY KEY,
	Login VARCHAR(20) NOT NULL UNIQUE CHECK(Login != ''),
	Password VARCHAR(50),
	FirstName VARCHAR(40),
	LastName VARCHAR(40),
	DateOfBirth DATE CHECK(DateOfBirth < GETDATE()),
	Gender VARCHAR(1) CHECK(Gender='M' OR Gender='F')
)
GO
CREATE TABLE Orders
(
	OrderID INT IDENTITY PRIMARY KEY,
	UserID INT NOT NULL REFERENCES Users(UserID),
    OrderDate DATETIME DEFAULT GETDATE(),
	OrderCost MONEY CHECK(OrderCost > 0),
	ItemsDescription VARCHAR(1000),
	ShippingAddress VARCHAR(1000)
)
GO
CREATE PROCEDURE CheckOrder 
	@userID INT,
    @orderDate DATETIME
AS 
--RETURN (SELECT COUNT(OrderID) FROM Orders WHERE UserID = @userID AND DATEDIFF(day, @orderDate, OrderDate) = 0)
RETURN (SELECT COUNT(OrderID) FROM Orders WHERE UserID = @userID AND CAST(@orderDate AS DATE) = CAST(OrderDate AS DATE))
GO
INSERT INTO Users (Login) VALUES ('test1'),('test2'),('test3'),('test4'),('test5')
GO
INSERT INTO Orders (UserID, OrderDate) VALUES 
(1, '2023-05-02 17:00'),(2, '2023-05-02 17:30'),(3, '2023-05-02 18:00'),
(4, '2023-05-03 16:00'),(5, '2023-05-03 16:30')
GO