using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JamTeamFormingTool.Models
{
    public class CoverageSetRoleCategory
    {
        [Key]
        public int ID { get; set; }
        public CoverageSet CoverageSet { get; set; } = null!;
        public IList<Role> Roles { get; set; } = null!;
    }
}
