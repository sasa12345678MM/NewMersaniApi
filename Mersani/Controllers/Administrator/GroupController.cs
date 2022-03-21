using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : GeneralBaseController
    {
        protected readonly IGroupRepo _groupRepo;
        public GroupController(IGroupRepo groupRepo)
        {
            _groupRepo = groupRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _groupRepo.GetGroup(new Group() { GROUP_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> bulkGroups([FromBody] List<Group> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _groupRepo.BulkGroups(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _groupRepo.DeleteGroup(new Group() { GROUP_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _groupRepo.GetLastCode(authParms));
        }
    }
}
