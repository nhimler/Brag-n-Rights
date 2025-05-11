// GlobalSetup.cs
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BDD_Tests
{
    [SetUpFixture]
    public class GlobalDriverSetup
    {
        public static IWebDriver Driver { get; private set; }

        [OneTimeSetUp]
        public void InitDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Run in headless mode (no UI)
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            Driver = new ChromeDriver(options);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        [OneTimeTearDown]
        public void QuitDriver()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
            }
        }
    }
}