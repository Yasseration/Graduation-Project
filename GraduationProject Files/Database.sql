-- Creating Database Academy
IF NOT EXISTS (
    SELECT *
    FROM sys.databases
    WHERE name = 'GraduationProject'
)
BEGIN
    CREATE DATABASE GraduationProject;
END;
GO

-- Use Academy Database
USE GraduationProject;
GO

IF OBJECT_ID(N'dbo.BloodGroup', N'U') IS NULL
    CREATE TABLE BloodGroup (
        ID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
        Name VARCHAR(3) NOT NULL,
    );
GO

IF OBJECT_ID(N'dbo.Patient', N'U') IS NULL
    CREATE TABLE Patient (
        ID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
        FName NVARCHAR(50) NOT NULL,
        LName NVARCHAR(50) NOT NULL,
        Age SMALLINT NOT NULL,
        Email NVARCHAR(320) NOT NULL
            UNIQUE,
        PW NVARCHAR(MAX) NOT NULL,
        Img VARBINARY(MAX),
        BloodGroupID INT Null
            FOREIGN KEY REFERENCES BloodGroup ( ID )
    );
GO
IF OBJECT_ID(N'dbo.PatientHistory', N'U') IS NULL
    CREATE TABLE PatientHistory (
        ID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
        ADate DATE NOT NULL,
        Img VARBINARY(MAX) NOT NULL,
        Result NVARCHAR(MAX),
        PatientID INT NOT NULL
            FOREIGN KEY REFERENCES Patient ( ID ),
    );
GO