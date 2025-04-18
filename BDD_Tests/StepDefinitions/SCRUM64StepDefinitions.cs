using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM64")]
public sealed class SCRUM64StepDefinitions : IDisposable
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
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
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

    [Given("I am on the exercise search page")]
    public void GivenIAmOnTheExerciseSearchPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts/ExerciseSearch");
    }

    [When("I search for an invalid exercise")]
    public void WhenISearchForAnInvalidExercise()
    {
        var searchField = _driver.FindElement(By.Id("exerciseInput"));
        var searchButton = _driver.FindElement(By.Id("exerciseSearchButtonAddon"));

        searchField.SendKeys("InvalidExerciseName");
        searchButton.Click();
    }

    [Then(@"I should see a message saying no exercises were found")]
    public void ThenIShouldSeeAMessageSayingNoExercisesWereFound()
    {
        var errorMessage = _driver.FindElement(By.Id("noResultsMessage"));
        Assert.IsTrue(errorMessage.Displayed, "Error message is not displayed.");
        Assert.IsTrue(errorMessage.Text.Contains("No results found"), "Error message text is incorrect.");
    }
}
