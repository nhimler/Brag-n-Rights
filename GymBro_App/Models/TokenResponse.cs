using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymBro_App.Models
{
    [Table("Token")]
    public class TokenEntity
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }  // UserID as Foreign Key and Primary Key
        
        [Required]
        public string AccessToken { get; set; } = null!;
        
        [Required]
        public string RefreshToken { get; set; } = null!;
        
        [Required]
        public DateTime ExpirationTime { get; set; }

        public string? Scope { get; set; }
        public string? TokenType { get; set; }

        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}
