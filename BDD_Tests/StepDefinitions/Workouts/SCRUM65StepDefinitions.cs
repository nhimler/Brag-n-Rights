using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM65")]
public sealed class SCRUM65StepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
    }

    [Given("I open the index page")]
    public void GivenIOpenTheIndexPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts/Index");
    }

    [When("I click on the search for an exercise button")]
    public void WhenIClickSearchForExerciseButton()
    {
        var dropdownText = _driver.FindElement(By.ClassName("dropdown-item-text"));
        dropdownText.Click();
        var button = _driver.FindElement(By.Id("search-exercise-button"));
        button.Click();
        // ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", button);
    }

    [When("I select Body Part from the exercise search type dropdown")]
    public void WhenISelectBodyPartFromTheExerciseSearchTypeDropdown()
    {
        var dropdown = _driver.FindElement(By.XPath("//*[@id='exerciseSearchType']"));
        var selectElement = new SelectElement(dropdown);
        selectElement.SelectByText("Body Part");
    }

    [When("I enter {string} in the search bar")]
    public void WhenIEnterInTheSearchBar(string searchText)
    {
        var searchBar = _driver.FindElement(By.Id("exerciseInput"));
        searchBar.Clear();
        searchBar.SendKeys(searchText);
    }

    [When("I click on the search button")]
    public void WhenIClickOnTheSearchButton()
    {
        var button = _driver.FindElement(By.Id("exerciseSearchButtonAddon"));
        button.Click();
    }

    [Then("I should see a list of exercises")]
    public void ThenIShouldSeeAListOfExercises()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        bool resultsLoaded = wait.Until(driver =>
        {
            var searchResultsDiv = driver.FindElement(By.Id("exerciseSearchResults"));
            return searchResultsDiv.FindElements(By.XPath("./*")).Count > 0;
        });
        Assert.IsTrue(resultsLoaded, "Exercise search results did not load.");
    }
}