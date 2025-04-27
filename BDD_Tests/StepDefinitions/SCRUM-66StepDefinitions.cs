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
[Scope(Tag = "SCRUM66")]
public sealed class SCRUM66StepDefinitions : IDisposable
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        var option = new FirefoxOptions();
        option.AddArgument("--headless");
        option.AddArgument("--no-sandbox");
        option.AddArgument("--disable-dev-shm-usage");

        _driver = new FirefoxDriver(option);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

    public void Dispose()
    {
        if (_driver != null)
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

    [Given("I open the index page")]
    public void GivenIOpenTheIndexPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts/Index");
    }

    [When("I click on the search for an exercise button")]
    public void WhenIClickSearchForExerciseButton()
    {
        var button = _driver.FindElement(By.Id("search-exercise-button"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", button);
    }

    [When(@"I enter an invalid query in the search bar")]
    public void WhenIEnterAnInvalidQueryInTheSearchBar()
    {
        var searchBar = _driver.FindElement(By.Id("exerciseInput"));
        searchBar.Clear();
        searchBar.SendKeys("Gunga");
    }

    [Then("I should see a bootstrap alert")]
    public void ThenIShouldSeeABootstrapAlert()
    {
        var alert = _driver.FindElement(By.ClassName("alert"));
        Assert.IsTrue(alert.Displayed, "The alert is not displayed.");
    }

}
