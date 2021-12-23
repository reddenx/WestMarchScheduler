using MySql.Data.MySqlClient;
using Dapper;
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
        UpdateResult Save(SessionEntity session);
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

        public UpdateResult Save(SessionEntity session)
        {
            var sessionData = new SessionData
            {
                HostKey = session.HostKey,
                LeadKey = session.LeadKey,
                PlayerKey = session.PlayerKey,

                Title = session.Title,
                Description = session.Description,
            };

            var players = new List<SessionPlayerData>();
            if (session.HostName != null)
                players.Add(new SessionPlayerData
                {
                    HostKey = session.HostKey,
                    Name = session.HostName,
                    Role = "host"
                });
            if (session.LeadName != null)
                players.Add(new SessionPlayerData
                {
                    HostKey = session.HostKey,
                    Name = session.LeadName,
                    Role = "lead"
                });
            if (session.Players?.Any() == true)
                players.AddRange(session.Players.Select(player => new SessionPlayerData
                {
                    HostKey = session.HostKey,
                    Name = player.Name,
                    Role = "player"
                }));

            var schedules = new List<SessionScheduleData>();
            if (session.HostSchedule?.Options?.Any() == true)
                schedules.AddRange(session.HostSchedule.Options.Select(d => new SessionScheduleData
                {
                    HostKey = session.HostKey,
                    Name = session.HostName,
                    Start = d.Start,
                    End = d.End
                }));
            if (session.LeadSchedule?.Options?.Any() == true)
                schedules.AddRange(session.LeadSchedule.Options.Select(d => new SessionScheduleData
                {
                    HostKey = session.HostKey,
                    Name = session.LeadName,
                    Start = d.Start,
                    End = d.End
                }));
            if (session.Players?.Any() == true)
            {
                foreach (var player in session.Players)
                {
                    schedules.AddRange(player.Schedule.Options.Select(d => new SessionScheduleData
                    {
                        HostKey = session.HostKey,
                        Name = player.Name,
                        Start = d.Start,
                        End = d.End
                    }));
                }
            }

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    SaveSessionData(conn, sessionData);
                    SavePlayers(conn, players);
                    SaveSchedules(conn, schedules);
                }
                return new UpdateResult();
            }
            catch
            {
                return new UpdateResult(UpdateResultErrors.Technical);
            }
        }

        public QueryResult<SessionEntity> GetSessionHostKey(string hostKey)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sessionData = GetSessionHostData(conn, hostKey);
                    if (sessionData == null)
                        return new QueryResult<SessionEntity>(QueryResultErrors.NotFound);

                    var players = GetPlayers(conn, hostKey);
                    var schedules = GetSchedules(conn, hostKey);

                    var session = AssembleSession(sessionData, players, schedules);
                    return new QueryResult<SessionEntity>(session);
                }
            }
            catch
            {
                return new QueryResult<SessionEntity>(QueryResultErrors.Technical);
            }
        }

        public QueryResult<SessionEntity> GetSessionLeadKey(string leadKey)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sessionData = GetSessionLeadData(conn, leadKey);
                    if (sessionData == null)
                        return new QueryResult<SessionEntity>(QueryResultErrors.NotFound);

                    var players = GetPlayers(conn, sessionData.HostKey);
                    var schedules = GetSchedules(conn, sessionData.HostKey);

                    var session = AssembleSession(sessionData, players, schedules);
                    return new QueryResult<SessionEntity>(session);
                }
            }
            catch
            {
                return new QueryResult<SessionEntity>(QueryResultErrors.Technical);
            }
        }

        public QueryResult<SessionEntity> GetSessionPlayerKey(string playerKey)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    var sessionData = GetSessionPlayerData(conn, playerKey);
                    if (sessionData == null)
                        return new QueryResult<SessionEntity>(QueryResultErrors.NotFound);

                    var players = GetPlayers(conn, sessionData.HostKey);
                    var schedules = GetSchedules(conn, sessionData.HostKey);

                    var session = AssembleSession(sessionData, players, schedules);
                    return new QueryResult<SessionEntity>(session);
                }
            }
            catch
            {
                return new QueryResult<SessionEntity>(QueryResultErrors.Technical);
            }
        }

        private SessionEntity AssembleSession(SessionData sessionData, SessionPlayerData[] players, SessionScheduleData[] schedules)
        {
            throw new NotImplementedException();
        }

        private SessionScheduleData[] GetSchedules(MySqlConnection conn, string hostKey)
        {
            throw new NotImplementedException();
        }

        private SessionPlayerData[] GetPlayers(MySqlConnection conn, string hostKey)
        {
            throw new NotImplementedException();
        }

        private SessionData GetSessionPlayerData(MySqlConnection conn, string playerKey)
        {
            throw new NotImplementedException();
        }

        private SessionData GetSessionLeadData(MySqlConnection conn, string leadKey)
        {
            throw new NotImplementedException();
        }

        private SessionData GetSessionHostData(MySqlConnection conn, string hostKey)
        {
            throw new NotImplementedException();
        }

        private void SaveSessionData(MySqlConnection conn, SessionData sessionData)
        {
            throw new NotImplementedException();
        }

        private void SaveSchedules(MySqlConnection conn, List<SessionScheduleData> schedules)
        {
            throw new NotImplementedException();
        }

        private void SavePlayers(MySqlConnection conn, List<SessionPlayerData> players)
        {
            throw new NotImplementedException();
        }

        private class SessionData
        {
            public string HostKey { get; set; }
            public string LeadKey { get; set; }
            public string PlayerKey { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        private class SessionPlayerData
        {
            public string HostKey { get; set; }
            public string Name { get; set; }
            public string Role { get; set; }
        }

        private class SessionScheduleData
        {
            public string HostKey { get; set; }
            public string Name { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }





        public class UpdateResult
        {
            public UpdateResultErrors? Error { get; private set; }
            public bool IsSuccess => Error == null;

            public UpdateResult()
            {
                this.Error = null;
            }

            public UpdateResult(UpdateResultErrors error)
            {
                this.Error = error;
            }
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
            public bool IsSuccess => Error == null;

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
