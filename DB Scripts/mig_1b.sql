

IF OBJECT_ID('MealFood', 'U') IS NOT NULL DROP TABLE [MealFood];

IF OBJECT_ID('Food', 'U') IS NOT NULL DROP TABLE [Food];

CREATE TABLE [Food] (
    [FoodID]                INT                         IDENTITY(1,1) PRIMARY KEY,
    [MealID]                INT,
    [ApiFoodID]             INT,
    [Amount]                INT,
    FOREIGN KEY (MealID)    REFERENCES Meal(MealID)     ON DELETE CASCADE
);