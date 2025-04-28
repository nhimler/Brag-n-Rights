using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM69")]
public sealed class SCRUM69StepDefinitions : IDisposable
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
        

        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
        _wait.Until(driver => _driver.Url != "http://localhost:5075/Identity/Account/Login");
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/"));
    }

    [Given(@"I have created a meal plan")]
    public void GivenIHaveCreatedAMealPlan()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMealPlan/new");
        _wait.Until(driver => driver.Url == "http://localhost:5075/CreateMealPlan/new");
        _driver.FindElement(By.Id("PlanName")).SendKeys("Test Meal Plan");
        _driver.FindElement(By.Id("create-btn")).Click();

        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
    }

    [Given("I visit the meal creation page")]
    public void GivenIVisitTheMealCreationPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMeal/new");
    }

    [Then("the meal creation fields should be filled with the selected suggestions details")]
    public void ThenTheMealCreationFieldsShouldBeFilledWithTheSelectedSuggestionSDetails()
    {
        Assert.That(_driver.FindElement(By.Id("meal-name")).GetAttribute("value"), Is.Not.Empty);
        Assert.That(_driver.FindElement(By.Id("meal-description")).GetAttribute("value"), Is.Not.Empty);
    }

    [When("I select a suggestion from the list")]
    public void WhenISelectASuggestionFromTheList()
    {
        _driver.FindElement(By.ClassName("suggestion-fill-btn")).Click();
    }

    [Given("I generate suggestions for the meal")]
    public void GivenIGenerateSuggestionsForTheMeal()
    {
        _driver.FindElement(By.Id("suggestBtn")).Click();
    }

    [Given("I add food to the meal")]
    public void GivenIAddFoodToTheMeal()
    {
        _driver.FindElement(By.Id("searchBar")).SendKeys("Chicken");
        _driver.FindElement(By.Id("searchButton")).Click();
        _wait.Until(driver => driver.FindElement(By.ClassName("accordion-item")));
        _driver.FindElements(By.ClassName("accordion-item"))[0].Click();
        _driver.FindElements(By.ClassName("btn-secondary"))[0].Click();
    }
}
