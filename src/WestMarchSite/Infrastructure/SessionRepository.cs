using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Core;
using static WestMarchSite.Infrastructure.SessionRepository;

namespace WestMarchSite.Infrastructure
{
    public interface ISessionRepository
    {
        UpdateResult<SessionEntity> Save(SessionEntity session);
        QueryResult<SessionEntity> GetSessionHostKey(string hostKey);
        QueryResult<SessionEntity> GetSessionLeadKey(string leadKey);
        QueryResult<SessionEntity> GetSessionPlayerKey(string playerKey);
    }

    public interface ISessionRepositoryConfiguration
    {
        string SessionDbConnectionString { get; }
    }

    public class SessionRepository : ISessionRepository
    {
        private readonly string _connectionString;

        public SessionRepository(ISessionRepositoryConfiguration config)
        {
            _connectionString = config.SessionDbConnectionString;
        }

        public UpdateResult<SessionEntity> Save(SessionEntity session)
        {
            throw new NotImplementedException();
        }

        public QueryResult<SessionEntity> GetSessionHostKey(string hostKey)
        {
            throw new NotImplementedException();
        }

        public QueryResult<SessionEntity> GetSessionLeadKey(string leadKey)
        {
            throw new NotImplementedException();
        }

        public QueryResult<SessionEntity> GetSessionPlayerKey(string playerKey)
        {
            throw new NotImplementedException();
        }





        public class UpdateResult<T>
            where T : class
        {
            public UpdateResultErrors? Error { get; private set; }
            public bool IsSuccess => Error != null;
        }
        public enum UpdateResultErrors
        {
            NotFound,
            Technical,
        }

        public class QueryResult<T>
            where T : class
        {
            public T Result { get; private set; }
            public QueryResultErrors? Error { get; private set; }
            public bool IsSuccess => Error != null;

            public QueryResult(T result)
            {
                Result = result;
                Error = null;
            }

            public QueryResult(QueryResultErrors error)
            {
                Result = null;
                Error = error;
            }
        }
        public enum QueryResultErrors
        {
            NotFound,
            Technical
        }
    }
}
