-- Google Places API uses strings for place IDs, so we need to use NVARCHAR(MAX) for the ApiGymID column.
-- Additionally, a Gym table is not needed since we are not allowed to store gym information in the database.
-- Source: https://developers.google.com/maps/documentation/places/web-service/place-id#save-id

IF OBJECT_ID('GymUser', 'U') IS NOT NULL DROP TABLE [GymUser];
IF OBJECT_ID('Gym', 'U') IS NOT NULL DROP TABLE [Gym];

CREATE TABLE [GymUser] (
    [GymUserID] INT PRIMARY KEY IDENTITY(1,1),
    [ApiGymID] NVARCHAR(MAX) NOT NULL,
    [UserID] INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES [User](UserID) ON DELETE CASCADE
);

ALTER TABLE WorkoutPlan
ADD ArchivedWorkout BIT NOT NULL DEFAULT 0;
