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
    public class ReportSettingsController : GeneralBaseController
    {

        protected IReportSettingsRepo _IReportSettingsRepo;
        public ReportSettingsController(IReportSettingsRepo IReportSettingsRepoc)
        {
            _IReportSettingsRepo = IReportSettingsRepoc;
        }

        [HttpGet("menu/{menuCode}")]
        public async Task<ActionResult> getMenuData([FromRoute] int menuCode)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _IReportSettingsRepo.getMenuData(new Menu() { MNU_CODE = menuCode }, authParms));
        }

        [HttpGet("MenuReport/{menuCode}")]
        public async Task<ActionResult> getMenuReportsData([FromRoute] int menuCode)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _IReportSettingsRepo.getMenuReportsData(new IMenuReports() { MNURPT_MNU_CODE = menuCode }, authParms));
        }

        [HttpGet("MenuReportParm/{menuCode}")]
        public async Task<ActionResult> geMenuReportParmData([FromRoute] int menuCode)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _IReportSettingsRepo.geMenuReportParmData(new IMenuReportParm() { RDTL_MNU_CODE = menuCode }, authParms));
        }
        

        [HttpPost("MenuReport")]
        public async Task<ActionResult> SaveMenuReportsData([FromBody] List<IMenuReports> entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _IReportSettingsRepo.SaveMenuReportsData(entity, authParms));
        }

        [HttpPost("MenuReportParm")]
        public async Task<ActionResult> SaveMenuReportParmData([FromBody] List<IMenuReportParm> entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _IReportSettingsRepo.SaveMenuReportParmData(entity, authParms));
        }

        //////////////////////////////////////////////////////////
        [HttpGet("ReportUsers/{menuReportCode}")]
        public async Task<ActionResult> geMenuReportUsersData([FromRoute] int menuReportCode)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _IReportSettingsRepo.getMenuReportUsers(new IMenuReportUsers() { GMRU_MNURPT_SYS_ID = menuReportCode }, authParms));

        }
        [HttpPost("ReportUsers")]
        public async Task<ActionResult> SaveMenuReportUsersData([FromBody] List<IMenuReportUsers> entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _IReportSettingsRepo.SaveIMenuReportUsersData(entity, authParms));
        }


        [HttpGet("MenuParamData/{menuCode}")]
        public async Task<ActionResult> GeMenuParamData([FromRoute] int menuCode)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _IReportSettingsRepo.GetMenuParamsByMenuCode(menuCode, authParms));
        }

        [HttpGet("MenuParamByPath/{menuPath}")]
        public async Task<ActionResult> GeMenuParamDataByUrl([FromRoute] string menuPath)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _IReportSettingsRepo.GetMenuParamsByPath(menuPath , authParms));
        }
    }
}
