Feature: SCRUM50

These are the tests for SCRUM50, which covers the user's ability to search for gyms by using a postal code.

Scenario: User can search for gyms using a postal code
    Given I am on the gym search page
    When I enter 97361 as a postal code in the search bar
    And I click the postal search button
    Then I should see a list of gyms appear