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
    public class ItemManufacturerController : GeneralBaseController
    {
        protected readonly IItemManufacturerRepo _itemManufacturerRepo;
        public ItemManufacturerController(IItemManufacturerRepo itemManufacturerRepo)
        {
            _itemManufacturerRepo = itemManufacturerRepo;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetItemManufacturers([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemManufacturerRepo.GetItemManufacturers(new StockItemManufacturer() { IIMF_SYS_ID = id }, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemManufacturer([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemManufacturerRepo.DeleteItemManufacturer(new StockItemManufacturer() { IIMF_SYS_ID = id }, authParms));
        }
                [HttpPost("bulk")]
        public async Task<ActionResult> BulkItemManufacturers([FromBody] List<StockItemManufacturer> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemManufacturerRepo.BulkItemManufacturers(entities, authParms));
        }
    }
}
