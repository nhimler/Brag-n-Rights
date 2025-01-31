-- User table
CREATE TABLE [User] (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    email NVARCHAR(255) UNIQUE NOT NULL,
    password NVARCHAR(255) NOT NULL,
    age INT,
    gender NVARCHAR(10) CHECK (gender IN ('Male', 'Female', 'Other')),
    weight DECIMAL(5,2),
    height DECIMAL(5,2),
    fitness_level NVARCHAR(20) CHECK (fitness_level IN ('Beginner', 'Intermediate', 'Advanced')),
    fitness_goals NVARCHAR(255),
    account_creation_date DATETIME DEFAULT GETDATE(),
    last_login DATETIME,
    profile_picture VARBINARY(MAX),
    preferred_workout_time NVARCHAR(20) CHECK (preferred_workout_time IN ('Morning', 'Afternoon', 'Evening')),
    location NVARCHAR(255)
);

-- Workout Plan table
CREATE TABLE Workout_Plan (
    workout_plan_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    plan_name NVARCHAR(255),
    start_date DATE,
    end_date DATE,
    frequency NVARCHAR(50),
    goal NVARCHAR(255),
    difficulty_level NVARCHAR(20) CHECK (difficulty_level IN ('Beginner', 'Intermediate', 'Advanced')),
    FOREIGN KEY (user_id) REFERENCES [User](user_id) ON DELETE CASCADE
);

-- Exercise table
CREATE TABLE Exercise (
    exercise_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    category NVARCHAR(20) CHECK (category IN ('Strength', 'Cardio', 'Flexibility')),
    muscle_group NVARCHAR(255),
    equipment_required NVARCHAR(255),
    reps INT,
    sets INT,
    rest_time INT
);

-- Linking Exercise to Workout Plan (Many-to-Many Relationship)
CREATE TABLE Workout_Plan_Exercise (
    workout_plan_id INT,
    exercise_id INT,
    PRIMARY KEY (workout_plan_id, exercise_id),
    FOREIGN KEY (workout_plan_id) REFERENCES Workout_Plan(workout_plan_id) ON DELETE CASCADE,
    FOREIGN KEY (exercise_id) REFERENCES Exercise(exercise_id) ON DELETE CASCADE
);

-- Meal Plan table
CREATE TABLE Meal_Plan (
    meal_plan_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    plan_name NVARCHAR(255),
    start_date DATE,
    end_date DATE,
    frequency NVARCHAR(50),
    target_calories INT,
    target_protein INT,
    target_carbs INT,
    target_fats INT,
    FOREIGN KEY (user_id) REFERENCES [User](user_id) ON DELETE CASCADE
);

-- Meal table
CREATE TABLE Meal (
    meal_id INT IDENTITY(1,1) PRIMARY KEY,
    meal_name NVARCHAR(255),
    meal_type NVARCHAR(20) CHECK (meal_type IN ('Breakfast', 'Lunch', 'Dinner', 'Snack')),
    date DATE,
    total_calories INT,
    total_protein INT,
    total_carbs INT,
    total_fats INT,
    meal_plan_id INT,
    FOREIGN KEY (meal_plan_id) REFERENCES Meal_Plan(meal_plan_id) ON DELETE CASCADE
);

-- Food table
CREATE TABLE Food (
    food_id INT IDENTITY(1,1) PRIMARY KEY,
    food_name NVARCHAR(255),
    calories_per_serving INT,
    protein_per_serving INT,
    carbs_per_serving INT,
    fat_per_serving INT,
    serving_size NVARCHAR(50)
);

-- Linking Food to Meal (Many-to-Many Relationship)
CREATE TABLE Meal_Food (
    meal_id INT,
    food_id INT,
    PRIMARY KEY (meal_id, food_id),
    FOREIGN KEY (meal_id) REFERENCES Meal(meal_id) ON DELETE CASCADE,
    FOREIGN KEY (food_id) REFERENCES Food(food_id) ON DELETE CASCADE
);

-- Biometric Data table
CREATE TABLE Biometric_Data (
    biometric_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT,
    date DATE,
    steps INT,
    calories_burned INT,
    heart_rate INT,
    sleep_duration INT,
    active_minutes INT,
    FOREIGN KEY (user_id) REFERENCES [User](user_id) ON DELETE CASCADE
);

-- Gym table
CREATE TABLE Gym (
    gym_id INT IDENTITY(1,1) PRIMARY KEY,
    gym_name NVARCHAR(255),
    address NVARCHAR(255),
    phone_number NVARCHAR(50),
    website_url NVARCHAR(255),
    available_equipment NVARCHAR(255),
    pricing_info NVARCHAR(MAX),
    membership_options NVARCHAR(MAX)
);

-- Gym-User relationship (Many-to-Many)
CREATE TABLE Gym_User (
    gym_id INT,
    user_id INT,
    PRIMARY KEY (gym_id, user_id),
    FOREIGN KEY (gym_id) REFERENCES Gym(gym_id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES [User](user_id) ON DELETE CASCADE
);

-- Fitness Challenge table
CREATE TABLE Fitness_Challenge (
    challenge_id INT IDENTITY(1,1) PRIMARY KEY,
    challenge_name NVARCHAR(255),
    description NVARCHAR(MAX),
    start_date DATE,
    end_date DATE,
    goal NVARCHAR(255),
    prize NVARCHAR(MAX)
);

-- Leaderboard table
CREATE TABLE Leaderboard (
    leaderboard_id INT IDENTITY(1,1) PRIMARY KEY,
    challenge_id INT,
    user_id INT,
    rank INT,
    score INT,
    FOREIGN KEY (challenge_id) REFERENCES Fitness_Challenge(challenge_id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES [User](user_id) ON DELETE CASCADE
);

-- Linking User to Fitness Challenge (Many-to-Many Relationship)
CREATE TABLE Challenge_User (
    challenge_id INT,
    user_id INT,
    PRIMARY KEY (challenge_id, user_id),
    FOREIGN KEY (challenge_id) REFERENCES Fitness_Challenge(challenge_id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES [User](user_id) ON DELETE CASCADE
);
