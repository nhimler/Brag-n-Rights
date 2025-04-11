Feature: User Dashboard

The user dashboard should display the user's information, such as their full name, email, profile picture, etc.

@mytag
Scenario: User dashboard shows profile picture
	Given I open the user page
	When I log in with "profileinfo" and "Password!1"
	Then I should see the user page
	And I should see a profile picture