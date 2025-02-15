using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("Food")]
public partial class Food
{
    [Key]
    [Column("FoodID")]
    public int FoodId { get; set; }

    [Column("MealID")]
    public int? MealId { get; set; }

    [Column("ApiFoodID")]
    public long? ApiFoodId { get; set; }

    public int? Amount { get; set; }

    [ForeignKey("MealId")]
    [InverseProperty("Foods")]
    public virtual Meal? Meal { get; set; }
}
