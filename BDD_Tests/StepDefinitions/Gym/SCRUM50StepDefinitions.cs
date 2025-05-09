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
public sealed class SCRUM50StepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
    }

    

    [When("I enter {int} as a postal code in the search bar")]
    public void WhenIEnterPostalCodeInTheSearchBar(int postalCode)
    {
        var searchBar = _driver.FindElement(By.Id("postal-code-gym-search"));
        searchBar.Clear();
        searchBar.SendKeys(postalCode.ToString());
    }

    [When("I click the postal search button")]
    public void WhenIClickThePostalSearchButton()
    {
        var button = _driver.FindElement(By.Id("postal-code-gym-search-button"));
        button.Click();
    }
}