Feature: Bootstrap Alert

When the user enters an invalid search, the bootstrap alert should be displayed

@Workouts
Scenario: Search by body part
    Given I open the workout index page
    When I click on the "Search for an exercise" button
    And I enter an invalid query in the search bar
    Then I should see a bootstrap alert