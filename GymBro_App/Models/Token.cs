using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

[Table("Token")]
public partial class Token
{
    [Key]
    public int UserId { get; set; }

    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ExpirationTime { get; set; }

    public string? Scope { get; set; }

    public string? TokenType { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Token")]
    public virtual User User { get; set; } = null!;
}
