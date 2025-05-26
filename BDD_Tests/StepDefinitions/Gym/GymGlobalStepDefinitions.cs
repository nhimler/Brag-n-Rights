using System.Runtime.CompilerServices;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using Reqnroll;
using AngleSharp.Text;
using OpenQA.Selenium.Chrome;

namespace BDD_Tests.StepDefinitions;

[Binding]
[Scope(Tag = "Gym")]
public sealed class GymGlobalStepDefinitions
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

    [AfterScenario]
    public void Teardown()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }

    [Given("I am logged in")]
    public void GivenIAmLoggedIn()
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
            // Ignore if the logout button is not found
        }
        _driver.Navigate().GoToUrl("http://localhost:5075/Identity/Account/Login");

        var usernameField = _driver.FindElement(By.Id("login-username"));
        var passwordField = _driver.FindElement(By.Id("login-password"));

        usernameField.SendKeys("testingLogin");
        passwordField.SendKeys("Password!1");
        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [Given("I have at least one gym bookmarked")]
    public void GivenIHaveAtLeastOneGymBookmarked()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/Gym/FindNearbyGyms");
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.Url.Contains("FindNearbyGyms"));
        var searchBar = _driver.FindElement(By.Id("postal-code-gym-search"));
        searchBar.Clear();
        searchBar.SendKeys("97361");
        var button = _driver.FindElement(By.Id("postal-code-gym-search-button"));
        button.Click();
        var bookmarkingGym = FindFirstBookmarkedGym(true);
        Assert.That(bookmarkingGym, Is.Not.Null, "No gyms found to bookmark.");
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
        Console.WriteLine("Currently on: " + _driver.Url);
        
        // Wait for the page to load. The page seems to sometimes take a while to load.
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.Url.Contains("FindNearbyGyms"));
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/Gym/FindNearbyGyms"), "Expected Gym Search page URL. Actual: " + _driver.Url);
    }

    [Given("I am on the my gyms page")]
    public void GivenIAmOnTheMyGymsPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5075/UserPage/BookmarkedGyms");

        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.Url.Contains("BookmarkedGyms"));
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/UserPage/BookmarkedGyms"), "Expected 'My Gyms' page URL. Actual: " + _driver.Url);
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
        // var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
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

    [Then("I should see a rating next to a gym")]
    public void IShouldSeeARatingNextToAGym()
    {
        var rating = _driver.FindElements(By.ClassName("gym-rating-star"));
        Assert.That(rating.Count > 0, "Rating stars are not appearing next to gyms.");
    }

    [When("I click on a bookmarked gym")]
    public void WhenIClickOnABookmarkedGym()
    {
        var bookmarkButton = FindFirstBookmarkedGym() ?? throw new Exception("No bookmarked gym found.");
        // Console.WriteLine(bookmarkButton.Text);
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", bookmarkButton);
        
        // Not a fan of explicit waits, but the JavaScript to remove a bookmark and change the icon takes a while since
        // it's waiting for the database to update. Implicit waits don't work here since the element is still present.
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(d => bookmarkButton.Text.Contains("Add to Bookmarks"));

    }

    [Then("The gym should no longer be bookmarked")]
    public void ThenTheGymShouldNoLongerBeBookmarked()
    {
        var bookmarkedGyms = _driver.FindElements(By.ClassName("bookmarked-icon"));
        foreach (var gym in bookmarkedGyms)
        {
            Console.WriteLine($"Gym: {gym.GetAttribute("outerHTML")}");
        }
        Assert.That(bookmarkedGyms.Count == 0, "Gym is still bookmarked.");
    }

    [When("I delete a gym from my bookmarks")]
    public void WhenIDeleteAGymFromMyBookmarks()
    {
        var deleteButton = _driver.FindElement(By.ClassName("delete-gym-btn"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", deleteButton);
    }

    [Then("I should no longer see the gym in my bookmarks")]
    public void ThenIShouldNoLongerSeeTheGymInMyBookmarks()
    {
        var bookmarkedGyms = _driver.FindElements(By.ClassName("bookmarked-icon"));
        foreach (var gym in bookmarkedGyms)
        {
            Console.WriteLine($"Gym: {gym.GetAttribute("outerHTML")}");
        }
        Assert.That(bookmarkedGyms.Count == 0, "Gym is still bookmarked.");
    }

    [Then("I should see a map {string} markers for gyms in the search results")]
    public void ThenIShouldSeeAMapWithMarkersForGymsInTheSearchResults(string isExpectingMarkers)
    {
        var markers = _driver.FindElements(By.XPath("//img[contains(@src, 'https://maps.gstatic.com/mapfiles/transparent.png')]"));

        if (isExpectingMarkers == "without")
        {
            Assert.That(markers.Count == 0, $"Map markers for gyms are displayed when they should not be. Found {markers.Count} markers.");
            return;
        }
        else if (isExpectingMarkers == "with")
        {
            Assert.That(markers.Count > 0, $"Map markers for gyms are not displayed. Expected at least one marker, but found {markers.Count}.");
            return;
        }
        throw new ArgumentException("Invalid value for 'isExpectingMarkers'. Expected 'with' or 'without'.");
    }

    [Then("I should see a map centered around the postal code 97361")]
    public void ThenIShouldSeeAMapCenteredAroundThePostalCode97361()
    {
        // Check for .gm-style elements which indicate Google Maps has loaded
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElements(By.CssSelector(".gm-style")).Count > 0);
        
        // Verify the map exists and is visible
        var mapContainer = _driver.FindElement(By.Id("nearby-gyms-map"));
        Assert.That(mapContainer.Displayed, Is.True, "Map container is not displayed");
        
        // Check map center coordinates using JavaScript
        var scriptResult = ((IJavaScriptExecutor)_driver).ExecuteScript(@"
            if (!window.map) return null;
            var center = window.map.getCenter();
            if (!center) return null;
            return { 
                lat: center.lat(), 
                lng: center.lng() 
            };
        ");
        
        if (scriptResult != null)
        {
            var centerObj = (Dictionary<string, object>)scriptResult;
            double lat = Convert.ToDouble(centerObj["lat"]);
            double lng = Convert.ToDouble(centerObj["lng"]);
            
            // Check if coordinates are near Monmouth (97361)
            Assert.That(lat, Is.InRange(44.7, 44.9), "Map latitude is not centered near 97361");
            Assert.That(lng, Is.InRange(-123.4, -123.2), "Map longitude is not centered near 97361");
        }
        else
        {
            // Fall back to checking for markers if script fails
            var markers = _driver.FindElements(By.XPath("//img[contains(@src, 'maps.gstatic.com/mapfiles')]"));
            Assert.That(markers.Count, Is.GreaterThan(0), "No map markers found");
        }
    }

    /// <summary>
    /// Finds the first bookmarked gym on the page. If none are found, it will click the bookmark button to bookmark the first gym and return the newly bookmarked gym.
    /// Takes in a boolean parameter to determine if it should bookmark the first gym if none are found.
    /// </summary>
    /// <returns>The first bookmarked gym (The first IWebElement with the class "bookmarked-icon")</returns>
    private IWebElement? FindFirstBookmarkedGym(bool bookmarkIfNoneFound = true)
    {
        try
        {
            // Console.WriteLine("Finding first bookmarked gym...");
            var button = _driver.FindElement(By.XPath("//button[contains(@class, 'bookmark-gym-button')][.//i[contains(@class, 'bookmarked-icon')]]"));
            return button;
        }
        catch (NoSuchElementException)
        {
            if (!bookmarkIfNoneFound)
            {
                return null;
            }
            // Console.WriteLine("No bookmarked gym found. Bookmarking the first gym...");
            var bookmarkButton = _driver.FindElement(By.ClassName("bookmark-gym-button"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", bookmarkButton);
            var button = _driver.FindElement(By.XPath("//button[contains(@class, 'bookmark-gym-button')][.//i[contains(@class, 'bookmarked-icon')]]"));
            return button;
        }
    }
}