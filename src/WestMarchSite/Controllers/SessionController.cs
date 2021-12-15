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
            throw new NotImplementedException();
        }

        [HttpPut("{key}/approve")]
        [ProducesResponseType(200, Type = typeof(ApproveSessionResultDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult ApproveSession([FromRoute]string key, [FromBody]ApproveSessionDto approval)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{key}/schedule")]
        [ProducesResponseType(200, Type = typeof(LeadScheduleResultDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult LeadSchedule([FromRoute]string key, [FromBody]LeadScheduleDto lead)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{key}/join")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult PlayerJoin([FromRoute]string key, [FromBody]PlayerJoinDto join)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{key}/finalize")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Finalize([FromRoute]string key, [FromBody]HostFinalizeDto finalize)
        {
            throw new NotImplementedException();
        }
    }
}
