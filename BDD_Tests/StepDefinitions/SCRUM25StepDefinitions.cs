using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
public sealed class SCRUM25StepDefinitions : IDisposable
{
    private IWebDriver _driver;
    
    [BeforeScenario]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        var geolocationPreference = new Dictionary<string, object>
        {
            { "profile.default_content_setting_values.geolocation", 1 },
            { "profile.default_content_setting_values.notifications", 1 }
        };
        foreach (var preference in geolocationPreference)
        {
            options.AddUserProfilePreference(preference.Key, preference.Value);
        }

        _driver = new ChromeDriver(options);

        decimal latitude = 44.854164m;
        decimal longitude = -123.239732m;
        var geolocationScript = $"navigator.geolocation.getCurrentPosition = function(success) {{ success({{ coords: {{ latitude: {latitude}, longitude: {longitude}}} }}); }};";
        ((IJavaScriptExecutor)_driver).ExecuteScript(geolocationScript);

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
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

    [Given("I am logged in")]
    public void GivenIAmLoggedIn()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");

        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");
        _driver.FindElement(By.Id("login-submit")).Click();

    }

    [When("I navigate to the gym search page")]
    public void WhenINavigateToTheGymSearchPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Gym/FindNearbyGyms");
    }

    [When("I click on the search button")]
    public void AndIClickOnTheSearchButton()
    {
        var searchButton = _driver.FindElement(By.Id("set-location-button"));
        searchButton.Click();
    }

    [Then("I should see a list of gyms")]
    public void ThenIShouldSeeAListOfGyms()
    {
        var gymList = _driver.FindElement(By.Id("nearby-gym-search-list"));
        var gymListItems = gymList.FindElements(By.ClassName("card"));
        Assert.That(gymListItems.Count, Is.GreaterThan(0), "No gyms found in the list.");
    }
}