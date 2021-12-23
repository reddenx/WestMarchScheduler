using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ISessionRepository _repo;

        public SessionService(ISessionRepository repo)
        {
            _repo = repo;
        }

        public SetResult<CreateSessionResultDto> StartSession(CreateSessionDto createDto)
        {
            var newSession = new SessionEntity();

            newSession.SetInfo(createDto.Title, createDto.Description);
            newSession.SetLead(createDto.Name);
            newSession.ProgressState();

            if (!newSession.IsValid)
            {
                return new SetResult<CreateSessionResultDto>(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(newSession);
            if (!saveResult.IsSuccess)
            {
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
                return ToDto(sessionResult);
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
                return ToDto(sessionResult);
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
                return ToDto(sessionResult);
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

        private FetchResult<SessionDto> ToDto(SessionRepository.QueryResult<SessionEntity> sessionResult)
        {
            switch (sessionResult.Error)
            {
                case SessionRepository.QueryResultErrors.NotFound:
                    return new FetchResult<SessionDto>(FetchResultErrors.NotFound);
                case SessionRepository.QueryResultErrors.Technical:
                    return new FetchResult<SessionDto>(FetchResultErrors.Technical);
                case null:
                    var dto = ToDto(sessionResult.Result);
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

        private SessionDto ToDto(SessionEntity session)
        {
            return new SessionDto
            {
                Description = session.Description,
                Host = new SessionDto.HostDto
                {
                    Name = session.HostName,
                    Schedule = ToDto(session.HostSchedule),
                },
                LeadName = session.LeadName,
                OpenSchedule = ToDto(session.OpenSchedule),
                Status = ToDto(session.SessionState),
                Title = session.Title,
                Players = session.Players.Select(p => ToDto(p)).ToArray()
            };
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
