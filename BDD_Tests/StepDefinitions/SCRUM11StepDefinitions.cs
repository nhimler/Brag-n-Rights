using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM11")]
public sealed class SCRUM11StepDefinitions : IDisposable
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

    [Given("I navigate to the userpage")]
    public void GivenINavigateToTheUserpage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/UserPage");
    }

    [Given("I log in with valid credentials")]
    public void AndILoginWithValidCredentials()
    {
        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [Then(@"I should be redirected to the user page")]
    public void ThenIShouldBeRedirectedToTheUserPage()
    {
        var pageTitle = _driver.Title;
        Assert.That(pageTitle, Is.EqualTo("User Page - GymBro_App"));
    }
}