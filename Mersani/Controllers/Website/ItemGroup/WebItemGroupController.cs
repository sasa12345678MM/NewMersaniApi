using Mersani.Interfaces.Website.ItemGroups;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Website
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebItemGroupController : GeneralBaseController
    {
        IWebItemGroup _WebitemGroupsRepo;

        public WebItemGroupController(IWebItemGroup WebitemGroupsRepo )
        {
            _WebitemGroupsRepo = WebitemGroupsRepo;
        }

        [HttpGet("GetItemGroups/{id}")]
        public async Task<ActionResult> GetItemGroups([FromRoute] int id)
        {    if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _WebitemGroupsRepo.GetItemsGroups(new Mersani.models.Stock.ItemGroups() { IIG_SYS_ID = id }, authParms));
        }

        [HttpGet("GetItemGroupChildren/{id}")]
        public async Task<ActionResult> GetItemGroupChildren([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms ="";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _WebitemGroupsRepo.GetItemsGroupChildren(id, authParms));
        }
        [HttpGet("GetItemGroupByLevel/{levels}")]

        public async Task<ActionResult> GetItemGroupByLevel([FromRoute] string levels)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _WebitemGroupsRepo.GetItemsGroupsByLevel(levels, authParms));
        }
    }
}
