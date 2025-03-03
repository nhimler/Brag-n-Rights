namespace GymBro_App.Models.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; } = 0;
        public string IdentityUserId { get; set; } = "";
        public string Username { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public int Age { get; set; } = 0;
        public string Gender { get; set; } = "";
        public decimal Weight { get; set; } = 0.0m;
        public decimal Height { get; set; } = 0.0m;
        public string FitnessLevel { get; set; } = "";
        public string Fitnessgoals { get; set; } = "";
        public DateTime AccountCreationDate { get; set; } = new DateTime();
        public DateTime LastLogin { get; set; } = new DateTime();
        public byte[] ProfilePicture { get; set; } = new byte[0];
        public string PreferredWorkoutTime { get; set; } = "";
        public string Location { get; set; } = "";
        public decimal Longitude { get; set; } = 0.0m;
        public decimal Latitude { get; set; } = 0.0m;
    }
}