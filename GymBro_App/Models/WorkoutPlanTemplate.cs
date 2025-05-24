using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymBro_App.Models
{


    [Table("WorkoutPlanTemplate")]
    public partial class WorkoutPlanTemplate
    {
        [Key]
        public int WorkoutPlanTemplateID { get; set; }

        [Required, StringLength(255)]
        public string PlanName { get; set; }

        [Required]
        public string DifficultyLevel { get; set; }

        // Navigation: one template → many exercises
        public virtual ICollection<WorkoutPlanTemplateExercise> Exercises { get; set; }
            = new List<WorkoutPlanTemplateExercise>();
    }

    [Table("WorkoutPlanTemplateExercise")]
    public partial class WorkoutPlanTemplateExercise
    {
        [Key]
        public int WorkoutPlanTemplateExerciseID { get; set; }

        [Required]
        public int WorkoutPlanTemplateID { get; set; }

        [ForeignKey(nameof(WorkoutPlanTemplateID))]
        public virtual WorkoutPlanTemplate Template { get; set; }

        [Required, StringLength(255)]
        public string ApiID { get; set; }

        [Required]
        public int Reps { get; set; }

        [Required]
        public int Sets { get; set; }
    }

    public class ApplyTemplateDto
    {
        public string PlanName         { get; set; }
        public string Difficulty       { get; set; }
        public List<string> ExerciseApiIds { get; set; }
    }
}
