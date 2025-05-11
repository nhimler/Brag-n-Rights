Feature: Sets and Reps in a Workout Plans

@Workouts
Scenario: When a user views the exercises within a workout plan, they should be able to set the sets and reps for each exercises
    Given I am a user who has logged in
    And I have a workout plan with exercises
    When I click on the View Exercises button
    Then I should see the list of exercises in the workout plan
    Then I should be able to set the sets and reps for the exercises