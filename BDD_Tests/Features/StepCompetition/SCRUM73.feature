Feature: SCRUM73

  @SCRUM73
  Scenario: User wants view past Competitions
    Given I go to the StepCompetition page
    When I login with "TokenUser" and "TokenUser!123"
    Then I should see "Step Competition" page
    Then  I click the "View Past Competitions" button

    
