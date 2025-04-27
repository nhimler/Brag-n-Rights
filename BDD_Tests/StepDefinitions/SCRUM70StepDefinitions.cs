using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM70")]
public sealed class SCRUM70StepDefinitions : IDisposable
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
        //Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/CreateMealPlan/new"));
        _driver.FindElement(By.Id("PlanName")).SendKeys("Test Meal Plan");
        _driver.FindElement(By.Id("create-btn")).Click();

        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
        // Thread.Sleep(1000); // Wait for the page to load
        // Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan"));
    }

    [Given(@"I have created a meal")]
    public void GivenIHaveCreatedAMeal()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMeal/new");
        // Thread.Sleep(3000); // Wait for the page to load
        _wait.Until(driver => driver.Url == "http://localhost:5075/CreateMeal/new");

        _driver.FindElement(By.Id("MealName")).SendKeys("Test Meal");
        _driver.FindElement(By.Id("Description")).SendKeys("A meal for testing purposes");
        _driver.FindElement(By.Id("create-btn")).Click();
        
        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
        // Thread.Sleep(1000); // Wait for the page to load
        // Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan"));
    }

    [Given(@"I visit the meal plan dashboard")]
    public void GivenIVisitTheMealPlanDashboard()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/MealPlan");
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan"));
    }

    [Given(@"I click the calendar view button")]
    public void GivenIClickTheCalendarViewButton()
    {
        var viewBtn = _driver.FindElement(By.Id("view-btn"));

        Assert.That(viewBtn, Is.Not.Null, "View button not found");
        viewBtn.Click();
    }

    [Then(@"the meal plans should switch to a calendar display")]
    public void ThenTheMealPlansShouldSwitchToACalendarDisplay()
    {
        var calendarView = _driver.FindElement(By.Id("calendar"));
        Assert.That(calendarView.GetAttribute("hidden"), Is.Null, "Calendar view is not displayed");
        var listView = _driver.FindElement(By.Id("list"));
        Assert.That(listView.GetAttribute("hidden"), Is.EqualTo("true"), "List view is displayed when it should not be");
    }

    [When(@"I click the list view button")]
    public void WhenIClickTheListViewButton()
    {
        var viewBtn = _driver.FindElement(By.Id("view-btn"));

        Assert.That(viewBtn, Is.Not.Null, "View button not found");
        viewBtn.Click();
    }

    [Then(@"the meal plans should switch to a list display")]
    public void TheMealPlansShouldSwitchToAListDisplay()
    {
        var listView = _driver.FindElement(By.Id("list"));
        Assert.That(listView.GetAttribute("hidden"), Is.Null, "List view is not displayed");
        var calendarView = _driver.FindElement(By.Id("calendar"));
        Assert.That(calendarView.GetAttribute("hidden"), Is.EqualTo("true"), "Calendar view is displayed when it should not be");
    }
}
