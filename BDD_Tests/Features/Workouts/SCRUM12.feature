Feature: Workout Plan Creation

The user should be able to create a workout Plan

@SCRUM12
Scenario: Create a workout Plan
    Given I open the landing page
    When I click on the "Create a workout plan" button
    Then I should see the workout plan creation form
    And I should be able to submit the form with valid data