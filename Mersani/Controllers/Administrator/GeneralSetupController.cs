using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSetupController : GeneralBaseController
    {
        protected IGeneralSetupRepo _GeneralSetupRepo;
         
        public object _GeneralSetupMaster { get; private set; }

        public GeneralSetupController(IGeneralSetupRepo generalSetupRepo)
        {
            _GeneralSetupRepo = generalSetupRepo;
        }
        // master
        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetMasterData(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralSetupRepo.GetMasterData(new GeneralSetupMaster() { GSH_SYS_ID = id }, authParms));
        }

        [HttpPost("master")]
        public async Task<ActionResult> PostMasterData([FromBody] GeneralSetupMaster master)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralSetupRepo.PostMasterData(master, authParms));
        }

        [HttpDelete("master/{id}")]
        public async Task<ActionResult> DeleteMasterData([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralSetupRepo.DeleteMasterData(new GeneralSetupMaster() { GSH_SYS_ID = id }, authParms));
        }

        // details
        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetDetailsData(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralSetupRepo.GetDetailsData(new GeneralSetupDetail() { GSD_GSH_SYS_ID = id }, authParms));
        }

        [HttpPost("details/bulk")]
        public async Task<ActionResult> BulkDetailsData([FromBody] List<GeneralSetupDetail> details)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralSetupRepo.BulkDetailsData(details, authParms));
        }

        [HttpDelete("details/{id}")]
        public async Task<ActionResult> PostGeneralSetupDetalis([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralSetupRepo.DeleteDetailsData(new GeneralSetupDetail() { GSD_SYS_ID = id }, authParms));
        }
    }


}