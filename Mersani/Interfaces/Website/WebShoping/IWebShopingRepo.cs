using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.models.website;
using System.Data;

namespace Mersani.Interfaces.Website.WebShoping
{
   
    public interface IWebShopingRepo
    {
        Task<DataSet> GetWishlistItems(int custometId,int curr, string authParms);
        Task<DataSet> DeleteWishlistItem(WebItemWishlist model, string authParms);
        Task<DataSet> AddWishlistItem(List<WebItemWishlist> models, string authParms);
        Task<DataSet> AddPaymentDetails(WebPaymentStripeReturnModel model, string authParms);

    }
}
