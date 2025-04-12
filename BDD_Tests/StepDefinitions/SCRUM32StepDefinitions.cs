using System.Diagnostics;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
public sealed class SCRUM32StepDefinitions : IDisposable
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

        // Logging in to a valid account. If the database is ever reset, this will need to be updated.
        // The seed script that starts up at launch should be able to take care of this, but that is
        // not always guaranteed to be the case (ie: the database already has users in it).
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");

        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
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

    [Given("I am on my change info page")]
    public void GivenIAmOnMyChangeInfoPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/UserPage/ChangeInfo");
    }

    [When("I set my fitness level to {string}")]
    public void WhenISetMyFitnessLevelToFitnesslevel(string fitnessLevel)
    {
        var fitnessLevelField = _driver.FindElement(By.Id("FitnessLevel-update"));
        var fitnessLevelSelect = new SelectElement(fitnessLevelField);
        Debug.WriteLine("Fitness level set to: " + fitnessLevelSelect.SelectedOption.Text);

        fitnessLevelSelect.SelectByText(fitnessLevel);
    }

    [When("I set my fitness goal to {string}")]
    public void AndISetMyFitnessGoalToFitnessgoal(string fitnessGoal)
    {
        var fitnessGoalField = _driver.FindElement(By.Id("Fitnessgoals-update"));
        fitnessGoalField.Clear();
        fitnessGoalField.SendKeys(fitnessGoal);
        Debug.WriteLine("Fitness goal set to: " + fitnessGoalField.GetAttribute("value"));
    }

    [When("I click on the update settings button")]
    public void AndIClickOnTheUpdateSettingsButton()
    {
        var updateButton = _driver.FindElement(By.Id("update-settings-button"));

        Debug.WriteLine("Button enabled: " + updateButton.Enabled);
        Debug.WriteLine("Button displayed: " + updateButton.Displayed);
        var blockingElement = ((IJavaScriptExecutor)_driver).ExecuteScript(
            "return document.elementFromPoint(arguments[0], arguments[1]);",
            339, 779); // Replace with the button's coordinates
        Debug.WriteLine("Blocking element: " + blockingElement);

        // Not a great way to do this, but it works for now. The button is not clickable because of some
        // blocking element that I cannot find. 
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", updateButton);
    }

    [Then("I should see my dashboard page")]
    public void ThenIShouldSeeMyDashboardPage()
    {
        Debug.WriteLine($"Title: {_driver.Title}");
        Assert.That(_driver.Title, Is.EqualTo("User Page - GymBro_App"),"The title does not contain 'Dashboard'.");
    }

    [Then("I should see my fitness level as {string}")]
    public void AndIShouldSeeMyFitnessLevelAsFitnesslevel(string fitnessLevel)
    {
        var userFitnessLevel = _driver.FindElement(By.Id("dashboard-user-fitness-level"));
        string expectedFitnessLevel = $"Fitness Level: {fitnessLevel}";
        Assert.That(userFitnessLevel.Text, Is.EqualTo(expectedFitnessLevel), "The fitness level is not set correctly.");
    }

    [Then("I should see my fitness goal as {string}")]
    public void AndIShouldSeeMyFitnessGoalAsFitnessgoal(string fitnessGoal)
    {
        var userFitnessGoal = _driver.FindElement(By.Id("dashboard-user-goals"));
        string expectedFitnessGoal = $"Fitness Goals: {fitnessGoal}";
        Assert.That(userFitnessGoal.Text, Is.EqualTo(expectedFitnessGoal), "The fitness goal is not set correctly.");
    }
}