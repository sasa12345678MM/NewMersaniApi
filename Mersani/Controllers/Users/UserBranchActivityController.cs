using Mersani.Interfaces.Users;
using Mersani.models.Users;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBranchActivityController : GeneralBaseController
    {
        protected readonly IUserBranchActivityRepo _userCompanyBranchRepo;
        public UserBranchActivityController(IUserBranchActivityRepo userCompanyBranchRepo)
        {
            _userCompanyBranchRepo = userCompanyBranchRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _userCompanyBranchRepo.GetUserBranchActivity(id, authParms);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostNewUser([FromBody] UserBranchActivity entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _userCompanyBranchRepo.PostNewUserBranchActivity(entity, authParms);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int id, [FromBody] UserBranchActivity entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == entity.UBA_SYS_ID)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

                if (entity.UBA_SYS_ID > 0)
                {
                    result = await _userCompanyBranchRepo.PostNewUserBranchActivity(entity, authParms);
                }
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            if (id > 0)
            {
                result = await _userCompanyBranchRepo.DeleteUserBranchActivity(id, authParms);
            }

            return Ok(result);
        }
        //[HttpPost]
        //public async Task<ActionResult> PostNewUser([FromBody] List<UserBranchActivity> entity)
        //{
        //    if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

        //    string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

        //    return Ok(await _userCompanyBranchRepo.BulkUserBranchActivity(entity, authParms));
        //}


        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteUser([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

        //    string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

        //    return Ok(await _userCompanyBranchRepo.DeleteUserBranchActivity(id, authParms));

        //}
    }
}
