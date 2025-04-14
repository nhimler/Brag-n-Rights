Feature: SCRUM42

As a user, I want to be able to view meals after creating them

@SCRUM42
Scenario: Navigation to details page
    Given I am a user who has logged in
    And I have created a meal plan
    And I have created a meal
    And I visit the meal plan dashboard
    Then I should be able to click on a meal and be taken to a page where I can view its details- Title, Type, Description, Meal Plan

@SCRUM42
Scenario: Navigation from details page to meal plan home page
    Given I am a user who has logged in
    And I have created a meal plan
    And I have created a meal
    And I visit the details page of a meal
    Then I should be able to click a button to return to the meal plan home page

@SCRUM42
Scenario: Navigation from meal plan details page to meal details page
    Given I am a user who has logged in
    And I have created a meal plan
    And I have created a meal
    And I visit the details page of a meal plan
    Then I should be able to click on a meal to go to its details page

@SCRUM42
Scenario: Navigation from meal details page to meal plan details page
    Given I am a user who has logged in
    And I have created a meal plan
    And I have created a meal
    And I visit the details page of a meal
    Then I should be able to click the meals meal plan and go to its details page