using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("WorkoutPlanExercise")]
public partial class WorkoutPlanExercise
{
    [Key]
    [Column("WorkoutPlanExerciseID")]
    public int WorkoutPlanExerciseId { get; set; }

    [Column("WorkoutPlanID")]
    public int? WorkoutPlanId { get; set; }

    [Column("ApiID")]
    public int? ApiId { get; set; }

    public int? Reps { get; set; }

    public int? Sets { get; set; }

    [InverseProperty("WorkoutPlanExercise")]
    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

    [ForeignKey("WorkoutPlanId")]
    [InverseProperty("WorkoutPlanExercises")]
    public virtual WorkoutPlan? WorkoutPlan { get; set; }
}
