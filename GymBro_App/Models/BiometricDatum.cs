using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

public partial class BiometricDatum
{
    [Key]
    [Column("BiometricID")]
    public int BiometricId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    public DateOnly? Date { get; set; }

    public int? Steps { get; set; }

    public int? CaloriesBurned { get; set; }

    public int? HeartRate { get; set; }

    public int? SleepDuration { get; set; }

    public int? ActiveMinutes { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("BiometricData")]
    public virtual User? User { get; set; }
}
