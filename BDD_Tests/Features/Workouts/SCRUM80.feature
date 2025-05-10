Feature: Complete Workout Plan

@SCRUM80
Scenario: When a user is done with a workout plan, they should be able to complete it on button press
    Given I am a user who has logged in
    And I have a workout plan
    When I click on the Complete Workout Plan button
    Then I should not see the workout plan on the page