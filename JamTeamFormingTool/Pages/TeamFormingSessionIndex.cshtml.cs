using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JamTeamFormingTool.Data;
using JamTeamFormingTool.Models;

namespace JamTeamFormingTool.Pages
{
    public class TeamFormingSessionIndexModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;

        public List<string> RoleTemplateNames = new();

        public TeamFormingSessionIndexModel(JamTeamFormingTool.Data.MyDBContext context)
        {
            _context = context;
        }

        public IList<JamTeamFormingSession> JamTeamFormingSession { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.JamTeamFormingSessions != null)
            {
                JamTeamFormingSession = await _context.JamTeamFormingSessions
                    .Include(s => s.RoleTemplate)
                    .OrderByDescending(s => s.CreateTime)
                    .AsNoTracking()
                    .ToListAsync();
                foreach (JamTeamFormingSession session in JamTeamFormingSession)
                {
                    RoleTemplateNames.Add(session.RoleTemplate.Name);
                    session.RoleTemplate = new RoleTemplate();
                    session.AdminEmail = "";
                    session.AdminPassCode = "";
                    session.GenericPassCode = null;
                }
            }
        }
    }
}
