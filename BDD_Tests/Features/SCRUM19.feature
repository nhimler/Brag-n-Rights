
Feature: Award Medals with No Fitbit Token

  @SCRUM19
  Scenario: Redirect User to Connect Fitbit Page If No Fitbit Token
    Given I go to the AwardMedals page
    When I login with "notokenuser" and "Password!1"
    Then I should see the "Connect Your Fitbit" page
