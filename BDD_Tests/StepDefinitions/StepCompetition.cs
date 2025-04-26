using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace BDD_Tests.StepDefinitions
{
    [Binding]
    [Scope(Tag = "SCRUM19")]
    [Scope(Tag = "SCRUM56")]
    [Scope(Tag = "SCRUM68")]
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
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(8);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
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
            // wait until *either* the <title> matches *or* an <h1>/<h2> matches
            _wait.Until(driver =>
            {
                if (driver.Title.Equals(expectedHeader, StringComparison.OrdinalIgnoreCase))
                    return true;

                var headers = driver
                    .FindElements(By.TagName("h1"))
                    .Concat(driver.FindElements(By.TagName("h2")));

                return headers.Any(h => 
                    h.Text.Equals(expectedHeader, StringComparison.OrdinalIgnoreCase));
            });

            // if it was the <title>, we’re done
            if (_driver.Title.Equals(expectedHeader, StringComparison.OrdinalIgnoreCase))
                return;

            // otherwise assert the header text
            var actualHeader = _driver
                .FindElements(By.TagName("h1"))
                .Concat(_driver.FindElements(By.TagName("h2")))
                .First(h => h.Text.Equals(expectedHeader, StringComparison.OrdinalIgnoreCase))
                .Text;

            Assert.That(actualHeader, Is.EqualTo(expectedHeader),
                        $"Expected page title or header '{expectedHeader}' was not found. " +
                        $"Actual title: '{_driver.Title}'; actual header: '{actualHeader}'.");
        }


        [When(@"I click the ""(.*)"" button")]
        public void WhenIClickTheButton(string buttonText)
            => _driver.FindElement(By.XPath($"//button[text()='{buttonText}']")).Click();
    }
}
