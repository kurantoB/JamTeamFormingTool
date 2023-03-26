using System.ComponentModel.DataAnnotations;

namespace JamTeamFormingTool.Models
{
    public class Team
    {
        [Key]
        public int ID { get; set; }
        public JamTeamFormingSession Session { get; set; } = null!;
        [StringLength(100, ErrorMessage = "Name must be 100 characters or fewer.")]
        public string? Name { get; set; } = null!;
        public ICollection<Role> OpenRoles { get; set; } = null!;
        [Required]
        [StringLength(60, ErrorMessage = "Handle must be 60 characters or fewer.")]
        public string Handle { get; set; } = null!;
        [StringLength(280, ErrorMessage = "Pitch must be 280 characters or fewer.")]
        public string? Pitch { get; set; }
        public Region? Region { get; set; }
        public string PassCode { get; set; } = null!;
    }
}