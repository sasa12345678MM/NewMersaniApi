using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupMenuController : GeneralBaseController
    {
        protected readonly IUserGroupMenuRepo _userGroupMenuRepo;
        public UserGroupMenuController(IUserGroupMenuRepo userGroupMenuRepo)
        {
            _userGroupMenuRepo = userGroupMenuRepo;
        }

        [HttpGet("{id}")]
        public ActionResult GetUserGroupMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_userGroupMenuRepo.GetUserGroupMenu(id, authParms));
        }

        [HttpGet("getByUserGroup/{id}")]
        public ActionResult GetMenuesByUserGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_userGroupMenuRepo.GetMenuesByUserGroup(id, authParms));
        }

        [HttpPost]
        public ActionResult PostNewUserGroup([FromBody] UserGroupMenu userGroupMenu)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_userGroupMenuRepo.PostNewUserGroupMenu(userGroupMenu, authParms));
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUserGroup([FromRoute] int id, [FromBody] UserGroupMenu userGroupMenu)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == userGroupMenu.USGRMN_SYS_ID)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

                if (userGroupMenu.USGRMN_SYS_ID > 0)
                {
                    result = _userGroupMenuRepo.PostNewUserGroupMenu(userGroupMenu, authParms);
                }
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUserGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            if (id > 0)
            {
                result = _userGroupMenuRepo.DeleteUserGroupMenu(id, authParms);
            }

            return Ok(result);
        }
    }
}
