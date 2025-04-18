using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM28")]
public sealed class SCRUM28StepDefinitions : IDisposable
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

    [Given(@"I am a user who has logged in")]
    public void GivenIAmAUserWhoHasLoggedIn()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");
        
        Thread.Sleep(3000); // Wait for the page to load
        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
        Thread.Sleep(1000); // Wait for the page to load
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/"));
    }

    [Given(@"I visit the meal plan dashboard")]
    public void GivenIVisitTheMealPlanPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/MealPlan");
    }

    [Then(@"I should see a button that takes me to a meal creation page")]
    public void ThenIShouldSeeAMealCreationButton()
    {
        var createMealBtn = _driver.FindElement(By.Id("create-meal-btn"));
        Assert.That(createMealBtn.Displayed, Is.True);
        createMealBtn.Click();
        Thread.Sleep(1000); // Wait for the page to load
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/CreateMeal/new"));
    }

    [Given(@"I click the button that allows me to design a meal")]
    public void GivenIClickTheDesignMealButton()
    {
        _driver.FindElement(By.Id("create-meal-btn")).Click();
    }

    [Then(@"I should see a form where I can design a meal, including controls for all attributes of a meal")]
    public void ThenIShouldSeeMealDesignForm()
    {
        Assert.That(_driver.FindElement(By.Id("MealName")).Displayed, Is.True);
        Assert.That(_driver.FindElement(By.Id("Description")).Displayed, Is.True);
        Assert.That(_driver.FindElement(By.Id("MealPlanId")).Displayed, Is.True);
        Assert.That(_driver.FindElement(By.Id("MealType")).Displayed, Is.True);
    }
}
