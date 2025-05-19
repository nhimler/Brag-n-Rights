Feature: SCRUM94

As a user, I want to be able to see if my targets are met

@MealPlan
Scenario: Graph on details page
    Given I am a user who has logged in
    And I have created a meal plan
    And I visit the meal plan dashboard
    And I visit the details page of a meal plan
    Then I should see a graph that shows my targets