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
        ApproveSessionResultDto DmApproveSession(string dmKey, ApproveSessionDto approvalDto);
        LeadScheduleResultDto LeadNarrowsSchedule(string leadKey, LeadScheduleDto leadScheduleDto);
        bool PlayerJoinSession(string playerKey, PlayerJoinDto playerDto);
        bool DmFinalizes(string dmKey);

        FetchResult<SessionDto> GetPlayerSession(string playerKey);
        FetchResult<SessionDto> GetDmSession(string dmKey);
        FetchResult<SessionDto> GetLeadSession(string leadKey);
    }

    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _repo;

        public SessionService(ISessionRepository repo)
        {
            _repo = repo;
        }

        public SetResult<CreateSessionResultDto> StartSession(string leadKey, CreateSessionDto createDto)
        {
            var newSession = new SessionEntity();

            newSession.Title = createDto.Title;
            newSession.Description = createDto.Description;
            newSession.LeadName = createDto.Name;

            if(!newSession.IsValid)
            {
                return new SetResult<CreateSessionResultDto>(SetResult<CreateSessionResultDto>.SetErrors.InvalidInput);
            }

            try
            {
                var saveResult = _repo.Save(newSession);
                switch (saveResult.Error)
                {
                    case SessionRepository.UpdateResult<SessionEntity>.UpdateErrors.NotFound:
                        return new SetResult<CreateSessionResultDto>(SetResult<CreateSessionResultDto>.SetErrors.NotFound);
                    case SessionRepository.UpdateResult<SessionEntity>.UpdateErrors.Technical:
                        return new SetResult<CreateSessionResultDto>(SetResult<CreateSessionResultDto>.SetErrors.Technical);
                    case null:
                        var dto = new CreateSessionResultDto()
                        {
                            DmKey = newSession.DmKey,
                            LeadKey = newSession.LeadKey,
                            PlayerKey = newSession.PlayerKey,
                        };
                        return new SetResult<CreateSessionResultDto>(dto);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                return new SetResult<CreateSessionResultDto>(SetResult<CreateSessionResultDto>.SetErrors.Technical);
            }
        }

        public ApproveSessionResultDto DmApproveSession(string dmKey, ApproveSessionDto approvalDto)
        {
            throw new NotImplementedException();
        }

        public LeadScheduleResultDto LeadNarrowsSchedule(string leadKey, LeadScheduleDto leadScheduleDto)
        {
            throw new NotImplementedException();
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
                return new FetchResult<SessionDto>(FetchResult<SessionDto>.FetchErrors.Technical);
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
                return new FetchResult<SessionDto>(FetchResult<SessionDto>.FetchErrors.Technical);
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
                return new FetchResult<SessionDto>(FetchResult<SessionDto>.FetchErrors.Technical);
            }
        }



        private FetchResult<SessionDto> TranslateGet(SessionRepository.QueryResult<SessionEntity> sessionResult)
        {
            switch (sessionResult.Error)
            {
                case SessionRepository.QueryResult<SessionEntity>.QueryErrors.NotFound:
                    return new FetchResult<SessionDto>(FetchResult<SessionDto>.FetchErrors.NotFound);
                case SessionRepository.QueryResult<SessionEntity>.QueryErrors.Technical:
                    return new FetchResult<SessionDto>(FetchResult<SessionDto>.FetchErrors.Technical);
                case null:
                    var dto = TransformSession(sessionResult.Result);
                    return new FetchResult<SessionDto>(dto);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sessionResult));
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
            public SetErrors? Error { get; private set; }
            public bool IsSuccess => Error != null;

            public SetResult(SetErrors error)
            {
                Result = null;
                Error = error;
            }
            public SetResult(T result)
            {
                Result = result;
                Error = null;
            }

            public enum SetErrors
            {
                NotFound,
                Technical,
                InvalidInput
            }
        }

        public class FetchResult<T>
            where T : class
        {
            public T Result { get; private set; }
            public FetchErrors? Error { get; private set; }
            public bool IsSuccess => Error != null;

            public FetchResult(T result)
            {
                Result = result;
                Error = null;
            }

            public FetchResult(FetchErrors error)
            {
                Result = null;
                Error = error;
            }

            public enum FetchErrors
            {
                NotFound,
                Technical
            }
        }
    }
}
