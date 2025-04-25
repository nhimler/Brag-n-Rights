using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI; // Added this line
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM68")]
public sealed class SCRUM68StepDefinitions : IDisposable
{
    private IWebDriver _driver;
    
        [BeforeScenario]    
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void Dispose()
        {
            if(_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }

        [AfterScenario]
        public void Teardown()
        {
            _driver.Quit();
        }

        [Given("I go to the StepCompetition page")]
        public void GivenIGoToTheStepCompetitionPage()
        {
            _driver.Navigate().GoToUrl("http://localhost:5075/StepCompetition");
        }

        [When("I login as step competition user with {string} and {string}")]
        public void WhenILogInWithUsernameAndPassword(string username, string password)
        {
            var usernameField = _driver.FindElement(By.Id("login-username"));
            var passwordField = _driver.FindElement(By.Id("login-password"));
            var loginButton = _driver.FindElement(By.Id("login-submit"));

            usernameField.SendKeys(username);
            passwordField.SendKeys(password);
            loginButton.Click();           
        }

        [Then(@"I should see ""(.*)"" page")]
        public void ThenIShouldSeePageWithHeader(string expectedHeader)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Wait until the header is present and contains the expected text
            wait.Until(driver =>
            {
                try
                {
                    // Re-locate the elements to avoid stale references
                    var headers = driver.FindElements(By.TagName("h1"))
                        .Concat(driver.FindElements(By.TagName("h2")));
                    return headers.Any(e => e.Text.Contains(expectedHeader));
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            // Re-locate the headers to assert the actual text
            var headers = _driver.FindElements(By.TagName("h1"))
                .Concat(_driver.FindElements(By.TagName("h2")));
            var actualHeader = headers.FirstOrDefault(e => e.Text.Contains(expectedHeader))?.Text;

            Assert.That(actualHeader, Is.EqualTo(expectedHeader), $"Expected header '{expectedHeader}' was not found.");
        }

        [Then(@"I should see a button to create step competition")]
        public void ThenIShouldSeeAButtonToCreateStepCompetition()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Wait for the button to be visible
            var createButton = wait.Until(driver => driver.FindElement(By.Id("openCompetitionFormBtn")));

            // Assert that the button is displayed
            Assert.That(createButton.Displayed, Is.True, "The 'Create New Competition' button is not visible.");
        }

        [When(@"I click the ""(.*)"" button")]
        public void WhenIClickTheButton(string buttonText)
        {
            var button = _driver.FindElement(By.XPath($"//button[text()='{buttonText}']"));
            button.Click();
        }

        [When(@"I submit the competition form without inviting users")]
        public void WhenISubmitTheCompetitionFormWithoutInvitingUsers()
        {
            var submitButton = _driver.FindElement(By.CssSelector("#competitionForm button[type='submit']"));
            submitButton.Click();
        }

        [Then(@"I should see the new competition in the competition list")]
        public void ThenIShouldSeeTheNewCompetitionInTheCompetitionList()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            // Wait for the competition list to update
            wait.Until(driver =>
            {
                var competitionList = driver.FindElement(By.Id("competitionListContainer"));
                return competitionList.Text.Contains("7 Day Step Competition🔥");
            });

            var competitionList = _driver.FindElement(By.Id("competitionListContainer"));
            Assert.That(competitionList.Text.Contains("7 Day Step Competition🔥"), Is.True, "The new competition was not found in the competition list.");
        }

        [Then(@"I click the Leave Competition button")]
        public void WhenIClickTheLeaveCompetitionButton()
        {
            // Find the "Leave Competition" button by its text
            var leaveButton = _driver.FindElement(By.XPath("//button[text()='Leave Competition']"));
            leaveButton.Click();
        }
        [Then(@"I should no longer see The competition")]
        public void ThenIShouldNoLongerSeeTheCompetition()
        {
            // Find the container where the competition list is rendered
            var competitionList = _driver.FindElement(By.Id("competitionListContainer"));
            
            
            // Wait until the competition element is removed or doesn't contain the expected competition anymore
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            wait.Until(driver => !competitionList.Text.Contains("7 Day Step Competition🔥"));

            // Assert that the competition is no longer visible in the competition list
            Assert.That(competitionList.Text.Contains("7 Day Step Competition🔥"), Is.False, "The competition is still visible in the competition list.");
        }

    }