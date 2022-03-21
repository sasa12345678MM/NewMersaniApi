using Mersani.Interfaces.Finance;
using Mersani.models.Finance;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodYearController : GeneralBaseController
    {
        protected readonly IPeriodYearRepo _periodYearRepo;
        public PeriodYearController(IPeriodYearRepo periodYearRepo)
        {
            _periodYearRepo = periodYearRepo;
        }

        [HttpGet("year/{id}")]
        public async Task<ActionResult> GetYears([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _periodYearRepo.GetFinancialYear(new FinsYear() { PERIOD_YEAR = id }, authParms));
        }

        [HttpGet("period/{id}")]
        public async Task<ActionResult> GetPeriods([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _periodYearRepo.GetFinancialPeriods(new FinsYear() { PERIOD_YEAR = id }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostFinancialYear([FromBody] FinancialYearPeriods entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _periodYearRepo.PostFinancialYearPeriods(entity, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFinancialYear([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _periodYearRepo.DeleteFinancialYear(new FinsYear() { YEAR_SYS_ID = id }, 1, authParms));
        }
    }
}