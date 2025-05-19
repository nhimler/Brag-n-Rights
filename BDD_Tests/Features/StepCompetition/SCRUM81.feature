Feature: SCRUM81

  @SCRUM81
  Scenario: User wants to view Recently Ended Competitions
    Given I go to the StepCompetition page
    When I login with "TokenUser" and "TokenUser!123"
    Then I should see "Recently Ended Competitions"