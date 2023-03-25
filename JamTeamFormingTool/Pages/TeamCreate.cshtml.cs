using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JamTeamFormingTool.Data;
using JamTeamFormingTool.Models;
using JamTeamFormingTool.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace JamTeamFormingTool.Pages
{
    public class TeamCreateModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;
        private JamTeamFormingSessionService _sessionService;
        private IWebHostEnvironment _webHostEnvironment;

        public string? PageStatus;
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

        public TeamCreateModel(JamTeamFormingTool.Data.MyDBContext context, JamTeamFormingSessionService sessionService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _sessionService = sessionService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet(int id, string? passCode)
        {
            SessionPassCode = passCode;
            Session = _sessionService.RetrieveSession(id);
            if (Session == null)
            {
                PageStatus = "Error retrieving session.";
                return Page();
            }
            if (_sessionService.SessionRequiresPassCode(Session)
                && (passCode == null || !_sessionService.GenericPasscodeAttempt(Session, passCode)))
            {
                PageStatus = "Error retrieving session.";
                return Page();
            }

            List<Role>? retrievedRoles = _sessionService.RetrieveRoles(Session);
            if (retrievedRoles == null)
            {
                PageStatus = "Error retrieving session.";
                return Page();
            }
            Roles = retrievedRoles.ToArray();
            
            return Page();
        }

        [BindProperty]
        public Team Team { get; set; } = default!;

        [BindProperty]
        public string? SessionPassCode { get; set; }

        [BindProperty]
        public JamTeamFormingSession? Session { get; set; }
        [BindProperty]
        public Role[] Roles { get; set; }
        [BindProperty]
        public bool[] RoleCheckedStatuses { get; set; }
        [BindProperty]
        public string? HelpMsg { get; set; }
        [BindProperty]
        public string? SubmitStatus { get; set; }
        [BindProperty]
        public string? TempRegion { get; set; }
        [BindProperty]
        public bool AssignedName { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            OnGet(Session.ID, SessionPassCode);
            if (_context.Participants == null || Team == null)
            {
                return Page();
            }

            JamTeamFormingSession? trackedSession = _sessionService.RetrieveTrackedSession(Session);
            if (trackedSession == null)
            {
                HelpMsg = "Failed to link with session.";
                SubmitStatus = "failure";
                return Page();
            }
            Team.Session = trackedSession;

            if (Team.Name == null)
            {
                AssignedName = true;
                IList<string>? names1 = JsonSerializer.Deserialize<IList<string>>(
                    System.IO.File.OpenText(Path.Combine(_webHostEnvironment.WebRootPath, "data", "suggested_team_names_1.json"))
                        .ReadToEnd()
                );
                IList<string>? names2 = JsonSerializer.Deserialize<IList<string>>(
                    System.IO.File.OpenText(Path.Combine(_webHostEnvironment.WebRootPath, "data", "suggested_team_names_2.json"))
                        .ReadToEnd()
                );
                if (names1 == null || names2 == null)
                {
                    Team.Name = "Default";
                } else
                {
                    Random rand = new Random();
                    Team.Name = names1[rand.Next(names1.Count)] + " " + names2[rand.Next(names2.Count)];
                }
            }

            List<Role> roles = new();
            for (int i = 0; i < RoleCheckedStatuses.Length; i++)
            {
                if (RoleCheckedStatuses[i])
                {
                    roles.Add(Roles[i]);
                }
            }
            if (roles.Count == 0)
            {
                HelpMsg = "You haven't selected any roles.";
                SubmitStatus = "failure";
                return Page();
            }
            List<Role> trackedRoles = new();
            foreach(Role role in roles)
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
            Team.Region = RegionMap[TempRegion!];
            //Team.PassCode = PassCode.Generate();
            Team.PassCode = "AAAA";

            if (!_sessionService.CanRegisterTeam(Session))
            {
                HelpMsg = "Unable to join - session has maxed out on teams.";
                SubmitStatus = "failure";
                return Page();
            }

            _context.Teams.Add(Team);
            await _context.SaveChangesAsync();
            HelpMsg = "Successfully joined session! Your participant passcode is " + Team.PassCode + ". Keep this in your records.";
            SubmitStatus = "success";

            return Page();
        }
    }
}
