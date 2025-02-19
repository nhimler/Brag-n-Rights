using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using GymBro_App;

public class NavbarTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public NavbarTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Navbar_ShouldContain_ExpectedLinks()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var links = doc.DocumentNode.SelectNodes("//nav//ul//li//a");
        Xunit.Assert.NotNull(links);
        Xunit.Assert.Contains(links, l => l.InnerText.Contains("Home"));
        Xunit.Assert.Contains(links, l => l.InnerText.Contains("User Page"));
        Xunit.Assert.Contains(links, l => l.InnerText.Contains("Meal Plan"));
        Xunit.Assert.Contains(links, l => l.InnerText.Contains("Workout Plan"));
        Xunit.Assert.Contains(links, l => l.InnerText.Contains("Check Medals"));
    }
}
