Feature: SCRUM88

These tests will cover the functionality of view markers on the gym search page map

@Gym
Scenario: Users should see a map with markers for gyms in the search results
    Given I am logged in
    And I am on the gym search page
    When I enter 97361 as a postal code in the search bar
    And I click the postal search button
    Then I should see a map "with" markers for gyms in the search results

@Gym
Scenario: Users should see a map without any markers when no gyms are found
    Given I am logged in
    And I am on the gym search page
    When I enter 00000 as a postal code in the search bar
    And I click the postal search button
    Then I should see a map "without" markers for gyms in the search results

# This scenario needs to be run in a non-headless environment to verify the map interaction.
# I'm not sure why, but it's apparently known that Google Maps' JavaScript API has problems in headless mode.
@GymNotHeadless
Scenario: Users should see a map move to the area they searched for
    Given I am logged in
    And I am on the gym search page
    When I enter 97361 as a postal code in the search bar
    And I click the postal search button
    Then I should see a map centered around the postal code 97361