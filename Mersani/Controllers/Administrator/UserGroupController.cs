using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : GeneralBaseController
    {
        protected readonly IUserGroupRepo _userGroupRepo;
        public UserGroupController(IUserGroupRepo userGroupRepo)
        {
            _userGroupRepo = userGroupRepo;
        }

        [HttpGet("{id}")]
        public ActionResult GetUserGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_userGroupRepo.GetUserGroup(id, authParms));
        }

        [HttpPost]
        public ActionResult PostNewUserGroup([FromBody] UserGroup userGroup)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_userGroupRepo.PostNewUserGroup(userGroup, authParms));
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUserGroup([FromRoute] int id, [FromBody] UserGroup userGroup)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == userGroup.USRGRP_CODE)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

                if (userGroup.USRGRP_CODE > 0)
                {
                    result = _userGroupRepo.PostNewUserGroup(userGroup, authParms);
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
                result = _userGroupRepo.DeleteUserGroup(id, authParms);
            }

            return Ok(result);
        }
    }
}
