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
    public class ModulesSettingController : GeneralBaseController
    {
        protected readonly IModulesSettingRepo _modulesSettingRepo;
        public ModulesSettingController(IModulesSettingRepo modulesSettingRepo)
        {
            _modulesSettingRepo = modulesSettingRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetModulesSetting([FromRoute] int reportId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _modulesSettingRepo.GetModulesSetting(new ModulesSetting() { GMS_SYS_ID = reportId }, authParms));
        }

        [HttpGet("ByType/{type}")]
        public async Task<ActionResult> GetModulesSettingByType([FromRoute] string type)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _modulesSettingRepo.GetSettingByType(type, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteModulesSetting([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _modulesSettingRepo.DeleteModulesSetting(new ModulesSetting() { GMS_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> bulkModulesSettings([FromBody] List<ModulesSetting> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _modulesSettingRepo.BulkModulesSetting(entities, authParms));
        }
    }

}

