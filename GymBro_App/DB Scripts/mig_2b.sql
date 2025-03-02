

ALTER TABLE Meal
DROP COLUMN TotalCalories, TotalProtein, TotalCarbs, TotalFats, [Date];

ALTER TABLE Meal
ADD Description NVARCHAR(255);

