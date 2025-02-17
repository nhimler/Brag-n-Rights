namespace GymBro_App.Models.DTOs
{
    // Represents the awarded medals for a user
    public class AwardMedal
    {
        public int UserId { get; set; }
        public List<AwardMedalDetails> AwardedMedals { get; set; }
    }

    // Represents details about a specific awarded medal
    public class AwardMedalDetails
    {
        public int MedalId { get; set; }
        public string MedalName { get; set; }
        public string MedalImage { get; set; }  // URL to the medal's image
        public DateTime AwardedDate { get; set; }

        // New fields for step threshold and progress
        public int StepThreshold { get; set; }
        public double ProgressPercentage { get; set; }
    }
}
