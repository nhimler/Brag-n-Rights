IF OBJECT_ID('WorkoutExercises', 'U') IS NOT NULL DROP TABLE [WorkoutExercises];
IF OBJECT_ID('WorkoutPlanExercise', 'U') IS NOT NULL DROP TABLE [WorkoutPlanExercise];
IF OBJECT_ID('WorkoutPlan', 'U') IS NOT NULL DROP TABLE [WorkoutPlan];

CREATE TABLE [WorkoutPlan] (
    [WorkoutPlanID]         INT                 IDENTITY(1,1) PRIMARY KEY,
    [WorkoutPlanExerciseID] INT,
    [ApiID]                 NVARCHAR(255),
    [UserID]                INT,
    [PlanName]              NVARCHAR(255),
    [StartDate]             DATE,
    [EndDate]               DATE,
    [Frequency]             NVARCHAR(50),
    [Goal]                  NVARCHAR(255),
    [IsCompleted]           INT                 CHECK (IsCompleted IN (0, 1)),
    [DifficultyLevel]       NVARCHAR(20)        CHECK (DifficultyLevel IN ('Beginner', 'Intermediate', 'Advanced')),
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE,
    UNIQUE (ApiID)
);

CREATE TABLE [WorkoutPlanExercise] (
    WorkoutPlanExerciseID INT IDENTITY(1,1) PRIMARY KEY,
    WorkoutPlanID INT,
    ApiID NVARCHAR(255),
    Reps INT,
    Sets INT,
    FOREIGN KEY (WorkoutPlanID) REFERENCES [WorkoutPlan](WorkoutPlanID) ON DELETE CASCADE,
    FOREIGN KEY (WorkoutPlanExerciseID) REFERENCES [WorkoutPlanExercise](WorkoutPlanExerciseID) ON DELETE NO ACTION
);

CREATE TABLE [WorkoutExercises] (
    [WorkoutExercisesID]    INT PRIMARY KEY,
    [WorkoutPlanID]         INT,
    [WorkoutPlanExerciseID] INT,
    FOREIGN KEY (WorkoutPlanID) REFERENCES [WorkoutPlan](WorkoutPlanID) ON DELETE CASCADE,
    FOREIGN KEY (WorkoutPlanExerciseID) REFERENCES [WorkoutPlanExercise](WorkoutPlanExerciseID) ON DELETE NO ACTION
)

ALTER TABLE [User]
ADD CONSTRAINT UQ_User_IdentityUserId UNIQUE (IdentityUserId);

CREATE TABLE StepCompetition (
    CompetitionId INT PRIMARY KEY IDENTITY(1,1),
    CreatorIdentityId NVARCHAR(450) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    IsActive BIT NOT NULL,
    
    FOREIGN KEY (CreatorIdentityId) REFERENCES [User](IdentityUserId) ON DELETE CASCADE
);


CREATE TABLE StepCompetitionParticipants (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StepCompetitionId INT NOT NULL,
    IdentityId NVARCHAR(450) NOT NULL,
    Steps INT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL,

    -- Foreign Key to StepCompetition table
    FOREIGN KEY (StepCompetitionId) REFERENCES StepCompetition(CompetitionId) ON DELETE CASCADE,

    -- Foreign Key to User table
    FOREIGN KEY (IdentityId) REFERENCES [User](IdentityUserId)
);
