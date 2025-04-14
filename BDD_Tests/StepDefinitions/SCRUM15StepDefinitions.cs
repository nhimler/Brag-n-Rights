using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM15")]
public sealed class SCRUM15StepDefinitions : IDisposable
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

    [Given(@"I have created a meal plan")]
    public void GivenIHaveCreatedAMealPlan()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMealPlan/new");
        Thread.Sleep(3000); // Wait for the page to load
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/CreateMealPlan/new"));
        _driver.FindElement(By.Id("PlanName")).SendKeys("Test Meal Plan");
        _driver.FindElement(By.Id("create-btn")).Click();

        Thread.Sleep(1000); // Wait for the page to load

        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan"));
    }

    [When(@"I go to create a meal")]
    public void WhenIGoToCreateAMeal()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMeal/new");
    }

    [When(@"I type {string} in the search bar")]
    public void WhenITypeATermInTheSearchBar(string term)
    {
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/CreateMeal/new"));
        _driver.FindElement(By.Id("searchBar")).SendKeys(term);
    }

    [When(@"I click the search button")]
    public void WhenIClickTheSearchButton()
    {
        _driver.FindElement(By.Id("searchButton")).Click();
    }

    [Then(@"I should see a list of food items related to chicken")]
    public void ThenIShouldSeeResultsRelatedTo()
    {
        Assert.That(_driver.FindElement(By.ClassName("accordion-item")).Displayed, Is.True);
    }

    [When(@"I search for {string}")]
    public void WhenISearchFor(string term)
    {
        // REUSE: Search logic
        WhenITypeATermInTheSearchBar(term);
        WhenIClickTheSearchButton();
    }

    [When(@"I select a food item")]
    public void WhenISelectAFoodItem()
    {
        //Have to fill meal elements
        _driver.FindElement(By.Id("MealName")).SendKeys("Test Meal"); 
        _driver.FindElement(By.Id("Description")).SendKeys("Test Meal"); 

        // Click food
        _driver.FindElement(By.ClassName("accordion-item")).Click();
        Thread.Sleep(500); // Wait for the page to load
        _driver.FindElement(By.CssSelector(".accordion-body button")).Click();
    }

    [When(@"I save the meal")]
    public void WhenISaveTheMeal()
    {
        _driver.FindElement(By.Id("create-btn")).Click();
    }

    [Then(@"the item should be added and visible on the dashboard")]
    public void ThenItemShouldAppearOnDashboard()
    {
        Assert.That(_driver.FindElement(By.ClassName("food-from-api")).Displayed, Is.True);
    }
}
