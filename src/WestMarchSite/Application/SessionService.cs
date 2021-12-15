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
        SetResult<ApproveSessionResultDto> DmApproveSession(string dmKey, ApproveSessionDto approvalDto);
        SetResult<LeadScheduleResultDto> LeadNarrowsSchedule(string leadKey, LeadScheduleDto leadScheduleDto);
        bool PlayerJoinSession(string playerKey, PlayerJoinDto playerDto);
        bool DmFinalizes(string dmKey);

        FetchResult<SessionDto> GetPlayerSession(string playerKey);
        FetchResult<SessionDto> GetDmSession(string dmKey);
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

            newSession.Title = createDto.Title;
            newSession.Description = createDto.Description;
            newSession.LeadName = createDto.Name;

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
                DmKey = newSession.DmKey,
                LeadKey = newSession.LeadKey,
                PlayerKey = newSession.PlayerKey,
            };
            return new SetResult<CreateSessionResultDto>(dto);
        }

        public SetResult<ApproveSessionResultDto> DmApproveSession(string dmKey, ApproveSessionDto approvalDto)
        {
            var sessionGetResult = _repo.GetSessionDmKey(dmKey);
            if (!sessionGetResult.IsSuccess)
            {
                return new SetResult<ApproveSessionResultDto>(Translate(sessionGetResult.Error.Value));
            }

            var session = sessionGetResult.Result;

            session.DmName = approvalDto.Name;
            session.SetDmSchedule(TranslateSchedule(approvalDto.Schedule));

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
                DmKey = dmKey
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
                DmKey = session.DmKey,
                LeadKey = session.LeadKey,
                PlayerKey = session.PlayerKey
            });
        }

        public bool PlayerJoinSession(string playerKey, PlayerJoinDto playerDto)
        {
            throw new NotImplementedException();
        }

        public bool DmFinalizes(string dmKey)
        {
            throw new NotImplementedException();
        }





        public FetchResult<SessionDto> GetDmSession(string dmKey)
        {
            try
            {
                var sessionResult = _repo.GetSessionDmKey(dmKey);
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

        public class SetResult<T>
            where T : class
        {
            public T Result { get; private set; }
            public SetResultErrors? Error { get; private set; }
            public bool IsSuccess => Error != null;

            public SetResult(SetResultErrors error)
            {
                Result = null;
                Error = error;
            }
            public SetResult(T result)
            {
                Result = result;
                Error = null;
            }
        }
        public enum SetResultErrors
        {
            NotFound,
            Technical,
            InvalidInput
        }

        public class FetchResult<T>
            where T : class
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
                Result = null;
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
