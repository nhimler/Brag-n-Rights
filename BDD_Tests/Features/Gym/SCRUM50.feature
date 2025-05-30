Feature: SCRUM50

These are the tests for SCRUM50, which covers the user's ability to search for gyms by using a postal code.

@Gym
Scenario: User can search for gyms using a postal code
    Given I am on the gym search page
    When I enter 97361 as a postal code in the search bar
    And I click the postal search button
    Then I should see a list of gyms appear

@Gym
Scenario: User can search for gyms using a postal code with no results
    Given I am on the gym search page
    When I enter 00000 as a postal code in the search bar
    And I click the postal search button
    Then I should see a message telling me there are no gyms nearby