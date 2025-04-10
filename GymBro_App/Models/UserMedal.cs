using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("UserMedal")]
public partial class UserMedal
{
    [Key]
    [Column("UserMedalID")]
    public int UserMedalId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("MedalID")]
    public int MedalId { get; set; }

    public DateOnly EarnedDate { get; set; }

    [ForeignKey("MedalId")]
    [InverseProperty("UserMedals")]
    public virtual Medal Medal { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserMedals")]
    public virtual User User { get; set; } = null!;
}
