-- User table
CREATE TABLE [User] (
    [UserID]                INT             IDENTITY(1,1) PRIMARY KEY,
    [IdentityUserId]        NVARCHAR(450),
    [Username]              NVARCHAR(100),
    [FirstName]             NVARCHAR(100),
    [LastName]              NVARCHAR(100),
    [Email]                 NVARCHAR(255)   UNIQUE NOT NULL,
    [Password]              NVARCHAR(255)   NOT NULL,
    [Age]                   INT,
    [Gender]                NVARCHAR(10)    CHECK (Gender IN ('Male', 'Female', 'Other')),
    [Weight]                DECIMAL(5,2),
    [Height]                DECIMAL(5,2),
    [FitnessLevel]          NVARCHAR(20)    CHECK (FitnessLevel IN ('Beginner', 'Intermediate', 'Advanced')),
    [Fitnessgoals]          NVARCHAR(255),
    [AccountCreationDate]   DATETIME        DEFAULT GETDATE(),
    [LastLogin]             DATETIME,
    [ProfilePicture]        VARBINARY(MAX),
    [PreferredWorkoutTime]  NVARCHAR(20)    CHECK (PreferredWorkoutTime IN ('Morning', 'Afternoon', 'Evening')),
    [Location]              NVARCHAR(255)
);

-- Workout Plan table
CREATE TABLE [WorkoutPlan] (
    [WorkoutPlanID]         INT                 IDENTITY(1,1) PRIMARY KEY,
    [UserID]                INT,
    [PlanName]              NVARCHAR(255),
    [StartDate]             DATE,
    [EndDate]               DATE,
    [Frequency]             NVARCHAR(50),
    [Goal]                  NVARCHAR(255),
    [IsCompleted]           INT                 CHECK (IsCompleted IN (0, 1)),
    [DifficultyLevel]       NVARCHAR(20)        CHECK (DifficultyLevel IN ('Beginner', 'Intermediate', 'Advanced')),
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);

-- Exercise table
CREATE TABLE [Exercise] (
    [ExerciseID]            INT             IDENTITY(1,1) PRIMARY KEY,
    [Name]                  NVARCHAR(255),
    [Category]              NVARCHAR(20)    CHECK (category IN ('Strength', 'Cardio', 'Flexibility')),
    [MuscleGroup]           NVARCHAR(255),
    [EquipmentRequired]     NVARCHAR(255),
    [Reps]                  INT,
    [Sets]                  INT,
    [Resttime]              INT
);

-- Linking Exercise to Workout Plan (Many-to-Many Relationship)
CREATE TABLE [WorkoutPlanExercise] (
    [WorkoutPlanID]     INT,
    [ExerciseID]        INT,
    PRIMARY KEY (WorkoutPlanID, ExerciseID),
    FOREIGN KEY (WorkoutPlanID) REFERENCES WorkoutPlan(WorkoutPlanID) ON DELETE CASCADE,
    FOREIGN KEY (ExerciseID) REFERENCES Exercise(ExerciseID) ON DELETE CASCADE
);

-- Meal Plan table
CREATE TABLE [MealPlan] (
    [MealPlanID]        INT     IDENTITY(1,1) PRIMARY KEY,
    [UserID]            INT,
    [PlanName]          NVARCHAR(255),
    [StartDate]         DATE,
    [EndDate]           DATE,
    [Frequency]         NVARCHAR(50),
    [TargetCalories]    INT,
    [TargetProtein]     INT,
    [TargetCarbs]       INT,
    [TargetFats]        INT,
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);

-- Meal table
CREATE TABLE [Meal] (
    [MealID]        INT             IDENTITY(1,1) PRIMARY KEY,
    [MealName]      NVARCHAR(255),
    [MealType]      NVARCHAR(20)    CHECK (MealType IN ('Breakfast', 'Lunch', 'Dinner', 'Snack')),
    [Date]          DATE,
    [TotalCalories] INT,
    [TotalProtein]  INT,
    [TotalCarbs]    INT,
    [TotalFats]     INT,
    [MealPlanID]    INT,
    FOREIGN KEY (MealPlanID) REFERENCES MealPlan(MealPlanID) ON DELETE CASCADE
);

-- Food table
CREATE TABLE [Food] (
    [FoodID]                INT             IDENTITY(1,1) PRIMARY KEY,
    [FoodName]              NVARCHAR(255),
    [CaloriesPerServing]    INT,
    [ProteinPerServing]     INT,
    [CarbsPerServing]       INT,
    [FatPerServing]         INT,
    [ServingSize]           NVARCHAR(50)
);

-- Linking Food to Meal (Many-to-Many Relationship)
CREATE TABLE [MealFood] (
    [MealID]    INT,
    [FoodID]    INT,
    PRIMARY KEY (MealID, FoodID),
    FOREIGN KEY (MealID) REFERENCES Meal(MealID) ON DELETE CASCADE,
    FOREIGN KEY (FoodID) REFERENCES Food(FoodID) ON DELETE CASCADE
);

-- Biometric Data table
CREATE TABLE BiometricDatum (
    BiometricID INT PRIMARY KEY IDENTITY(1,1),        -- Auto-increment primary key
    UserID INT NULL,                                   -- Foreign key to User (nullable)
    Date DATE NULL,                                    -- The date of the biometric data entry
    Steps INT NULL,                                    -- Number of steps for the day
    CaloriesBurned INT NULL,                           -- Number of calories burned
    HeartRate INT NULL,                                -- User's heart rate
    SleepDuration INT NULL,                            -- Duration of sleep in minutes
    ActiveMinutes INT NULL,                            -- Duration of active minutes
    LastUpdated DATETIME NULL,                         -- DateTime when the biometric data was last updated
    
    FOREIGN KEY (UserID) REFERENCES [User](UserID)    -- Foreign key reference to the User table
);


-- Gym table
CREATE TABLE [Gym] (
    [GymID]                 INT             IDENTITY(1,1) PRIMARY KEY,
    [GymName]               NVARCHAR(255),
    [Address]               NVARCHAR(255),
    [PhoneNumber]           NVARCHAR(50),
    [WebsiteUrl]            NVARCHAR(255),
    [AvailableEquipment]    NVARCHAR(255),
    [PricingInfo]           NVARCHAR(MAX),
    [MembershipOptions]     NVARCHAR(MAX)
);

-- Gym-User relationship (Many-to-Many)
CREATE TABLE [GymUser] (
    [GymID]     INT,
    [UserID]    INT,
    PRIMARY KEY (GymID, UserID),
    FOREIGN KEY (GymID) REFERENCES Gym(GymID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);

-- Fitness Challenge table
CREATE TABLE [FitnessChallenge] (
    [ChallengeID]   INT             IDENTITY(1,1) PRIMARY KEY,
    [ChallengeName] NVARCHAR(255),
    [Description]   NVARCHAR(MAX),
    [StartDate]     DATE,
    [EndDate]       DATE,
    [Goal]          NVARCHAR(255),
    [Prize]         NVARCHAR(MAX)
);

-- Leaderboard table
CREATE TABLE [Leaderboard] (
    [LeaderboardID]     INT     IDENTITY(1,1) PRIMARY KEY,
    [ChallengeID]       INT,
    [UserID]            INT,
    [Rank]              INT,
    [Score]             INT,
    FOREIGN KEY (ChallengeID) REFERENCES FitnessChallenge(ChallengeID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);

-- Linking User to Fitness Challenge (Many-to-Many Relationship)
CREATE TABLE [ChallengeUser] (
    [ChallengeID]   INT,
    [UserID]        INT,
    PRIMARY KEY (ChallengeID, UserID),
    FOREIGN KEY (ChallengeID) REFERENCES FitnessChallenge(ChallengeID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);

CREATE TABLE Medal (
    MedalID INT PRIMARY KEY IDENTITY(1,1),         -- Auto-increment primary key
    Name NVARCHAR(100) NOT NULL,                    -- Medal name (e.g., "5K Step Champion")
    Description NVARCHAR(255) NULL,                 -- Optional description of the medal
    StepThreshold INT NOT NULL,                     -- Steps required to earn the medal
    Image NVARCHAR(255) NOT NULL                    -- Image URL for the medal
);

CREATE TABLE UserMedal (
    UserMedalID INT PRIMARY KEY IDENTITY(1,1), -- Auto-increment primary key
    UserID INT NOT NULL,                        -- Foreign key to User
    MedalID INT NOT NULL,                       -- Foreign key to Medal
    EarnedDate DATE NOT NULL,                   -- Date when the medal was earned

    CONSTRAINT FK_UserMedal_User FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    CONSTRAINT FK_UserMedal_Medal FOREIGN KEY (MedalID) REFERENCES Medals(MedalID) ON DELETE CASCADE
);
