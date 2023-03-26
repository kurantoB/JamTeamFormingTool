using JamTeamFormingTool.Data;
using JamTeamFormingTool.Models;
using Microsoft.EntityFrameworkCore;

namespace JamTeamFormingTool.Services
{
    public class ParticipantService
    {
        private MyDBContext _dbContext;

        public ParticipantService(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ParticipantPassCodeAttempt(int participantID, string passCode)
        {
            Participant? participant =  _dbContext.Participants
                .Where(p => p.ID == participantID)
                .FirstOrDefault();
            if (participant == null)
            {
                return false;
            }
            return participant.PassCode == passCode;
        }

        public bool TeamPassCodeAttempt(int teamID, string passCode)
        {
            Team? team = _dbContext.Teams
                .Where(t => t.ID == teamID)
                .FirstOrDefault();
            if (team == null)
            {
                return false;
            }
            return team.PassCode == passCode;
        }

        public JamTeamFormingSession? RetrieveSessionFromParticipant(int participantID)
        {
            return _dbContext.Participants.Where(p => p.ID == participantID).Select(p => p.Session).FirstOrDefault();
        }

        public JamTeamFormingSession? RetrieveSessionFromTeam(int teamID)
        {
            return _dbContext.Teams.Where(t => t.ID == teamID).Select(t => t.Session).FirstOrDefault();
        }

        public SessionPhase? GetParticipantSessionPhase(int participantID)
        {
            Participant? participant = _dbContext.Participants
                .Where(p => p.ID == participantID)
                .Include(p => p.Session)
                .FirstOrDefault();
            if (participant == null)
            {
                return null;
            } else
            {
                return participant.Session.Phase;
            }
        }

        public SessionPhase? GetTeamSessionPhase(int teamID)
        {
            Team? team= _dbContext.Teams
                .Where(t => t.ID == teamID)
                .Include(t => t.Session)
                .FirstOrDefault();
            if (team== null)
            {
                return null;
            }
            else
            {
                return team.Session.Phase;
            }
        }

        public List<Team> RetrieveAllTeamsInSession(int sessionID)
        {
            JamTeamFormingSession? session = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == sessionID).FirstOrDefault();
            if (session == null)
            {
                return new();
            }
            return _dbContext.Teams
                .Where(t => t.Session == session)
                .Include(t => t.OpenRoles)
                .AsNoTracking()
                .ToList();
        }

        public List<Participant> RetrieveAllOtherParticipantsInSession(int sessionID, int participantID)
        {
            JamTeamFormingSession? session = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == sessionID).FirstOrDefault();
            if (session == null)
            {
                return new();
            }
            return _dbContext.Participants
                .Where(p => p.Session == session && p.ID != participantID)
                .Include(p => p.Roles)
                .AsNoTracking()
                .ToList();
        }

        public ICollection<Role> RetrieveAllRolesForParticipant(int participantID)
        {
            Participant? participant = _dbContext.Participants
                .Where(p => p.ID == participantID)
                .Include(p => p.Roles)
                .AsNoTracking()
                .FirstOrDefault();
            if (participant == null)
            {
                return new List<Role>();
            }
            return participant.Roles.ToHashSet();
        }

        public ICollection<Role> RetrieveAllOpenRolesForTeam(int teamID)
        {
            Team? team= _dbContext.Teams
                .Where(t => t.ID == teamID)
                .Include(t => t.OpenRoles)
                .AsNoTracking()
                .FirstOrDefault();
            if (team == null)
            {
                return new List<Role>();
            }
            return team.OpenRoles.ToHashSet();
        }

        public List<CoverageSet> RetrieveCoveragesForSession(int sessionID)
        {
            JamTeamFormingSession? session = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == sessionID)
                .Include(s => s.RoleTemplate)
                .ThenInclude(t => t.CoverageSets)
                .ThenInclude(cs => cs.RoleCategories)
                .ThenInclude(rc => rc.Roles)
                .FirstOrDefault();
            if (session == null)
            {
                return new();
            }
            return session.RoleTemplate.CoverageSets.ToList();
        }

        public void DeleteTeamByID(int teamID)
        {
            Team? team = _dbContext.Teams
                .Where(t => t.ID == teamID)
                .Include(t => t.OpenRoles)
                .FirstOrDefault();
            if (team == null)
            {
                return;
            }
            team.OpenRoles.Clear();
            _dbContext.SaveChanges();
            _dbContext.Teams.Remove(team);
            _dbContext.SaveChanges();
        }

        public void DeleteParticipantByID(int participantID)
        {
            Participant? participant = _dbContext.Participants
                .Where(p => p.ID == participantID)
                .Include(p => p.Roles)
                .FirstOrDefault();
            if (participant == null)
            {
                return;
            }
            participant.Roles.Clear();
            _dbContext.SaveChanges();
            _dbContext.Participants.Remove(participant);
            _dbContext.SaveChanges();
        }

        public Participant? ReturnUpdatedTrackedParticipant(Participant participant)
        {
            Participant? maybeParticipant = _dbContext.Participants
                .Where(p => p.ID == participant.ID)
                .Include(p => p.Roles)
                .FirstOrDefault();
            if (maybeParticipant == null)
            {
                return null;
            }
            maybeParticipant.Name = participant.Name;
            maybeParticipant.Handle = participant.Handle;
            maybeParticipant.Portfolio = participant.Portfolio;
            maybeParticipant.Region = participant.Region;
            maybeParticipant.Roles.Clear();
            foreach (Role role in participant.Roles)
            {
                maybeParticipant.Roles.Add(role);
            }
            return maybeParticipant;
        }

        public Team ReturnUpdatedTrackedTeam(Team team)
        {
            Team? maybeTeam = _dbContext.Teams
                .Where(t => t.ID == team.ID)
                .Include(t => t.OpenRoles)
                .FirstOrDefault();
            if (maybeTeam == null)
            {
                return null;
            }
            maybeTeam.Name = team.Name;
            maybeTeam.Handle = team.Handle;
            maybeTeam.Pitch = team.Pitch;
            maybeTeam.Region = team.Region;
            maybeTeam.OpenRoles.Clear();
            foreach (Role role in team.OpenRoles)
            {
                maybeTeam.OpenRoles.Add(role);
            }
            return maybeTeam;
        }
    }
}
