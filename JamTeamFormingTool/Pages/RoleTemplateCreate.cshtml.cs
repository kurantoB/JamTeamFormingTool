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
using Microsoft.AspNetCore.Components;

namespace JamTeamFormingTool.Pages
{
    public class RoleTemplateCreateModel : PageModel
    {
        private readonly JamTeamFormingTool.Data.MyDBContext _context;
        private RoleTemplateService _service;

        public RoleTemplateCreateModel(JamTeamFormingTool.Data.MyDBContext context, RoleTemplateService service)
        {
            _context = context;
            _service = service;
        }

        public IActionResult OnGet()
        {
            if (!_service.CanRegisterStagedOnTotalLimit())
            {
                return RedirectToPage("./RoleTemplateIndex");
            }
            if (!_service.CanRegisterStagedOnLimit())
            {
                return RedirectToPage("./RoleTemplateIndex");
            }
            return Page();
        }

        [BindProperty]
        public RoleTemplate RoleTemplate { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.RoleTemplates == null || RoleTemplate == null)
            {
                return Page();
            }

            _context.RoleTemplates.Add(RoleTemplate);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
