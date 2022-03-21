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
    public class ItemGroupsController : GeneralBaseController
    {
        protected readonly IItemGroupsRepo _itemGroupsRepo;
        public ItemGroupsController(IItemGroupsRepo itemGroupsRepo)
        {
            _itemGroupsRepo = itemGroupsRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetItemGroups([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemGroupsRepo.GetItemsGroups(new ItemGroups() { IIG_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemGroups([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemGroupsRepo.DeleteItemGroup(new ItemGroups() { IIG_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> BulkItemGroups([FromBody] List<ItemGroups> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemGroupsRepo.BulkItemsGroups(entities, authParms));
        }
    }
}
