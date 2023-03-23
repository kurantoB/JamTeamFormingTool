using JamTeamFormingTool.Data;
using JamTeamFormingTool.Models;
using Microsoft.EntityFrameworkCore;

namespace JamTeamFormingTool.Services
{
    public class JamTeamFormingSessionService
    {
        private MyDBContext _dbContext;

        public JamTeamFormingSessionService(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CanAddNewSession()
        {
            return _dbContext.JamTeamFormingSessions.Count() < DBConstants.MaxSessions;
        }

        public List<RoleTemplate> GetRoleTemplatesForSession()
        {
            return _dbContext.RoleTemplates.Where(t => true).AsNoTracking().ToList();
        }

        public RoleTemplate? GetRoleTemplateByName(string name)
        {
            return _dbContext.RoleTemplates.Where(t => t.Name == name).FirstOrDefault();
        }

        public void RegisterSession(JamTeamFormingSession session, bool isInviteOnly)
        {
            session.Phase = SessionPhase.Registration;
            //session.AdminPassCode = PassCode.Generate();
            session.AdminPassCode = "XXXX";
            if (isInviteOnly)
            {
                //session.GenericPassCode = PassCode.Generate();
                session.GenericPassCode = "AAAA";
            }
            _dbContext.JamTeamFormingSessions.Add(session);
            _dbContext.SaveChanges();
        }

        public bool IsInviteOnly(JamTeamFormingSession session)
        {
            JamTeamFormingSession? maybeSession = _dbContext.JamTeamFormingSessions.Where(s => s.ID == session.ID).AsNoTracking().FirstOrDefault();
            if (maybeSession == null) {
                return true;
            }
            return maybeSession.GenericPassCode != null;
        }

        public bool AdminPasscodeAttempt(JamTeamFormingSession session, string passCodeAttempt)
        {
            JamTeamFormingSession? maybeSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .AsNoTracking()
                .FirstOrDefault();
            if (maybeSession == null)
            {
                return false;
            }
            return maybeSession.AdminPassCode == passCodeAttempt;
        }
        public bool GenericPasscodeAttempt(JamTeamFormingSession session, string passCodeAttempt)
        {
            JamTeamFormingSession? maybeSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .AsNoTracking()
                .FirstOrDefault();
            if (maybeSession == null)
            {
                return false;
            }
            return maybeSession.GenericPassCode == passCodeAttempt;
        }

        public bool TryAdvanceToTeamForming(JamTeamFormingSession session)
        {
            JamTeamFormingSession? maybeSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .FirstOrDefault();
            if (maybeSession == null)
            {
                return false;
            }
            maybeSession.Phase = SessionPhase.TeamForming;
            _dbContext.SaveChanges();
            return true;
        }

        public bool TryDelete(JamTeamFormingSession session)
        {
            JamTeamFormingSession? maybeSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .FirstOrDefault();
            if (maybeSession == null)
            {
                return false;
            }
            _dbContext.JamTeamFormingSessions.Remove(maybeSession);
            _dbContext.SaveChanges();
            return true;
        }

        public bool CanRegisterParticipant(JamTeamFormingSession session)
        {
            JamTeamFormingSession? maybeSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .FirstOrDefault();
            if (maybeSession == null)
            {
                return false;
            }
            return _dbContext.Participants.Where(p => p.Session == maybeSession).Count() < DBConstants.MaxSessionParticipants;
        }

        public bool CanRegisterTeam(JamTeamFormingSession session)
        {
            JamTeamFormingSession? maybeSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .FirstOrDefault();
            if (maybeSession == null)
            {
                return false;
            }
            return _dbContext.Participants.Where(p => p.Session == maybeSession).Count() < DBConstants.MaxSessionTeams;
        }

        public JamTeamFormingSession? RetrieveSession(int sessionID)
        {
            JamTeamFormingSession? session = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == sessionID)
                .AsNoTracking()
                .FirstOrDefault();
            if (session == null)
            {
                return null;
            }
            session.AdminEmail = "";
            session.AdminPassCode = "";
            session.GenericPassCode = null;
            return session;
        }

        public JamTeamFormingSession? RetrieveTrackedSession(JamTeamFormingSession session)
        {
            JamTeamFormingSession? trackedSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .FirstOrDefault();
            if (trackedSession == null)
            {
                return null;
            }
            return trackedSession;
        }

        public Role? RetrieveTrackedRole(Role role)
        {
            Role? trackedRole = _dbContext.Roles
                .Where(r => r.ID == role.ID)
                .FirstOrDefault();
            if (trackedRole == null)
            {
                return null;
            }
            return trackedRole;
        }

        public bool SessionRequiresPassCode(JamTeamFormingSession session)
        {
            JamTeamFormingSession? retrievedSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .AsNoTracking()
                .FirstOrDefault();
            if (retrievedSession == null)
            {
                return true;
            }
            return retrievedSession.GenericPassCode != null;
        }

        public List<Role>? RetrieveRoles(JamTeamFormingSession session)
        {
            JamTeamFormingSession? retrievedSession = _dbContext.JamTeamFormingSessions
                .Where(s => s.ID == session.ID)
                .Include(s => s.RoleTemplate)
                .AsNoTracking()
                .FirstOrDefault();
            if (retrievedSession == null)
            {
                return null;
            }
            RoleTemplate? roleTemplate = _dbContext.RoleTemplates
                .Where(t => t.Name == retrievedSession.RoleTemplate.Name)
                .Include(t => t.Roles)
                .AsNoTracking()
                .FirstOrDefault();
            if (roleTemplate == null)
            {
                return null;
            }
            List<Role> roles = roleTemplate.Roles.OrderBy(r => r.Name).ToList();
            foreach (Role role in roles)
            {
                role.RoleTemplate = null;
            }
            return roles;
        }

        public List<Participant> GetParticipantsForSession(JamTeamFormingSession session)
        {
            return _dbContext.Participants
                .Where(t => t.Session == session)
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToList();
        }

        public List<Team> GetTeamsForSession(JamTeamFormingSession session)
        {
            return _dbContext.Teams
                .Where(t => t.Session == session)
                .OrderBy(t => t.Name)
                .AsNoTracking()
                .ToList();
        }

        public void TeamParticipantMatchRequest(Team team, Participant participant)
        {
            participant.TeamMatchRequest = team;
            _dbContext.Participants.Update(participant);
            _dbContext.SaveChanges();
        }

        public void ParticipantTeamMatchRequest(Participant participant, Team team)
        {
            team.ParticipantMatchRequest = participant;
            _dbContext.Teams.Update(team);
            _dbContext.SaveChanges();
        }

        public void ParticipantParticipantMatchRequest(Participant participantA, Participant participantB)
        {
            participantB.ParticipantMatchRequest = participantA;
            _dbContext.Participants.Update(participantB);
            _dbContext.SaveChanges();
        }

        public void ResolveParticipantMatch(Participant participant)
        {
            if (participant.ParticipantMatchRequest != null)
            {
                _dbContext.Participants.Remove(participant.ParticipantMatchRequest);
                _dbContext.Participants.Remove(participant);
            } else if (participant.TeamMatchRequest != null)
            {
                _dbContext.Teams.Remove(participant.TeamMatchRequest);
                _dbContext.Participants.Remove(participant);
            }
            _dbContext.SaveChanges();
        }

        public void ResolveTeamMatch(Team team)
        {
            if (team.ParticipantMatchRequest != null)
            {
                _dbContext.Participants.Remove(team.ParticipantMatchRequest);
                team.ParticipantMatchRequest = null;
                _dbContext.Teams.Update(team);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteTeam(Team team)
        {
            _dbContext.Teams.Remove(team);
            _dbContext.SaveChanges();
        }


    }
}
