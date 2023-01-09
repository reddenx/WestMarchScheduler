using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpGet("{key}")]
        [ProducesResponseType(typeof(SessionDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetSession([FromRoute] string key)
        {
            var playerResult = _sessionService.GetPlayerSession(key);
            if (playerResult.IsSuccess)
            {
                return Json(playerResult.Result);
            }
            else if (playerResult.Error != SessionService.FetchResultErrors.NotFound)
            {
                return MapError(playerResult.Error);
            }

            var leadResult = _sessionService.GetLeadSession(key);
            if (leadResult.IsSuccess)
            {
                return Json(leadResult.Result);
            }
            else if (leadResult.Error != SessionService.FetchResultErrors.NotFound)
            {
                return MapError(leadResult.Error);
            }

            var hostResult = _sessionService.GetHostSession(key);
            if (hostResult.IsSuccess)
            {
                return Json(hostResult.Result);
            }
            return MapError(hostResult.Error);
        }

        [HttpGet("{key}/host")]
        [ProducesResponseType(200, Type = typeof(SessionDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetHostSession([FromRoute] string key)
        {
            var result = _sessionService.GetHostSession(key);
            if (result.IsSuccess)
            {
                return Json(result.Result);
            }

            return MapError(result.Error);
        }

        [HttpGet("{key}/player")]
        [ProducesResponseType(200, Type = typeof(SessionDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetPlayerSession([FromRoute] string key)
        {
            var result = _sessionService.GetPlayerSession(key);
            if (result.IsSuccess)
            {
                return Json(result.Result);
            }

            return MapError(result.Error);
        }

        [HttpGet("{key}/lead")]
        [ProducesResponseType(200, Type = typeof(SessionDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetLeadSession([FromRoute] string key)
        {
            var result = _sessionService.GetLeadSession(key);
            if (result.IsSuccess)
            {
                return Json(result.Result);
            }

            return MapError(result.Error);
        }

        [HttpPost("")]
        [ProducesResponseType(200, Type = typeof(CreateSessionResultDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateSession([FromBody] CreateSessionDto create)
        {
            var result = _sessionService.StartSession(create);
            if (result.IsSuccess)
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
        public IActionResult ApproveSession([FromRoute] string key, [FromBody] ApproveSessionDto approval)
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
        public IActionResult LeadSchedule([FromRoute] string key, [FromBody] LeadScheduleDto lead)
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
        public IActionResult PlayerJoin([FromRoute] string key, [FromBody] PlayerJoinDto join)
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
        public IActionResult Finalize([FromRoute] string key, [FromBody] HostFinalizeDto finalize)
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

        private IActionResult MapError(SessionService.FetchResultErrors? error)
        {
            switch (error)
            {
                case SessionService.FetchResultErrors.NotFound:
                    return StatusCode(404);
                case SessionService.FetchResultErrors.Technical:
                    return StatusCode(500);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
