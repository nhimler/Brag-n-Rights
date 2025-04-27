
Feature: SCRUM19

  @SCRUM19
  Scenario: Redirect User to Connect Fitbit Page If No Fitbit Token
    Given I go to the AwardMedals page
    When I login with "notokenuser" and "Password!1"
    Then I should see "Connect Your Fitbit" page

  @SCRUM19
  Scenario: Show awarded medals if user has valid Fitbit token
    Given I go to the AwardMedals page
    When I login with "TokenUser" and "TokenUser!123"
    Then I should see "Awarded Medals" page
