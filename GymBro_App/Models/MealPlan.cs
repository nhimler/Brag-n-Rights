using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("MealPlan")]
public partial class MealPlan
{
    [Key]
    [Column("MealPlanID")]
    public int MealPlanId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [StringLength(255)]
    public string? PlanName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [StringLength(50)]
    public string? Frequency { get; set; }

    public int? TargetCalories { get; set; }

    public int? TargetProtein { get; set; }

    public int? TargetCarbs { get; set; }

    public int? TargetFats { get; set; }

    [Column("Archived")]
    public bool Archived { get; set; } = false;

    [InverseProperty("MealPlan")]
    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    [ForeignKey("UserId")]
    [InverseProperty("MealPlans")]
    public virtual User? User { get; set; }
}
