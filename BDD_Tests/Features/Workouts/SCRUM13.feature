Feature: Exercise API Return

The user should be able to search for an exercise in the search bar on the exercise search page and have a value be returned.

@Workouts
Scenario: Exercise Search API returns results
    Given I open the workout index page
    When I click on the "Search for an exercise" button
    And I enter "sit-up" in the search bar
    And I click on the search button
    Then I should see a list of exercises