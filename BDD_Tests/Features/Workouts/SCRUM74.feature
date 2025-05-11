Feature: Sets and Reps in a Workout Plans

@Workouts
Scenario: When a user views the exercises within a workout plan, they should be able to set the sets and reps for each exercises
    Given I am logged in as the user "Bond_007" with the password "Password!1"
    When I navigate to the index page
    When I click on the View Exercises button
    Then I should see the list of exercises in the workout plan
    Then I should be able to set the sets and reps for the exercises