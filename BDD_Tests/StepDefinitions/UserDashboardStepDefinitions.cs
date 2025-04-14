using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
public sealed class UserDashboardStepDefinitions : IDisposable
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

    [Given("I open the user page")]
    public void GivenIOpenTheUserPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/UserPage");
    }

    [When("I log in with {string} and {string}")]
    public void WhenILogInWithUsernameAndPassword(string username, string password)
    {
        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));
        var loginButton = _driver.FindElement(By.Id("login-submit"));

        usernameField.SendKeys(username);
        passwordField.SendKeys(password);
        loginButton.Click();
    }

    [Then(@"I should see ""(.*)""")]
    public void ThenIShouldSee(string text)
    {
        Assert.IsTrue(_driver.PageSource.Contains(text));
    }

    [Then(@"I should see the user page")]
    public void ThenIShouldSeeTheUserPage()
    {
        Assert.IsTrue(_driver.PageSource.Contains("User Page"));
    }

    [Then(@"I should see a profile picture")]
    public void AndIShouldSeeAProfilePicture()
    {
        var profilePicture = _driver.FindElement(By.Id("profile-pic"));
        Assert.IsTrue(profilePicture.Displayed);
    }
}
