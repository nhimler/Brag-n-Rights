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

    [Column("Reps")]
    public int? Reps { get; set; }

    [Column("Sets")]
    public int? Sets { get; set; }

    [ForeignKey("WorkoutPlanId")]
    [InverseProperty("WorkoutPlanExercises")]
    public virtual WorkoutPlan? WorkoutPlan { get; set; }

}