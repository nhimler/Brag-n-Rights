using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM67")]
public sealed class SCRUM67StepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    [Given("I am a user who has logged in")]
    public void GivenIAmAUserWhoHasLoggedIn()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/UserPage");
        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));
        
        usernameField.SendKeys("Bond_007");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [When("I navigate to the landing page")]
    public void WhenINavigateToTheLandingPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts/Index");
    }

    [Then("I should see my workout plans")]
    public void ThenIshouldSeeMyWorkoutPlans()
    {
        // Wait for the page to load and display workout plans
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.Id("workoutPlansContainer")));

        // Check if workout plans are displayed
        var workoutPlansContainer = _driver.FindElement(By.Id("workoutPlansContainer"));
        Assert.IsTrue(workoutPlansContainer.Displayed, "Workout plans are not displayed on the landing page.");
    }
}