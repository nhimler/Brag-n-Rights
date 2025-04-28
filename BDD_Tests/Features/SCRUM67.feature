Feature: Workout Plans on Landing page

If a user has workout plans created, they should be able to see them on the landing page.

@SCRUM67
Scenario: User has workout plans created
    Given I am a user who has logged in
    When I navigate to the landing page
    Then I should see my workout plans

