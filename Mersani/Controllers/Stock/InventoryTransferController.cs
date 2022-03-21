using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mersani.Interfaces.Stock;
using Mersani.models.Stock;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryTransferController : GeneralBaseController
    {
        protected readonly IInventoryTransferRepo _inventoryTransferRepo;

        public InventoryTransferController(IInventoryTransferRepo IInventoryTransferRepo)
        {
            _inventoryTransferRepo = IInventoryTransferRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetTransferMaster(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryTransferRepo.GetTransferMaster(new TransferMaster() { ITM_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetTransferDetails(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryTransferRepo.GetTransferDetails(new TransferMaster() { ITM_SYS_ID = id }, authParms));
        }

        [HttpGet("GetByRequest/{id}")]
        public async Task<ActionResult> GetTransferDetailsByRequest(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryTransferRepo.GetTransferDetailsByRequest(id, authParms));
        }

        [HttpGet("GetLastCode/{id}")]
        public async Task<ActionResult> GetLastInvoiceCode([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryTransferRepo.GetTransferLastCode(id, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostTransfer([FromBody] InventoryTransfer entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryTransferRepo.PostTransferMasterDetails(entity, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransfer([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryTransferRepo.DeleteTransferMasterDetails(new TransferDetails() { ITD_ITM_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteTransferItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryTransferRepo.DeleteTransferMasterDetails(new TransferDetails() { ITD_SYS_ID = id }, 2, authParms));
        }
    }
}
