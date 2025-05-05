using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
public sealed class GymGlobalStepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
    }

    [Given("I am on the gym search page")]
    public void WhenINavigateToTheGymSearchPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Gym/FindNearbyGyms");
    }

    [Then("I should see a list of gyms appear")]
    public void ThenIShouldSeeAListOfGyms()
    {
        var gymList = _driver.FindElement(By.Id("nearby-gym-search-list"));
        var gymListItems = gymList.FindElements(By.ClassName("card"));
        Assert.That(gymListItems.Count, Is.GreaterThan(0), "No gyms found in the list.");
    }

    [Then("I should see a message telling me there are no gyms nearby")]
    public void ThenIShouldSeeAMessageTellingMeThereAreNoGymsNearby()
    {
        var gymResultHeader = _driver.FindElement(By.Id("nearby-gyms-results-header"));
        Assert.That(gymResultHeader.Displayed, Is.True, "No gyms message is not displayed.");
    }
}