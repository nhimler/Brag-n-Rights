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