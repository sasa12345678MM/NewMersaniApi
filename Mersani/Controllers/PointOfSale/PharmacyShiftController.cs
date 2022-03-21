using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.PointOfSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyShiftController : GeneralBaseController
    {
        protected readonly IPharmacyShiftRepo _pharmacyShiftRepo;

        public PharmacyShiftController(IPharmacyShiftRepo ipharmacyShiftRepo)
        {
            _pharmacyShiftRepo = ipharmacyShiftRepo;
        }


        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetPharmacyShiftMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _pharmacyShiftRepo.GetPharmacyShiftMaster(new PharmacyShiftMaster() { PSH_PHARM_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetPharmacyShiftDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _pharmacyShiftRepo.GetPharmacyShiftDetails(new PharmacyShiftMaster() { PSH_SYS_ID = id }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostPharmacyShiftMasterDetails([FromBody] PharmacyShiftData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _pharmacyShiftRepo.PostPharmacyShiftMasterDetails(entity, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePharmacyShiftMasterDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _pharmacyShiftRepo.DeletePharmacyShiftMasterDetails(new PharmacyShiftDetail() { PSD_PSH_SYS_ID = id }, 1, authParms));
        }

        [HttpPost("CheckShift/search")]
        public async Task<ActionResult> CheckUserShiftForPharmacy([FromBody] PharmacyShiftActivation entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _pharmacyShiftRepo.CheckUserShiftForPharmacy(entity, authParms));
        }


        [HttpGet("CashLeft/{id}")]
        public async Task<ActionResult> GetPharmacyLastShiftCashLeft([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _pharmacyShiftRepo.GetPharmacyLastShiftCashLeft(id, authParms));
        }


        [HttpPost("PharmacyShift")]
        public async Task<ActionResult> PostPharmacyShift([FromBody] PharmacyShiftActivation entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _pharmacyShiftRepo.PostPharmacyShift(entity, authParms));
        }

        [HttpGet("GetCashierShiftTotals/{id}")]
        public async Task<ActionResult> GetCashierShiftTotals([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _pharmacyShiftRepo.GetCashierShiftTotals(id, authParms));
        }
    }



}
