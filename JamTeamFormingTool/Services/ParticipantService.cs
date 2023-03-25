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
            }
            else if (participant.TeamMatchRequest != null)
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
