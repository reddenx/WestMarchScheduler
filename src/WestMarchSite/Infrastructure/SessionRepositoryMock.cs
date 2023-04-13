using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Core;

namespace WestMarchSite.Infrastructure
{
    public class SessionRepositoryMock : ISessionRepository
    {
        private readonly List<SessionEntity> _sessions = new List<SessionEntity>();

        public SessionRepository.QueryResult<SessionEntity> GetSessionHostKey(string hostKey)
        {
            var session = _sessions.FirstOrDefault(s => s.HostKey == hostKey);
            if (session == null)
                return new SessionRepository.QueryResult<SessionEntity>(SessionRepository.QueryResultErrors.NotFound);
            else
                return new SessionRepository.QueryResult<SessionEntity>(session);
        }

        public SessionRepository.QueryResult<SessionEntity> GetSessionLeadKey(string leadKey)
        {
            var session = _sessions.FirstOrDefault(s => s.LeadKey == leadKey);
            if (session == null)
                return new SessionRepository.QueryResult<SessionEntity>(SessionRepository.QueryResultErrors.NotFound);
            else
                return new SessionRepository.QueryResult<SessionEntity>(session);
        }

        public SessionRepository.QueryResult<SessionEntity> GetSessionPlayerKey(string playerKey)
        {
            var session = _sessions.FirstOrDefault(s => s.PlayerKey == playerKey);
            if (session == null)
                return new SessionRepository.QueryResult<SessionEntity>(SessionRepository.QueryResultErrors.NotFound);
            else
                return new SessionRepository.QueryResult<SessionEntity>(session);
        }

        public SessionRepository.UpdateResult Save(SessionEntity session)
        {
            _sessions.RemoveAll(s => s.LeadKey == session.LeadKey);
            _sessions.Add(session);

            return new SessionRepository.UpdateResult();
        }
    }
}