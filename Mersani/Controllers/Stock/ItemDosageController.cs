using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDosageController : GeneralBaseController
    {
        protected readonly IItemDosageRepo _itemDosageRepo;
        public ItemDosageController(IItemDosageRepo itemDosageRepo)
        {
            _itemDosageRepo = itemDosageRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetIUnits([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemDosageRepo.GetItemDosages(new StockItemDosage() { IIDF_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUnit([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemDosageRepo.DeleteItemDosage(new StockItemDosage() { IIDF_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> BulkUnits([FromBody] List<StockItemDosage> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemDosageRepo.BulkItemDosages(entities, authParms));
        }
    }
}
