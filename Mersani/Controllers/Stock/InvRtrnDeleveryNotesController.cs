using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvRtrnDeleveryNotesController : GeneralBaseController
    {
        readonly InvRtrnDeleveryNotesRepo _ReturnDeleveryNoteRepo;
        public InvRtrnDeleveryNotesController(InvRtrnDeleveryNotesRepo ReturnDeleveryNoteRepo)
        {
            this._ReturnDeleveryNoteRepo = ReturnDeleveryNoteRepo;
        }
 

        [HttpGet("master/{id}/{PostedType}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id, string PostedType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ReturnDeleveryNoteRepo.GetRtrnDeleveryNoteHdr(new InvRtrnDnHdr() { IRDNH_SYS_ID = id }, PostedType, authParms));
        }

        [HttpGet("Item/{id}")]
        public async Task<ActionResult> GetInvoicesReturnDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ReturnDeleveryNoteRepo.GetInvRtrnDnDtl(new InvRtrnDnDtl() { IRDND_IRDNH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode/{inventory}")]
        public async Task<ActionResult> GetLastInvoicesReturnCode(int inventory)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ReturnDeleveryNoteRepo.GetRtrnDeleveryNoteLastCode(inventory, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostInvoicesReturnData([FromBody] InvRtrnDeleveryNotesData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ReturnDeleveryNoteRepo.SaveRtrnDeleveryNoteHdrandItem(entity, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRtrnDeleveryNoteMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _ReturnDeleveryNoteRepo.DeleteRtrnDeleveryNote(new InvRtrnDnHdr() { IRDNH_SYS_ID = id }, authParms));
        }
        [HttpPost("posting")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<InvRtrnDnHdr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ReturnDeleveryNoteRepo.RtrnDeleveryNotePosting(entities, authParms));
        }


    }
}
