Feature: SCRUM82

As a user, I want to be able to assign a date to meals

@MealPlan
Scenario: Can input a date on meal creation
    Given I am a user who has logged in
    And I have created a meal plan
    And I visit the meal plan dashboard
    And I click the button that allows me to design a meal
    Then I should see a date input field