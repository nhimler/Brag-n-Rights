using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM38")]
public sealed class SCRUM38StepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    [Given("I open the index page")]
    public void GivenIOpenTheIndexPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts/Index");
    }

    [When("I click on the {string} button")]
    public void WhenIClickOnTheButton(string buttonText)
    {
        var button = _driver.FindElement(By.Id("search-exercise-button"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", button);
    }

    [Then("I should see the exercise search bar")]
    public void ThenIShouldSeeTheExerciseSearchBar()
    {
        var searchBar = _driver.FindElement(By.Id("exerciseInput"));
        Assert.IsTrue(searchBar.Displayed);
    }

}