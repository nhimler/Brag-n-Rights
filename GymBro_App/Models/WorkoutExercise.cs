using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

public partial class WorkoutExercise
{
    [Key]
    [Column("WorkoutExercisesID")]
    public int WorkoutExercisesId { get; set; }

    [Column("WorkoutPlanID")]
    public int? WorkoutPlanId { get; set; }

    [Column("WorkoutPlanExerciseID")]
    public int? WorkoutPlanExerciseId { get; set; }

    [ForeignKey("WorkoutPlanId")]
    [InverseProperty("WorkoutExercises")]
    public virtual WorkoutPlan? WorkoutPlan { get; set; }

    [ForeignKey("WorkoutPlanExerciseId")]
    [InverseProperty("WorkoutExercises")]
    public virtual WorkoutPlanExercise? WorkoutPlanExercise { get; set; }
}
