using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
public sealed class AccountStepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
    }

    [Given("I am on the login page")]
    [When("I am on the login page")]
    [Then("I am on the login page")]
    public void IAmOnTheLoginPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");
    }

    [Given("I am on the registration page")]
    [When("I am on the registration page")]
    [Then("I am on the registration page")]
    public void IAmOnTheRegistrationPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");
    }

    [Given("I login with username {string} and password {string}")]
    [When("I login with username {string} and password {string}")]
    [Then("I login with username {string} and password {string}")]
    public void ILoginWithCredentials(string username, string password)
    {
        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys(username);
        passwordField.SendKeys(password);

        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [Given("I submit a registration form with random valid data")]
    [When("I submit a registration form with random valid data")]
    [Then("I submit a registration form with random valid data")]
    public void ISubmitARegistrationFormWithRandomValidData()
    {
        var firstNameField = _driver.FindElement(By.Id("first-name-register"));
        var lastNameField = _driver.FindElement(By.Id("last-name-register"));
        var usernameField = _driver.FindElement(By.Id("username-register"));
        var emailField = _driver.FindElement(By.Id("email-register"));
        var passwordField = _driver.FindElement(By.Id("password-register"));
        var confirmPasswordField = _driver.FindElement(By.Id("confirm-password-register"));

        string user = "testUser" + DateTime.Now.ToString("yyyyMMddHHmmss");

        firstNameField.SendKeys("John");
        lastNameField.SendKeys("Doe");
        usernameField.SendKeys(user);
        emailField.SendKeys($"{user}@test.com");
        passwordField.SendKeys("Password1!");
        confirmPasswordField.SendKeys("Password1!");

        _driver.FindElement(By.Id("registerSubmit")).Click();
    }

    [When("I click on the search button")]
    public void WhenIClickSearchButton()
    {
        var button = _driver.FindElement(By.Id("exerciseSearchButtonAddon"));
        button.Click();
    }

    [Then("I should see a list of exercises")]
    public void ThenIShouldSeeAListOfExercises()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        bool resultsLoaded = wait.Until(driver =>
        {
            var searchResultsDiv = driver.FindElement(By.Id("exerciseSearchResults"));
            return searchResultsDiv.FindElements(By.XPath("./*")).Count > 0;
        });
        Assert.IsTrue(resultsLoaded, "Exercise search results did not load.");
    }
}