Feature: SCRUM32

This will cover the gym search functionality

@SCRUM32
Scenario: User updates their information and confirms changes
	Given I am on my change info page
    When I set my fitness level to "Beginner"
    And I set my fitness goal to "Lose 15 pounds"
    And I click on the update settings button
    Then I should see my dashboard page
    And I should see my fitness level as "Beginner"
    And I should see my fitness goal as "Lose 15 pounds"