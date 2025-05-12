using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("GymUser")]
public partial class GymUser
{
    [Key]
    [Column("GymUserID")]
    public int GymUserId { get; set; }

    [Column("ApiGymID")]
    public string ApiGymId { get; set; } = null!;

    [Column("UserID")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("GymUsers")]
    public virtual User User { get; set; } = null!;
}
