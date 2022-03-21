using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.PointOfSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceContractController : GeneralBaseController
    {
        protected readonly IInsuranceContractRepo _insuranceContractRepo;
        public InsuranceContractController(IInsuranceContractRepo insuranceContractRepo)
        {
            _insuranceContractRepo = insuranceContractRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetInsuranceContract([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceContractRepo.GetInsuranceContractDataList(new InsuranceContract() { PICNT_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInsuranceContract([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceContractRepo.DeleteInsuranceContract(new InsuranceContract() { PICNT_SYS_ID = id }, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostInsuranceContractClassRows([FromBody] InsuranceContract entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceContractRepo.InsertUpdateInsuranceContract(entity, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetContractLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _insuranceContractRepo.GetContractLastCode(authParms));
        }


        #region classes
        [HttpGet("class/{id}")]
        public async Task<ActionResult> GetInsuranceContractClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceContractRepo.GetInsuranceContractClassList(new InsuranceContractClass() { PICNTC_PICNT_SYS_ID = id }, authParms));
        }

        [HttpGet("class/GetById/{id}")]
        public async Task<ActionResult> GetInsuranceContractClassById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceContractRepo.GetInsuranceContractClassById(new InsuranceContractClass() { PICNTC_SYS_ID = id }, authParms));
        }

        [HttpDelete("class/{id}")]
        public async Task<ActionResult> DeleteInsuranceContractClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceContractRepo.DeleteInsuranceContractClasses(new InsuranceContractClass() { PICNTC_SYS_ID = id }, authParms));
        }

        [HttpPost("class")]
        public async Task<ActionResult> PostInsuranceContractClass([FromBody] InsuranceContractClass entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _insuranceContractRepo.PostInsuranceContractClasses(entity, authParms));
        }
        #endregion
    }
}
