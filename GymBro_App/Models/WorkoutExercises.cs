using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("WorkoutExercises")]
public partial class WorkoutExercises
{
    [Key]
    [Column("WorkoutExercisesID")]
    public int WorkoutExercisesID { get; set; }

    [Column("WorkoutPlanID")]
    public int WorkoutPlanId { get; set; }

    [Column("WorkoutPlanExerciseID")]
    public int WorkoutPlanExerciseId { get; set; }

    [ForeignKey("WorkoutPlanID")]
    [InverseProperty("WorkoutExercises")]
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;

    [ForeignKey("WorkoutPlanExerciseID")]
    [InverseProperty("WorkoutExercises")]
    public virtual WorkoutPlanExercise WorkoutPlanExercise { get; set; } = null!;
}