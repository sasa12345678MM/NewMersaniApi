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
    public class SalesDeleveryNoteController : GeneralBaseController
    {
        protected readonly SalesDeleveryNoteRepo _SalesDeleveryNoteRepo;

        public SalesDeleveryNoteController(SalesDeleveryNoteRepo SalesDeleveryNoteRepo)
        {
            _SalesDeleveryNoteRepo = SalesDeleveryNoteRepo;
        }

        [HttpGet("master/{id}/{PostedType}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id, string PostedType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesDeleveryNoteRepo.GetDeleveryNoteHdr(new InvSalesDnHdr() { ISDH_SYS_ID = id }, PostedType, authParms));
        }

        [HttpGet("Item/{id}")]
        public async Task<ActionResult> GetSalesInvoicesReturnDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesDeleveryNoteRepo.GetInvSalesDnDtl(new InvSalesDnDtl() { ISDD_ISDH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastSalesInvoicesReturnCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesDeleveryNoteRepo.GetDeleveryNoteLastCode(authParms));
        }

 
        [HttpPost]
        public async Task<ActionResult> PostSalesInvoicesReturnData([FromBody] InvSalesDeleveryNote entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesDeleveryNoteRepo.SaveDeleveryNoteHdrandItem(entity, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeleveryNoteMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _SalesDeleveryNoteRepo.DeleteDeleveryNote(new InvSalesDnHdr() { ISDH_SYS_ID = id }, authParms));
        }
        [HttpPost("posting")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<InvSalesDnHdr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesDeleveryNoteRepo.DeleveryNotePosting(entities, authParms));
        }
        [HttpGet("SalesOrder")]
        public async Task<ActionResult> getInvSalesOrderDtl(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesDeleveryNoteRepo.getInvSalesOrderDtl(id,authParms));
        }

        [HttpGet("currStk/{invSysId}/{itemSysId}/{batchSysId}/{uomSysId}")]
        public async Task<ActionResult> getInvItemcurrStk(int invSysId, int itemSysId, int batchSysId, int uomSysId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesDeleveryNoteRepo.getInvItemcurrStk(invSysId, itemSysId, batchSysId, uomSysId,authParms));
        }
    }
}