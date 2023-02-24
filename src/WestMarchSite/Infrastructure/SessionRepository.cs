using MySql.Data.MySqlClient;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Core;
using static WestMarchSite.Infrastructure.SessionRepository;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<SessionRepository> _logger;

        public SessionRepository(ISessionRepositoryConfiguration config, ILogger<SessionRepository> logger)
        {
            _connectionString = config.SessionDbConnectionString;
            _logger = logger;
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

                Resolution = TranslateResolution(session.Resolution),
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
            if(session.FinalizedSchedule?.Options?.Any() == true)
                schedules.AddRange(session.FinalizedSchedule.Options.Select(d => new SessionScheduleData
                {
                    HostKey = session.HostKey,
                    Name = null,
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
                    SavePlayers(conn, players.ToArray(), session.HostKey);
                    SaveSchedules(conn, schedules.ToArray(), session.HostKey);
                }
                return new UpdateResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "repo failed to save session");
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
            catch (Exception e)
            {
                _logger.LogError(e, "repo failed to get session for host");
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
            catch (Exception e)
            {
                _logger.LogError(e, "repo failed to get session for lead");
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
            catch (Exception e)
            {
                _logger.LogError(e, "repo failed to get session for player");
                return new QueryResult<SessionEntity>(QueryResultErrors.Technical);
            }
        }

        //this should probably be moved to a session entity factory, meh
        private SessionEntity AssembleSession(SessionData sessionData, SessionPlayerData[] players, SessionScheduleData[] schedules)
        {
            var session = new SessionEntity(sessionData.HostKey, sessionData.LeadKey, sessionData.PlayerKey);
            var lead = players.FirstOrDefault(p => p.Role == "lead"); //TODO: these should probably be const'd somewhere... another time
            if (sessionData.Title != null
                && sessionData.Description != null
                && lead != null)
            {
                session.SetInfo(sessionData.Title, sessionData.Description, ParseResolution(sessionData.Resolution));
                session.SetLead(lead.Name);
                session.ProgressState();

                var host = players.FirstOrDefault(p => p.Role == "host");
                var hostSchedule = schedules.Where(s => s.Name == host.Name).ToArray();
                if (host != null && hostSchedule?.Any() == true)
                {
                    session.SetHost(host.Name);
                    session.SetHostSchedule(new SessionSchedule(hostSchedule.Select(s => new SessionScheduleOption(s.Start, s.End)).ToArray()));
                    session.ProgressState();

                    var leadSchedule = schedules.Where(s => s.Name == lead.Name).ToArray();
                    if (leadSchedule?.Any() == true)
                    {
                        session.SetLeadSchedule(new SessionSchedule(leadSchedule.Select(s => new SessionScheduleOption(s.Start, s.End)).ToArray()));
                        session.ProgressState();

                        var playerPlayers = players.Where(p => p.Role == "player").ToArray();
                        if (playerPlayers.Any())
                        {
                            //lol
                            foreach (var playerPlayersPlayer in playerPlayers)
                            {
                                var playerPlayerSchedule = schedules.Where(s => s.Name == playerPlayersPlayer.Name).ToArray();

                                session.AddPlayer(playerPlayersPlayer.Name, new SessionSchedule(playerPlayerSchedule.Select(s => new SessionScheduleOption(s.Start, s.End)).ToArray()));
                            }
                        }

                        var finalSchedule = schedules.Where(s => s.Name == null).ToArray();
                        if (finalSchedule?.Any() == true)
                        {
                            session.SetFinalSchedule(new SessionSchedule(finalSchedule.Select(s => new SessionScheduleOption(s.Start, s.End)).ToArray()));
                            session.ProgressState();
                        }
                    }
                }
            }

            return session;
        }

        private TimeResolutions ParseResolution(string resolution)
        {
            switch (resolution)
            {
                case "precise":
                    return TimeResolutions.Precise;
                case "hour":
                    return TimeResolutions.Hour;
                case "halfday":
                    return TimeResolutions.HalfDay;
                case "day":
                    return TimeResolutions.Day;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolution), resolution, "could not parse db string value into TimeResolutions");
            }
        }

        private string TranslateResolution(TimeResolutions resolution)
        {
            switch (resolution)
            {
                case TimeResolutions.Precise:
                    return "precise";
                case TimeResolutions.Hour:
                    return "hour";
                case TimeResolutions.HalfDay:
                    return "halfday";
                case TimeResolutions.Day:
                    return "day";
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolution), resolution, "could not parse db string value into TimeResolutions");
            }
        }

        private void SaveSessionData(MySqlConnection conn, SessionData sessionData)
        {
            var existingSession = GetSessionHostData(conn, sessionData.HostKey);

            if (existingSession == null)
            {
                var update = @"
insert into `SessionInfo` (`HostKey`, `LeadKey`, `PlayerKey`, `Title`, `Description`, `Resolution`)
values (@HostKey, @LeadKey, @PlayerKey, @Title, @Description, @Resolution);";
                conn.Execute(update, new
                {
                    HostKey = sessionData.HostKey,
                    LeadKey = sessionData.LeadKey,
                    PlayerKey = sessionData.PlayerKey,
                    Title = sessionData.Title,
                    Description = sessionData.Description,
                    Resolution = sessionData.Resolution,
                });
            }
            else
            {
                var insert = @"
update `SessionInfo`
set
	`Title` = @Title,
	`Description` = @Description,
    `Resolution` = @Resolution
where `HostKey` = @HostKey;";
                conn.Execute(insert, new
                {
                    HostKey = sessionData.HostKey,
                    Title = sessionData.Title,
                    Description = sessionData.Description,
                    Resolution = sessionData.Resolution,
                });
            }

        }

        private void SaveSchedules(MySqlConnection conn, SessionScheduleData[] schedules, string hostKey)
        {
            var delete = @"
delete
from `ScheduleInfo`
where `HostKey` = @HostKey";
            conn.Execute(delete, new { HostKey = hostKey });

            //this is brutally inefficient, TODO fix with TVPs or something later

            var insert = @"
insert into `ScheduleInfo` (`HostKey`, `Name`, `Start`, `End`)
values (@HostKey, @Name, @Start, @End);";
            try
            {
                foreach (var schedule in schedules)
                {
                    conn.Execute(insert, new
                    {
                        HostKey = schedule.HostKey,
                        Name = schedule.Name,
                        Start = schedule.Start,
                        End = schedule.End
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "repo failed to save schedules");
                throw;
            }
        }

        private void SavePlayers(MySqlConnection conn, SessionPlayerData[] players, string hostKey)
        {
            var delete = @"
delete from `Player`
where `HostKey` = @HostKey";
            conn.Execute(delete, new { HostKey = hostKey });

            var insert = @"
insert into `Player` (`HostKey`, `Name`, `Role`)
values (@HostKey, @Name, @Role)";

            try 
            {
                foreach (var player in players)
                {
                    conn.Execute(insert, new
                    {
                        HostKey = player.HostKey,
                        Name = player.Name,
                        Role = player.Role
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "repo failed to save players");
                throw;
            }
        }

        private SessionScheduleData[] GetSchedules(MySqlConnection conn, string hostKey)
        {
            var sql = @"
select 
	s.`HostKey`,
    s.`Name`,
    s.`Start`,
    s.`End`
from `ScheduleInfo` s
where s.`HostKey` = @HostKey";

            var schedules = conn.Query<SessionScheduleData>(sql, new { HostKey = hostKey });
            return schedules.ToArray();
        }

        private SessionPlayerData[] GetPlayers(MySqlConnection conn, string hostKey)
        {
            var sql = @"
select 
	p.`HostKey`,
	p.`Name`,
    p.`Role`
from `Player` p
where p.`HostKey` = @HostKey";

            var players = conn.Query<SessionPlayerData>(sql, new { HostKey = hostKey });
            return players.ToArray();
        }

        private SessionData GetSessionPlayerData(MySqlConnection conn, string playerKey)
        {
            var sql = @"
select 
	s.`HostKey`,
    s.`LeadKey`,
    s.`PlayerKey`,
    s.`Title`,
    s.`Description`,
    s.`Resolution`
from `SessionInfo` s
where s.`PlayerKey` = @PlayerKey";

            var data = conn.Query<SessionData>(sql, new { PlayerKey = playerKey });
            return data.FirstOrDefault();
        }

        private SessionData GetSessionLeadData(MySqlConnection conn, string leadKey)
        {
            var sql = @"
select 
	s.`HostKey`,
    s.`LeadKey`,
    s.`PlayerKey`,
    s.`Title`,
    s.`Description`,
    s.`Resolution`
from `SessionInfo` s
where s.`LeadKey` = @LeadKey";

            var data = conn.Query<SessionData>(sql, new { LeadKey = leadKey });
            return data.FirstOrDefault();
        }

        private SessionData GetSessionHostData(MySqlConnection conn, string hostKey)
        {
            var sql = @"
select 
	s.`HostKey`,
    s.`LeadKey`,
    s.`PlayerKey`,
    s.`Title`,
    s.`Description`,
    s.`Resolution`
from `SessionInfo` s
where s.`HostKey` = @HostKey";

            var data = conn.Query<SessionData>(sql, new { HostKey = hostKey });
            return data.FirstOrDefault();
        }

        private class SessionData
        {
            public string HostKey { get; set; }
            public string LeadKey { get; set; }
            public string PlayerKey { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Resolution { get; set; }
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
