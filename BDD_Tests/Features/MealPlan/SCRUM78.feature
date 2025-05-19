Feature: SCRUM78

As a user, I want to be able to delete meal plans from the archive so that I can manage my meal plan history.

@MealPlan
Scenario: Can clear all archived meal plans
    Given I am a user who has logged in
    And I create an out of date meal plan
    When I click on the link to the meal plan archive
    And I click the delete all button
    Then I should see no archived meal plans