using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM42")]
public sealed class SCRUM70StepDefinitions : IDisposable
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
