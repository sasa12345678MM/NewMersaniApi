using Mersani.Utility;
using Mersani.models.FinancialSetup;
using Microsoft.AspNetCore.Mvc;
using Mersani.Interfaces.FinancialSetup;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinsAccountLevelController : GeneralBaseController
    {
        IFinsAccountLevelRepository _finsAccountLevelRepository;

        public FinsAccountLevelController(IFinsAccountLevelRepository finAccountLevelRepository)
        {
            _finsAccountLevelRepository = finAccountLevelRepository;
        }
        [HttpGet("{id}")]
        public ActionResult GetFinancialAccountLevel([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_finsAccountLevelRepository.GetFinAccountLevelDetails(id, authParms));

        }

        [HttpPost]
        public ActionResult PostNewfinAccount([FromBody] FinsAccountLevel finAccountLevel)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_finsAccountLevelRepository.PostNewFinAccountLevel(finAccountLevel, authParms));
        }

        [HttpPut("{id}")]
        public ActionResult UpdatefinAccount([FromRoute] int id, [FromBody] FinsAccountLevel finAccountLevel)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == finAccountLevel.ACC_LEVEL_CODE)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

                if (finAccountLevel.ACC_LEVEL_CODE > 0)
                {
                    result = _finsAccountLevelRepository.UpdateFinAccountLevel(id, finAccountLevel, authParms);
                }
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletefinAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            if (id > 0)
            {
                result = _finsAccountLevelRepository.DeleteFinAccountLevel(id, authParms);
            }

            return Ok(result);
        }
    }
}

