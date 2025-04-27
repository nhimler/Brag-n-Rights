Feature: SCRUM71

As a user, I want my meal plans to be archived if they are no longer relevant

@SCRUM71
Scenario: Old meal plans are archived
    Given I am a user who has logged in
    And I have created an out of date meal plan
    And I visit the meal plan dashboard
    Then That meal plan should not appear and I should see a message communicating that "Weve archived some old meal plans for you"
    And I should be able to see a link to the meal plan archive
    When I click on the link to the meal plan archive
    Then I should be taken to the meal plan archive page
    And I should see my archived meal plans