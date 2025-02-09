using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("Gym")]
public partial class Gym
{
    [Key]
    [Column("GymID")]
    public int GymId { get; set; }

    [StringLength(255)]
    public string? GymName { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    [StringLength(255)]
    public string? WebsiteUrl { get; set; }

    [StringLength(255)]
    public string? AvailableEquipment { get; set; }

    public string? PricingInfo { get; set; }

    public string? MembershipOptions { get; set; }

    [ForeignKey("GymId")]
    [InverseProperty("Gyms")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
