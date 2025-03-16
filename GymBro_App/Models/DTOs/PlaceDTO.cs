using System.Text.Json.Serialization;

namespace GymBro_App.Models.DTOs
{
    public class Close
    {
        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("hour")]
        public int Hour { get; set; }

        [JsonPropertyName("minute")]
        public int Minute { get; set; }
    }

    public class Open
    {
        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("hour")]
        public int Hour { get; set; }

        [JsonPropertyName("minute")]
        public int Minute { get; set; }
    }

    public class Period
    {
        [JsonPropertyName("open")]
        public Open Open { get; set; } = new Open();

        [JsonPropertyName("close")]
        public Close Close { get; set; } = new Close();
    }

    public class DisplayName
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        [JsonPropertyName("languageCode")]
        public string LanguageCode { get; set; } = "";
    }

    public class RegularOpeningHours
    {
        public bool OpenNow { get; set; } = false;
        public List<Period> Periods { get; set; } = [];
        public List<string> WeekdayDescriptions { get; set; } = [];
        public DateTime NextCloseTime { get; set; } = new DateTime();
    }
    
    public class PlaceDTO
    {
        public DisplayName DisplayName { get; set; } = new DisplayName();
        public string FormattedAddress { get; set; } = "";
        public RegularOpeningHours RegularOpeningHours { get; set; } = new RegularOpeningHours();
        public string WebsiteUri { get; set; } = "";
    }
}

// namespace GymBro_App.Models.DTOs
// {
//         // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
//     public class AccessibilityOptions
//     {
//         [JsonPropertyName("wheelchairAccessibleParking")]
//         public bool WheelchairAccessibleParking { get; set; }

//         [JsonPropertyName("wheelchairAccessibleEntrance")]
//         public bool WheelchairAccessibleEntrance { get; set; }

//         [JsonPropertyName("wheelchairAccessibleRestroom")]
//         public bool? WheelchairAccessibleRestroom { get; set; }
//     }

//     public class AddressComponent
//     {
//         [JsonPropertyName("longText")]
//         public string LongText { get; set; } = "";

//         [JsonPropertyName("shortText")]
//         public string ShortText { get; set; } = "";

//         [JsonPropertyName("types")]
//         public List<string> Types { get; set; } = new List<string>();

//         [JsonPropertyName("languageCode")]
//         public string LanguageCode { get; set; } = "";
//     }

//     public class AddressDescriptor
//     {
//         [JsonPropertyName("landmarks")]
//         public List<Landmark> Landmarks { get; set; } = new List<Landmark>();

//         [JsonPropertyName("areas")]
//         public List<Area> Areas { get; set; } = new List<Area>();
//     }

//     public class Area
//     {
//         [JsonPropertyName("name")]
//         public string Name { get; set; } = "";

//         [JsonPropertyName("placeId")]
//         public string PlaceId { get; set; } = "";

//         [JsonPropertyName("displayName")]
//         public DisplayName DisplayName { get; set; } = new DisplayName();

//         [JsonPropertyName("containment")]
//         public string Containment { get; set; } = "";
//     }

//     public class AuthorAttribution
//     {
//         [JsonPropertyName("displayName")]
//         public string DisplayName { get; set; } = "";

//         [JsonPropertyName("uri")]
//         public string Uri { get; set; } = "";

//         [JsonPropertyName("photoUri")]
//         public string PhotoUri { get; set; } = "";
//     }

//     public class AuthorAttribution2
//     {
//         [JsonPropertyName("displayName")]
//         public string DisplayName { get; set; } = "";

//         [JsonPropertyName("uri")]
//         public string Uri { get; set; } = "";

//         [JsonPropertyName("photoUri")]
//         public string PhotoUri { get; set; } = "";
//     }

//     public class Close
//     {
//         [JsonPropertyName("day")]
//         public int Day { get; set; }

//         [JsonPropertyName("hour")]
//         public int Hour { get; set; }

//         [JsonPropertyName("minute")]
//         public int Minute { get; set; }

//         [JsonPropertyName("date")]
//         public Date Date { get; set; } = new Date();

//         [JsonPropertyName("truncated")]
//         public bool? Truncated { get; set; }
//     }

//     public class CurrentOpeningHours
//     {
//         [JsonPropertyName("openNow")]
//         public bool OpenNow { get; set; }

//         [JsonPropertyName("periods")]
//         public List<Period> Periods { get; set; } = new List<Period>();

//         [JsonPropertyName("weekdayDescriptions")]
//         public List<string> WeekdayDescriptions { get; set; } = new List<string>();

//         [JsonPropertyName("nextCloseTime")]
//         public DateTime NextCloseTime { get; set; } = DateTime.MinValue;

//         [JsonPropertyName("nextOpenTime")]
//         public DateTime? NextOpenTime { get; set; }
//     }

//     public class Date
//     {
//         [JsonPropertyName("year")]
//         public int Year { get; set; }

//         [JsonPropertyName("month")]
//         public int Month { get; set; }

//         [JsonPropertyName("day")]
//         public int Day { get; set; }
//     }

//     public class DisplayName
//     {
//         [JsonPropertyName("text")]
//         public string Text { get; set; } = "";

//         [JsonPropertyName("languageCode")]
//         public string LanguageCode { get; set; } = "";
//     }

//     public class GenerativeSummary
//     {
//         [JsonPropertyName("overview")]
//         public Overview Overview { get; set; } = new Overview();

//         [JsonPropertyName("overviewFlagContentUri")]
//         public string OverviewFlagContentUri { get; set; } = "";
//     }

//     public class GoogleMapsLinks
//     {
//         [JsonPropertyName("directionsUri")]
//         public string DirectionsUri { get; set; } = "";

//         [JsonPropertyName("placeUri")]
//         public string PlaceUri { get; set; } = "";

//         [JsonPropertyName("writeAReviewUri")]
//         public string WriteAReviewUri { get; set; } = "";

//         [JsonPropertyName("reviewsUri")]
//         public string ReviewsUri { get; set; } = "";

//         [JsonPropertyName("photosUri")]
//         public string PhotosUri { get; set; } = "";
//     }

//     public class High
//     {
//         [JsonPropertyName("latitude")]
//         public double Latitude { get; set; }

//         [JsonPropertyName("longitude")]
//         public double Longitude { get; set; }
//     }

//     public class Landmark
//     {
//         [JsonPropertyName("name")]
//         public string Name { get; set; } = "";

//         [JsonPropertyName("placeId")]
//         public string PlaceId { get; set; } = "";

//         [JsonPropertyName("displayName")]
//         public DisplayName DisplayName { get; set; } = new DisplayName();

//         [JsonPropertyName("types")]
//         public List<string> Types { get; set; } = new List<string>();

//         [JsonPropertyName("straightLineDistanceMeters")]
//         public double StraightLineDistanceMeters { get; set; }

//         [JsonPropertyName("travelDistanceMeters")]
//         public double? TravelDistanceMeters { get; set; }

//         [JsonPropertyName("spatialRelationship")]
//         public string SpatialRelationship { get; set; } = "";
//     }

//     public class Location
//     {
//         [JsonPropertyName("latitude")]
//         public double Latitude { get; set; }

//         [JsonPropertyName("longitude")]
//         public double Longitude { get; set; }
//     }

//     public class Low
//     {
//         [JsonPropertyName("latitude")]
//         public double Latitude { get; set; }

//         [JsonPropertyName("longitude")]
//         public double Longitude { get; set; }
//     }

//     public class Open
//     {
//         [JsonPropertyName("day")]
//         public int Day { get; set; }

//         [JsonPropertyName("hour")]
//         public int Hour { get; set; }

//         [JsonPropertyName("minute")]
//         public int Minute { get; set; }

//         [JsonPropertyName("date")]
//         public Date Date { get; set; } = new Date();

//         [JsonPropertyName("truncated")]
//         public bool? Truncated { get; set; }
//     }

//     public class OriginalText
//     {
//         [JsonPropertyName("text")]
//         public string Text { get; set; } = "";

//         [JsonPropertyName("languageCode")]
//         public string LanguageCode { get; set; } = "";
//     }

//     public class Overview
//     {
//         [JsonPropertyName("text")]
//         public string Text { get; set; } = "";

//         [JsonPropertyName("languageCode")]
//         public string LanguageCode { get; set; } = "";
//     }

//     public class ParkingOptions
//     {
//         [JsonPropertyName("valetParking")]
//         public bool ValetParking { get; set; }
//     }

//     public class Period
//     {
//         [JsonPropertyName("open")]
//         public Open Open { get; set; } = new Open();

//         [JsonPropertyName("close")]
//         public Close Close { get; set; } = new Close();
//     }

//     public class Photo
//     {
//         [JsonPropertyName("name")]
//         public string Name { get; set; } = "";

//         [JsonPropertyName("widthPx")]
//         public int WidthPx { get; set; }

//         [JsonPropertyName("heightPx")]
//         public int HeightPx { get; set; }

//         [JsonPropertyName("authorAttributions")]
//         public List<AuthorAttribution> AuthorAttributions { get; set; } = new List<AuthorAttribution>();

//         [JsonPropertyName("flagContentUri")]
//         public string FlagContentUri { get; set; } = "";

//         [JsonPropertyName("googleMapsUri")]
//         public string GoogleMapsUri { get; set; } = "";
//     }

//     public class Place
//     {
//         [JsonPropertyName("name")]
//         public string Name { get; set; } = "";

//         [JsonPropertyName("id")]
//         public string Id { get; set; } = "";

//         [JsonPropertyName("types")]
//         public List<string> Types { get; set; } = new List<string>();

//         [JsonPropertyName("nationalPhoneNumber")]
//         public string NationalPhoneNumber { get; set; } = "";

//         [JsonPropertyName("internationalPhoneNumber")]
//         public string InternationalPhoneNumber { get; set; } = "";

//         [JsonPropertyName("formattedAddress")]
//         public string FormattedAddress { get; set; } = "";

//         [JsonPropertyName("addressComponents")]
//         public List<AddressComponent> AddressComponents { get; set; } = new List<AddressComponent>();

//         [JsonPropertyName("plusCode")]
//         public PlusCode PlusCode { get; set; } = new PlusCode();

//         [JsonPropertyName("location")]
//         public Location Location { get; set; } = new Location();

//         [JsonPropertyName("viewport")]
//         public Viewport Viewport { get; set; } = new Viewport();

//         [JsonPropertyName("rating")]
//         public double Rating { get; set; }

//         [JsonPropertyName("googleMapsUri")]
//         public string GoogleMapsUri { get; set; } = "";

//         [JsonPropertyName("websiteUri")]
//         public string WebsiteUri { get; set; } = "";

//         [JsonPropertyName("regularOpeningHours")]
//         public RegularOpeningHours RegularOpeningHours { get; set; } = new RegularOpeningHours();

//         [JsonPropertyName("utcOffsetMinutes")]
//         public int UtcOffsetMinutes { get; set; }

//         [JsonPropertyName("adrFormatAddress")]
//         public string AdrFormatAddress { get; set; } = "";

//         [JsonPropertyName("businessStatus")]
//         public string BusinessStatus { get; set; } = "";

//         [JsonPropertyName("userRatingCount")]
//         public int UserRatingCount { get; set; }

//         [JsonPropertyName("iconMaskBaseUri")]
//         public string IconMaskBaseUri { get; set; } = "";

//         [JsonPropertyName("iconBackgroundColor")]
//         public string IconBackgroundColor { get; set; } = "";

//         [JsonPropertyName("displayName")]
//         public DisplayName DisplayName { get; set; } = new DisplayName();

//         [JsonPropertyName("primaryTypeDisplayName")]
//         public PrimaryTypeDisplayName PrimaryTypeDisplayName { get; set; } = new PrimaryTypeDisplayName();

//         [JsonPropertyName("currentOpeningHours")]
//         public CurrentOpeningHours CurrentOpeningHours { get; set; } = new CurrentOpeningHours();

//         [JsonPropertyName("primaryType")]
//         public string PrimaryType { get; set; } = "";

//         [JsonPropertyName("shortFormattedAddress")]
//         public string ShortFormattedAddress { get; set; } = "";

//         [JsonPropertyName("reviews")]
//         public List<Review> Reviews { get; set; } = new List<Review>();

//         [JsonPropertyName("photos")]
//         public List<Photo> Photos { get; set; } = new List<Photo>();

//         [JsonPropertyName("parkingOptions")]
//         public ParkingOptions ParkingOptions { get; set; } = new ParkingOptions();

//         [JsonPropertyName("accessibilityOptions")]
//         public AccessibilityOptions AccessibilityOptions { get; set; } = new AccessibilityOptions();

//         [JsonPropertyName("generativeSummary")]
//         public GenerativeSummary GenerativeSummary { get; set; } = new GenerativeSummary();

//         [JsonPropertyName("addressDescriptor")]
//         public AddressDescriptor AddressDescriptor { get; set; } = new AddressDescriptor();

//         [JsonPropertyName("googleMapsLinks")]
//         public GoogleMapsLinks GoogleMapsLinks { get; set; } = new GoogleMapsLinks();

//         [JsonPropertyName("timeZone")]
//         public TimeZone TimeZone { get; set; } = new TimeZone();

//         [JsonPropertyName("goodForChildren")]
//         public bool? GoodForChildren { get; set; }

//         [JsonPropertyName("allowsDogs")]
//         public bool? AllowsDogs { get; set; }

//         [JsonPropertyName("restroom")]
//         public bool? Restroom { get; set; }
//     }

//     public class PlusCode
//     {
//         [JsonPropertyName("globalCode")]
//         public string GlobalCode { get; set; } = "";

//         [JsonPropertyName("compoundCode")]
//         public string CompoundCode { get; set; } = "";
//     }

//     public class PrimaryTypeDisplayName
//     {
//         [JsonPropertyName("text")]
//         public string Text { get; set; } = "";

//         [JsonPropertyName("languageCode")]
//         public string LanguageCode { get; set; } = "";
//     }

//     public class RegularOpeningHours
//     {
//         [JsonPropertyName("openNow")]
//         public bool OpenNow { get; set; }

//         [JsonPropertyName("periods")]
//         public List<Period> Periods { get; set; } = new List<Period>();

//         [JsonPropertyName("weekdayDescriptions")]
//         public List<string> WeekdayDescriptions { get; set; } = new List<string>();

//         [JsonPropertyName("nextCloseTime")]
//         public DateTime NextCloseTime { get; set; } = DateTime.MinValue;

//         [JsonPropertyName("nextOpenTime")]
//         public DateTime? NextOpenTime { get; set; }
//     }

//     public class Review
//     {
//         [JsonPropertyName("name")]
//         public string Name { get; set; } = "";

//         [JsonPropertyName("relativePublishTimeDescription")]
//         public string RelativePublishTimeDescription { get; set; } = "";

//         [JsonPropertyName("rating")]
//         public int Rating { get; set; }

//         [JsonPropertyName("text")]
//         public Text Text { get; set; } = new Text();

//         [JsonPropertyName("originalText")]
//         public OriginalText OriginalText { get; set; } = new OriginalText();

//         [JsonPropertyName("authorAttribution")]
//         public AuthorAttribution AuthorAttribution { get; set; } = new AuthorAttribution();

//         [JsonPropertyName("publishTime")]
//         public DateTime PublishTime { get; set; } = DateTime.MinValue;

//         [JsonPropertyName("flagContentUri")]
//         public string FlagContentUri { get; set; } = "";

//         [JsonPropertyName("googleMapsUri")]
//         public string GoogleMapsUri { get; set; } = "";
//     }

//     public class Root
//     {
//         [JsonPropertyName("places")]
//         public List<Place> Places { get; set; } = new List<Place>();
//     }

//     public class Text
//     {
//         [JsonPropertyName("text")]
//         public string Content { get; set; } = "";

//         [JsonPropertyName("languageCode")]
//         public string LanguageCode { get; set; } = "";
//     }

//     public class TimeZone
//     {
//         [JsonPropertyName("id")]
//         public string Id { get; set; } = "";
//     }

//     public class Viewport
//     {
//         [JsonPropertyName("low")]
//         public Low Low { get; set; } = new Low();

//         [JsonPropertyName("high")]
//         public High High { get; set; } = new High();
//     }


//     public class Place
//     {
//         [JsonPropertyName("name")]
//         public string Name { get; set; } = "";

//         [JsonPropertyName("id")]
//         public string Id { get; set; } = "";

//         [JsonPropertyName("types")]
//         public List<string> Types { get; set; } = new List<string>();

//         [JsonPropertyName("nationalPhoneNumber")]
//         public string NationalPhoneNumber { get; set; } = "";

//         [JsonPropertyName("internationalPhoneNumber")]
//         public string InternationalPhoneNumber { get; set; } = "";

//         [JsonPropertyName("formattedAddress")]
//         public string FormattedAddress { get; set; } = "";

//         [JsonPropertyName("addressComponents")]
//         public List<AddressComponent> AddressComponents { get; set; } = new List<AddressComponent>();

//         [JsonPropertyName("plusCode")]
//         public PlusCode PlusCode { get; set; } = new PlusCode();

//         [JsonPropertyName("location")]
//         public Location Location { get; set; } = new Location();

//         [JsonPropertyName("viewport")]
//         public Viewport Viewport { get; set; } = new Viewport();

//         [JsonPropertyName("rating")]
//         public double Rating { get; set; }

//         [JsonPropertyName("googleMapsUri")]
//         public string GoogleMapsUri { get; set; } = "";

//         [JsonPropertyName("websiteUri")]
//         public string WebsiteUri { get; set; } = "";

//         [JsonPropertyName("regularOpeningHours")]
//         public RegularOpeningHours RegularOpeningHours { get; set; } = new RegularOpeningHours();

//         [JsonPropertyName("utcOffsetMinutes")]
//         public int UtcOffsetMinutes { get; set; }

//         [JsonPropertyName("adrFormatAddress")]
//         public string AdrFormatAddress { get; set; } = "";

//         [JsonPropertyName("businessStatus")]
//         public string BusinessStatus { get; set; } = "";

//         [JsonPropertyName("userRatingCount")]
//         public int UserRatingCount { get; set; }

//         [JsonPropertyName("iconMaskBaseUri")]
//         public string IconMaskBaseUri { get; set; } = "";

//         [JsonPropertyName("iconBackgroundColor")]
//         public string IconBackgroundColor { get; set; } = "";

//         [JsonPropertyName("displayName")]
//         public DisplayName DisplayName { get; set; } = new DisplayName();

//         [JsonPropertyName("primaryTypeDisplayName")]
//         public PrimaryTypeDisplayName PrimaryTypeDisplayName { get; set; } = new PrimaryTypeDisplayName();

//         [JsonPropertyName("currentOpeningHours")]
//         public CurrentOpeningHours CurrentOpeningHours { get; set; } = new CurrentOpeningHours();

//         [JsonPropertyName("primaryType")]
//         public string PrimaryType { get; set; } = "";

//         [JsonPropertyName("shortFormattedAddress")]
//         public string ShortFormattedAddress { get; set; } = "";

//         [JsonPropertyName("reviews")]
//         public List<Review> Reviews { get; set; } = new List<Review>();

//         [JsonPropertyName("photos")]
//         public List<Photo> Photos { get; set; } = new List<Photo>();

//         [JsonPropertyName("parkingOptions")]
//         public ParkingOptions ParkingOptions { get; set; } = new ParkingOptions();

//         [JsonPropertyName("accessibilityOptions")]
//         public AccessibilityOptions AccessibilityOptions { get; set; } = new AccessibilityOptions();

//         [JsonPropertyName("generativeSummary")]
//         public GenerativeSummary GenerativeSummary { get; set; } = new GenerativeSummary();

//         [JsonPropertyName("addressDescriptor")]
//         public AddressDescriptor AddressDescriptor { get; set; } = new AddressDescriptor();

//         [JsonPropertyName("googleMapsLinks")]
//         public GoogleMapsLinks GoogleMapsLinks { get; set; } = new GoogleMapsLinks();

//         [JsonPropertyName("timeZone")]
//         public TimeZone TimeZone { get; set; } = new TimeZone();

//         [JsonPropertyName("goodForChildren")]
//         public bool? GoodForChildren { get; set; }

//         [JsonPropertyName("allowsDogs")]
//         public bool? AllowsDogs { get; set; }

//         [JsonPropertyName("restroom")]
//         public bool? Restroom { get; set; }
//     }
// }