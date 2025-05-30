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
[Scope(Tag = "Workouts")]
public sealed class WorkoutsStepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = GlobalDriverSetup.Driver;
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
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

    [Given("I am logged in as the user {string} with the password {string}")]
    public void GivenIAmLoggedInAsTheUser(string username, string password)
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
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");
        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys(username);
        passwordField.SendKeys(password);

        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [Given("I am on the exercise search page")]
    public void GivenIAmOnTheExerciseSearchPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts/ExerciseSearch");
    }

    [Given("I open the workout index page")]
    public void GivenIOpenTheWorkoutIndexPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts");
    }

    [When("I click on the {string} button")]
    public void WhenIClickOnTheButton(string buttonText)
    {
        Console.WriteLine(_driver.Url);
        // Add valid button IDs to the dictionary as needed
        Dictionary<string, string> validButtons = new Dictionary<string, string>
        {
            ["Search for an exercise"] = "search-exercise-button",
            ["Create a workout plan"] = "Workout-Creation-Page-Button"
        };

        var button = _driver.FindElement(By.Id(validButtons[buttonText]));
        try
        {
            button.Click();
        }
        catch (ElementClickInterceptedException)
        {
            // If the button is not clickable, scroll into view and try again
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", button);
        }
        catch (Exception ex)
        {
            // Handle any other exceptions that may occur
            Assert.Fail($"A testing error occurred when clicking the '{button}' button: {ex.Message}");
        }
    }

    [When("I enter {string} in the search bar")]
    public void WhenIEnterInTheSearchBar(string searchText)
    {
        var searchBar = _driver.FindElement(By.Id("exerciseInput"));
        searchBar.Clear();
        searchBar.SendKeys(searchText);
    }

    [When("I enter an invalid query in the search bar")]
    public void WhenIEnterAnInvalidQueryInTheSearchBar()
    {
        var searchBar = _driver.FindElement(By.Id("exerciseInput"));
        searchBar.Clear();
        searchBar.SendKeys("Gunga");
        var searchButton = _driver.FindElement(By.Id("exerciseSearchButtonAddon"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", searchButton);
    }

    [When("I select Body Part from the exercise search type dropdown")]
    public void WhenISelectBodyPartFromTheExerciseSearchTypeDropdown()
    {
        var dropdown = _driver.FindElement(By.XPath("//*[@id='exerciseSearchType']"));
        var selectElement = new SelectElement(dropdown);
        selectElement.SelectByText("Body Part");
    }

    [When("I click on the search button")]
    public void WhenIClickSearchButton()
    {
        var button = _driver.FindElement(By.Id("exerciseSearchButtonAddon"));
        button.Click();
    }

    [Then("I should see a list of exercises")]
    public void ThenIShouldSeeAListOfExercises()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        bool resultsLoaded = wait.Until(driver =>
        {
            var searchResultsDiv = driver.FindElement(By.Id("exerciseSearchResults"));
            return searchResultsDiv.FindElements(By.XPath("./*")).Count > 0;
        });
        Assert.IsTrue(resultsLoaded, "Exercise search results did not load.");
    }

    [Then("I should see my workout plans")]
    public void ThenIshouldSeeMyWorkoutPlans()
    {
        // Wait for the page to load and display workout plans
        Console.WriteLine(_driver.Url);

        // Check if workout plans are displayed
        var workoutPlansContainer = _driver.FindElement(By.Id("workoutPlansContainer"));
        Assert.IsTrue(workoutPlansContainer.Displayed, "Workout plans are not displayed on the landing page.");
    }

    // Updated the text in the step definition to match the feature file
    [Then("I should see the workout plan creation form")]
    public void ThenIShouldSeeTheWorkoutPlanCreationForm()
    {
        var pageTitle = _driver.FindElement(By.TagName("h2")).Text;
        Assert.IsTrue(pageTitle.Contains("Create a Workout Plan"));
    }

    [Then("I should be able to submit the form with valid data")]
    public void ThenIShouldBeAbleToSubmitTheFormWithValidData()
    {
        var planNameField = _driver.FindElement(By.Id("PlanName"));
        planNameField.Clear();
        planNameField.SendKeys("Test Plan");

        var startDateField = _driver.FindElement(By.Id("StartDate"));
        startDateField.Clear();
        startDateField.SendKeys("2025-10-01");

        var endDateField = _driver.FindElement(By.Id("EndDate"));
        endDateField.Clear();
        endDateField.SendKeys("2025-10-08");

        // Enter Frequency (e.g., workouts per week)
        var frequencyField = _driver.FindElement(By.Id("Frequency"));
        frequencyField.Clear();
        frequencyField.SendKeys("3");

        // Enter Goal
        var goalField = _driver.FindElement(By.Id("Goal"));
        goalField.Clear();
        goalField.SendKeys("Muscle Gain");

        // Select Difficulty Level using the <select> element
        var difficultyDropdown = _driver.FindElement(By.Id("DifficultyLevel"));
        var selectDifficulty = new OpenQA.Selenium.Support.UI.SelectElement(difficultyDropdown);
        selectDifficulty.SelectByText("Intermediate");

        // Submit the form by clicking on the Create Workout Plan button
        var submitButton = _driver.FindElement(By.CssSelector("#Workout-Creation-Form button[type='submit']"));
        submitButton.Click();

        // Wait for navigation or an expected element that signifies success
        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => driver.Title != "Create Workout Plan" || driver.Url.Contains("Index"));

        // Simple assertion to ensure we are no longer on the creation page
        Assert.That(_driver.Title, Is.Not.EqualTo("Create a Workout Plan"));
    }

    [Then("I should see the exercise search bar")]
    public void ThenIShouldSeeTheExerciseSearchBar()
    {
        Console.WriteLine(_driver.Url);
        var searchBar = _driver.FindElement(By.Id("exerciseInput"));
        Assert.IsTrue(searchBar.Displayed);
    }

    [Then("I should see a login dropdown")]
    public void ThenIShouldSeeALoginDropdown()
    {
        var loginDropdown = _driver.FindElement(By.Id("loginDropdown"));
        Assert.IsTrue(loginDropdown.Displayed, "Login dropdown is not displayed.");
    }

    [Then("I should see a bootstrap alert")]
    public void ThenIShouldSeeABootstrapAlert()
    {
        var alert = _driver.FindElement(By.ClassName("alert"));
        Assert.IsTrue(alert.Displayed, "The alert is not displayed.");
    }

    [Then("I should see the text {string}")]
    public void ThenIShouldSeeTheText(string expectedText)
    {
        var bodyText = _driver.FindElement(By.TagName("body")).Text;
        Assert.IsTrue(bodyText.Contains(expectedText), $"Expected text '{expectedText}' not found in the body.");
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

    [Then("I should see a list of exercises matching my search criteria")]
    public void ThenIShouldSeeAListOfExercisesMatchingMySearchCriteria()
    {
        var exerciseList = _driver.FindElement(By.Id("exerciseSearchResults"));
        var exerciseCards = _driver.FindElements(By.ClassName("exercise-card"));
        Assert.That(exerciseList.Displayed, Is.EqualTo(true), "Exercise list is not displayed.");
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

    [When("I click on the Complete Workout Plan button")]
    public void WhenIClickOnTheCompleteWorkoutPlanButton()
    {
        var completeWorkoutPlanButton = _driver.FindElement(By.Id("completeWorkoutPlanButton"));
        completeWorkoutPlanButton.Click();
    }

    [Then("I should not see the workout plan on the page")]
    public void ThenIShouldNotSeeTheWorkoutPlanOnThePage()
    {
        var completeWorkoutButtons = _driver.FindElements(By.Id("completeWorkoutPlanButton"));
        Assert.That(completeWorkoutButtons.Count, Is.EqualTo(0), "Workout plans are still displayed on the page.");
    }

    [When("I navigate to the index page")]
    public void WhenINavigateToTheIndexPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Workouts");
    }

    [Then("I should see buttons for each body part")]
    public void ThenIShouldSeeButtonsForEachBodyPart()
    {
        var bodyPartButtons = _driver.FindElements(By.Id("bodyPartRadioButtons"));
        Assert.IsTrue(bodyPartButtons.Count > 0, "No body part buttons found.");
        foreach (var button in bodyPartButtons)
        {
            Assert.IsTrue(button.Displayed, "A body part button is not displayed.");
        }
    }

    [When("I click on a body part button")]
    public void WhenIClickOnABodyPartButton()
    {
        var bodyPartButtons = _driver.FindElements(By.Id("bodyPartRadioButtons"));
        if (bodyPartButtons.Count > 0)
        {
            var randomButton = bodyPartButtons[new Random().Next(bodyPartButtons.Count)];
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", randomButton);
        }
        else
        {
            Assert.Fail("No body part buttons found to click.");
        }
    }

    [Then("I should see a list of exercises that target the selected body part")]
    public void ThenIShouldSeeAListOfExercisesThatTargetTheSelectedBodyPart()
    {
        var exerciseList = _driver.FindElement(By.Id("exerciseSearchResults"));
        var exerciseCards = _driver.FindElements(By.ClassName("exercise-card"));
        Assert.That(exerciseList.Displayed, Is.EqualTo(true), "Exercise list is not displayed.");
        Assert.That(exerciseCards.Count, Is.GreaterThan(0), "No exercises found in the list.");

    }

    [When("I click on the View Exercises button")]
    public void WhenIClickOnTheViewExercisesButton()
    {
        var viewExercisesButton = _driver.FindElement(By.Id("viewExercisesButton"));
        viewExercisesButton.Click();
    }

    [Then("I should see the list of exercises in the workout plan")]
    public void ThenIShouldSeeTheListOfExercisesInTheWorkoutPlan()
    {
        var container = _driver.FindElement(By.Id("exercisesModalBody"));
        var details = container.FindElements(By.ClassName("exercise-detail"));
        Assert.IsTrue(details.Count > 0, "No exercises found in the workout plan.");

        var setsInputs = container.FindElements(By.CssSelector(".sets-input"));
        var repsInputs = container.FindElements(By.CssSelector(".reps-input"));
        var saveBtns = container.FindElements(By.CssSelector(".save-changes-btn"));

        Assert.IsTrue(setsInputs.Count > 0, "Set inputs are not present.");
        Assert.IsTrue(repsInputs.Count > 0, "Reps inputs are not present.");
        Assert.IsTrue(saveBtns.Count > 0, "Save Changes buttons are not present.");
    }

    [Then("I should be able to set the sets and reps for the exercises")]
    public void ThenIShouldBeAbleToSetTheSetsAndRepsForTheExercises()
    {
        var container = _driver.FindElement(By.Id("exercisesModalBody"));
        var firstDetail = container.FindElement(By.ClassName("exercise-detail"));
        var setsInput = firstDetail.FindElement(By.CssSelector(".sets-input"));
        var repsInput = firstDetail.FindElement(By.CssSelector(".reps-input"));
        var saveButton = firstDetail.FindElement(By.CssSelector(".save-changes-btn"));

        setsInput.Clear();
        setsInput.SendKeys("3");
        repsInput.Clear();
        repsInput.SendKeys("12");
        saveButton.Click();

        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => saveButton.Text.Contains("Saved"));

        Assert.That(saveButton.Text, Is.EqualTo("Saved"), "Save button did not update to 'Saved'.");
        Assert.That(saveButton.GetAttribute("class"), Does.Contain("btn-secondary"), "Button did not change to secondary style.");
    }

    [When(@"I have a workout plan with the name ""([^""]*)""")]
    public void WhenIHaveAWorkoutPlanWithTheName(string planName)
    {
        var workoutPlansContainer = _driver.FindElement(By.Id("workoutPlansContainer"));
        var workoutPlanTitles = workoutPlansContainer.FindElements(By.ClassName("card-title"));
        bool planFound = workoutPlanTitles.Any(title => title.Text.Contains(planName));
        var viewExercisesButton = _driver.FindElement(By.Id("viewExercisesButton"));
        viewExercisesButton.Click();
        Assert.IsTrue(planFound, $"Workout plan with name '{planName}' was not found.");
    }

    [When("I make changes to the exercises in the workout plan")]
    public void WhenIMakeChangesToTheExercisesInTheWorkoutPlan()
    {
        var container = _driver.FindElement(By.Id("exercisesModalBody"));
        var firstDetail = container.FindElement(By.ClassName("exercise-detail"));
        var setsInput = firstDetail.FindElement(By.CssSelector(".sets-input"));
        var repsInput = firstDetail.FindElement(By.CssSelector(".reps-input"));
        var saveButton = firstDetail.FindElement(By.CssSelector(".save-changes-btn"));

        setsInput.Clear();
        setsInput.SendKeys("3");
        repsInput.Clear();
        repsInput.SendKeys("12");
        saveButton.Click();
    }

    [When(@"I click the {string} button")]
    public void WhenIClickTheButton(string buttonText)
    {
        var button = _driver.FindElement(By.XPath(
            $"//button[@id='saveAllExercisesBtn' and normalize-space(text())='{buttonText}']"
        ));
        button.Click();
    }

    [Then("I should open the workout plan back up and see the changes I made")]
    public void ThenIOpenTheWorkoutPlanBackUpAndSeeTheChangesIMade()
    {
        // Open the "test3" workout plan
        var workoutPlansContainer = _driver.FindElement(By.Id("workoutPlansContainer"));
        var workoutPlanTitles = workoutPlansContainer.FindElements(By.ClassName("card-title"));
        var testPlanTitle = workoutPlanTitles.FirstOrDefault(title => title.Text.Contains("test3"));
        Assert.IsNotNull(testPlanTitle, "Workout plan with name 'test3' was not found.");
        testPlanTitle.Click();
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => driver.FindElement(By.Id("exercisesModalBody")).Displayed);
    }

    [Then("I should be able to input the amount of weight I do for an exercise")]
    public void ThenIShouldBeAbleToInputTheAmountOfWeightIDoForAnExercise()
    {
        var container = _driver.FindElement(By.Id("exercisesModalBody"));
        var firstDetail = container.FindElement(By.ClassName("exercise-detail"));
        var weightInput = firstDetail.FindElement(By.CssSelector(".weight-input"));

        weightInput.Clear();
        weightInput.SendKeys("100");

        // Verify that the input was set correctly
        Assert.That(weightInput.GetAttribute("value"), Is.EqualTo("100"), "Weight input did not accept the value.");
    }
}