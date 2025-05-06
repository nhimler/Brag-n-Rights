using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "SCRUM64")]
public sealed class SCRUM64StepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
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

    [When("I search for a valid exercise")]
    public void WhenISearchForAValidExercise()
    {
        // Note: This is currently still a valid exercise name. However, the API may change in the future.
        // If the API changes, you may need to update this test to use a different valid exercise name.
        string validExerciseName = "bench press";
        var searchField = _driver.FindElement(By.Id("exerciseInput"));
        var searchButton = _driver.FindElement(By.Id("exerciseSearchButtonAddon"));

        searchField.SendKeys(validExerciseName);
        searchButton.Click();
    }

    [Then(@"I should see a list of exercises matching my search criteria")]
    public void ThenIShouldSeeAListOfExercisesMatchingMySearchCriteria()
    {
        var exerciseList = _driver.FindElement(By.Id("exerciseSearchResults"));
        var exerciseCards = _driver.FindElements(By.ClassName("exercise-card"));
        Assert.That(exerciseList.Displayed, Is.EqualTo(true),"Exercise list is not displayed.");
        Assert.That(exerciseCards.Count, Is.GreaterThan(0), "No exercises found in the list.");

        var exerciseTitles = _driver.FindElements(By.ClassName("card-title"));
        foreach (var title in exerciseTitles)
        {
            Assert.That(title.Text.Contains("Bench Press"), Is.EqualTo(true), "Exercise title is empty.");
        }
    }

    [When("I view the exercise details")]
    public void WhenIViewTheExerciseDetails()
    {
        var exerciseCards = _driver.FindElements(By.ClassName("exercise-card"));
        var exerciseDetailsButton = exerciseCards[0].FindElement(By.ClassName("exercise-details-btn"));
        exerciseDetailsButton.Click();
    }

    [Then("I should be able to see a window with additional information about the exercise")]
    public void ThenIShouldBeAbleToSeeAWindowWithAdditionalInformationAboutTheExercise()
    {
        var exerciseDetailsModal = _driver.FindElement(By.Id("exerciseModal"));
        // Console.WriteLine("Exercise Details Modal: " + exerciseDetailsModal.GetAttribute("outerHTML"));
        var exerciseTitle = _driver.FindElement(By.Id("modalExerciseName"));
        var exerciseContent = _driver.FindElement(By.Id("modalExerciseContent"));
        var exerciseImage = _driver.FindElement(By.Id("exerciseGif"));
        // Console.WriteLine("Exercise Image: " + exerciseImage.GetAttribute("src") + ".jpg");
        // Console.WriteLine("Exercise Title: " + exerciseTitle.GetAttribute("innerHTML"));

        Assert.That(exerciseTitle.GetAttribute("innerHTML"), Is.Not.Empty, "Exercise title is empty.");
        Assert.That(exerciseContent.GetAttribute("outerHTML"), Is.Not.Empty, "Exercise content is empty.");
        Assert.That(exerciseImage.GetAttribute("src"), Is.Not.Empty, "Exercise image is empty.");
    }
        
}
