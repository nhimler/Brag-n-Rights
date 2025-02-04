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

    [StringLength(255)]
    public string? FoodName { get; set; }

    public int? CaloriesPerServing { get; set; }

    public int? ProteinPerServing { get; set; }

    public int? CarbsPerServing { get; set; }

    public int? FatPerServing { get; set; }

    [StringLength(50)]
    public string? ServingSize { get; set; }

    [ForeignKey("FoodId")]
    [InverseProperty("Foods")]
    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
