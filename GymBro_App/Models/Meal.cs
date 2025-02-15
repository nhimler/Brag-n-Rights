﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("Meal")]
public partial class Meal
{
    [Key]
    [Column("MealID")]
    public int MealId { get; set; }

    [StringLength(255)]
    public string? MealName { get; set; }

    [StringLength(20)]
    public string? MealType { get; set; }

    public DateOnly? Date { get; set; }

    public int? TotalCalories { get; set; }

    public int? TotalProtein { get; set; }

    public int? TotalCarbs { get; set; }

    public int? TotalFats { get; set; }

    [Column("MealPlanID")]
    public int? MealPlanId { get; set; }

    [InverseProperty("Meal")]
    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();

    [ForeignKey("MealPlanId")]
    [InverseProperty("Meals")]
    public virtual MealPlan? MealPlan { get; set; }
}
