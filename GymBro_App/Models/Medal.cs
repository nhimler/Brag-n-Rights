using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GymBro_App.Models
{
    public partial class Medal
    {
        [Key]
        [Column("MedalID")]
        public int MedalId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;  // Name of the medal (e.g., "5K Step Champion")

        [StringLength(255)]
        public string? Description { get; set; }  // Optional description of the medal

        public int StepThreshold { get; set; }  // Steps required to earn the medal

        public string Image { get; set; }  // Image URL for the medal

        [InverseProperty("Medal")]
        public virtual ICollection<UserMedal> UserMedals { get; set; } = new List<UserMedal>();
    }
}
