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
        SetResult<CreateSessionResultDto> StartSession(string leadKey, CreateSessionDto createDto);
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
        private readonly ISiteLogger _logger;

        public SessionService(ISessionRepository repo, ISiteLogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public SetResult<CreateSessionResultDto> StartSession(string leadKey, CreateSessionDto createDto)
        {
            var newSession = new SessionEntity();

            newSession.SetInfo(createDto.Title, createDto.Description);
            newSession.SetLead(createDto.Name);

            if (!newSession.IsValid)
            {
                return new SetResult<CreateSessionResultDto>(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(newSession);
            if (!saveResult.IsSuccess)
            {
                return new SetResult<CreateSessionResultDto>(Translate(saveResult.Error.Value));
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
                return new SetResult<ApproveSessionResultDto>(Translate(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;

            session.SetHost(approvalDto.Name);
            session.SetHostSchedule(TranslateSchedule(approvalDto.Schedule));

            if (!session.IsValid)
            {
                return new SetResult<ApproveSessionResultDto>(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(session);
            if (!saveResult.IsSuccess)
            {
                return new SetResult<ApproveSessionResultDto>(Translate(saveResult.Error.Value));
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
                return new SetResult<LeadScheduleResultDto>(Translate(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;

            session.SetLeadSchedule(TranslateSchedule(leadScheduleDto.Schedule));

            if (!session.IsValid)
            {
                return new SetResult<LeadScheduleResultDto>(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(session);
            if (!saveResult.IsSuccess)
            {
                return new SetResult<LeadScheduleResultDto>(Translate(saveResult.Error.Value));
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
                return new SetResult<bool>(Translate(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;

            session.AddPlayer(playerDto.Name, TranslateSchedule(playerDto.Schedule));

            if (!session.IsValid)
            {
                return new SetResult(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(session);
            if (!saveResult.IsSuccess)
            {
                return new SetResult(Translate(saveResult.Error.Value));
            }

            return new SetResult();
        }

        public SetResult HostFinalizes(string hostKey, HostFinalizeDto finalizeDto)
        {
            var sessionGetResult = _repo.GetSessionHostKey(hostKey);
            if (!sessionGetResult.IsSuccess)
            {
                return new SetResult<bool>(Translate(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;


            session.SetFinalSchedule(finalizeDto);
            session.Finalize();


            if (!session.IsValid)
            {
                return new SetResult(SetResultErrors.InvalidInput);
            }

            var saveResult = _repo.Save(session);
            if (!saveResult.IsSuccess)
            {
                return new SetResult(Translate(saveResult.Error.Value));
            }

            return new SetResult();
        }



        public FetchResult<SessionDto> GetHostSession(string hostKey)
        {
            try
            {
                var sessionResult = _repo.GetSessionHostKey(hostKey);
                return TranslateGet(sessionResult);
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
                return TranslateGet(sessionResult);
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
                return TranslateGet(sessionResult);
            }
            catch
            {
                return new FetchResult<SessionDto>(FetchResultErrors.Technical);
            }
        }


        private SetResultErrors Translate(SessionRepository.QueryResultErrors error)
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

        private SessionSchedule[] TranslateSchedule(SessionScheduleDateDto[] schedule)
        {
            throw new NotImplementedException();
        }

        private FetchResult<SessionDto> TranslateGet(SessionRepository.QueryResult<SessionEntity> sessionResult)
        {
            switch (sessionResult.Error)
            {
                case SessionRepository.QueryResultErrors.NotFound:
                    return new FetchResult<SessionDto>(FetchResultErrors.NotFound);
                case SessionRepository.QueryResultErrors.Technical:
                    return new FetchResult<SessionDto>(FetchResultErrors.Technical);
                case null:
                    var dto = TransformSession(sessionResult.Result);
                    return new FetchResult<SessionDto>(dto);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sessionResult));
            }
        }

        private SetResultErrors Translate(SessionRepository.UpdateResultErrors error)
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

        private SessionDto TransformSession(SessionEntity session)
        {
            throw new NotImplementedException();
        }

        public class SetResult
        {
            public SetResultErrors? Error { get; private set; }
            public bool IsSuccess => Error != null;

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
            public bool IsSuccess => Error != null;

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
    }
}
