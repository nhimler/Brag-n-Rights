Feature: SCRUM90

This covers showcasing the guest experience of the homepage and the navbar

@Home
Scenario: Guest user sees the feature descriptions on the home page
	Given I am on the home page
    And I am not logged in
    Then I should see descriptions of the features available on the home page

@Home
Scenario: Guest user should not see the navbar links that require login
    Given I am on the home page
    And I am not logged in
    Then I should not see the "FitBit Features" link in the navbar
    And I should not see the "Meal Plan" link in the navbar
    And I should not see the "Exercise" link in the navbar

@Home
Scenario: Guest user should see the navbar links that do not require login
    Given I am on the home page
    And I am not logged in
    Then I should see the "Exercise Search" link in the navbar
    Then I should see the "Find A Gym" link in the navbar
    Then I should see the "Login" link in the navbar
    Then I should see the "Register" link in the navbar