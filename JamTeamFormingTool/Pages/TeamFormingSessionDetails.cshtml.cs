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

        public List<Team> Teams = null!;
        public List<Participant> Participants = null!;

        public Dictionary<Region, string> RegionMap = new() {
            { Region.NAM_LATAM, "Americas" },
            { Region.EMEA, "Europe/ Middle East/ Africa" },
            { Region.APAC, "Asia / Pacific" }
        };

        public TeamFormingSessionDetailsModel(JamTeamFormingTool.Data.MyDBContext context, JamTeamFormingSessionService teamFormingService)
        {
            _context = context;
            _teamFormingService = teamFormingService;
        }
        
        public JamTeamFormingSession JamTeamFormingSession { get; set; } = default!;
        public string? GenericPassCode;
        public string? AdminPassCode;

        public async Task<IActionResult> OnGetAsync(int? id, string? passCode, string? adminPassCode)
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
                if (adminPassCode != null)
                {
                    if (_teamFormingService.AdminPasscodeAttempt(jamteamformingsession, adminPassCode))
                    {
                        IsAdmin = true;
                        AdminPassCode = adminPassCode;
                    } else
                    {
                        return NotFound();
                    }
                }
                if (!IsAdmin
                    && _teamFormingService.SessionRequiresPassCode(jamteamformingsession)
                    && (passCode == null || !_teamFormingService.GenericPasscodeAttempt(jamteamformingsession, passCode)))
                {
                    return NotFound();
                }
                JamTeamFormingSession = jamteamformingsession;
                GenericPassCode = passCode;
            }
            RoleTemplateName = JamTeamFormingSession.RoleTemplate.Name;
            JamTeamFormingSession.RoleTemplate = new RoleTemplate();
            JamTeamFormingSession.AdminEmail = "";
            JamTeamFormingSession.AdminPassCode = "";
            JamTeamFormingSession.GenericPassCode = null;

            Teams = _teamFormingService.GetTeamsForSession(JamTeamFormingSession);
            Participants = _teamFormingService.GetParticipantsForSession(JamTeamFormingSession);

            return Page();
        }
    }
}
