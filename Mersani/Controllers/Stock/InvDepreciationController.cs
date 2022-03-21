using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvDepreciationController : GeneralBaseController
    {
        protected readonly InvDepreciationRepo _InvDepreciationRepo;
        public InvDepreciationController(InvDepreciationRepo InvDepreciationRepo)
        {
            _InvDepreciationRepo = InvDepreciationRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetinvAdjstDepMstr([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _InvDepreciationRepo.GetAdjstDepMstr(new invAdjstDepMstr() { IADM_SYS_ID = id }, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> AddAdjstDepMasterDetails([FromBody] AdjstDepData entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _InvDepreciationRepo.PostAdjstDepMasterDetails(entities, authParms);

            return Ok(result);
        }
       
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdjstDepMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _InvDepreciationRepo.DeleteAdjstDepMasterDetails(new invAdjstDepMstr() { IADM_SYS_ID = id }, authParms));
        }
        [HttpGet("Dtl/{id}")]
        public async Task<ActionResult> GetinvAdjstDepDtls([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _InvDepreciationRepo.GetinvAdjstDepDtls(new invAdjstDepDtls() { IADD_IADM_SYS_ID = id }, authParms));
        }
       
        [HttpPost("approval")]
        public async Task<ActionResult> approvalinvAdjstDepMstr([FromBody] List<invAdjstDepMstr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _InvDepreciationRepo.approvalinvAdjstDepMstr(entities, authParms);

            return Ok(result);
        }
    }
}
