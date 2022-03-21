using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemBatchesController : GeneralBaseController
    {
        protected readonly IItemBatchesRepo _itemBatchesPharmRepo;
        private readonly IInventoryItemsRepo _inventoryItemsRepo;
        public ItemBatchesController(IItemBatchesRepo itemBatchesPharmRepo, IInventoryItemsRepo inventoryItemsRepo)
        {
            _itemBatchesPharmRepo = itemBatchesPharmRepo;
            _inventoryItemsRepo = inventoryItemsRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetItemBatches([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemBatchesPharmRepo.GetItemBatches(new ItemBatches() { IIB_III_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemBatches([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemBatchesPharmRepo.DeleteItemBatches(new ItemBatches() { IIB_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> BulkItemBatches([FromBody] List<ItemBatches> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemBatchesPharmRepo.BulkItemBatches(entities, authParms));
        }

        [HttpGet("GetInventoryByPharmacyId")]
        public async Task<ActionResult> GetInventoryByPharmacyId()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryItemsRepo.GetInventoryByPharmacyId(authParms));
        }


        [HttpGet("GetInventoryItemBatches/{id}/{code}")]
        public async Task<ActionResult> GetInventoryItemBatches([FromRoute] int id, int code)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryItemsRepo.GetInventoryItemBatches(id, code, authParms));
        }
    }
}
