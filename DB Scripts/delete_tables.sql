-- Drop the junction table for the many-to-many relationship between Meal and Food
IF OBJECT_ID('Meal_Food', 'U') IS NOT NULL DROP TABLE Meal_Food;

-- Drop the Leaderboard table
IF OBJECT_ID('Leaderboard', 'U') IS NOT NULL DROP TABLE Leaderboard;

-- Drop the table for Challenge-User relationship
IF OBJECT_ID('Challenge_User', 'U') IS NOT NULL DROP TABLE Challenge_User;

-- Drop the Fitness Challenge table
IF OBJECT_ID('Fitness_Challenge', 'U') IS NOT NULL DROP TABLE Fitness_Challenge;

-- Drop the Gym-User relationship table
IF OBJECT_ID('Gym_User', 'U') IS NOT NULL DROP TABLE Gym_User;

-- Drop the Gym table
IF OBJECT_ID('Gym', 'U') IS NOT NULL DROP TABLE Gym;

-- Drop the Biometric Data table
IF OBJECT_ID('Biometric_Data', 'U') IS NOT NULL DROP TABLE Biometric_Data;

-- Drop the Meal table
IF OBJECT_ID('Meal', 'U') IS NOT NULL DROP TABLE Meal;

-- Drop the Meal Plan table
IF OBJECT_ID('Meal_Plan', 'U') IS NOT NULL DROP TABLE Meal_Plan;

-- Drop the Workout Plan Exercise linking table
IF OBJECT_ID('Workout_Plan_Exercise', 'U') IS NOT NULL DROP TABLE Workout_Plan_Exercise;

-- Drop the Exercise table
IF OBJECT_ID('Exercise', 'U') IS NOT NULL DROP TABLE Exercise;

-- Drop the Workout Plan table
IF OBJECT_ID('Workout_Plan', 'U') IS NOT NULL DROP TABLE Workout_Plan;

-- Drop the Food table
IF OBJECT_ID('Food', 'U') IS NOT NULL DROP TABLE Food;

-- Drop the User table
IF OBJECT_ID('[User]', 'U') IS NOT NULL DROP TABLE [User];
