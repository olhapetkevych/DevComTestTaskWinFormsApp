# DevComTestTaskWinFormsApp

Script for creating database and tables and adding some initial data to the tables is in file InitialData.sql


Task:
Realize MS SQL DB CRUD operation using .Net application (any of: Web API/ WinForms/ Web) 
Here is the two tables DB structure   
Table 1: Users
Fields
  UserID INT NOT NULL - Autoincrement, Primary Key
  Login VARCHAR(20) NOT NULL - Uniq user name
  Password VARCHAR(50)    
  FirstName VARCHAR(40)
  LastName VARCHAR(40)
  DateOfBirth DATE
  Gender VARCHAR(1) - M/F
Table 2: Orders 
Fields
    OrderID  INT NOT NULL - Autoincrement, Primary Key
    UserID INT NOT NULL - Foreight Key to Users table
    OrderDate  DATETIME
	OrderCost  MONEY
	ItemsDescription  VARCHAR(1000)
	ShippingAddress  VARCHAR(1000)
Required features:
 - Add New users
 - Remove User
 - Update User info
 - Create Order
 - Update Order info
Rules:
 - User Login Should be unique
 - Only one order may be created for user per day
 - Do not allow to remove user if he has any order assigned
Tech details: MS SQL is preferred
The result of task should also include script for adding some initial data to the tables (3-10 records for each) Do validation on DB side. Do not update invalid data and throw errors
