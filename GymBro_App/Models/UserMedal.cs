using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymBro_App.Models
{
    public partial class UserMedal
    {
        [Key]
        [Column("UserMedalID")]
        public int UserMedalId { get; set; }

        [Column("UserID")]
        public int UserId { get; set; }  // Foreign key to User

        [Column("MedalID")]
        public int MedalId { get; set; }  // Foreign key to Medal

        public DateOnly EarnedDate { get; set; }  // The date the medal was earned

        [ForeignKey("UserId")]
        [InverseProperty("UserMedals")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("MedalId")]
        [InverseProperty("UserMedals")]
        public virtual Medal Medal { get; set; } = null!;
    }
}
