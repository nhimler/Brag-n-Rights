using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System.Text.RegularExpressions;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM43")]
public sealed class SCRUM43StepDefinitions : IDisposable
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
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
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
        IWebElement usernameField = null;
        IWebElement passwordField = null;
        _wait.Until(driver => {
            usernameField = _driver.FindElement(By.Id("login-username"));
            passwordField = _driver.FindElement(By.Id("login-password"));
            
            return usernameField != null && passwordField != null;
        });
        
        //Thread.Sleep(3000); // Wait for the page to load

        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
        _wait.Until(driver => _driver.Url != "http://localhost:5075/Identity/Account/Login");
        //Thread.Sleep(1000); // Wait for the page to load
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/"));
    }

    [Given(@"I have created a meal plan")]
    public void GivenIHaveCreatedAMealPlan()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMealPlan/new");
        _wait.Until(driver => driver.Url == "http://localhost:5075/CreateMealPlan/new");
        // Thread.Sleep(3000); // Wait for the page to load
        // Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/CreateMealPlan/new"));
        _driver.FindElement(By.Id("PlanName")).SendKeys("Test Meal Plan");
        _driver.FindElement(By.Id("create-btn")).Click();

        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
        // Thread.Sleep(1000); // Wait for the page to load]
        // Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan"));
    }


    [Given(@"I visit the details page of a meal plan")]
    public void GivenIVisitTheDetailsPageOfAMealPlan()
    {
        _driver.FindElement(By.CssSelector(".title-link h3")).Click();
        
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/MealPlanDetails/\\d+"));
    }

    [Then(@"I should be able to click an “Edit” button and be taken to a page where I can edit my meal plan")]
    public void ThenICanClickEditAndNavigateToEditPage()
    {
        var editButton = _driver.FindElement(By.Id("edit-mealplan-btn"));
        Assert.That(editButton.Displayed, Is.True);
        editButton.Click();
        _wait.Until(driver => Regex.Match(driver.Url, "http://localhost:5075/CreateMealPlan/\\d+").Success);
        // Thread.Sleep(1000); // Wait for the page to load
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/CreateMealPlan/\\d+"));
    }

    [Given(@"I visit the Edit page for a meal plan")]
    public void GivenIVisitTheEditPageForAMeal()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/MealPlan");
        // Thread.Sleep(1000);
        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
        _driver.FindElement(By.CssSelector(".title-link h3")).Click();
        // Thread.Sleep(1000);
        _wait.Until(driver => Regex.Match(driver.Url, "http://localhost:5075/MealPlanDetails/\\d+").Success);
        _driver.FindElement(By.Id("edit-mealplan-btn")).Click();
    }

    [Then(@"I should be able to Enter new information, and click Create to save my changes")]
    public void ThenICanEnterNewMealInfoAndSubmit()
    {
        string newName = "Meal Plan Name " + DateTime.Now.ToString("yyyyMMddHH");
        _driver.FindElement(By.Id("PlanName")).Clear();
        _driver.FindElement(By.Id("PlanName")).SendKeys(newName);
        // Thread.Sleep(1000); // Wait for the page to load
        _driver.FindElement(By.Id("create-btn")).Click();
        // Thread.Sleep(1000); // Wait for the page to load
        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan"));
        Assert.That(_driver.FindElement(By.CssSelector(".title-link h3")).Text.Contains(newName));
    }

    [Then(@"the old information from the meal plan being edited should already be filled in when I get there\.")]
    public void ThenOldInfoShouldBePrepopulated()
    {
        Assert.That(_driver.FindElement(By.Id("PlanName")).Displayed, Is.True);
        Assert.That(_driver.FindElement(By.Id("PlanName")).GetAttribute("value"), Is.Not.Empty);
    }
}
