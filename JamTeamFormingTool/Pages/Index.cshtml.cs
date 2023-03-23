using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JamTeamFormingTool.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string LifecycleHelp = "1. The jam host creates a team forming session, kicking off the registration phase. Actual matching does not take place yet."
                + "\n\n2. During the registration phase, pre-made teams with open spots and individual team-seekers register with the created session."
                + "\n\n3. After a predefined amount of time, the jam host advances the session to the team forming phase. Here, teams and team seekers are provided with ranked matches according to compatibility. Participants are taken off the registry as they settle on a match. Registration for the session remains open to new participants."
                + "\n\n4. The jam host shuts down the team forming session when the time is apt. They may consider leaving the session open throughout the duration of the jam if there are still unmatched teams or team seekers, in case there are late-comers.";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}