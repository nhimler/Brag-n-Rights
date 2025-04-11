

ALTER TABLE Meal
DROP COLUMN TotalCalories, TotalProtein, TotalCarbs, TotalFats, [Date];

ALTER TABLE Meal
ADD Description NVARCHAR(255);

ALTER TABLE [User]
ADD CONSTRAINT UQ_User_IdentityUserId UNIQUE (IdentityUserId);

CREATE TABLE StepCompetition (
    CompetitionId INT PRIMARY KEY IDENTITY(1,1),
    CreatorIdentityId NVARCHAR(450) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    IsActive BIT NOT NULL,
    
    FOREIGN KEY (CreatorIdentityId) REFERENCES User(IdentityUserId) ON DELETE CASCADE
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
