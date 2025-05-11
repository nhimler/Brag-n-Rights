Feature: Workout Plans on Landing page

If a user has workout plans created, they should be able to see them on the landing page.

@Workouts
Scenario: User has workout plans created
    Given I am logged in as the user "Bond_007" with the password "Password!1"
    And I open the workout index page
    Then I should see my workout plans