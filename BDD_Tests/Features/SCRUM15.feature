Feature: SCRUM15

As a user, I want to search for food items and view their nutritional information.

@SCRUM15
Scenario: Food search
    Given I am a user who has logged in
    And I have created a meal plan
    When I go to create a meal
    And I type "chicken" in the search bar
    And I click the search button
    Then I should see a list of food items related to chicken

@SCRUM15
Scenario: Navigation from details page to meal plan home page
    Given I am a user who has logged in
    When I go to create a meal
    And I search for "chicken"
    And I select a food item
    And I save the meal
    Then the item should be added and visible on the dashboard