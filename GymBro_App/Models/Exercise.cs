using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("Exercise")]
public partial class Exercise
{
    [Key]
    [Column("ExerciseID")]
    public int ExerciseId { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(20)]
    public string? Category { get; set; }

    [StringLength(255)]
    public string? MuscleGroup { get; set; }

    [StringLength(255)]
    public string? EquipmentRequired { get; set; }

    public int? Reps { get; set; }

    public int? Sets { get; set; }

    public int? Resttime { get; set; }

    [ForeignKey("ExerciseId")]
    [InverseProperty("Exercises")]
    public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
}
