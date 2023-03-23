using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JamTeamFormingTool.Models
{
    public class RoleTemplate
    {
        [Key]
        [DisplayName("Role template")]
        [Required]
        [StringLength(100, ErrorMessage = "Role template name must be 100 characters or fewer.")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Role template description must be 500 characters or fewer.")]
        public string? Description { get; set; }

        public IList<Role> Roles { get; set; } = null!;

        public IList<CoverageSet> CoverageSets { get; set; } = null!;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        public bool IsStaged { get; set; }
        // provided by DBA on demand from authorized email
        // used to submit (create) and delete
        public string? AuthorizePassCode { get; set; }
    }
}
