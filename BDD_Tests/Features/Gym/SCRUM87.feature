Feature: SCRUM87

This will cover the functionality of deleting a gym from the user's bookmarks

@Gym
Scenario: User deletes existing bookmark from the gym search page
    Given I am logged in as "noBookmarks" with password "Password!1"
    And I am on the gym search page
    When I enter 97361 as a postal code in the search bar
    And I click the postal search button
    Then I should see a list of gyms appear
    And I should see a bookmark button next to a gym
    When I click on a bookmarked gym
    Then The gym should no longer be bookmarked