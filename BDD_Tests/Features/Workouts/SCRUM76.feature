Feature: Body Part Filter in Exercise Search

@Workouts
Scenario: When a user navigates to the exercise search page, they should be able to choose what body part they want to search body
    Given I am on the exercise search page
    Then I should see buttons for each body part
    When I click on a body part button
    Then I should see a list of exercises that target the selected body part