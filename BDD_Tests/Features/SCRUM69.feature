Feature: SCRUM69

As a user, I want to be able to fill meal creation fields based on suggestions

@SCRUM69
Scenario: Fill meal creation fields based on suggestions
    Given I am a user who has logged in
    And I have created a meal plan
    And I visit the meal creation page
    And I add food to the meal
    And I generate suggestions for the meal
    When I select a suggestion from the list
    Then the meal creation fields should be filled with the selected suggestions details