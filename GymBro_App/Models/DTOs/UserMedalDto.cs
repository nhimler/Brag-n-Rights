public class UserMedalDto
{
    public int UserMedalId { get; set; }
    public DateOnly EarnedDate { get; set; }
    public int MedalId { get; set; }
    public string MedalImage { get; set; } = string.Empty;
    public string MedalName { get; set; } = string.Empty;
}
