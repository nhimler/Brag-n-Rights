using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
public sealed class SCRUM10StepDefinitions : IDisposable
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

    [Given("I open the application")]
    public void GivenIOpenTheApplication()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075");
    }

    [When(@"I click on the register link")]
    public void WhenIClickOnTheRegisterLink()
    {
        var registerLink = _driver.FindElement(By.Id("register"));
        registerLink.Click();
    }

    [When(@"I submit a registration form with valid data")]
    public void AndISubmitARegistrationFormWithValidData()
    {
        var firstNameField = _driver.FindElement(By.Id("first-name-register"));
        var lastNameField = _driver.FindElement(By.Id("last-name-register"));
        var usernameField = _driver.FindElement(By.Id("username-register"));
        var emailField = _driver.FindElement(By.Id("email-register"));
        var passwordField = _driver.FindElement(By.Id("password-register"));
        var confirmPasswordField = _driver.FindElement(By.Id("confirm-password-register"));

        
        string user = "scrum10user" + DateTime.Now.ToString("yyyyMMddHHmmss");

        firstNameField.SendKeys("John");
        lastNameField.SendKeys("Doe");
        usernameField.SendKeys(user);
        emailField.SendKeys($"{user}@test.com");
        passwordField.SendKeys("Password1!");
        confirmPasswordField.SendKeys("Password1!");

        _driver.FindElement(By.Id("registerSubmit")).Click();
    }

    [When(@"I click on the message to confirm my email address")]
    public void IClickOnTheMessageToConfirmMyEmailAddress()
    {
        // During SCRUM-10, we did not properly implement email confirmation. 
        // As a result, we've stuck with the default page Identity gives us.
        // TODO: In the future, if we implement email confirmation, the following test will need to be updated.

        var confirmationLink = _driver.FindElement(By.Id("confirm-link"));
        confirmationLink.Click();
    }

    [Then(@"I should see a confirmation message")]
    public void ThenIShouldSeeAConfirmationMessage()
    {
        var confirmationMessage = _driver.FindElement(By.ClassName("alert-success"));
        Assert.That(confirmationMessage.Displayed, Is.True);
    }
}
