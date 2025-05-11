Feature: Body Part Filter in Exercise Search

@SCRUM76
Scenario: When a user navigates to the exercise search page, they should be able to choose what body part they want to search body
    Given I am on the exercise search page
    When I select a body part from the body part Filter
    Then I should see a list of exercises that target the selected body part