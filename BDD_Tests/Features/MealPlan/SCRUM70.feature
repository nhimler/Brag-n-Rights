Feature: SCRUM70

As a user, I want to be able to view my meal plans in a calendar format

@SCRUM70
Scenario: Can view meal plans in calendar view
    Given I am a user who has logged in
    And I have created a meal plan
    And I have created a meal
    And I visit the meal plan dashboard
    And I click the calendar view button
    Then the meal plans should switch to a calendar display
    When I click the list view button
    Then the meal plans should switch to a list display