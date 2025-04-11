

IF OBJECT_ID('WorkoutPlanExercise', 'U') IS NOT NULL DROP TABLE [WorkoutPlanExercise];

IF OBJECT_ID('WorkoutPlan', 'U') IS NOT NULL DROP TABLE [WorkoutPlan];

IF OBJECT_ID('Exercise', 'U') IS NOT NULL DROP TABLE [Exercise];


CREATE TABLE [WorkoutPlan] (
    [WorkoutPlanID]         INT                 IDENTITY(1,1) PRIMARY KEY,
    [WorkoutPlanExerciseID] INT,
    [ApiID]                 INT,
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

CREATE TABLE [WorkoutPlanExercise](
    [WorkoutPlanExerciseID]     INT PRIMARY KEY,
    [WorkoutPlanID] INT,
    [ApiID]        INT,
    [Reps]  INT,
    [Sets]  INT,
    FOREIGN KEY (WorkoutPlanID) REFERENCES [WorkoutPlan](WorkoutPlanID) ON DELETE CASCADE

);

CREATE TABLE [WorkoutExercises] (
    [WorkoutExercisesID]    INT PRIMARY KEY,
    [WorkoutPlanID]         INT,
    [WorkoutPlanExerciseID] INT,
    FOREIGN KEY (WorkoutPlanID) REFERENCES [WorkoutPlan](WorkoutPlanID) ON DELETE CASCADE,
    FOREIGN KEY (WorkoutPlanExerciseID) REFERENCES [WorkoutPlanExercise](WorkoutPlanExerciseID) ON DELETE NO ACTION
)