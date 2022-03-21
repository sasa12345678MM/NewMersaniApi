using Mersani.Interfaces.CallCenter;
using Mersani.models.CostCenter;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.CallCenter
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketMasterController : GeneralBaseController
    {
        protected readonly ITicketMasterRepo _ticketMasterRepo;

        public TicketMasterController(ITicketMasterRepo TicketMasterRepo)
        {
            _ticketMasterRepo = TicketMasterRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTicketMasterData([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ticketMasterRepo.GetTicketMasterData(new TicketMaster() { TTM_SYS_ID = id }, authParms));
        }

        [HttpGet("ByCustomer")]
        public async Task<ActionResult> GetTicketMasterDataByCustomer()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ticketMasterRepo.GetTicketMasterDataByCustomer(authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ticketMasterRepo.GetLastCode(authParms));
        }


        [HttpPost]
        public async Task<ActionResult> UpdateTicketMaster([FromBody] TicketMaster entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _ticketMasterRepo.UpdateTicketMaster(entity, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteTicketMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _ticketMasterRepo.deleteTicketMaster(new TicketMaster() { TTM_SYS_ID = id }, authParms));
        }
        [HttpGet("Detail/{id}/{ParentId}")]
        public async Task<ActionResult> GetTicketDetail([FromRoute] int id,int ParentId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ticketMasterRepo.GetTicketDetail(new TktTicketDetail() { TTD_SYS_ID = id,TTD_TTM_SYS_ID= ParentId },authParms));
        }
 

        [HttpPost("Detail")]
        public async Task<ActionResult> SaveTicketDetail([FromBody] List<TktTicketDetail> entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _ticketMasterRepo.SaveTicketDetail(entity, authParms));
        }


        [HttpGet("UnAnswerd/{id}/{calltype}")]
        public async Task<ActionResult> getUnAnswerdTickedMaster([FromRoute] int id, string calltype)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ticketMasterRepo.getUnAnswerdTickedMaster(id, calltype, authParms));
        }
        [HttpPost("MasteDetail")]
        public async Task<ActionResult> SaveTicketMasteDetail([FromBody] TktTicketData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _ticketMasterRepo.SaveTicketMasteDetail(entity, authParms));
        }

        [HttpGet("Log/{id}")]
        public async Task<ActionResult> GetTicketMasterLogData([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ticketMasterRepo.GetTicketMasterLogData(new TicketMasterLog() { TTML_TTM_SYS_ID = id }, authParms));
        }


    }
}
