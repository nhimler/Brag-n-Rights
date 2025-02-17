using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("FitnessChallenge")]
public partial class FitnessChallenge
{
    [Key]
    [Column("ChallengeID")]
    public int ChallengeId { get; set; }

    [StringLength(255)]
    public string? ChallengeName { get; set; }

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [StringLength(255)]
    public string? Goal { get; set; }

    public string? Prize { get; set; }

    [InverseProperty("Challenge")]
    public virtual ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();

    [ForeignKey("ChallengeId")]
    [InverseProperty("Challenges")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
