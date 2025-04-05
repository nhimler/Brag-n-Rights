using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymBro_App.Models
{
        [Table("StepCompetition")]
        public class StepCompetitionEntity
    {
        public int CompetitionID { get; set; }  // Primary Key (CompetitionID)
        public string CreatorIdentityId { get; set; }  // IdentityId of the user who created the competition
        public DateTime StartDate { get; set; }  // Start date of the competition
        public DateTime EndDate { get; set; }  // End date of the competition
        public bool IsActive { get; set; }  // Flag to track if the competition is active

        // Navigation property to the User who created the competition
        public virtual User? Creator { get; set; }
    }
}