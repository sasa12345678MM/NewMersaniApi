using Mersani.Interfaces.CallCenter;
using Mersani.models.Hubs;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.CallCenter
{
    [Route("api/[controller]")]
    [ApiController]
    public class TktChatController : GeneralBaseController
    {
        private readonly ITktChatRepo _tktChatRepo;
        public TktChatController(ITktChatRepo tktChatRepo)
        {
            _tktChatRepo = tktChatRepo;
        }

        [HttpPost("getChatHistory/search")]
        public async Task<ActionResult> GetChatHistory([FromBody] TktChat entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = ""; // CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _tktChatRepo.GetChatHistory(entity, authParms));
        }

        [HttpPost("getChatHistoryForCustomer/search")]
        public async Task<ActionResult> GetChatHistoryForCustomer([FromBody] TktChat entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";//CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _tktChatRepo.GetChatHistoryForCustomer(entity.TC_SENDER, authParms));
        }

        [HttpPost("getChatRecieversHistory/search")]
        public async Task<ActionResult> GetChatRecieversHistory([FromBody] TktChat entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";
            return Ok(await _tktChatRepo.GetChatRecieversHistory(entity, authParms));
        }
    }
}
