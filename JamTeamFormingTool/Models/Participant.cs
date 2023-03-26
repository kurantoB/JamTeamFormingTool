using System.ComponentModel.DataAnnotations;

namespace JamTeamFormingTool.Models
{
    public class Participant
    {
        [Key]
        public int ID { get; set; }
        public JamTeamFormingSession Session { get; set; } = null!;
        [Required]
        [StringLength(60, ErrorMessage = "Name must be 60 characters or fewer.")]
        public string Name { get; set; } = null!;
        public ICollection<Role> Roles { get; set; } = null!;
        [Required]
        [StringLength(60, ErrorMessage = "Handle must be between 60 characters or fewer.")]
        public string Handle { get; set; } = null!;
        [StringLength(500, ErrorMessage = "Portfolio must be 500 characters or fewer.")]
        public string? Portfolio { get; set; }
        public Region? Region { get; set; }
        public string PassCode { get; set; } = null!;
    }
}
