using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "Home")]
public sealed class HomeStepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
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

    [Given("I am on the home page")]
    public void GivenIAmOnTheHomePage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075");
    }

    [Given("I am not logged in")]
    public void GivenIAmNotLoggedIn()
    {
        try
        {
            var logoutButton = _driver.FindElement(By.Id("logout"));
            if (logoutButton.Displayed)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", logoutButton);
            }
        }
        catch (NoSuchElementException)
        {
            // Continue if the logout button is not found. They are logged out.
        }
    }

    [Then("I should see descriptions of the features available on the home page")]
    public void ThenIShouldSeeDescriptionsOfTheFeaturesAvailableOnTheHomePage()
    {
        var featureDescriptions = _driver.FindElements(By.ClassName("feature-carousel"));
        Assert.That(featureDescriptions.Count, Is.GreaterThan(0), "Expected to see at least one feature description on the home page.");
        
        foreach (var description in featureDescriptions)
        {
            Assert.That(description.Displayed, Is.True, "Expected feature description to be displayed.");
        }
    }

    [Then("I should not see the {string} link in the navbar")]
    public void ThenIShouldNotSeeTheLinkInTheNavbar(string linkText)
    {
        var navbarLinks = _driver.FindElements(By.ClassName("nav-link"));
        if (navbarLinks.Count == 0)
        {
            Assert.Fail("No navbar links found. Cannot verify the absence of the specified link.");
        }
        var linkExists = navbarLinks.Any(link => link.GetAttribute("innerText").Equals(linkText, StringComparison.OrdinalIgnoreCase));
        
        Assert.That(linkExists, Is.False, $"Expected not to see the '{linkText}' link in the navbar.");
    }

    [Then("I should see the {string} link in the navbar")]
    public void ThenIShouldSeeTheLinkInTheNavbar(string linkText)
    {
        var navbarLinks = _driver.FindElements(By.ClassName("nav-link"));
        if (navbarLinks.Count == 0)
        {
            Assert.Fail("No navbar links found.");
        }
        var linkExists = navbarLinks.Any(link => link.GetAttribute("innerText").Equals(linkText, StringComparison.OrdinalIgnoreCase));
        
        Assert.That(linkExists, Is.True, $"Expected to see the '{linkText}' link in the navbar.");
    }
}