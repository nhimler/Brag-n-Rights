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
[Scope(Tag = "SCRUM12")]
public sealed class SCRUM12StepDefinitions : IDisposable
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new WebDriverManager.DriverConfigs.Impl.FirefoxConfig());
        var options = new FirefoxOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        _driver = new FirefoxDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");
        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys("Bond_007");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
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
        _driver.Quit();
    }

    [Given("I open the landing page")]
    public void GivenIOpenTheLandingPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts/Index");
    }

    [When("I click on the {string} button")]
    public void WhenIClickOnTheButton(string buttonText)
    {
        var button = _driver.FindElement(By.Id("Workout-Creation-Page-Button"));
        button.Click();
    }

    // Updated the text in the step definition to match the feature file
    [Then("I should see the workout plan creation form")]
    public void ThenIShouldSeeTheWorkoutPlanCreationForm()
    {
        var pageTitle = _driver.FindElement(By.TagName("h2")).Text;
        Assert.IsTrue(pageTitle.Contains("Create a Workout Plan"));
    }

    [Then("I should be able to submit the form with valid data")]
    public void ThenIShouldBeAbleToSubmitTheFormWithValidData()
    {
        var planNameField = _driver.FindElement(By.Id("PlanName"));
        planNameField.Clear();
        planNameField.SendKeys("Test Plan");

        var startDateField = _driver.FindElement(By.Id("StartDate"));
        startDateField.Clear();
        startDateField.SendKeys("2025-10-01");

        var endDateField = _driver.FindElement(By.Id("EndDate"));
        endDateField.Clear();
        endDateField.SendKeys("2025-10-08");

        // Enter Frequency (e.g., workouts per week)
        var frequencyField = _driver.FindElement(By.Id("Frequency"));
        frequencyField.Clear();
        frequencyField.SendKeys("3");

        // Enter Goal
        var goalField = _driver.FindElement(By.Id("Goal"));
        goalField.Clear();
        goalField.SendKeys("Muscle Gain");

        // Select Difficulty Level using the <select> element
        var difficultyDropdown = _driver.FindElement(By.Id("DifficultyLevel"));
        var selectDifficulty = new OpenQA.Selenium.Support.UI.SelectElement(difficultyDropdown);
        selectDifficulty.SelectByText("Intermediate");

        // Submit the form by clicking on the Create Workout Plan button
        var submitButton = _driver.FindElement(By.CssSelector("#Workout-Creation-Form button[type='submit']"));
        submitButton.Click();

        // Wait for navigation or an expected element that signifies success
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => driver.Title != "Create Workout Plan" || driver.Url.Contains("Index"));

        // Simple assertion to ensure we are no longer on the creation page
        Assert.That(_driver.Title, Is.Not.EqualTo("Create a Workout Plan"));
    }
}