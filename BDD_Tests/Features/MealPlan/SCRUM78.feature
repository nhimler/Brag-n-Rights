Feature: SCRUM78

As a user, I want my meal plans to be archived if they are no longer relevant

@MealPlan
Scenario: Old meal plans are archived
    Given I am a user who has logged in
    And I create an out of date meal plan
    When I click on the link to the meal plan archive
    And I click the delete all button
    Then I should see no archived meal plans