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
[Scope(Tag = "SCRUM46")]
public sealed class SCRUM46StepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
    }

    [Given("I open the index page")]
    public void GivenIOpenTheIndexPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts/Index");
    }

    [Then("I should see a login dropdown")]
    public void ThenIShouldSeeALoginDropdown()
    {
        var loginDropdown = _driver.FindElement(By.Id("loginDropdown"));
        Assert.IsTrue(loginDropdown.Displayed, "Login dropdown is not displayed.");
    }

    [Then("I should see the text {string}")]
    public void ThenIShouldSeeTheText(string expectedText)
    {
        var bodyText = _driver.FindElement(By.TagName("body")).Text;
        Assert.IsTrue(bodyText.Contains(expectedText), $"Expected text '{expectedText}' not found in the body.");
    }
}