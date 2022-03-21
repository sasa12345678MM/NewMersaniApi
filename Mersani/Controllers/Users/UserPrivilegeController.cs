using Mersani.Interfaces.Users;
using Mersani.models.Users;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPrivilegeController : GeneralBaseController
    {
        protected readonly IUserPrivilegeRepo _userPrivilegeRepo;
        public UserPrivilegeController(IUserPrivilegeRepo userPrivilegeRepo)
        {
            _userPrivilegeRepo = userPrivilegeRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _userPrivilegeRepo.GetUserPrivilegeById(new UserPrivilege() { UCBF_SYS_ID = id }, authParms));
        }

        [HttpGet("getByUserActivity/{id}")]
        public async Task<ActionResult> GetByUserActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _userPrivilegeRepo.GetUserPrivilegesByActivity(new UserPrivilege() { UBA_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostNewUser([FromBody] List<UserPrivilege> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _userPrivilegeRepo.BulkUserPrivileges(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _userPrivilegeRepo.DeleteUserPrivilege(new UserPrivilege() { UCBF_SYS_ID = id }, authParms));
        }

        [HttpPost("checkUserMenuPrivilege/search")]
        public async Task<ActionResult> CheckUserMenuPrivilege([FromBody] UserPrivilege entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _userPrivilegeRepo.CheckUserPrivilegeByUrl(entity, authParms));
        }

    }
}
