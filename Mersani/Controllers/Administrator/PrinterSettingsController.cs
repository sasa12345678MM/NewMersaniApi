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
    public class PrinterSettingsController : GeneralBaseController
    {
        protected readonly IPrinterSettingsRepo _printerSettingsRepo;
        public PrinterSettingsController(IPrinterSettingsRepo printerSettingsRepo)
        {
            _printerSettingsRepo = printerSettingsRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPrinterSettings([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _printerSettingsRepo.GetPrinterSettings(new PrinterSetup() { GPS_SYS_ID = id }, authParms));
        }

        [HttpGet("printers")]
        public ActionResult GetPrinters()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_printerSettingsRepo.GetSystemPrinterDevices(authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostNewVoucherType([FromBody] List<PrinterSetup> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _printerSettingsRepo.BulkPrinterSettings(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVoucherType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _printerSettingsRepo.DeletePrinterSettings(new PrinterSetup() { GPS_SYS_ID = id }, authParms));
        }


    }
}
