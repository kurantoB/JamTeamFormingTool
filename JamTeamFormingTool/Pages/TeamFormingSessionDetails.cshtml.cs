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
    public class TeamFormingSessionDetailsModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;
        private JamTeamFormingSessionService _teamFormingService;

        public string RoleTemplateName = null!;
        public bool IsAdmin = false;


        public TeamFormingSessionDetailsModel(JamTeamFormingTool.Data.MyDBContext context, JamTeamFormingSessionService teamFormingService)
        {
            _context = context;
            _teamFormingService = teamFormingService;
        }
        
        public JamTeamFormingSession JamTeamFormingSession { get; set; } = default!;
        public string? GenericPassCode;

        public async Task<IActionResult> OnGetAsync(int? id, string? passCode)
        {
            if (id == null || _context.JamTeamFormingSessions == null)
            {
                return NotFound();
            }

            var jamteamformingsession = await _context.JamTeamFormingSessions
                .Include(s => s.RoleTemplate)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (jamteamformingsession == null)
            {
                return NotFound();
            }
            else 
            {
                JamTeamFormingSession = jamteamformingsession;
                GenericPassCode = passCode;
            }
            RoleTemplateName = JamTeamFormingSession.RoleTemplate.Name;
            JamTeamFormingSession.RoleTemplate = new RoleTemplate();
            JamTeamFormingSession.AdminEmail = "";
            JamTeamFormingSession.AdminPassCode = "";
            JamTeamFormingSession.GenericPassCode = null;

            if (passCode != null && _teamFormingService.AdminPasscodeAttempt(JamTeamFormingSession, passCode)) {
                IsAdmin = true;
            }
            return Page();
        }
    }
}
