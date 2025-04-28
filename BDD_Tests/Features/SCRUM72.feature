Feature: SCRUM72

  @SCRUM72
  Scenario: User wants view past medals
    Given I go to the AwardMedals page
    When I login with "TokenUser" and "TokenUser!123"
    Then I should see "Awarded Medals" page
    Then  I click the "View Past Medals" button

    
