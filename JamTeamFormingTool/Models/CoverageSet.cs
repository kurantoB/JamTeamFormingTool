using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JamTeamFormingTool.Models
{
    public class CoverageSet
    {
        [Key]
        public int ID { get; set; }
        public RoleTemplate RoleTemplate { get; set; } = null!;
        [Required]
        [StringLength(60, ErrorMessage = "Coverage name must be 60 characters or fewer.")]
        public string Name { get; set; } = null!;
        [DisplayName("Role categories")]
        public IList<CoverageSetRoleCategory> RoleCategories { get; set; } = null!;
    }
}
