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

namespace JamTeamFormingTool.Pages
{
    public class TeamSeekerCreateModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;
        private JamTeamFormingSessionService _sessionService;

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

        public TeamSeekerCreateModel(JamTeamFormingTool.Data.MyDBContext context, JamTeamFormingSessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
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
            if (_sessionService.SessionRequiresPassCode(Session) && passCode == null)
            {
                PageStatus = "Error retrieving session.";
                return Page();
            }
            if (Session != null && passCode != null && !_sessionService.GenericPasscodeAttempt(Session, passCode))
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
        public Participant Participant { get; set; } = default!;

        [BindProperty]
        public string? SessionPassCode { get; set; }

        [BindProperty]
        public JamTeamFormingSession? Session { get; set; }
        [BindProperty]
        public string HelpMsg { get; set; }
        [BindProperty]
        public string? SubmitStatus { get; set; }
        [BindProperty]
        public bool[] RoleCheckedStatuses { get; set; }
        [BindProperty]
        public string? TempRegion { get; set; }
        [BindProperty]
        public Role[] Roles { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            OnGet(Session.ID, SessionPassCode);
            if (_context.Participants == null || Participant == null)
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
            Participant.Session = trackedSession;

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
            Participant.Roles = trackedRoles;
            Participant.Region = RegionMap[TempRegion!];
            //Participant.PassCode = PassCode.Generate();
            Participant.PassCode = "AAAA";

            if (!_sessionService.CanRegisterParticipant(Session))
            {
                HelpMsg = "Unable to join - session has maxed out on team seekers.";
                SubmitStatus = "failure";
                return Page();
            }

            _context.Participants.Add(Participant);
            await _context.SaveChangesAsync();
            HelpMsg = "Successfully joined session! Your participant passcode is " + Participant.PassCode + ". Keep this in your records.";
            SubmitStatus = "success";

            return Page();
        }
    }
}
