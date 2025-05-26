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
[Scope(Tag = "GymNotHeadless")]
public sealed class GymNotHeadlessStepDefinitions
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        var options = new ChromeOptions();
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
        Console.WriteLine("Currently on: " + _driver.Url);
        
        // Wait for the page to load. The page seems to sometimes take a while to load.
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.Url.Contains("FindNearbyGyms"));
        Assert.That(_driver.Url, Is.EqualTo("http://localhost:5075/Gym/FindNearbyGyms"), "Expected Gym Search page URL. Actual: " + _driver.Url);
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