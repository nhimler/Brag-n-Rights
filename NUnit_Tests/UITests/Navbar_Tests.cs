using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using GymBro_App;

namespace UI_Tests;

[TestFixture]
public class NavbarTests
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task Navbar_ShouldContain_ExpectedLinks()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var links = doc.DocumentNode.SelectNodes("//nav//ul//li//a");
        Assert.NotNull(links);
        Assert.IsTrue(links.Any(l => l.InnerText.Contains("Home")));
        Assert.IsTrue(links.Any(l => l.InnerText.Contains("User Page")));
        Assert.IsTrue(links.Any(l => l.InnerText.Contains("Meal Plan")));
        Assert.IsTrue(links.Any(l => l.InnerText.Contains("Workout Plan")));
        Assert.IsTrue(links.Any(l => l.InnerText.Contains("Step Medals")));
    }
}
