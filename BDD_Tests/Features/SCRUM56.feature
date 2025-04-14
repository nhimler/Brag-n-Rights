Feature: SCRUM56

  @SCRUM56
  Scenario: Redirect User to Connect Fitbit Page If No Fitbit Token
    Given I go to the StepCompetition page
    When I login as step competition user with "notokenuser" and "Password!1"
    Then I should see "Connect Your Fitbit" page

  @SCRUM56
  Scenario: Create Step Competition Without Inviting Users
    Given I go to the StepCompetition page
    When I login as step competition user with "TokenUser" and "TokenUser!123"
    And I click the "Create New Competition" button
    And I submit the competition form without inviting users
    Then I should see the new competition in the competition list