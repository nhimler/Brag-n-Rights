using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("WorkoutPlan")]
public partial class WorkoutPlan
{
    [Key]
    [Column("WorkoutPlanID")]
    public int WorkoutPlanId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [StringLength(255)]
    public string? PlanName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [StringLength(50)]
    public string? Frequency { get; set; }

    [StringLength(255)]
    public string? Goal { get; set; }

    [StringLength(20)]
    public string? DifficultyLevel { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("WorkoutPlans")]
    public virtual User? User { get; set; }

    [ForeignKey("WorkoutPlanId")]
    [InverseProperty("WorkoutPlans")]
    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
