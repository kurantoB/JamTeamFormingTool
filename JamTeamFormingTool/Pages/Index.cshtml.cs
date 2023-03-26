using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JamTeamFormingTool.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string LifecycleHelp = "1. The jam host creates a team forming session, kicking off the registration phase. Actual tool-driven matching does not take place yet."
                + "\n\n2. During the registration phase, pre-made teams with open spots and individual team-seekers register with the created session."
                + "\n\n3. At an agreed-upon time, the jam host advances the session to the team forming phase. Teams and team-seekers can now go to their profiles and run the matching process to see other participants ranked according to compatibility. They are to reach out to each other privately (outside this site) to team up. Registration for the session remains open to new participants."
                + "\n\n4. The jam host shuts down the team forming session when the time is apt, clearing all participant profiles, although they may have considered leaving the session open throughout the duration of the jam in case there are late-comers and not everyone has found a match.";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}