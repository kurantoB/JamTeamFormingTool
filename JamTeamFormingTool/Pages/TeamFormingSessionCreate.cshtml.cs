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

namespace JamTeamFormingTool.Pages
{
    public class TeamFormingSessionCreateModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;
        private JamTeamFormingSessionService _sessionService;

        public TeamFormingSessionCreateModel(JamTeamFormingTool.Data.MyDBContext context, JamTeamFormingSessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public IActionResult OnGet()
        {
            if (!_sessionService.CanAddNewSession())
            {
                PostStatus = "maxedout";
            }
            RoleTemplateNames = GetRoleTemplateNames();
            return Page();
        }

        public string? PostStatus;

        [BindProperty]
        public JamTeamFormingSession JamTeamFormingSession { get; set; } = default!;

        [BindProperty]
        public string RoleTemplateName { get; set; } = null!;

        [BindProperty]
        public string AccessString { get; set; } = null!;
        public IList<string> RoleTemplateNames = null!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            RoleTemplate? roleTemplate = _sessionService.GetRoleTemplateByName(RoleTemplateName);
            if (roleTemplate == null) {
                PostStatus = "roletemplatenotfound";
                return OnGet();
            }
            JamTeamFormingSession.RoleTemplate = roleTemplate;

            if (_context.JamTeamFormingSessions == null || JamTeamFormingSession == null)
            {
                return OnGet();
            }

            _sessionService.RegisterSession(JamTeamFormingSession, AccessString == "Invite-only");

            PostStatus = "success";
            return OnGet();
        }

        public List<string> GetRoleTemplateNames()
        {
            return _sessionService.GetRoleTemplatesForSession().ConvertAll(new Converter<RoleTemplate, string>((t => t.Name)));
        }
    }
}
