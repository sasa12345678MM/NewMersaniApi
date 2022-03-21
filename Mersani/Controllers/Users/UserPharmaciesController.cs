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
    public class UserPharmaciesController : GeneralBaseController
    {
        protected readonly IUserPharmaciesRepo _userPharmaciesRepo;
        public UserPharmaciesController(IUserPharmaciesRepo userPharmaciesRepo)
        {
            _userPharmaciesRepo = userPharmaciesRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _userPharmaciesRepo.GetUserPharmciesByUserId(id, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> BulkUserPharmacies([FromBody] List<UserPharmacies> pharmacies)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _userPharmaciesRepo.BulkUserPharmcies(pharmacies, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserPharmacies([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _userPharmaciesRepo.DeleteUserPharmcy(id, authParms));
        }
    }
}
