using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Application;
using WestMarchSite.Infrastructure;

namespace WestMarchSite.Controllers
{
    [Route("api/sessions")]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost("")]
        [ProducesResponseType(200, Type = typeof(CreateSessionResultDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateSession([FromBody]CreateSessionDto create)
        {
            var result = _sessionService.StartSession(create);
            if(result.IsSuccess)
            {
                return Json(result.Result);
            }

            return MapError(result.Error);
        }

        [HttpPut("{key}/approve")]
        [ProducesResponseType(200, Type = typeof(ApproveSessionResultDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult ApproveSession([FromRoute]string key, [FromBody]ApproveSessionDto approval)
        {
            var result = _sessionService.HostApproveSession(key, approval);
            if (result.IsSuccess)
            {
                return Json(result.Result);
            }

            return MapError(result.Error);
        }

        [HttpPut("{key}/schedule")]
        [ProducesResponseType(200, Type = typeof(LeadScheduleResultDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult LeadSchedule([FromRoute]string key, [FromBody]LeadScheduleDto lead)
        {
            var result = _sessionService.LeadNarrowsSchedule(key, lead);
            if (result.IsSuccess)
            {
                return Json(result.Result);
            }

            return MapError(result.Error);
        }

        [HttpPut("{key}/join")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult PlayerJoin([FromRoute]string key, [FromBody]PlayerJoinDto join)
        {
            var result = _sessionService.PlayerJoinSession(key, join);
            if (result.IsSuccess)
            {
                return StatusCode(204);
            }

            return MapError(result.Error);
        }

        [HttpPut("{key}/finalize")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Finalize([FromRoute]string key, [FromBody]HostFinalizeDto finalize)
        {
            var result = _sessionService.HostFinalizes(key, finalize);
            if (result.IsSuccess)
            {
                return StatusCode(204);
            }

            return MapError(result.Error);
        }

        private IActionResult MapError(SessionService.SetResultErrors? error)
        {
            switch (error)
            {
                case SessionService.SetResultErrors.InvalidInput:
                    return StatusCode(400);
                case SessionService.SetResultErrors.NotFound:
                    return StatusCode(404);
                case SessionService.SetResultErrors.Technical:
                    return StatusCode(500);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
