Feature: SCRUM64

@SCRUM64
Scenario: Invalid exercise search results in no exercises found
    Given I am on the exercise search page
    When I search for an invalid exercise
    Then I should see a message saying no exercises were found

@SCRUM64
Scenario: Valid exercise search returns results
    Given I am on the exercise search page
    When I search for a valid exercise
    Then I should see a list of exercises matching my search criteria

@SCRUM64
Scenario: Exercises should be able to display additional information
    Given I am on the exercise search page
    When I search for a valid exercise
    And I view the exercise details
    Then I should be able to see a window with additional information about the exercise