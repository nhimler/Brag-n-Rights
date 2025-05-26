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


DECLARE @BeginnerPlanID INT;
SELECT @BeginnerPlanID = WorkoutPlanTemplateID
FROM   WorkoutPlanTemplate
WHERE  DifficultyLevel = 'Beginner';

INSERT INTO WorkoutPlanTemplateExercise (WorkoutPlanTemplateID, ApiID, Reps, Sets)
VALUES
  (@BeginnerPlanID, '0009',  8, 3),
  (@BeginnerPlanID, '0025',   8, 3),
  (@BeginnerPlanID, '0033',   8, 3); 

DECLARE @IntermediatePlanID INT;
SELECT @IntermediatePlanID = WorkoutPlanTemplateID
FROM   WorkoutPlanTemplate
WHERE  DifficultyLevel = 'Intermediate';

INSERT INTO WorkoutPlanTemplateExercise (WorkoutPlanTemplateID, ApiID, Reps, Sets)
VALUES
  (@IntermediatePlanID, '0047', 8, 3),
  (@IntermediatePlanID, '0052', 8, 3),
  (@IntermediatePlanID, '0089', 8, 3);


DECLARE @AdvancedPlanID INT;
SELECT @AdvancedPlanID = WorkoutPlanTemplateID
FROM   WorkoutPlanTemplate
WHERE  DifficultyLevel = 'Advanced';

INSERT INTO WorkoutPlanTemplateExercise (WorkoutPlanTemplateID, ApiID, Reps, Sets)
VALUES
  (@AdvancedPlanID, '0061', 8, 3),
  (@AdvancedPlanID, '0074', 8, 3),
  (@AdvancedPlanID, '0098', 8, 3);



ALTER TABLE WorkoutPlanExercise
ADD [Weight] INT;

