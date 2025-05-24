CREATE TABLE WorkoutPlanTemplate (
  WorkoutPlanTemplateID   INT             IDENTITY(1,1) PRIMARY KEY,
  PlanName                NVARCHAR(255)   NOT NULL,
  DifficultyLevel         NVARCHAR(20)    NOT NULL
    CHECK (DifficultyLevel IN ('Beginner','Intermediate','Advanced'))
);

CREATE TABLE WorkoutPlanTemplateExercise (
  WorkoutPlanTemplateExerciseID  INT           IDENTITY(1,1) PRIMARY KEY,
  WorkoutPlanTemplateID          INT           NOT NULL
    CONSTRAINT FK_WPTX_Template
      FOREIGN KEY (WorkoutPlanTemplateID)
      REFERENCES WorkoutPlanTemplate(WorkoutPlanTemplateID)
      ON DELETE CASCADE,
  ApiID                          NVARCHAR(255) NOT NULL,  -- external ID or “slug”
  Reps                           INT           NOT NULL,
  Sets                           INT           NOT NULL
);

INSERT INTO WorkoutPlanTemplate (PlanName, DifficultyLevel)
VALUES
  ('Beginner workout plan',     'Beginner'),
  ('Intermediate workout plan', 'Intermediate'),
  ('Advanced workout plan',     'Advanced');


-- 1) Grab the Beginner-plan’s template ID
DECLARE @BeginnerPlanID INT;
SELECT @BeginnerPlanID = WorkoutPlanTemplateID
FROM   WorkoutPlanTemplate
WHERE  DifficultyLevel = 'Beginner';

-- 2) Insert multiple exercises in one go
INSERT INTO WorkoutPlanTemplateExercise (WorkoutPlanTemplateID, ApiID, Reps, Sets)
VALUES
  (@BeginnerPlanID, '0009',  8, 3),
  (@BeginnerPlanID, '0025',   8, 3),
  (@BeginnerPlanID, '0033',   8, 3); 


ALTER TABLE WorkoutPlanExercise
ADD [Weight] INT;

