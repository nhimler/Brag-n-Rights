Feature: Workout Plan Creation

The user should be able to create a workout Plan

# Note: The "When I click on the "button" step is case sensitive
# and should match the button text exactly.
@Workouts
Scenario: Create a workout Plan
    Given I am logged in as the user "Bond_007" with the password "Password!1"
    And I open the workout index page
    When I click on the "Create a workout plan" button
    Then I should see the workout plan creation form
    And I should be able to submit the form with valid data