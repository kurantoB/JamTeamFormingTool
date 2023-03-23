using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JamTeamFormingTool.Data;
using JamTeamFormingTool.Models;
using JamTeamFormingTool.Services;

namespace JamTeamFormingTool.Pages
{
    public class RoleTemplateIndexModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;
        private RoleTemplateService _service;

        public RoleTemplateIndexModel(JamTeamFormingTool.Data.MyDBContext context, RoleTemplateService service)
        {
            _context = context;
            _service = service;
        }

        public IList<RoleTemplate> RoleTemplate { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.RoleTemplates != null)
            {
                RoleTemplate = await _context.RoleTemplates
                    .Include(t => t.Roles)
                    .AsNoTracking()
                    .OrderBy(t => t.Name)
                    .ToListAsync();
            }
            return Page();
        }
    }
}
