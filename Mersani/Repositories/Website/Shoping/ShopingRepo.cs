using Mersani.Interfaces.Website.WebShoping;
using Mersani.models.Stock;
using Mersani.models.website;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Website.Shoping
{
    public class ShopingRepo : IWebShopingRepo
    {




        public async Task<DataSet> AddWishlistItem(List<WebItemWishlist> wishlists, string authParms)
        {
            foreach (WebItemWishlist wishlist in wishlists)
            {
                if (wishlist.WWL_SYS_ID > 0) wishlist.STATE = (int)OperationType.Update;
                else wishlist.STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_WEB_WISH_LIST_XML", wishlists.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteWishlistItem(WebItemWishlist wishlist, string authParms)
        {
            wishlist.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_WEB_WISH_LIST_XML", new List<dynamic>() { wishlist }, authParms);
        }

        public async Task<DataSet> GetWishlistItems(int custometId, int curr, string authParms)
        {

            //var query = $"SELECT wish.*, item.* FROM WEB_WISH_LIST wish,  (( SELECT * FROM  (SELECT item_sys_id, item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n   ,NVL(fn_get_item_sale_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1," + curr + "), 0) AS ITEM_SALE_PRICE, fn_get_ITEM_DISCOUNT_PCT (item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master))) item " +
            //    $" WHERE wish.WWL_ITEM_SYS_ID = item.ITEM_SYS_ID AND wish.WWL_CUST_SYS_ID = :pCUST_SYS_ID";

            var query = $"SELECT wish.*, item.* FROM WEB_WISH_LIST wish,  ((SELECT item_sys_id, item_name_ar, item_name_en, item_need_mdchl_desc_y_n, item_need_auth_y_n, SUM(QTY_BASIC) AS STOCK_QTY, ITEM_SALE_PRICE, ITEM_DISCOUNT_PCT FROM(SELECT inv_item_master.item_sys_id, QTY_BASIC, item_name_ar, item_name_en, item_need_mdchl_desc_y_n, item_need_auth_y_n, NVL(fn_get_item_sale_price(inv_item_master.ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {curr}), 0) AS ITEM_SALE_PRICE," +
                                       " fn_get_ITEM_DISCOUNT_PCT(inv_item_master.item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master   join  V_INV_ALL_QTY_SUM ON inv_item_master.ITEM_SYS_ID = V_INV_ALL_QTY_SUM.ITEM_SYS_ID)" +
                                        " GROUP BY     item_sys_id, item_name_ar, item_name_en, item_need_mdchl_desc_y_n, item_need_auth_y_n, ITEM_SALE_PRICE, ITEM_DISCOUNT_PCT)) item" +
                                " WHERE wish.WWL_ITEM_SYS_ID = item.ITEM_SYS_ID AND wish.WWL_CUST_SYS_ID =:pCUST_SYS_ID ";

            var parms = new List<OracleParameter>() { new OracleParameter("pCUST_SYS_ID", custometId) };
            var query2 = $"SELECT * FROM INV_ITEM_IMAGES ";
            DataSet items = await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text, _public: true);
            DataSet images = await OracleDQ.ExcuteGetQueryAsync(query2, null, authParms, CommandType.Text, _public: true);
           return GetItemsDataWithImages(items, images);
            // return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public DataSet GetItemsDataWithImages(DataSet items, DataSet images)
        {
            DataTable itemsTable = items.Tables["result"];
            DataTable imagesTable = images.Tables["result"];
            for (int i = 0; i < itemsTable.Rows.Count; i++)
            {
                DataRow row = itemsTable.Rows[i];
                decimal item_id = row.Field<decimal>("ITEM_SYS_ID");
                if (!itemsTable.Columns.Contains("IMAGES"))
                {
                    itemsTable.Columns.Add("IMAGES", typeof(List<InvItemImages>));
                }

                DataView dv = new DataView(imagesTable);
                dv.RowFilter = $"IMG_ITEM_SYS_ID = {(int)item_id}";
                List<InvItemImages> itemImages = new List<InvItemImages>();
                foreach (DataRowView drv in dv)
                {
                    decimal IMG_SYS_ID = (decimal)drv["IMG_SYS_ID"];
                    decimal IMG_ITEM_SYS_ID = (decimal)drv["IMG_ITEM_SYS_ID"];
                    string IMG_PATH = (string)drv["IMG_PATH"];
                    string IMG_DESC_AR = (string)drv["IMG_DESC_AR"];
                    string IMG_DESC_EN = (string)drv["IMG_DESC_EN"];
                    itemImages.Add(new InvItemImages()
                    {
                        IMG_SYS_ID = (int)IMG_SYS_ID,
                        IMG_ITEM_SYS_ID = (int)IMG_ITEM_SYS_ID,
                        IMG_PATH = IMG_PATH,
                        IMG_DESC_AR = IMG_DESC_AR,
                        IMG_DESC_EN = IMG_DESC_EN
                    });
                }
          
                //}
                row["IMAGES"] = itemImages;
            }
            return items;
        }

        public async Task<DataSet> AddPaymentDetails(WebPaymentStripeReturnModel model, string authParms)
        {
            model.STATE = (int)OperationType.Add;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_TKT_SALES_ORDER_PAY_XML", new List<dynamic>() { model }, authParms);
        }
    }
}
