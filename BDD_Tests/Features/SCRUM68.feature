Feature: SCRUM68

  @SCRUM68
  Scenario: User wants to leave the competition
    Given I go to the StepCompetition page
    When I login as step competition user with "LeaveComp" and "LeaveComp!123"
    And I click the "Create New Competition" button
    And I submit the competition form without inviting users
    Then I should see the new competition in the competition list
    And I click the Leave Competition button
    Then I should no longer see The competition 
    