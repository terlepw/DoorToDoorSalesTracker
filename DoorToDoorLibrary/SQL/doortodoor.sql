USE [master];
GO
BEGIN TRY
CREATE DATABASE DoorToDoor;
END TRY
BEGIN CATCH
END CATCH
GO
USE DoorToDoor;
GO

BEGIN TRANSACTION
	BEGIN TRY
		
		CREATE TABLE Roles (
			id			int				IDENTITY(0,1) NOT NULL,
			[name]		varchar(50)		UNIQUE NOT NULL,
			CONSTRAINT PK_Roles PRIMARY KEY ([id])
		);

		INSERT INTO Roles ([name])
		VALUES ('Uknown'), ('Administrator'), ('Manager'), ('Salesperson');

		CREATE TABLE Users (
			id					int				IDENTITY(1,1) NOT NULL,
			firstName			varchar(50)		NOT NULL,
			lastName			varchar(50)		NOT NULL,
			emailAddress		varchar(100)	NOT NULL,
			[hash]				varchar(50)		NOT NULL,
			salt				varchar(50)		NOT NULL,
			roleID				int				NOT NULL,
			updatePassword		bit				NOT NULL,
			CONSTRAINT PK_Users PRIMARY KEY (id),
			CONSTRAINT FK_Roles FOREIGN KEY (roleID) REFERENCES Roles(id)
		);

		CREATE TABLE Manager_Saleperson (
			managerID			int		NOT NULL,
			salespersonID		int		NOT NULL,
			CONSTRAINT PK_Manager_Salesperson PRIMARY KEY (managerID, salespersonID),
			CONSTRAINT FK_Users_Manager FOREIGN KEY (managerID) REFERENCES Users(id),
			CONSTRAINT FK_Users_Salesperson FOREIGN KEY (salespersonID) REFERENCES Users(id)
		);

		CREATE TABLE House_Status (
			id			int				IDENTITY(1,1) NOT NULL,
			[status]	varchar(20)		NOT NULL,
			CONSTRAINT PK_House_Status PRIMARY KEY (id)
		);

		INSERT INTO House_Status ([status])
		VALUES ('To Be Visited'), ('Contacted by Phone'), ('Contacted in Person'), ('Interested'), ('Not Interested');
		
		CREATE TABLE Houses (
			id					int				IDENTITY(1,1) NOT NULL,
			street				varchar(50)		NOT NULL,
			city				varchar(50)		NOT NULL,
			district			varchar(50)		NOT NULL,
			zipCode				varchar(10)		NOT NULL,
			country				varchar(50)		NOT NULL,
			managerID			int				NOT NULL,
			salespersonID		int				NOT NULL,
			statusID			int				NOT NULL,
			CONSTRAINT PK_Houses PRIMARY KEY (id),
			CONSTRAINT FK_Users_Manager_House FOREIGN KEY (managerID) REFERENCES Users(id),
			CONSTRAINT FK_Users_Salesperson_House FOREIGN KEY (salespersonID) REFERENCES Users(id),
			CONSTRAINT FK_House_Status FOREIGN KEY (statusID) REFERENCES House_Status(id),
			CONSTRAINT U_Houses UNIQUE (street, city, district, zipCode, country)
		);

		CREATE TABLE Residents (
			id				int				IDENTITY(1,1) NOT NULL,
			houseID			int				NOT NULL,
			firstName		varchar(50)		NOT NULL,
			lastName		varchar(50)		NOT NULL,
			CONSTRAINT PK_Residents PRIMARY KEY (id),
			CONSTRAINT FK_Houses_Residents FOREIGN KEY (houseID) REFERENCES Houses(id)
		);

		CREATE TABLE Houses_Notes (
			id				int					IDENTITY(1,1) NOT NULL,
			houseID			int					NOT NULL,
			userID			int					NOT NULL,
			[date]			datetime			NOT NULL,
			note			varchar(255)		NOT NULL,
			CONSTRAINT PK_Houses_Notes PRIMARY KEY (id),
			CONSTRAINT FK_Houses FOREIGN KEY (houseID) REFERENCES Houses(id),
			CONSTRAINT FK_Users_House_Notes FOREIGN KEY (userID) REFERENCES Users(id)
		);

		CREATE TABLE Products (
			id			int				IDENTITY(1,1) NOT NULL,
			[name]		varchar(50)		NOT NULL,
			CONSTRAINT PK_Products PRIMARY KEY (id)
		);

		CREATE TABLE Manager_Products (
			managerID		int		NOT NULL,
			productID		int		NOT NULL,
			CONSTRAINT PK_Manager_Products PRIMARY KEY (managerID, productID),
			CONSTRAINT FK_Users_Manager_Products FOREIGN KEY (managerID) REFERENCES Users(id),
			CONSTRAINT FK_Products_Manager_Products FOREIGN KEY (productID) REFERENCES Products(id)
		);

		CREATE TABLE Sales_Transactions (
			id				int			IDENTITY(1,1) NOT NULL,
			[date]			datetime	NOT NULL,
			houseID			int			NOT NULL,
			amount			real		NOT NULL,
			productID		int			NOT NULL,
			salespersonID	int			NOT NULL,
			CONSTRAINT PK_Sales_Transactions PRIMARY KEY (id),
			CONSTRAINT FK_Houses_Sales_Transactions FOREIGN KEY (houseID) REFERENCES Houses(id),
			CONSTRAINT FK_Products_Sales_Transactions FOREIGN KEY (productID) REFERENCES Products(id),
			CONSTRAINT FK_Salesperson_Sales_Transaction FOREIGN KEY (salespersonID) REFERENCES Users(id)
		);

		INSERT INTO Users (firstName, lastName, emailAddress, [hash], salt, roleID, updatePassword)
		VALUES ('Admin', 'Admin', 'd2dadmin@mailinator.com', 'lDLmjuHZW6BRfCnga2T9QmsHYU4=', 'uIyPEFee3dHgDjqqESbcrw==', 1, 0);

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH;