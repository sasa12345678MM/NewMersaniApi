using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : GeneralBaseController
    {
        protected readonly IUserRolesRepo _UserRolesRepo;
        public UserRolesController(IUserRolesRepo UserRolesRepo)
        {
            _UserRolesRepo = UserRolesRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> getUserRoles([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _UserRolesRepo.getUserRoles(new UserRoles() { GUR_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteUserRoles([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _UserRolesRepo.deleteUserRoles(new UserRoles() { GUR_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> bulkUserRoles([FromBody] List<UserRoles> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _UserRolesRepo.bulkUserRoles(entities, authParms));
        }

    }

}
