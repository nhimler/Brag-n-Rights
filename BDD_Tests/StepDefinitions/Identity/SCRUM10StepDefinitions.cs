using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM10")]
public sealed class SCRUM10StepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [Given("I open the application")]
    public void GivenIOpenTheApplication()
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
                logoutButton.Click();
            }
        }
        catch (NoSuchElementException)
        {
            // Continue if the logout button is not found. They are logged out.
        }
    }

    [When("I click on the {string} link")]
    public void WhenIClickOnTheLink(string link)
    {
        var registerLink = _driver.FindElement(By.Id(link));
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

    [Then(@"I should see an email verification message")]
    public void ThenIShouldSeeAnEmailVerificationMessage()
    {
        // Write out the current outer html
        Console.WriteLine(_driver.PageSource);

        Assert.That(_driver.Title, Is.EqualTo("Register confirmation - GymBro_App"));
        var verificationMessage = _driver.FindElement(By.Id("verification-message"));
        Assert.That(verificationMessage.Displayed, Is.True);
    }

    [When("I login with {string} and {string}")]
    public void AndILoginWithUsernameAndPassword(string username, string password)
    {
        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys(username);
        passwordField.SendKeys(password);

        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [Then(@"I should be redirected back to the home page")]
    public void ThenIShouldBeRedirectedBackToTheHomePage()
    {
        var homePageTitle = _driver.Title;
        Assert.That(homePageTitle, Is.EqualTo("Home Page - GymBro_App"));
    }

    [Then("I should see {string} displayed on the page")]
    public void ThenIShouldSeeUsernameDisplayedOnThePage(string username)
    {
        var displayedText = _driver.FindElement(By.Id("manage")).Text;
        string expectedText = $"Hello {username}!";
        Assert.That(expectedText, Is.EqualTo(displayedText));
    }

    [When("I submit a registration form with optional information")]
    public void AndISubmitARegistrationFormWithOptionalInformation()
    {
        var firstNameField = _driver.FindElement(By.Id("first-name-register"));
        var lastNameField = _driver.FindElement(By.Id("last-name-register"));
        var usernameField = _driver.FindElement(By.Id("username-register"));
        var emailField = _driver.FindElement(By.Id("email-register"));
        var passwordField = _driver.FindElement(By.Id("password-register"));
        var confirmPasswordField = _driver.FindElement(By.Id("confirm-password-register"));

        var optionalInfoButton = _driver.FindElement(By.Id("optional-register-info-btn"));

        // Click on the button with Javascript to show optional fields
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", optionalInfoButton);

        // Optional information
        var ageField = _driver.FindElement(By.Id("age-register"));
        var genderField = _driver.FindElement(By.Id("gender-register"));
        var heightField = _driver.FindElement(By.Id("height-register"));
        var weightField = _driver.FindElement(By.Id("weight-register"));
        var fitnessLevelField = _driver.FindElement(By.Id("fitnessLevel-register"));
        var fitnessGoalsField = _driver.FindElement(By.Id("fitnessGoals-register"));
        var preferredWorkoutTimeField = _driver.FindElement(By.Id("preferredWorkoutTime-register"));
        
        // Filling in required fields
        string user = "scrum10user" + DateTime.Now.ToString("yyyyMMddHHmmss");
        firstNameField.SendKeys("John");
        lastNameField.SendKeys("Doe");
        usernameField.SendKeys(user);
        emailField.SendKeys($"{user}@test.com");
        passwordField.SendKeys("Password1!");
        confirmPasswordField.SendKeys("Password1!");

        // Filling in optional fields
        ageField.SendKeys("25");
        genderField.SendKeys("Male");
        heightField.SendKeys("180");
        weightField.SendKeys("180");
        fitnessLevelField.SendKeys("Intermediate");
        fitnessGoalsField.SendKeys("Gain 20 pounds of muscle");
        preferredWorkoutTimeField.SendKeys("Morning");

        // Use JavaScript to click the submit button
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", optionalInfoButton);

        var registerSubmitButton = _driver.FindElement(By.Id("registerSubmit"));
        // Use JavaScript to click the submit button
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", registerSubmitButton);
    }
}
