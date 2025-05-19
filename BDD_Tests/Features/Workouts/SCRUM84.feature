Feature: Exercise Weight Input Field

@Workouts
Scenario: The user can input the amount of weight they do for each exercise in a workout plan
    Given I am logged in as the user "Bond_007" with the password "Password!1"
    When I navigate to the index page
    And I have a workout plan with the name "test3"
    Then I should be able to input the amount of weight I do for an exercise