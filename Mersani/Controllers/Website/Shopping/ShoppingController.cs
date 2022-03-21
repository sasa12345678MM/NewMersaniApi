using Mersani.Interfaces.Website.WebShoping;
using Mersani.models.website;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Website.Shopping
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : GeneralBaseController
    {
        IWebShopingRepo _ShopingRepo;
        public ShoppingController(IWebShopingRepo ShopingRepo)
        {
            this._ShopingRepo = ShopingRepo;
        }
        [HttpGet("{curr}/{customerId}")]
        public async Task<ActionResult> GetWishlistItems([FromRoute] int curr, [FromRoute] int customerId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ShopingRepo.GetWishlistItems(customerId, curr, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWishlistItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
           
            return Ok(await _ShopingRepo.DeleteWishlistItem(new WebItemWishlist() {WWL_SYS_ID=id }, authParms));
        }
        [HttpPost("bulk")]
        public async Task<ActionResult> AddWishlistItem([FromBody] List<WebItemWishlist> models) 
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ShopingRepo.AddWishlistItem(models, authParms));
        }
      [HttpPost("PayByStripe")]
       public async Task<ActionResult> PayByStripe(WebPaymentStripeModel model)
        {
            var optionsCust = new CustomerCreateOptions
            {
                Email =model.stripeEmail,
                Name = "Robert",
                Phone = "04-234567"
            };
            var serviceCust = new CustomerService();
            Customer customer = await serviceCust.CreateAsync(optionsCust);
            var optionsCharge = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(model.amount),
                Currency = "USD",
                Description = "Buying Flowers",
                Source = model.stripeToken,
                ReceiptEmail = model.stripeEmail
            };
            var service = new ChargeService();
            Charge charge = await service.CreateAsync(optionsCharge);
            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                WebPaymentStripeReturnModel ReturnModel = new WebPaymentStripeReturnModel()
                {
                    INS_DATE = DateTime.Now,
                    TSOP_AMOUNT_CAPTURED = charge.AmountCaptured,
                    TSOP_TKT_AMOUNT = charge.Amount,
                    TSOP_BALANCE_TRANSACTION = charge.BalanceTransactionId
                    , TSOP_CURRENCY = charge.Currency,
                    TSOP_STATUS = charge.PaymentMethod,
                    TSOP_TRANS_ID = charge.PaymentMethodDetails.Card.Brand, TSOP_FUNDING = charge.PaymentMethodDetails.Card.Funding,
                    TSOP_BRAND = charge.PaymentMethodDetails.Card.Brand,
                    TSOP_CAPTURED = "",
                    TSOP_PAID = charge.Paid.ToString(),
                    TSOP_PAYMENT_METHOD=charge.PaymentMethod,
                  //SOP_TKT_ID 
                    //INS_USER=charge.Customer\
                  //TSOP_TKT_ID
                  //INS_USER
                 // STATE
                 
                };
                return Ok(new { message = true, result = ReturnModel});            
            }
            return Ok(new { message = false, result = new WebPaymentStripeReturnModel() { } }  );
        }

        [HttpPost("payment")]
        public async Task<ActionResult> AddPaymentDetails([FromBody] WebPaymentStripeReturnModel model)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ShopingRepo.AddPaymentDetails(model, authParms));
        }

    }
}
