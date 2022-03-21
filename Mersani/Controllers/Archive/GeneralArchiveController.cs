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
    public class GeneralArchiveController : GeneralBaseController
    {
        protected readonly GeneralArchiveRepo _GeneralArchiveRepo;
        private IHubContext<NotificationsHub> _hubContext;

        public GeneralArchiveController(GeneralArchiveRepo GeneralArchiveRepo, IHubContext<NotificationsHub> hubContext)
        {
            _GeneralArchiveRepo = GeneralArchiveRepo;
            _hubContext = hubContext;
        }


        #region header
        [HttpGet("head/{id}")]
        public async Task<ActionResult> GetGeneralArchiveHeaders([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveRepo.GetGeneralArchiveHeaders(new ArchiveHead() { AH_SYS_ID = id }, authParms));
        }

 
        [HttpPost("")]
        public async Task<ActionResult> saveGeneralArchive([FromBody] GeneralArchives entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveRepo.saveGeneralArchive(entities, authParms));
        }
        [HttpDelete("head/{id}")]
        public async Task<ActionResult> DeleteGeneralArchiveHeader([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveRepo.DeleteGeneralArchiveHeader(new ArchiveHead() { AH_SYS_ID = id }, authParms));
        }
        #endregion

        #region detail
        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetGeneralArchiveDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveRepo.GetGeneralArchiveDetails(new ArchiveDetail() { AD_AH_SYS_ID= id }, authParms));
        }

        [HttpPost("detail/bulk")]
        public async Task<ActionResult> bulkGeneralArchiveDetails([FromBody] List<ArchiveDetail> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveRepo.BulkGeneralArchiveDetails(entities, authParms));
        }

        [HttpDelete("detail/{id}")]
        public async Task<ActionResult> DeleteGeneralArchiveDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveRepo.DeleteGeneralArchiveDetail(new ArchiveDetail() { AD_SYS_ID = id }, authParms));
        }
        #endregion
        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralArchiveRepo.GetLastCode(authParms));
        }

    }

}
