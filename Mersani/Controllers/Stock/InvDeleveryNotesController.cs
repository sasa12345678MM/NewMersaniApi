using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.Interfaces.Stock;
using Mersani.models.Stock;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvDeleveryNotesController : GeneralBaseController
    {
        readonly InvDeleveryNotesRepo _DeleveryNotesRepo;
        public InvDeleveryNotesController (InvDeleveryNotesRepo deleveryNotesRepo)
        {
            this._DeleveryNotesRepo = deleveryNotesRepo;
        }
        [HttpGet("{id}/{PostType}")]
        public async Task<ActionResult> GetDeleveryNoteHdr([FromRoute] int id,string PostType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _DeleveryNotesRepo.GetinvDeleveryNoteHdr(new invDeleveryNoteHdr() { IDNH_SYS_ID = id }, PostType, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> AddDeleveryNoteMasterDetails([FromBody] DeleveryNotesData entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _DeleveryNotesRepo.PostinvDeleveryNoteHdrDtl(entities, authParms);

            return Ok(result);
        }
        [HttpPost("approval")]
        public async Task<ActionResult> approvalDeleveryNoteMasterDetails([FromBody] List<invDeleveryNoteHdr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _DeleveryNotesRepo.approvalinvDeleveryNote(entities, authParms);

            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeleveryNoteMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _DeleveryNotesRepo.DeleteinvDeleveryNoteHdrDtl(new invDeleveryNoteHdr() { IDNH_SYS_ID = id }, authParms));
        }
        [HttpGet("Dtl/{id}")]
        public async Task<ActionResult> GetinvDeleveryNoteDtls([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _DeleveryNotesRepo.GetinvDeleveryNoteDtls(new invDeleveryNoteDtl() { IDND_IDNH_SYS_ID = id }, authParms));
        }
        [HttpGet("GetLastCode/{inventory}/{type}")]
        public async Task<ActionResult> GetLastCode(int inventory, string type)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _DeleveryNotesRepo.GetLastCode(inventory, type,authParms));
        }
    }
}
