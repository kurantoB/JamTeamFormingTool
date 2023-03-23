using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JamTeamFormingTool.Data;
using JamTeamFormingTool.Models;
using System.Collections;

namespace JamTeamFormingTool.Pages
{
    public class RoleTemplateDetailsModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;

        public RoleTemplateDetailsModel(JamTeamFormingTool.Data.MyDBContext context)
        {
            _context = context;
        }
        
        public RoleTemplate RoleTemplate { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string name)
        {
            if (name == null || _context.RoleTemplates == null)
            {
                return NotFound();
            }

            var roletemplate = await _context.RoleTemplates
                .Where(m => m.Name == name)
                .Include(t => t.Roles)
                .Include(t => t.CoverageSets)
                .ThenInclude(s => s.RoleCategories)
                .ThenInclude(c => c.Roles)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            
            if (roletemplate == null)
            {
                return NotFound();
            }
            else 
            {
                RoleTemplate = roletemplate;
                RoleTemplate.Email = "";
                RoleTemplate.AuthorizePassCode = "";
            }
            return Page();
        }
    }
}
