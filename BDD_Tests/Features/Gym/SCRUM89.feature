Feature: SCRUM89

This will cover the functionality of seeing reviews for gyms that the user searches for

@Gym
Scenario: User searches for nearby gyms with their location
	Given I am logged in
    And I am on the gym search page
    When I enter 97361 as a postal code in the search bar
    And I click the postal search button
    Then I should see a list of gyms appear
    And I should see a rating next to a gym