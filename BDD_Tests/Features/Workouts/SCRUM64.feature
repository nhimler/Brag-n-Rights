Feature: SCRUM64

Feature to search for exercises and view their details

@Workouts
Scenario: Valid exercise search returns results
    Given I am on the exercise search page
    When I search for a valid exercise
    Then I should see a list of exercises matching my search criteria

@Workouts
Scenario: Exercises should be able to display additional information
    Given I am on the exercise search page
    When I search for a valid exercise
    And I view the exercise details
    Then I should be able to see a window with additional information about the exercise