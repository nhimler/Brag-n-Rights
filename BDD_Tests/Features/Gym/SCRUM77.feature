Feature: SCRUM77

This will cover the functionality of bookmarking any gyms that the user searches for

@Gym
Scenario: User searches for nearby gyms with their location
	Given I am logged in
    And I am on the gym search page
    When I enter 97361 as a postal code in the search bar
    And I click the postal search button
    Then I should see a list of gyms appear
    And I should see a bookmark button next to a gym


# This scenario has been deprecated as the bookmark buttons are no longer disabled when a gym is bookmarked
# @Gym
# Scenario: User tries to bookmark a gym that has already been bookmarked
#     Given I am logged in as "allBookmarked" with password "Password!1"
#     And I am on the gym search page
#     When I enter 97361 as a postal code in the search bar
#     And I click the postal search button
#     Then I should see a list of gyms appear
#     And I should see a disabled bookmark button next to a gym