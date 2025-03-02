

IF OBJECT_ID('MealFood', 'U') IS NOT NULL DROP TABLE [MealFood];

IF OBJECT_ID('Food', 'U') IS NOT NULL DROP TABLE [Food];

CREATE TABLE [Food] (
    [FoodID]                INT                         IDENTITY(1,1) PRIMARY KEY,
    [MealID]                INT,
    [ApiFoodID]             BIGINT,
    [Amount]                INT,
    FOREIGN KEY (MealID)    REFERENCES Meal(MealID)     ON DELETE CASCADE
);

INSERT INTO Medal (Name, Description, StepThreshold, Image)
VALUES
('10k Steps Medal', 'Awarded for completing 10000 steps.', 10000, 'image_10000.jpg'),
('7.5k Steps Medal', 'Awarded for completing 7500 steps.', 7500, 'image_7500.jpg'),
('5k Steps Medal', 'Awarded for completing 5000 steps.', 5000, 'image_5000.jpg'),
('2.5k Steps Medal', 'Awarded for completing 2500 steps.', 2500, 'image_2500.jpg'),
('1250 Steps Medal', 'Awarded for completing 1250 steps.', 1250, 'image_1250.jpg');
