namespace GymBro_App.ViewModels
{
    public class UserCompetitionViewModel
    {
        public int CompetitionID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public List<ParticipantViewModel> Participants { get; set; } = new();
    }

    public class ParticipantViewModel
    {
        public string Username { get; set; } = string.Empty;
        public int Steps { get; set; }
    }
}