﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WestMarchSite.Core;
using WestMarchSite.Infrastructure;
using static WestMarchSite.Application.SessionService;

namespace WestMarchSite.Application
{
    public interface ISessionService
    {
        SetResult<CreateSessionResultDto> StartSession(CreateSessionDto createDto);
        SetResult<ApproveSessionResultDto> HostApproveSession(string hostKey, ApproveSessionDto approvalDto);
        SetResult<LeadScheduleResultDto> LeadNarrowsSchedule(string leadKey, LeadScheduleDto leadScheduleDto);
        SetResult PlayerJoinSession(string playerKey, PlayerJoinDto playerDto);
        SetResult HostFinalizes(string hostKey, HostFinalizeDto finalizeDto);

        FetchResult<SessionDto> GetPlayerSession(string playerKey);
        FetchResult<SessionDto> GetHostSession(string hostKey);
        FetchResult<SessionDto> GetLeadSession(string leadKey);
    }

    public class SessionService : ISessionService
    {
        private readonly ILogger _logger;
        private readonly ISessionRepository _repo;

        public SessionService(ISessionRepository repo, ILogger<SessionService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public SetResult<CreateSessionResultDto> StartSession(CreateSessionDto createDto)
        {
            var newSession = new SessionEntity();

            TimeResolutions resolution;
            try
            {
                resolution = ToApp(createDto.Resolution);
            }
            catch
            {
                return new SetResult<CreateSessionResultDto>(SetResultErrors.InvalidInput);
            }

            newSession.SetInfo(createDto.Title, createDto.Description, resolution);
            newSession.SetLead(createDto.Name);
            newSession.ProgressState();

            if (!newSession.IsValid)
            {
                _logger.LogDebug("invalid input", newSession.ValidationErrors);
                return new SetResult<CreateSessionResultDto>(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(newSession);
            if (!saveResult.IsSuccess)
            {
                _logger.LogError("unable to save new session", saveResult.Error);
                return new SetResult<CreateSessionResultDto>(ToApp(saveResult.Error.Value));
            }

            var dto = new CreateSessionResultDto()
            {
                HostKey = newSession.HostKey,
                LeadKey = newSession.LeadKey,
                PlayerKey = newSession.PlayerKey,
            };
            return new SetResult<CreateSessionResultDto>(dto);
        }

        private TimeResolutions ToApp(string resolution)
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

        private string ToDto(TimeResolutions resolution)
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

        public SetResult<ApproveSessionResultDto> HostApproveSession(string hostKey, ApproveSessionDto approvalDto)
        {
            var sessionGetResult = _repo.GetSessionHostKey(hostKey);
            if (!sessionGetResult.IsSuccess)
            {
                return new SetResult<ApproveSessionResultDto>(ToApp(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;

            session.SetHost(approvalDto.Name);
            session.SetHostSchedule(ToCore(approvalDto.Schedule));
            session.ProgressState();

            if (!session.IsValid)
            {
                return new SetResult<ApproveSessionResultDto>(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(session);
            if (!saveResult.IsSuccess)
            {
                return new SetResult<ApproveSessionResultDto>(ToApp(saveResult.Error.Value));
            }

            return new SetResult<ApproveSessionResultDto>(new ApproveSessionResultDto
            {
                HostKey = hostKey
            });
        }

        public SetResult<LeadScheduleResultDto> LeadNarrowsSchedule(string leadKey, LeadScheduleDto leadScheduleDto)
        {
            var sessionGetResult = _repo.GetSessionLeadKey(leadKey);
            if (!sessionGetResult.IsSuccess)
            {
                return new SetResult<LeadScheduleResultDto>(ToApp(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;

            session.SetLeadSchedule(ToCore(leadScheduleDto.Schedule));
            session.ProgressState();

            if (!session.IsValid)
            {
                return new SetResult<LeadScheduleResultDto>(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(session);
            if (!saveResult.IsSuccess)
            {
                return new SetResult<LeadScheduleResultDto>(ToApp(saveResult.Error.Value));
            }

            return new SetResult<LeadScheduleResultDto>(new LeadScheduleResultDto
            {
                HostKey = session.HostKey,
                LeadKey = session.LeadKey,
                PlayerKey = session.PlayerKey
            });
        }

        public SetResult PlayerJoinSession(string playerKey, PlayerJoinDto playerDto)
        {
            var sessionGetResult = _repo.GetSessionPlayerKey(playerKey);
            if (!sessionGetResult.IsSuccess)
            {
                return new SetResult<bool>(ToApp(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;

            session.AddPlayer(playerDto.Name, ToCore(playerDto.Schedule));

            if (!session.IsValid)
            {
                return new SetResult(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(session);
            if (!saveResult.IsSuccess)
            {
                return new SetResult(ToApp(saveResult.Error.Value));
            }

            return new SetResult();
        }

        public SetResult HostFinalizes(string hostKey, HostFinalizeDto finalizeDto)
        {
            var sessionGetResult = _repo.GetSessionHostKey(hostKey);
            if (!sessionGetResult.IsSuccess)
            {
                return new SetResult<bool>(ToApp(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;

            session.SetFinalSchedule(ToCore(finalizeDto.Schedule));
            session.ProgressState();

            if (!session.IsValid)
            {
                return new SetResult(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(session);
            if (!saveResult.IsSuccess)
            {
                return new SetResult(ToApp(saveResult.Error.Value));
            }

            return new SetResult();
        }


        public FetchResult<SessionDto> GetHostSession(string hostKey)
        {
            try
            {
                var sessionResult = _repo.GetSessionHostKey(hostKey);
                return ToDto(hostKey, sessionResult);
            }
            catch
            {
                return new FetchResult<SessionDto>(FetchResultErrors.Technical);
            }
        }

        public FetchResult<SessionDto> GetLeadSession(string leadKey)
        {
            try
            {
                var sessionResult = _repo.GetSessionLeadKey(leadKey);
                return ToDto(leadKey, sessionResult);
            }
            catch
            {
                return new FetchResult<SessionDto>(FetchResultErrors.Technical);
            }
        }

        public FetchResult<SessionDto> GetPlayerSession(string playerKey)
        {
            try
            {
                var sessionResult = _repo.GetSessionPlayerKey(playerKey);
                return ToDto(playerKey, sessionResult);
            }
            catch
            {
                return new FetchResult<SessionDto>(FetchResultErrors.Technical);
            }
        }


        #region Mappings

        private SetResultErrors ToApp(SessionRepository.QueryResultErrors error)
        {
            switch (error)
            {
                case SessionRepository.QueryResultErrors.NotFound:
                    return SetResultErrors.NotFound;
                case SessionRepository.QueryResultErrors.Technical:
                    return SetResultErrors.Technical;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SessionSchedule ToCore(SessionScheduleDateDto[] schedule)
        {
            return new SessionSchedule(
                options: schedule?
                    .Select(s => new SessionScheduleOption(s.Start, s.End)
                ).ToArray());
        }

        private FetchResult<SessionDto> ToDto(string getKey, SessionRepository.QueryResult<SessionEntity> sessionResult)
        {
            switch (sessionResult.Error)
            {
                case SessionRepository.QueryResultErrors.NotFound:
                    return new FetchResult<SessionDto>(FetchResultErrors.NotFound);
                case SessionRepository.QueryResultErrors.Technical:
                    return new FetchResult<SessionDto>(FetchResultErrors.Technical);
                case null:
                    var dto = ToDto(getKey, sessionResult.Result);
                    return new FetchResult<SessionDto>(dto);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sessionResult));
            }
        }

        private SetResultErrors ToApp(SessionRepository.UpdateResultErrors error)
        {
            switch (error)
            {
                case SessionRepository.UpdateResultErrors.NotFound:
                    return SetResultErrors.NotFound;
                case SessionRepository.UpdateResultErrors.Technical:
                    return SetResultErrors.Technical;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SessionDto ToDto(string getKey, SessionEntity session)
        {
            var dto = new SessionDto
            {
                HostKey = session.HostKey,
                LeadKey = session.LeadKey,
                PlayerKey = session.PlayerKey,

                Status = ToDto(session.SessionState),

                Title = session.Title,
                Description = session.Description,
                Resolution = ToDto(session.Resolution),
                //post date

                Players = session.Players.Select(p => ToDto(p)).ToArray(),

                FinalizedSchedule = ToDto(session.FinalizedSchedule),
            };
            if (session.LeadName != null)
            {
                dto.Lead = new SessionDto.PlayerDto
                {
                    Name = session.LeadName,
                    Schedule = ToDto(session.LeadSchedule),
                };
            }
            if(session.HostName != null) 
            {
                dto.Host = new SessionDto.PlayerDto
                {
                    Name = session.HostName,
                    Schedule = ToDto(session.HostSchedule),
                };
            }

            if(getKey == session.PlayerKey)
            {
                dto.HostKey = null;
                dto.LeadKey = null;
            }
            if(getKey == session.HostKey)
            {
                dto.LeadKey = null;
            }

            return dto;
        }

        private SessionDto.PlayerDto ToDto(Player player)
        {
            return new SessionDto.PlayerDto
            {
                Name = player.Name,
                Schedule = ToDto(player.Schedule)
            };
        }

        private SessionDto.SessionStatusDto ToDto(SessionStates sessionState)
        {
            switch (sessionState)
            {
                case SessionStates.Created:
                    throw new InvalidOperationException();
                case SessionStates.UnApproved:
                    return SessionDto.SessionStatusDto.Posted;
                case SessionStates.Approved:
                    return SessionDto.SessionStatusDto.Approved;
                case SessionStates.Open:
                    return SessionDto.SessionStatusDto.Open;
                case SessionStates.Finalized:
                    return SessionDto.SessionStatusDto.Finalized;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SessionScheduleDateDto[] ToDto(SessionSchedule schedule)
        {
            return schedule?.Options.Select(o => new SessionScheduleDateDto
            {
                Start = o.Start,
                End = o.End
            }).ToArray();
        }

        #endregion

        #region Local Result Typings

        public class SetResult
        {
            public SetResultErrors? Error { get; private set; }
            public bool IsSuccess => Error == null;

            public SetResult(SetResultErrors? error = null)
            {
                Error = error;
            }
        }
        public class SetResult<T> : SetResult
        {
            public T Result { get; private set; }

            public SetResult(SetResultErrors error)
                : base(error)
            {
                Result = default(T);
            }
            public SetResult(T result)
            {
                Result = result;
            }
        }
        public enum SetResultErrors
        {
            NotFound,
            Technical,
            InvalidInput
        }

        public class FetchResult<T>
        {
            public T Result { get; private set; }
            public FetchResultErrors? Error { get; private set; }
            public bool IsSuccess => Error == null;

            public FetchResult(T result)
            {
                Result = result;
                Error = null;
            }

            public FetchResult(FetchResultErrors error)
            {
                Result = default(T);
                Error = error;
            }
        }
        public enum FetchResultErrors
        {
            NotFound,
            Technical
        }

        #endregion
    }
}
