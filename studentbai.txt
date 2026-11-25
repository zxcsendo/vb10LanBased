-- Student Information System (LAN-based)
-- Run this on the SERVER machine in SSMS (Windows Auth or sa)

IF DB_ID(N'StudentInformationSystem') IS NULL
BEGIN
    CREATE DATABASE StudentInformationSystem;
END
GO

USE StudentInformationSystem;
GO

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students (
        StudentID       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        StudentNumber   NVARCHAR(30)  NOT NULL,
        FirstName       NVARCHAR(50)  NOT NULL,
        LastName        NVARCHAR(50)  NOT NULL,
        MiddleName      NVARCHAR(50)  NULL,
        Course          NVARCHAR(80)  NOT NULL,
        YearLevel       NVARCHAR(20)  NOT NULL,
        Section         NVARCHAR(20)  NOT NULL,
        Gender          NVARCHAR(20)  NOT NULL,
        ContactNumber   NVARCHAR(30)  NULL,
        Email           NVARCHAR(100) NULL,
        Address         NVARCHAR(255) NULL,
        DateRegistered  DATETIME      NOT NULL CONSTRAINT DF_Students_DateRegistered DEFAULT (GETDATE())
    );
END
GO

-- Optional SQL login for LAN clients (change password before production use)
-- CREATE LOGIN sis_user WITH PASSWORD = 'Sis@12345';
-- USE StudentInformationSystem;
-- CREATE USER sis_user FOR LOGIN sis_user;
-- EXEC sp_addrolemember N'db_datareader', N'sis_user';
-- EXEC sp_addrolemember N'db_datawriter', N'sis_user';
GO
