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
        QueryResult<SessionEntity> GetSessionDmKey(string dmKey);
        QueryResult<SessionEntity> GetSessionLeadKey(string leadKey);
        QueryResult<SessionEntity> GetSessionPlayerKey(string playerKey);
    }

    public class SessionRepository
    {
        public class UpdateResult<T>
            where T : class
        {
            public UpdateErrors? Error { get; private set; }
            public bool IsSuccess => Error != null;

            public enum UpdateErrors
            {
                NotFound,
                Technical,
            }
        }

        public class QueryResult<T>
            where T : class
        {
            public T Result { get; private set; }
            public QueryErrors? Error { get; private set; }
            public bool IsSuccess => Error != null;

            public QueryResult(T result)
            {
                Result = result;
                Error = null;
            }

            public QueryResult(QueryErrors error)
            {
                Result = null;
                Error = error;
            }

            public enum QueryErrors
            {
                NotFound,
                Technical
            }
        }
    }
}
