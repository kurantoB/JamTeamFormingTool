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
            List<Participant> participants = _dbContext.Participants.Where(p => p.Session == maybeSession).ToList();
            foreach (Participant participant in participants)
            {
                DeleteParticipantByID(participant.ID);
            }
            List<Team> teams = _dbContext.Teams.Where(t => t.Session == maybeSession).ToList();
            foreach (Team team in teams)
            {
                DeleteTeamByID(team.ID);
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
                .Where(p => p.Session == session)
                .Include(p => p.Roles.OrderBy(r => r.Name))
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToList();
        }

        public List<Team> GetTeamsForSession(JamTeamFormingSession session)
        {
            return _dbContext.Teams
                .Where(t => t.Session == session)
                .Include(t => t.OpenRoles.OrderBy(r => r.Name))
                .OrderBy(t => t.Name)
                .AsNoTracking()
                .ToList();
        }

        public bool ParticipantCanMatch(JamTeamFormingSession session, Participant participant)
        {
            if (participant.ParticipantMatchRequest != null || participant.TeamMatchRequest != null)
            {
                return false;
            }
            if (_dbContext.Participants.Where(p => p.Session == session && p.ParticipantMatchRequest == participant).FirstOrDefault() != null)
            {
                return false;
            }
            if (_dbContext.Teams.Where(t => t.Session == session && t.ParticipantMatchRequest == participant).FirstOrDefault() != null)
            {
                return false;
            }
            return true;
        }

        public bool TeamCanMatch(JamTeamFormingSession session, Team team)
        {
            if (team.ParticipantMatchRequest != null)
            {
                return false;
            }
            if (_dbContext.Participants.Where(p => p.Session == session && p.TeamMatchRequest == team).FirstOrDefault() != null)
            {
                return false;
            }
            return true;
        }

        public bool TeamParticipantMatchRequest(Team team, Participant participant)
        {
            if (team.ParticipantMatchRequest != null
                || participant.TeamMatchRequest != null
                || participant.ParticipantMatchRequest != null)
            {
                return false;
            }
            participant.TeamMatchRequest = team;
            _dbContext.Participants.Update(participant);
            _dbContext.SaveChanges();
            return true;
        }

        public bool ParticipantTeamMatchRequest(Participant participant, Team team)
        {
            if (team.ParticipantMatchRequest != null
                || participant.TeamMatchRequest != null
                || participant.ParticipantMatchRequest != null)
            {
                return false;
            }
            team.ParticipantMatchRequest = participant;
            _dbContext.Teams.Update(team);
            _dbContext.SaveChanges();
            return true;
        }

        public bool ParticipantParticipantMatchRequest(Participant participantA, Participant participantB)
        {
            if (participantA.TeamMatchRequest != null
                || participantA.ParticipantMatchRequest != null
                || participantB.TeamMatchRequest != null
                || participantB.ParticipantMatchRequest != null)
            {
                return false;
            }
            participantB.ParticipantMatchRequest = participantA;
            _dbContext.Participants.Update(participantB);
            _dbContext.SaveChanges();
            return true;
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
    }
}
