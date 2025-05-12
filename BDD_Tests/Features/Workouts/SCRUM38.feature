Feature: Exercise Search Bar

The user should be able to see the exercise search bar on the exercise search page.

@Workouts
Scenario: Exercise Search Bar is visible to all users
    Given I am logged in as the user "Bond_007" with the password "Password!1"
    And I open the workout index page
    When I click on the "Search for an exercise" button
    Then I should see the exercise search bar