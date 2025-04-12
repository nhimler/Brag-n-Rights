Feature: SCRUM11

This will cover the user's ability to create and login to an account

@SCRUM11
Scenario: User can access their user page when they are logged in
	Given I navigate to the userpage
    And I log in with valid credentials
    Then I should be redirected to the user page