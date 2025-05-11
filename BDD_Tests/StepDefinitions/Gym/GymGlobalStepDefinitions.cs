using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "Gym")]
public sealed class GymGlobalStepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
    }

    [Given("I am logged in")]
    public void GivenIAmLoggedIn()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");

        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");
        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [Given("I am logged in as {string} with password {string}")]
    public void GivenIAmLoggedInAs(string username, string password)
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");

        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys(username);
        passwordField.SendKeys(password);
        _driver.FindElement(By.Id("login-submit")).Click();
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

        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(d => gymList.Displayed);
        var gymListHeader = _driver.FindElement(By.Id("nearby-gyms-results-header"));
        var gymListItems = gymList.FindElement(By.ClassName("diplayed-gym-card"));
        Assert.That(gymListItems.Displayed, Is.True, "Gym list is not displayed.");
    }

    [Then("I should see a message telling me there are no gyms nearby")]
    public void ThenIShouldSeeAMessageTellingMeThereAreNoGymsNearby()
    {
        var gymResultHeader = _driver.FindElement(By.Id("nearby-gyms-results-header"));
        Assert.That(gymResultHeader.Displayed, Is.True, "No gyms message is not displayed.");
    }

    [Then("I should see a bookmark button next to a gym")]
    public void ThenIShouldSeeABookmarkButtonNextToAGym()
    {
        var bookmarkButton = _driver.FindElements(By.ClassName("bookmark-gym-button"));
        Assert.That(bookmarkButton.Count > 0, "Bookmark buttons are not appearing next to gyms.");
    }

    [When("I enter {int} as a postal code in the search bar")]
    public void WhenIEnterPostalCodeInTheSearchBar(int postalCode)
    {
        var searchBar = _driver.FindElement(By.Id("postal-code-gym-search"));
        searchBar.Clear();
        searchBar.SendKeys(postalCode.ToString());
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [When("I click the postal search button")]
    public void WhenIClickThePostalSearchButton()
    {
        var button = _driver.FindElement(By.Id("postal-code-gym-search-button"));
        button.Click();
    }

    [Then("I should see a disabled bookmark button next to a gym")]
    public void ThenIShouldSeeADisabledBookmarkButtonNextToAGym()
    {
        var allButtons = _driver.FindElements(By.ClassName("disabled"));
        Assert.That(allButtons, Is.Not.Empty, "Disabled bookmark buttons are not appearing next to gyms.");
    }
}