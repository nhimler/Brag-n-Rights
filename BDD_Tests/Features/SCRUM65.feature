Feature: Search by body part

The user should be able to choose to search by body part

@SCRUM65
Scenario: Search by body part
    Given I open the index page
    When I click on the search for an exercise button
    And I select Body Part from the exercise search type dropdown
    And I enter "back" in the search bar
    And I click on the search button
    Then I should see a list of exercises