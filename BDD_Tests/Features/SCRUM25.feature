Feature: SCRUM25

This will cover the gym search functionality

@mytag
Scenario: User searches for nearby gyms with their location
	Given I am logged in
    When I navigate to the gym search page
    And I click on the search button
    Then I should see a list of gyms