Feature: SCRUM10

This will cover the user's ability to create and login to an account

@SCRUM10
Scenario: User registers an account with valid data
	Given I open the application
    And I am not logged in
    When I click on the "register" link
    And I submit a registration form with valid data
    Then I should see an email verification message

@SCRUM10
Scenario: User logs in with valid credentials
    Given I open the application
    And I am not logged in
    When I click on the "login" link
    And I login with "testingLogin" and "Password!1"
    Then I should be redirected back to the home page
    And I should see "testingLogin" displayed on the page

@SCRUM10
Scenario: User registers an account with optional information
    Given I open the application
    And I am not logged in
    When I click on the "register" link
    And I submit a registration form with optional information
    Then I should see an email verification message