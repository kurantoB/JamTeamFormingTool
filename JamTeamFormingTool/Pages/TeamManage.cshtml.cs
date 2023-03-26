using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JamTeamFormingTool.Data;
using JamTeamFormingTool.Models;
using JamTeamFormingTool.Services;

namespace JamTeamFormingTool.Pages
{
    public class TeamManageModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;
        private ParticipantService _participantService;
        private JamTeamFormingSessionService _sessionService;

        public Dictionary<string, Region?> RegionMap = new() {
            { "(Unspecified)", null },
            { "Americas", Region.NAM_LATAM },
            { "Europe/ Middle East/ Africa", Region.EMEA },
            { "Asia / Pacific", Region.APAC }
        };
        public string[] RegionMapKeys = {
            "(Unspecified)",
            "Americas",
            "Europe/ Middle East/ Africa",
            "Asia / Pacific"
        };

        public TeamManageModel(JamTeamFormingTool.Data.MyDBContext context,
            ParticipantService participantService,
            JamTeamFormingSessionService sessionService)
        {
            _context = context;
            _participantService = participantService;
            _sessionService = sessionService;
        }

        [BindProperty]
        public Team Team { get; set; } = default!;

        [BindProperty]
        public string PassCode { get; set; } = null!;

        [BindProperty]
        public int SessionID { get; set; }

        [BindProperty]
        public string? SessionPassCode { get; set; }

        [BindProperty]
        public bool[] RoleCheckedStatuses { get; set; }

        [BindProperty]
        public string? TempRegion { get; set; }

        [BindProperty]
        public Role[] Roles { get; set; }

        [BindProperty]
        public string? HelpMsg { get; set; }

        [BindProperty]
        public string? SubmitStatus { get; set; }

        public SessionPhase CurrentSessionPhase { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string? passCode, int? sessionID, string? sessionPassCode)
        {
            if (id == null || sessionID == null || passCode == null || _context.Teams == null)
            {
                return NotFound();
            }
            if (!_participantService.TeamPassCodeAttempt(id ?? -1, passCode))
            {
                return NotFound();
            }
            PassCode = passCode;
            SessionID = sessionID ?? -1;
            SessionPassCode = sessionPassCode;

            var team =  await _context.Teams
                .Include(t => t.OpenRoles)
                .Include(t => t.Session)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }
            Team = team;

            List<Role>? retrievedRoles = _sessionService.RetrieveRoles(Team.Session);
            if (retrievedRoles == null)
            {
                return NotFound();
            }
            Roles = retrievedRoles.ToArray();
            RoleCheckedStatuses = new bool[Roles.Length];
            ICollection<Role> teamOpenRoles = Team.OpenRoles;
            HashSet<int> teamOpenRoleIDs = teamOpenRoles.Select(r => r.ID).ToHashSet();
            for (int i = 0; i < Roles.Length; i++)
            {
                if (teamOpenRoleIDs.Contains(Roles[i].ID))
                {
                    RoleCheckedStatuses[i] = true;
                }
            }

            SessionPhase? sessionPhase = _participantService.GetTeamSessionPhase(Team.ID);
            if (sessionPhase == null)
            {
                return NotFound();
            }
            else
            {
                CurrentSessionPhase = sessionPhase ?? SessionPhase.Registration;
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Team.Region = RegionMap[TempRegion!];

            JamTeamFormingSession? session = _participantService.RetrieveSessionFromTeam(Team.ID);
            if (session == null)
            {
                HelpMsg = "Failed to retrieve session.";
                SubmitStatus = "failure";
                return Page();
            }
            List<Role>? retrievedRoles = _sessionService.RetrieveRoles(session);
            if (retrievedRoles == null)
            {
                HelpMsg = "Failed to retrieve session roles.";
                SubmitStatus = "failure";
                return Page();
            }
            List<Role> roles = new();
            for (int i = 0; i < RoleCheckedStatuses.Length; i++)
            {
                if (RoleCheckedStatuses[i])
                {
                    roles.Add(retrievedRoles[i]);
                }
            }
            if (roles.Count == 0)
            {
                HelpMsg = "You haven't selected any roles.";
                SubmitStatus = "failure";
                Roles = retrievedRoles.ToArray();
                return Page();
            }
            List<Role> trackedRoles = new();
            foreach (Role role in roles)
            {
                Role? trackedRole = _sessionService.RetrieveTrackedRole(role);
                if (trackedRole == null)
                {
                    HelpMsg = "Failed to link with role.";
                    SubmitStatus = "failure";
                    return Page();
                }
                trackedRoles.Add(trackedRole);
            }
            Team.OpenRoles = trackedRoles;

            Team = _participantService.ReturnUpdatedTrackedTeam(Team);
            if (Team == null)
            {
                HelpMsg = "Failed to update team.";
                SubmitStatus = "failure";
                return Page();
            }

            _context.Attach(Team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                HelpMsg = "Team profile saved.";
                SubmitStatus = "success";
                return Page();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(Team.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
        }

        private bool TeamExists(int id)
        {
          return (_context.Teams?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
