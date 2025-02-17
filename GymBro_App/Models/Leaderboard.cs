using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("Leaderboard")]
public partial class Leaderboard
{
    [Key]
    [Column("LeaderboardID")]
    public int LeaderboardId { get; set; }

    [Column("ChallengeID")]
    public int? ChallengeId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    public int? Rank { get; set; }

    public int? Score { get; set; }

    [ForeignKey("ChallengeId")]
    [InverseProperty("Leaderboards")]
    public virtual FitnessChallenge? Challenge { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Leaderboards")]
    public virtual User? User { get; set; }
}
