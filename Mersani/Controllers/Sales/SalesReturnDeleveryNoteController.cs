using Mersani.Interfaces.Sales;
using Mersani.models.Sales;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReturnDeleveryNoteController : GeneralBaseController
    {
        protected readonly SalesReturnDeleveryNoteRepo _SalesReturnDeleveryNoteRepo;

        public SalesReturnDeleveryNoteController(SalesReturnDeleveryNoteRepo SalesReturnDeleveryNoteRepo)
        {
            _SalesReturnDeleveryNoteRepo = SalesReturnDeleveryNoteRepo;
        }

        [HttpGet("master/{id}/{PostedType}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id, string PostedType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesReturnDeleveryNoteRepo.GetRtrnDeleveryNoteHdr(new InvSalesRtrnDnHdr() { ISRDH_SYS_ID = id }, PostedType, authParms));
        }

        [HttpGet("Item/{id}")]
        public async Task<ActionResult> GetSalesInvoicesReturnDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesReturnDeleveryNoteRepo.GetInvSalesRtrnDnDtl(new InvSalesRtrnDnDtl() { ISRDD_ISRDH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastSalesInvoicesReturnCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesReturnDeleveryNoteRepo.GetRtrnDeleveryNoteLastCode(authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostSalesInvoicesReturnData([FromBody] InvSalesReturnDeleveryNote entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesReturnDeleveryNoteRepo.SaveRtrnDeleveryNoteHdrandItem(entity, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRtrnDeleveryNoteMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _SalesReturnDeleveryNoteRepo.DeleteRtrnDeleveryNote(new InvSalesRtrnDnHdr() { ISRDH_SYS_ID = id }, authParms));
        }
        [HttpPost("posting")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<InvSalesRtrnDnHdr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesReturnDeleveryNoteRepo.RtrnDeleveryNotePosting(entities, authParms));
        }


    }
}