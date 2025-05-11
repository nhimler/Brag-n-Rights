Feature: Complete Workout Plan

@Workouts
Scenario: When a user is done with a workout plan, they should be able to complete it on button press
    Given I am logged in as the user "SCRUM80" with the password "Password!1"
    Then I should see my workout plans
    When I click on the Complete Workout Plan button
    Then I should not see the workout plan on the page