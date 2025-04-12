Feature: Exercise Search Bar

The user should be able to see the exercise search bar on the exercise search page.

@SCRUM13
Scenario: Exercise Search Bar is visible to all users
    Given I open the index page
    When I click on the "Search for an exercise" button
    Then I should see the exercise search bar