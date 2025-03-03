using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymBro_App.Models;

[Table("Medal")]
public partial class Medal
{
    [Key]
    [Column("MedalID")]
    public int MedalId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public int StepThreshold { get; set; }

    [StringLength(255)]
    public string Image { get; set; } = null!;

    [InverseProperty("Medal")]
    public virtual ICollection<UserMedal> UserMedals { get; set; } = new List<UserMedal>();
}
