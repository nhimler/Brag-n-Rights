using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("User")]
[Index("Email", Name = "UQ__User__A9D10534C8A0007A", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(450)]
    public string? IdentityUserId { get; set; }

    [StringLength(100)]
    public string? Username { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Weight { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Height { get; set; }

    [StringLength(20)]
    public string? FitnessLevel { get; set; }

    [StringLength(255)]
    public string? Fitnessgoals { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AccountCreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    public byte[]? ProfilePicture { get; set; }

    [StringLength(20)]
    public string? PreferredWorkoutTime { get; set; }

    [StringLength(255)]
    public string? Location { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? Longitude { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<BiometricDatum> BiometricData { get; set; } = new List<BiometricDatum>();

    [InverseProperty("User")]
    public virtual ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();

    [InverseProperty("User")]
    public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();

    [InverseProperty("User")]
    public virtual Token? Token { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<UserMedal> UserMedals { get; set; } = new List<UserMedal>();

    [InverseProperty("User")]
    public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<FitnessChallenge> Challenges { get; set; } = new List<FitnessChallenge>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Gym> Gyms { get; set; } = new List<Gym>();
}
