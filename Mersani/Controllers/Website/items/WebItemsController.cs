using Mersani.Interfaces.Website.items;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Website.items
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebItemsController : GeneralBaseController
    {

        protected readonly Iwebitems _webitems;
        public WebItemsController(Iwebitems webitems)
        {
            _webitems = webitems;
        }

        [HttpGet("GetItemsWithPriceAndDiscount/{itemId}/{Curr}")]
        public async Task<ActionResult> GetItemsWithPriceAndDiscount([FromRoute] int itemId, int Curr)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _webitems.GetItemsWithPriceAndDiscount(itemId, Curr, authParms));
        }

        [HttpGet("GetItemOffers/{Curr}")]
        public async Task<ActionResult> GetItemOffers([FromRoute] int Curr)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _webitems.GetItemOffers(Curr, authParms));
        }
        [HttpGet("GetItemsWithName/{name}/{Curr}")]
        public async Task<ActionResult> GetItemsWithName([FromRoute]string name,  [FromRoute] int Curr)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _webitems.GetItemsWithName(name,Curr, authParms));
        }
        [HttpGet("GetItemImages/{itemId}")]
        public async Task<ActionResult> GetItemImages([FromRoute] int itemId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";//CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _webitems.GetItemsImages(itemId, authParms));
        }

        [HttpGet("GetItemByGroup/{groupId}/{curr}")]
        public async Task<ActionResult> GetItemByGroup([FromRoute] int groupId, [FromRoute] int curr)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _webitems.GetItemsByGroup(groupId,curr, authParms));
        }

        [HttpGet("GetRelatedItems/{itemid}/{curr}")]
        public async Task<ActionResult> GetRelatedItems([FromRoute] int itemid, [FromRoute] int curr)
        {
           if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
           string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
           return Ok(await _webitems.GetRelatedItems(itemid,curr, authParms));
        }

        [HttpGet("GetItemsByMenufacturer/{menid}/{curr}")]
        public async Task<ActionResult> GetItemsByMenufacturer([FromRoute] int menid, [FromRoute] int curr)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _webitems.GetItemsByMenufacturer(menid, curr, authParms));
        } 
    }
}
