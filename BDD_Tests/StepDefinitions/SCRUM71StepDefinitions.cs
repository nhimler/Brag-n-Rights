using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM71")]
public sealed class SCRUM71StepDefinitions : IDisposable
{
    private IWebDriver _driver;
    private WebDriverWait _wait;
    
    [BeforeScenario]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(8));
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

    [Given(@"I am a user who has logged in")]
    public void GivenIAmAUserWhoHasLoggedIn()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");
        IWebElement usernameField = null;
        IWebElement passwordField = null;
        _wait.Until(driver => {
            usernameField = _driver.FindElement(By.Id("login-username"));
            passwordField = _driver.FindElement(By.Id("login-password"));
            
            return usernameField != null && passwordField != null;
        });
        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");

        _driver.FindElement(By.Id("login-submit")).Click();
        _wait.Until(driver => _driver.Url != "http://localhost:5075/Identity/Account/Login");
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/"));
    }

    [Given(@"I create an out of date meal plan")]
    public void GivenIHaveCreatedAnOutOfDateMealPlan()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/CreateMealPlan/new");
        _wait.Until(driver => driver.Url == "http://localhost:5075/CreateMealPlan/new");
        _driver.FindElement(By.Id("PlanName")).SendKeys("Very Old Test Meal Plan");
        _driver.FindElement(By.Id("StartDate")).SendKeys("01010001");
        _driver.FindElement(By.Id("EndDate")).SendKeys("01010001");
        _driver.FindElement(By.Id("create-btn")).Click();

        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan");
    }

    [When("I click on the link to the meal plan archive")]
    public void WhenIClickOnTheLinkToTheMealPlanArchive()
    {
        _driver.FindElement(By.Id("archive-btn")).Click();
        _wait.Until(driver => driver.Url == "http://localhost:5075/MealPlan/Archive");
    }

    [Then("I should see my archived meal plans")]
    public void ThenIShouldSeeMyArchivedMealPlans()
    {
        Assert.That(_driver.PageSource, Does.Contain("Very Old Test Meal Plan"));
    }

    [Then("I should be taken to the meal plan archive page")]
    public void ThenIShouldBeTakenToTheMealPlanArchivePage()
    {
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/MealPlan/Archive"));
    }

    [Then("I should be able to see a link to the meal plan archive")]
    public void ThenIShouldBeAbleToSeeALinkToTheMealPlanArchive()
    {
       Assert.That(_driver.FindElement(By.Id("archive-btn")).Displayed, Is.True);
    }

    [Then("That meal plan should not appear on the dashboard and I should see a message communicating that {string}")]
    public void ThenThatMealPlanShouldNotAppearAndIShouldSeeAMessageCommunicatingThat(string s)
    {
        Assert.That(_driver.PageSource, Does.Not.Contain("Very Old Test Meal Plan"));
        Assert.That(_driver.PageSource, Does.Contain(s));
    }
}
