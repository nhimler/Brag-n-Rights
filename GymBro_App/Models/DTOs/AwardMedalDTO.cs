namespace GymBro_App.Models.DTOs
{
    // Represents the awarded medals for a user
    public class AwardMedal
    {
        public int UserId { get; set; }
        public List<AwardMedalDetails> AwardedMedals { get; set; }
    }

    public class AwardMedalDetails
    {
        public int MedalId { get; set; }
        public string MedalName { get; set; }
        public string MedalImage { get; set; }  // URL to the medal's image
        public string MedalDescription { get; set; }
        // New fields for step threshold and progress
        public int StepThreshold { get; set; }

        // New fields for locked medals
        public bool Locked { get; set; }  // Indicates if the medal is locked
        public int StepsRemaining { get; set; }  // Not nullable  // Steps remaining to unlock the medal
    }
}
