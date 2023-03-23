using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JamTeamFormingTool.Models
{
    public class Role
    {
        [Key]
        public int ID { get; set; }
        public RoleTemplate RoleTemplate { get; set; } = null!;

        public ICollection<CoverageSetRoleCategory> RoleCategories { get; set; } = null!;
        public ICollection<Participant> Participants { get; set; } = null!;

        public ICollection<Team> Teams { get; set; } = null!;
        [Required]
        [StringLength(60, ErrorMessage = "Role name must be 60 characters or fewer.")]
        public string Name { get; set; } = null!;

        [StringLength(120, ErrorMessage = "Role description must be 120 characters or fewer.")]
        public string? Description { get; set; }
    }
}
