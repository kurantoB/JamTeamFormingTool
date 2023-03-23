using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JamTeamFormingTool.Models
{
    public enum SessionPhase
    {
        Registration,
        TeamForming
    }
    public class JamTeamFormingSession
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Role template")]
        public RoleTemplate RoleTemplate { get; set; } = null!;
        [Required]
        [StringLength(100, ErrorMessage = "Name must be 100 characters or fewer.")]
        public string Name { get; set; } = null!;
        [StringLength(120, ErrorMessage = "Info must be 120 characters or fewer.")]
        public string? Info { get; set; }
        [DisplayName("Create time (UTC)")]
        public DateTime CreateTime { get; set; }
        public SessionPhase Phase { get; set; }
        // For DBA to contact in case a role template needs to be deleted and this session still exists
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string AdminEmail { get; set; } = null!;
        public string AdminPassCode { get; set; } = null!;
        public string? GenericPassCode { get; set; }

        public JamTeamFormingSession()
        {
            CreateTime = DateTime.UtcNow;
        }
    }
}
