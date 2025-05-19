Feature: Save all changes exercise button

@Workouts
Scenario: When a user makes multiple changes to the exercises within a workout plan, they should be able to save all changes at once by clicking the "Save All Changes" button.
    Given I am logged in as the user "Bond_007" with the password "Password!1"
    When I navigate to the index page
    And I have a workout plan with the name "test3"
    When I make changes to the exercises in the workout plan
    And I click the "Save All Changes" button
    Then I should open the workout plan back up and see the changes I made
