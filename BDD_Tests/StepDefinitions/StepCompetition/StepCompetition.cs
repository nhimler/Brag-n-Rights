using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SeleniumExtras.WaitHelpers;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    [Scope(Tag = "SCRUM19")]
    [Scope(Tag = "SCRUM56")]
    [Scope(Tag = "SCRUM68")]
    [Scope(Tag = "SCRUM72")]
    [Scope(Tag = "SCRUM73")]
    [Scope(Tag = "SCRUM81")]
    public sealed partial class SCRUMStepDefinitions
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [BeforeScenario]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }


        [AfterScenario]
        public void Teardown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        [Given(@"I go to the (.*) page")]
        public void GivenIGoToPage(string pageName)
        {
            var urls = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "StepCompetition", "http://localhost:5075/StepCompetition" },
                { "AwardMedals",     "http://localhost:5075/AwardMedal/AwardMedals" }
            };
            if (!urls.TryGetValue(pageName, out var url))
                throw new ArgumentException($"No URL mapping found for page '{pageName}'");

            _driver.Navigate().GoToUrl(url);
        }

        
        [When("I login with {string} and {string}")]
        public void WhenILogInWithUsernameAndPassword(string username, string password)
        {
            _driver.FindElement(By.Id("login-username")).SendKeys(username);
            _driver.FindElement(By.Id("login-password")).SendKeys(password);
            _driver.FindElement(By.Id("login-submit")).Click();
        }

        [Then(@"I should see ""(.*)"" page")]
        public void ThenIShouldSeePageWithHeader(string expectedHeader)
        {
            // Increase the timeout if needed:
            var localWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            // Wait until an H1 element that contains the expected header text is displayed.
            var headerText = localWait.Until(driver =>
            {
                try
                {
                    var headerElement = driver.FindElement(By.XPath($"//h1[contains(., '{expectedHeader}')]"));
                    return headerElement.Displayed ? headerElement.Text : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });

            // Verify that we found the expected header.
            Assert.IsNotNull(headerText, $"Header containing '{expectedHeader}' was not found within the timeout.");
            Assert.That(headerText, Does.Contain(expectedHeader), $"Expected header to contain '{expectedHeader}', but found '{headerText}'.");
        }

        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string expectedText)
        {
            // wait up to 10 seconds for the text to appear in the DOM
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath($"//*[contains(normalize-space(.), '{expectedText}')]")));

            // verify the text is actually on the page
            var pageBody = _driver.FindElement(By.TagName("body")).Text;
            Assert.That(pageBody, Does.Contain(expectedText),
                        $"Expected to find text '{expectedText}' on the page.");
        }

        [When(@"I click the ""(.*)"" button")]
        [Then(@"I click the ""(.*)"" button")]
        public void WhenIClickTheButton(string buttonText)
        {
            var locator = By.XPath($"//button[normalize-space(text()) = \"{buttonText}\"]");

            var clickableBtn = _wait.Until(ExpectedConditions.ElementToBeClickable(locator));

            ((IJavaScriptExecutor)_driver)
                .ExecuteScript("arguments[0].scrollIntoView({block:'center'})", clickableBtn);

            try
            {
                clickableBtn.Click();
            }
            catch (ElementClickInterceptedException)
            {
                System.Threading.Thread.Sleep(200);
                ((IJavaScriptExecutor)_driver)
                    .ExecuteScript("arguments[0].click();", clickableBtn);
            }
        }
    }
}
