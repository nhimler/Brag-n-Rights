using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using OpenQA.Selenium.Support.UI;


namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM19")]
public sealed class SCRUM19StepDefinitions : IDisposable
{
    private IWebDriver _driver;
    
        [BeforeScenario]    public void Setup()
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

        [Then(@"I should see the ""(.*)"" page")]
        public void ThenIShouldSeePageWithHeader(string expectedHeader)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            wait.Until(driver =>
            {
                try
                {
                    var h1OrH2 = driver.FindElements(By.TagName("h1")).Concat(_driver.FindElements(By.TagName("h2")));
                    return h1OrH2.Any(e => e.Text.Contains(expectedHeader));
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            var headers = _driver.FindElements(By.TagName("h1")).Concat(_driver.FindElements(By.TagName("h2")));
            var actualHeader = headers.FirstOrDefault(e => e.Text.Contains(expectedHeader))?.Text;

            Assert.That(actualHeader, Is.EqualTo(expectedHeader));
        }

    }