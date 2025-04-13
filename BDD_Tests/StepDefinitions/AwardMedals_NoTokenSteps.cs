using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SeleniumExtras.WaitHelpers;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    public sealed class AwardMedals_NoTokenSteps : IDisposable
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
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }

        [AfterScenario]
        public void Teardown()
        {
            // Cleanup handled in Dispose
        }

        [Given("I go to the AwardMedals page")]
        public void GivenIGoToTheAwardMedalsPage()
        {
            _driver.Navigate().GoToUrl("http://localhost:5075/AwardMedal/AwardMedals");
        }

        [When("I login with {string} and {string}")]
        public void WhenILogInWithUsernameAndPassword(string username, string password)
        {
            var usernameField = _driver.FindElement(By.Id("login-username"));
            var passwordField = _driver.FindElement(By.Id("login-password"));
            var loginButton = _driver.FindElement(By.Id("login-submit"));

            usernameField.SendKeys(username);
            passwordField.SendKeys(password);
            loginButton.Click();

           
        }

        [Then(@"I should see the ""Connect Your Fitbit"" page")]
        public void ThenIShouldSeeConnectYourFitbitPage()
        {
            var header = _driver.FindElement(By.TagName("h2")).Text;
            Assert.That(header, Is.EqualTo("Connect Your Fitbit"));
        }
    }
}
