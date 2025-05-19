using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Reqnroll;
using System.Text.RegularExpressions;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "MealPlan")]
public sealed class MealPlanStepDefinitions
{
    private WebDriverWait _wait;
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
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

        usernameField.SendKeys("profileInfo");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
        _wait.Until(driver => _driver.Url != "http://localhost:5075/Identity/Account/Login");
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/"));
    }

    [Given(@"I create an out of date meal plan")]
    public void GivenIHaveCreatedAnOutOfDateMealPlan()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMealPlan/new");
        _wait.Until(driver => driver.Url == "http://localhost:5075/CreateMealPlan/new");
        _driver.FindElement(By.Id("PlanName")).SendKeys("Very Old Test Meal Plan");
        _driver.FindElement(By.Id("StartDate")).SendKeys("01010001");
        _driver.FindElement(By.Id("EndDate")).SendKeys("01010001");
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("create-btn")));

        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
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
        // _driver.FindElement(By.Id("edit-mealplan-btn")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("dropdownMenuButton")));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("edit-mealplan-btn")));
    }

    [Given(@"I have created a meal plan")]
    public void GivenIHaveCreatedAMealPlan()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMealPlan/new");
        _wait.Until(driver => driver.Url == "http://localhost:5075/CreateMealPlan/new");
        _driver.FindElement(By.Id("PlanName")).SendKeys("Test Meal Plan");
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("create-btn")));
        //_driver.FindElement(By.Id("create-btn")).Click();
        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
    }

    [Given(@"I visit the meal plan dashboard")]
    public void GivenIVisitTheMealPlanDashboard()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/MealPlan");
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan"));
    }

    [Given(@"I have created a meal")]
    public void GivenIHaveCreatedAMeal()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMeal/new");
        _wait.Until(driver => driver.Url == "http://localhost:5075/CreateMeal/new");
        _driver.FindElement(By.Id("MealName")).SendKeys("Test Meal");
        _driver.FindElement(By.Id("Description")).SendKeys("A meal for testing purposes");
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("create-btn")));
        //_driver.FindElement(By.Id("create-btn")).Click();
        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
    }

    [When("I click on the link to the meal plan archive")]
    public void WhenIClickOnTheLinkToTheMealPlanArchive()
    {
        // _driver.FindElement(By.Id("archive-btn")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("archive-btn")));
        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan/Archive");
    }

    [When("I click the delete all button")]
    public void WhenIClickTheDeleteAllButton()
    {
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("del-btn")));
    }

    [Then("I should see my archived meal plans")]
    public void ThenIShouldSeeMyArchivedMealPlans()
    {
        Assert.That(_driver.PageSource, Does.Contain("Very Old Test Meal Plan"));
    }

    [Then("I should see no archived meal plans")]
    public void ThenIShouldSeeNoArchivedMealPlans()
    {
        Assert.That(_driver.PageSource, Does.Not.Contain("Very Old Test Meal Plan"));
    }

    [Then("I should be taken to the meal plan archive page")]
    public void ThenIShouldBeTakenToTheMealPlanArchivePage()
    {
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan/Archive"));
    }

    [Then("I should be able to see a link to the meal plan archive")]
    public void ThenIShouldBeAbleToSeeALinkToTheMealPlanArchive()
    {
       Assert.That(_driver.FindElement(By.Id("archive-btn")).Displayed, Is.True);
    }

    [Then("That meal plan should not appear on the dashboard and I should see a message communicating that {string}")]
    public void ThenThatMealPlanShouldNotAppearAndIShouldSeeAMessageCommunicatingThat(string s)
    {
        Assert.That(_driver.PageSource, Does.Not.Contain("Very Old Test Meal Plan"));
        Assert.That(_driver.PageSource, Does.Contain(s));
    }

    [When(@"I click the search button")]
    public void WhenIClickTheSearchButton()
    {
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("searchButton")));
    }

    [When("I search for {string}")]
    public void WhenISearchFor(string term)
    {
        _driver.FindElement(By.Id("searchBar")).SendKeys(term);
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("searchButton")));
    }

    [Given("I add food to the meal")]
    public void GivenIAddFoodToTheMeal()
    {
        _driver.FindElement(By.Id("searchBar")).SendKeys("Chicken");
        //_driver.FindElement(By.Id("searchButton")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("searchButton")));
        _wait.Until(driver => driver.FindElement(By.ClassName("accordion-item")));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.ClassName("accordion-item")));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.ClassName("btn-secondary")));
        // _driver.FindElements(By.ClassName("accordion-item"))[0].Click();
        // _driver.FindElements(By.ClassName("btn-secondary"))[0].Click();
    }

    [Given("I visit the meal creation page")]
    public void GivenIVisitTheMealCreationPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMeal/new");
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

    [Then(@"I should see a list of food items related to chicken")]
    public void ThenIShouldSeeResultsRelatedTo()
    {
        Assert.That(_driver.FindElement(By.ClassName("accordion-item")).Displayed, Is.True);
    }

    [When(@"I select a food item")]
    public void WhenISelectAFoodItem()
    {
        _driver.FindElement(By.Id("MealName")).SendKeys("Test Meal"); 
        _driver.FindElement(By.Id("Description")).SendKeys("Test Meal"); 

        
        // _driver.FindElement(By.ClassName("accordion-item")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.ClassName("accordion-item")));
        Thread.Sleep(500); // Wait for the page to load
        // _driver.FindElement(By.CssSelector(".accordion-body button")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.CssSelector(".accordion-body button")));
    }

    [When(@"I save the meal")]
    public void WhenISaveTheMeal()
    {
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("create-btn")));
        //_driver.FindElement(By.Id("create-btn")).Click();
    }

    [Then(@"the item should be added and visible on the dashboard")]
    public void ThenItemShouldAppearOnDashboard()
    {
        Assert.That(_driver.FindElement(By.ClassName("food-from-api")).Displayed, Is.True);
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

    [Then(@"I should be able to click on a meal and be taken to a page where I can view its details- Title, Type, Description, Meal Plan")]
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

    [Then(@"I should be able to click on a meal to go to its details page")]
    public void ThenIClickMealToSeeItsDetails()
    {
        // _driver.FindElement(By.CssSelector(".title-link")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.CssSelector(".title-link")));
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/MealDetails/\\d+"));
    }

    [Then(@"I should be able to click the meals meal plan and go to its details page")]
    public void ThenIClickMealPlanLinkToSeeDetails()
    {
        _driver.FindElement(By.CssSelector(".title-link")).Click();
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/MealPlanDetails/\\d+"));
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
        // var editButton = _driver.FindElement(By.Id("edit-mealplan-btn"));
        // Assert.That(editButton.Displayed, Is.True);
        // editButton.Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("dropdownMenuButton")));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("edit-mealplan-btn")));
        _wait.Until(driver => Regex.Match(driver.Url, "http://localhost:5075/CreateMealPlan/\\d+").Success);
        Assert.That(_driver.Url, Does.Match("http://localhost:5075/CreateMealPlan/\\d+"));
    }

    [Then(@"I should be able to Enter new information, and click Create to save my changes")]
    public void ThenICanEnterNewMealInfoAndSubmit()
    {
        string newName = "Meal Plan Name " + DateTime.Now.ToString("yyyyMMddHH");
        _driver.FindElement(By.Id("PlanName")).Clear();
        _driver.FindElement(By.Id("PlanName")).SendKeys(newName);
        // _driver.FindElement(By.Id("create-btn")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("create-btn")));
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

    [Then("the meal creation fields should be filled with the selected suggestions details")]
    public void ThenTheMealCreationFieldsShouldBeFilledWithTheSelectedSuggestionsDetails()
    {
        _wait.Until(driver => driver.FindElement(By.Id("MealName")).GetAttribute("value") != string.Empty);
        Assert.That(_driver.FindElement(By.Id("MealName")).GetAttribute("value"), Is.Not.Empty);
        Assert.That(_driver.FindElement(By.Id("Description")).GetAttribute("value"), Is.Not.Empty);
    }

    [When("I select a suggestion from the list")]
    public void WhenISelectASuggestionFromTheList()
    {
        // _driver.FindElement(By.ClassName("suggestion-fill-btn")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.ClassName("suggestion-fill-btn")));
    }

    [Given("I generate suggestions for the meal")]
    public void GivenIGenerateSuggestionsForTheMeal()
    {
        // _driver.FindElement(By.Id("suggestBtn")).Click();
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("suggestBtn")));
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
    public void ThenTheMealPlansShouldSwitchToAListDisplay()
    {
        var listView = _driver.FindElement(By.Id("list"));
        Assert.That(listView.GetAttribute("hidden"), Is.Null, "List view is not displayed");
        var calendarView = _driver.FindElement(By.Id("calendar"));
        Assert.That(calendarView.GetAttribute("hidden"), Is.EqualTo("true"), "Calendar view is displayed when it should not be");
    }

    [Then("I should see a date input field")]
    public void ThenIShouldSeeADateInputField()
    {
        Assert.That(_driver.FindElement(By.Id("Date")).Displayed, Is.True);
    }

    [Then("I should see a graph that shows my targets")]
    public void ThenIShouldSeeAGraphThatShowsMyTargets()
    {
        Assert.That(_driver.FindElement(By.Id("targetGraph")).Displayed, Is.True);
    }
}