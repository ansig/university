IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Instructor] (
    [ID] int NOT NULL IDENTITY,
    [LastName] nvarchar(50) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [HireDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Instructor] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [Student] (
    [ID] int NOT NULL IDENTITY,
    [LastName] nvarchar(50) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [EnrollmentDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Student] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [Department] (
    [DepartmentID] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NULL,
    [Budget] money NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [InstructorID] int NULL,
    [RowVersion] rowversion NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY ([DepartmentID]),
    CONSTRAINT [FK_Department_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID]) ON DELETE NO ACTION
);

GO

CREATE TABLE [OfficeAssignment] (
    [InstructorID] int NOT NULL,
    [Location] nvarchar(50) NULL,
    CONSTRAINT [PK_OfficeAssignment] PRIMARY KEY ([InstructorID]),
    CONSTRAINT [FK_OfficeAssignment_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [Course] (
    [CourseID] int NOT NULL,
    [Title] nvarchar(50) NULL,
    [Credits] int NOT NULL,
    [DepartmentID] int NOT NULL,
    CONSTRAINT [PK_Course] PRIMARY KEY ([CourseID]),
    CONSTRAINT [FK_Course_Department_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [Department] ([DepartmentID]) ON DELETE CASCADE
);

GO

CREATE TABLE [CourseAssignment] (
    [InstructorID] int NOT NULL,
    [CourseID] int NOT NULL,
    CONSTRAINT [PK_CourseAssignment] PRIMARY KEY ([CourseID], [InstructorID]),
    CONSTRAINT [FK_CourseAssignment_Course_CourseID] FOREIGN KEY ([CourseID]) REFERENCES [Course] ([CourseID]) ON DELETE CASCADE,
    CONSTRAINT [FK_CourseAssignment_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID]) ON DELETE CASCADE
);

GO

CREATE TABLE [Enrollment] (
    [EnrollmentID] int NOT NULL IDENTITY,
    [CourseID] int NOT NULL,
    [StudentID] int NOT NULL,
    [Grade] int NULL,
    CONSTRAINT [PK_Enrollment] PRIMARY KEY ([EnrollmentID]),
    CONSTRAINT [FK_Enrollment_Course_CourseID] FOREIGN KEY ([CourseID]) REFERENCES [Course] ([CourseID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Enrollment_Student_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [Student] ([ID]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Course_DepartmentID] ON [Course] ([DepartmentID]);

GO

CREATE INDEX [IX_CourseAssignment_InstructorID] ON [CourseAssignment] ([InstructorID]);

GO

CREATE INDEX [IX_Department_InstructorID] ON [Department] ([InstructorID]);

GO

CREATE INDEX [IX_Enrollment_CourseID] ON [Enrollment] ([CourseID]);

GO

CREATE INDEX [IX_Enrollment_StudentID] ON [Enrollment] ([StudentID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200515065416_InitialCreate', N'3.1.3');

GO

