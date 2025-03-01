using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymBro_App.Models;
[Table("BiometricData")]
public partial class BiometricDatum
{
    [Key]
    [Column("BiometricID")]
    public int BiometricId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    public int? Steps { get; set; }

    public int? CaloriesBurned { get; set; }

    public int? HeartRate { get; set; }

    public int? SleepDuration { get; set; }

    public int? ActiveMinutes { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdated { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("BiometricData")]
    public virtual User? User { get; set; }
}
