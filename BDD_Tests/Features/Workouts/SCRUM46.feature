Feature: Login dropdown appears

When a user is not logged in, the login dropdown should appear on the index page.

@Workouts
Scenario: Login dropdown displays
    Given I am not logged in
    And I open the workout index page
    Then I should see a login dropdown
    And I should see the text "For full access to workout plans, please login or register."