using Mersani.Interfaces.Archive;
using Mersani.Interfaces.Notifications;
using Mersani.models.Archive;
using Mersani.models.Hubs;
using Mersani.models.Notifications;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralArchiveSetupController : GeneralBaseController
    {
        protected readonly GeneralArchiveSetupRepo _GeneralArchiveSetupRepo;
        private IHubContext<NotificationsHub> _hubContext;

        public GeneralArchiveSetupController(GeneralArchiveSetupRepo GeneralArchiveSetupRepo, IHubContext<NotificationsHub> hubContext)
        {
            _GeneralArchiveSetupRepo = GeneralArchiveSetupRepo;
            _hubContext = hubContext;
        }


        #region header
        [HttpGet("head/{id}")]
        public async Task<ActionResult> GetGeneralArchiveSetupHeaders([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveSetupRepo.GetGeneralArchiveSetupHeaders(new LArchiveHead() { AH_SYS_ID = id }, authParms));
        }

        [HttpPost("head/bulk")]
        public async Task<ActionResult> bulkGeneralArchiveSetupHeaders([FromBody] List<LArchiveHead> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveSetupRepo.BulkGeneralArchiveSetupHeaders(entities, authParms));
        }

        [HttpDelete("head/{id}")]
        public async Task<ActionResult> DeleteGeneralArchiveSetupHeader([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveSetupRepo.DeleteGeneralArchiveSetupHeader(new LArchiveHead() { AH_SYS_ID = id }, authParms));
        }
        #endregion

        #region detail
        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetGeneralArchiveSetupDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveSetupRepo.GetGeneralArchiveSetupDetails(new LArchiveDetail() { AD_AH_CODE= id }, authParms));
        }

        [HttpPost("detail/bulk")]
        public async Task<ActionResult> bulkGeneralArchiveSetupDetails([FromBody] List<LArchiveDetail> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveSetupRepo.BulkGeneralArchiveSetupDetails(entities, authParms));
        }

        [HttpDelete("detail/{id}")]
        public async Task<ActionResult> DeleteGeneralArchiveSetupDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveSetupRepo.DeleteGeneralArchiveSetupDetail(new LArchiveDetail() { AD_CODE = id }, authParms));
        }
        #endregion

     
    }

}
