Feature: User Dashboard

This is an example of a test for the group. It is not meant to be taken as a real test for Sprint 4.

@example
Scenario: User dashboard shows profile picture
	Given I open the user page
	When I log in with "profileinfo" and "Password!1"
	Then I should see the user page
	And I should see a profile picture