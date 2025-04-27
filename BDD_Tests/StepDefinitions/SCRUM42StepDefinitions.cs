using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM42")]
public sealed class SCRUM42StepDefinitions : IDisposable
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

    [Then(@"I should be able to click on a meal and be taken to a page where I can view its details- Title, Type, Description, Meal Plan")] // (Title, Type, Description, Meal Plan)
    public void ThenIClickMealAndSeeDetails()
    {
        _driver.FindElement(By.CssSelector("ol .title-link h3")).Click();
        
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/MealDetails/\\d+"));
    }

    [Given(@"I visit the details page of a meal")]
    public void GivenIVisitTheDetailsPageOfAMeal()
    {
        _driver.FindElement(By.CssSelector("ol .title-link h3")).Click();
        
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/MealDetails/\\d+"));
    }

    [Then(@"I should be able to click a button to return to the meal plan home page")]
    public void ThenIClickButtonToReturnHome()
    {
        _driver.FindElement(By.Id("home-btn")).Click();
        
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan"));
    }

    [Given(@"I visit the details page of a meal plan")]
    public void GivenIVisitTheDetailsPageOfAMealPlan()
    {
        _driver.FindElement(By.CssSelector(".title-link h3")).Click();
        
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/MealPlanDetails/\\d+"));
    }

    [Then(@"I should be able to click on a meal to go to its details page")]
    public void ThenIClickMealToSeeItsDetails()
    {
        _driver.FindElement(By.CssSelector(".title-link")).Click();
        
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/MealDetails/\\d+"));
    }

    [Then(@"I should be able to click the meals meal plan and go to its details page")]
    public void ThenIClickMealPlanLinkToSeeDetails()
    {
        _driver.FindElement(By.CssSelector(".title-link")).Click();

        Assert.That(_driver.Url, Does.Match("http://localhost:5075/MealPlanDetails/\\d+"));
    }
}
