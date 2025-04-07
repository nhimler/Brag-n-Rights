namespace GymBro_App.Models.DTOs
{
    public class ExerciseDTO
    {
        public string BodyPart { get; set; } = "";
        public string Equipment { get; set; } = "";
        public string GifUrl { get; set; } = "";
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Target { get; set; } = "";
        public List<string> SecondaryMuscles { get; set; } = new List<string>();
        public List<string> Instructions { get; set; } = new List<string>();
    }
}