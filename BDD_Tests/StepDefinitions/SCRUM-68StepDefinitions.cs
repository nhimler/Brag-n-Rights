using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace BDD_Tests.StepDefinitions
{
    [Scope(Tag = "SCRUM68")]
    [Scope(Tag = "SCRUM56")]
    public sealed partial class SCRUMStepDefinitions
    {
        [When(@"I submit the competition form without inviting users")]
        public void WhenISubmitTheCompetitionFormWithoutInvitingUsers()
            => _driver.FindElement(By.CssSelector("#competitionForm button[type='submit']")).Click();

        [Then(@"I should see the new competition in the competition list")]
        public void ThenIShouldSeeTheNewCompetitionInTheCompetitionList()
        {
            _wait.Until(driver =>
            {
                var competitionList = driver.FindElement(By.Id("competitionListContainer"));
                return competitionList.Text.Contains("7 Day Step Competition🔥");
            });
            var competitionList = _driver.FindElement(By.Id("competitionListContainer"));
            Assert.That(competitionList.Text.Contains("7 Day Step Competition🔥"),
                        "The new competition was not found in the competition list.");
        }

        [Then(@"I should no longer see The competition")]
        public void ThenIShouldNoLongerSeeTheCompetition()
        {
            var longWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            longWait.Until(driver =>
            {
                var competitionList = driver.FindElement(By.Id("competitionListContainer"));
                return !competitionList.Text.Contains("7 Day Step Competition🔥");
            });
            var competitionList = _driver.FindElement(By.Id("competitionListContainer"));
            Assert.That(!competitionList.Text.Contains("7 Day Step Competition🔥"),
                        "The competition is still visible in the competition list.");
        }
    }
}
