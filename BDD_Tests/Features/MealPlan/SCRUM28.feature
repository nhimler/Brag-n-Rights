Feature: SCRUM28

As a user, I want to be able to organize the food I save into meals that I design.

@MealPlan
Scenario: Can go to meal creation
    Given I am a user who has logged in
    And I visit the meal plan dashboard
    Then I should see a button that takes me to a meal creation page

@MealPlan
Scenario: User can design their own meals
    Given I am a user who has logged in
    And I visit the meal plan dashboard
    And I click the button that allows me to design a meal
    Then I should see a form where I can design a meal, including controls for all attributes of a meal