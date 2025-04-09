using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("WorkoutPlan")]
[Index("ApiId", Name = "UQ__WorkoutP__024B3BD2ADBD4BBB", IsUnique = true)]
public partial class WorkoutPlan
{
    [Key]
    [Column("WorkoutPlanID")]
    public int WorkoutPlanId { get; set; }

    [Column("WorkoutPlanExerciseID")]
    public int? WorkoutPlanExerciseId { get; set; }

    [Column("ApiID")]
    [StringLength(255)]
    public string? ApiId { get; set; }

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

    public int? IsCompleted { get; set; }

    [StringLength(20)]
    public string? DifficultyLevel { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("WorkoutPlans")]
    public virtual User? User { get; set; }

    [InverseProperty("WorkoutPlan")]
    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

    [InverseProperty("WorkoutPlan")]
    public virtual ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
}
