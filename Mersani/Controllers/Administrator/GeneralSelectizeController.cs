using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSelectizeController : GeneralBaseController
    {
        IGeneralSelectize _generalRepository;
        public GeneralSelectizeController(IGeneralSelectize generalRepository)
        {
            _generalRepository = generalRepository;
        }

        // first: get activities
        [HttpGet("GetUserActivities/{id}")]
        public async Task<ActionResult> GetUserActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext); //"Custom Select";
            var result = await _generalRepository.GetUserActivities(id, authParms);

            return Ok(result);
        }

        // second: get branches by activity
        [HttpGet("GetBranchesByActivity/{id}")]
        public async Task<ActionResult> GetBranchesByActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext); //"Custom Select";
            var result = await _generalRepository.GetBranchesByActivity(id, authParms);

            return Ok(result);
        }

        // third: get companies by branch
        [HttpGet("GetCompaniesByBranch/{id}")]
        public async Task<ActionResult> GetCompaniesByBranch([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext); //"Custom Select";
            var result = await _generalRepository.GetCompaniesByBranch(id, authParms);

            return Ok(result);
        }

        [HttpGet("FinsAccountClass/{id}")]
        public ActionResult GetFinsAccountClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = _generalRepository.GetFinsAccountClass(id, authParms);

            return Ok(result);
        }
        [HttpGet("FinsAccountLevel/{id}")]
        public ActionResult GetFinsAccountLevel([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = _generalRepository.GetFinsAccountLevel(id, authParms);

            return Ok(result);
        }
        [HttpGet("FinsCostCenter/{id}")]
        public ActionResult GetFinsCostCenter([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = _generalRepository.GetFinsCostCenter(id, authParms);

            return Ok(result);
        }
        [HttpPost("MirsaniSelect/search")]
        public async Task<ActionResult> getMirsaniSelectData([FromBody] ISelectSearch IselectSearch)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _generalRepository.getMirsaniSelectData(IselectSearch, authParms);
            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<ActionResult> GetDynamicDataByAppCode([FromBody] ISelectSearch IselectSearch)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _generalRepository.GetDynamicDataByAppCode(IselectSearch, authParms));
        }


    }
}
