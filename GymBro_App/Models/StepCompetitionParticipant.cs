using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models
{
    public class StepCompetitionParticipant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StepCompetitionId { get; set; }

        [Required]
        [StringLength(450)]
        public string IdentityId { get; set; } = null!;

        public int Steps { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual StepCompetition StepCompetition { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}