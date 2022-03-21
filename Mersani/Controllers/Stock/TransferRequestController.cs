using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.Interfaces.Stock;
using Mersani.models.Stock;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferRequestController : GeneralBaseController
    {
        protected readonly ITransferRequestRepo _transferRequestRepo;

        public TransferRequestController(ITransferRequestRepo transferRequestRepo)
        {
            _transferRequestRepo = transferRequestRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetTransferRequestMaster(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _transferRequestRepo.GetTransferRequestMaster(new TransferRequestMaster() { ITRH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetTransferDetails(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _transferRequestRepo.GetTransferRequestDetails(new TransferRequestMaster() { ITRH_SYS_ID = id }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostTransfer([FromBody] TransferRequest entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _transferRequestRepo.PostTransferRequestMasterDetails(entity, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastInvoiceCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _transferRequestRepo.GetTransferRequestLastCode(authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransfer([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _transferRequestRepo.DeleteTransferRequestMasterDetails(new TransferRequestDetails() { ITRD_ITRH_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteTransferItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _transferRequestRepo.DeleteTransferRequestMasterDetails(new TransferRequestDetails() { ITRD_SYS_ID = id }, 2, authParms));
        }
    }
}
