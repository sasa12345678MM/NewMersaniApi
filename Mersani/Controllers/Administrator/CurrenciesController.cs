using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : GeneralBaseController
    {
        protected readonly ICurrenciesRepository _currenciesRepository;
        public CurrenciesController(ICurrenciesRepository currenciesRepository)
        {
            _currenciesRepository = currenciesRepository;
        }

        #region currency settings
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCurrencies([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";//CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _currenciesRepository.GetCurrencyDataList(new Currencies() { CURR_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostNewCurrency([FromBody] List<Currencies> currencies)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _currenciesRepository.BulkInsertUpdateCurrency(currencies, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCurrency([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _currenciesRepository.DeleteCurrencyData(new Currencies() { CURR_SYS_ID = id }, authParms));
        }
        #endregion

        #region currency exchange rates

        [HttpGet("GetCurrencyRates/{id}")]
        public async Task<ActionResult> GetSupplier([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _currenciesRepository.GetCurrencyRates(new CurrencyRate() { CURRR_MAIN_CURR_SYS_ID = id }, authParms));
        }

        [HttpDelete("DeleteCurrencyRate/{id}")]
        public async Task<ActionResult> DeleteSupplierClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _currenciesRepository.DeleteCurrencyRates(new CurrencyRate() { CURRR_SYS_ID = id }, authParms));
        }


        [HttpPost("BulkCurrencyRates")]
        public async Task<ActionResult> PostSupplierClassRows([FromBody] CurrencyRate entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _currenciesRepository.BulkCurrencyRates(entity, authParms));
        }
        #endregion


        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _currenciesRepository.GetLastCode(authParms));
        }
    }
}
