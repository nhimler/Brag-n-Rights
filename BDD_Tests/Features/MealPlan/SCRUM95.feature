Feature: SCRUM95

As a user, I want the dates I enter for meals and meal plans to be double checked

@MealPlan
Scenario: Frequency and Duration Validation
    Given I am a user who has logged in
    And I create a meal plan longer than it's Frequency
    Then I should see an error message indicating that the meal plan's duration exceeds its frequency

Scenario: Meal Date Validation
    Given I am a user who has logged in
    And I have created a meal plan
    And I go to create a meal
    Then I should not be able to select a date outside the meal plan's timeframe
