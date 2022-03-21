using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.PointOfSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceCompanyController : GeneralBaseController
    {
        protected readonly IInsuranceCompanyRepo _insuranceCompanyRepo;
        public InsuranceCompanyController(IInsuranceCompanyRepo insuranceCompanyRepo)
        {
            _insuranceCompanyRepo = insuranceCompanyRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetInsuranceCompany([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceCompanyRepo.GetInsuranceCompanyDataList(new InsuranceCompany() { PIC_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInsuranceCompanyClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceCompanyRepo.DeleteInsuranceCompany(new InsuranceCompany() { PIC_SYS_ID = id }, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostInsuranceCompanyClassRows([FromBody] InsuranceCompany entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceCompanyRepo.InsertUpdateInsuranceCompany(entity, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _insuranceCompanyRepo.GetLastCode(authParms));
        }
    }
}
