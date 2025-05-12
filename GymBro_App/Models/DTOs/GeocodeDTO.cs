using System.Text.Json.Serialization;

namespace GymBro_App.Models.DTOs
{
    // Yes, all of this this could have been placed in GoogleMapsService.cs, but this makes testing easier
    public class GeocodeDTO
    {
        [JsonPropertyName("plus_code")]
        public PlusCode PlusCode { get; set; } = new PlusCode();

        [JsonPropertyName("results")]
        public List<Result> Results { get; set; } = [];

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
    }

    public class AddressComponent
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; } = "";

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; } = "";

        [JsonPropertyName("types")]
        public List<string> Types { get; set; } = [];
    }

    public class Bounds
    {
        [JsonPropertyName("northeast")]
        public Northeast Northeast { get; set; } = new Northeast();

        [JsonPropertyName("southwest")]
        public Southwest Southwest { get; set; } = new Southwest();
    }

    public class Geometry
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; } = new Location();

        [JsonPropertyName("location_type")]
        public string LocationType { get; set; } = "";

        [JsonPropertyName("viewport")]
        public Viewport Viewport { get; set; } = new Viewport();

        [JsonPropertyName("bounds")]
        public Bounds Bounds { get; set; } = new Bounds();
    }

    public class Location
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    public class NavigationPoint
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; } = new Location();
    }

    public class Northeast
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public class PlusCode
    {
        [JsonPropertyName("compound_code")]
        public string CompoundCode { get; set; } = "";

        [JsonPropertyName("global_code")]
        public string GlobalCode { get; set; } = "";
    }

    public class Result
    {
        [JsonPropertyName("address_components")]
        public List<AddressComponent> AddressComponents { get; set; } = [];

        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; } = "";

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; } = new Geometry();

        [JsonPropertyName("navigation_points")]
        public List<NavigationPoint> NavigationPoints { get; set; } = [];

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = "";

        [JsonPropertyName("plus_code")]
        public PlusCode PlusCode { get; set; } = new PlusCode();

        [JsonPropertyName("types")]
        public List<string> Types { get; set; } = [];
    }

    public class Southwest
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public class Viewport
    {
        [JsonPropertyName("northeast")]
        public Northeast Northeast { get; set; } = new Northeast();

        [JsonPropertyName("southwest")]
        public Southwest Southwest { get; set; } = new Southwest();
    }
}
