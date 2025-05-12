Feature: SCRUM43

As a user, I want to be able to edit meals after creating them

@MealPlan
Scenario: Can get to edit page
    Given I am a user who has logged in
    And I have created a meal plan
    And I visit the details page of a meal plan
    Then I should be able to click an “Edit” button and be taken to a page where I can edit my meal plan

@MealPlan
Scenario: Can edit a meal
    Given I am a user who has logged in
    And I visit the Edit page for a meal plan
    Then I should be able to Enter new information, and click Create to save my changes

@MealPlan
Scenario: Meal information is pre-filled
    Given I am a user who has logged in
    And I visit the Edit page for a meal plan
    Then the old information from the meal plan being edited should already be filled in when I get there.