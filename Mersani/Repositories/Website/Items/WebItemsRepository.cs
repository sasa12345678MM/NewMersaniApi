using Mersani.Interfaces.Website.items;
using Mersani.models.Stock;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Website.Items
{
    public class WebItemsRepository : Iwebitems
    {
        public async Task<DataSet> GetItemsWithPriceAndDiscount(int itemId, int Curr, string authParms)
        {

            var query = $"SELECT  item_sys_id , item_name_ar, item_name_en,item_iig_sys_id, item_need_mdchl_desc_y_n,item_need_auth_y_n   ,  SUM (QTY_BASIC) AS STOCK_QTY ,ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT FROM  (SELECT inv_item_master.item_sys_id,QTY_BASIC , item_name_ar, item_iig_sys_id, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n    ,NVL(fn_get_item_sale_price(inv_item_master.ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {Curr}), 0) AS ITEM_SALE_PRICE," +
                      "  fn_get_ITEM_DISCOUNT_PCT(inv_item_master.item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master   join  V_INV_ALL_QTY_SUM ON inv_item_master.ITEM_SYS_ID = V_INV_ALL_QTY_SUM.ITEM_SYS_ID  WHERE (inv_item_master.ITEM_SYS_ID = :pItemId or :pItemId = 0))    " +
                       "  GROUP BY item_sys_id , item_name_ar,item_iig_sys_id, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n ,ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT";            
            var query2 = $"SELECT * FROM INV_ITEM_IMAGES WHERE (IMG_ITEM_SYS_ID = :pItemId or :pItemId = 0)";
            var parms = new List<OracleParameter>() { new OracleParameter("pItemId", itemId) };
            DataSet items = await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text, _public: true);
            DataSet images = await OracleDQ.ExcuteGetQueryAsync(query2, parms, authParms, CommandType.Text, _public: true);

            return GetItemsDataWithImages(items, images);
        }


        public async Task<DataSet> GetItemOffers(int Curr, string authParms)
        {

            var query = $"SELECT  item_sys_id , item_name_ar, item_name_en,item_iig_sys_id, item_need_mdchl_desc_y_n,item_need_auth_y_n   ,  SUM (QTY_BASIC) AS STOCK_QTY ,ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT FROM  (SELECT inv_item_master.item_sys_id,QTY_BASIC , item_name_ar, item_iig_sys_id, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n    ,NVL(fn_get_item_sale_price(inv_item_master.ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {Curr}), 0) AS ITEM_SALE_PRICE," +
                      "  fn_get_ITEM_DISCOUNT_PCT(inv_item_master.item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master   join  V_INV_ALL_QTY_SUM ON inv_item_master.ITEM_SYS_ID = V_INV_ALL_QTY_SUM.ITEM_SYS_ID    )    " +
                       "  GROUP BY     item_sys_id , item_name_ar,item_iig_sys_id, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n    ,ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT  having ITEM_DISCOUNT_PCT > 0 and ITEM_SALE_PRICE>0";

            var query2 = $"SELECT * FROM INV_ITEM_IMAGES ";

            DataSet items = await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);
            DataSet images = await OracleDQ.ExcuteGetQueryAsync(query2, null, authParms, CommandType.Text, _public: true);

            return GetItemsDataWithImages(items, images);
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
                //for (int k = 0; k < dv.Count; k++)
                //{
                //    DataRow imgRow = dv[i].Row;
                //    decimal IMG_SYS_ID = imgRow.Field<decimal>("IMG_SYS_ID");
                //    decimal IMG_ITEM_SYS_ID = imgRow.Field<decimal>("IMG_ITEM_SYS_ID");
                //    string IMG_PATH = imgRow.Field<string>("IMG_PATH");
                //    string IMG_DESC_AR = imgRow.Field<string>("IMG_DESC_AR");
                //    string IMG_DESC_EN = imgRow.Field<string>("IMG_DESC_EN");
                //    itemImages.Add(new InvItemImages()
                //    {
                //        IMG_SYS_ID = (int)IMG_SYS_ID,
                //        IMG_ITEM_SYS_ID = (int)IMG_ITEM_SYS_ID,
                //        IMG_PATH = IMG_PATH,
                //        IMG_DESC_AR = IMG_DESC_AR,
                //        IMG_DESC_EN = IMG_DESC_EN
                //    });
                //}
                row["IMAGES"] = itemImages;
            }
            return items;
        }
        public async Task<DataSet> GetItemsImages(int itemId, string authParms)
        {
            var query = $"SELECT * FROM INV_ITEM_IMAGES WHERE (IMG_ITEM_SYS_ID = :pIMG_ITEM_SYS_ID OR :pIMG_ITEM_SYS_ID = 0)";
            var parms = new List<OracleParameter>() { new OracleParameter("pIMG_ITEM_SYS_ID", itemId) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text, _public: true);
        }
        public async Task<DataSet> GetItemsByGroup(int GroupId, int curr, string authParms)
        {
            //var query = $"SELECT item_sys_id, item_name_ar, item_name_en, item_need_mdchl_desc_y_n,item_need_auth_y_n, NVL(fn_get_item_sale_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, 7), 0) AS ITEM_SALE_PRICE, fn_get_ITEM_DISCOUNT_PCT (item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master WHERE ITEM_IIG_SYS_ID IN " +
            //    $" (SELECT IIG_SYS_ID FROM inv_item_group WHERE IIG_PARENT_SYS_ID = :pIIG_SYS_ID " +
            //    $" OR IIG_PARENT_SYS_ID IN(SELECT IIG_SYS_ID FROM inv_item_group WHERE IIG_PARENT_SYS_ID = :pIIG_SYS_ID))";

            var query = $"SELECT item_sys_id, item_name_ar, item_name_en, item_need_mdchl_desc_y_n, item_need_auth_y_n, SUM (QTY_BASIC) AS STOCK_QTY, ITEM_SALE_PRICE, ITEM_DISCOUNT_PCT, ITEM_IIG_SYS_ID" +
                        $" FROM(SELECT inv_item_master.item_sys_id, QTY_BASIC, item_name_ar, item_name_en, item_need_mdchl_desc_y_n, item_need_auth_y_n, NVL(fn_get_item_sale_price(inv_item_master.ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {curr}), 0) AS ITEM_SALE_PRICE, " +
                        $" fn_get_ITEM_DISCOUNT_PCT(inv_item_master.item_sys_id, 1) AS ITEM_DISCOUNT_PCT, ITEM_IIG_SYS_ID FROM inv_item_master  LEFT JOIN V_INV_ALL_QTY_SUM ON inv_item_master.ITEM_SYS_ID = V_INV_ALL_QTY_SUM.ITEM_SYS_ID)" +
                        $" WHERE ITEM_IIG_SYS_ID IN(SELECT IIG_SYS_ID  FROM inv_item_group   WHERE    IIG_PARENT_SYS_ID ={GroupId}   OR IIG_PARENT_SYS_ID IN(SELECT IIG_SYS_ID   FROM inv_item_group    WHERE IIG_PARENT_SYS_ID = {GroupId}))" +
                        $"GROUP BY item_sys_id,  item_name_ar,item_name_en,item_need_mdchl_desc_y_n, item_need_auth_y_n,  ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT,ITEM_IIG_SYS_ID";
            //   var parms = new List<OracleParameter>() { new OracleParameter("pIIG_SYS_ID", GroupId) };

            DataSet items = await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);
            var query2 = $"SELECT * FROM INV_ITEM_IMAGES ";

            DataSet images = await OracleDQ.ExcuteGetQueryAsync(query2, null, authParms, CommandType.Text, _public: true);

            return GetItemsDataWithImages(items, images);
        }
        public async Task<DataSet> GetRelatedItems(int itemId, int curr, string authParms)
        {
            //var query = $"SELECT * FROM  (SELECT IIR_ITEM_MSTR_SYS_ID, item_sys_id, item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n    ,NVL(fn_get_item_sale_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, 7), 0) AS ITEM_SALE_PRICE," +
            //             "fn_get_ITEM_DISCOUNT_PCT(item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master right join INV_ITEM_RELATED on IIR_ITEM_RLTD_SYS_ID = item_sys_id )   where IIR_ITEM_MSTR_SYS_ID =: p_IIR_ITEM_MSTR_SYS_ID";

            var query = $"SELECT * FROM  (     SELECT  item_sys_id , item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n   ,  SUM (QTY_BASIC) AS STOCK_QTY ,ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT FROM  (SELECT inv_item_master.item_sys_id,QTY_BASIC , item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n    ,NVL(fn_get_item_sale_price(inv_item_master.ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1,{curr}), 0) AS ITEM_SALE_PRICE,"
                     + " fn_get_ITEM_DISCOUNT_PCT(inv_item_master.item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master   join V_INV_ALL_QTY_SUM ON inv_item_master.ITEM_SYS_ID = V_INV_ALL_QTY_SUM.ITEM_SYS_ID   ) "
                      + " GROUP BY     item_sys_id , item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n    ,ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT    )    right join INV_ITEM_RELATED on IIR_ITEM_RLTD_SYS_ID = item_sys_id where IIR_ITEM_MSTR_SYS_ID =:p_IIR_ITEM_MSTR_SYS_ID";

            var parms = new List<OracleParameter>() { new OracleParameter("p_IIR_ITEM_MSTR_SYS_ID", itemId) };
            DataSet items = await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text, _public: true);

            var query2 = $"SELECT * FROM INV_ITEM_IMAGES ";

            DataSet images = await OracleDQ.ExcuteGetQueryAsync(query2, null, authParms, CommandType.Text, _public: true);
            return GetItemsDataWithImages(items, images);

        }
        public async Task<DataSet> GetItemsByMenufacturer(int menId, int curr, string authParms)
        {
            var query = $"SELECT  item_sys_id , item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n   ,  SUM(QTY_BASIC) AS STOCK_QTY, ITEM_SALE_PRICE, ITEM_DISCOUNT_PCT, ITEM_MANUFACTURER_SYS_ID FROM(SELECT inv_item_master.item_sys_id, QTY_BASIC, item_name_ar, item_name_en, item_need_mdchl_desc_y_n, item_need_auth_y_n, NVL(fn_get_item_sale_price(inv_item_master.ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {curr}), 0) AS ITEM_SALE_PRICE, ITEM_MANUFACTURER_SYS_ID," +
          $"fn_get_ITEM_DISCOUNT_PCT(inv_item_master.item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master   join  V_INV_ALL_QTY_SUM ON inv_item_master.ITEM_SYS_ID = V_INV_ALL_QTY_SUM.ITEM_SYS_ID)" +
           $"GROUP BY     item_sys_id , item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n    ,ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT ,ITEM_MANUFACTURER_SYS_ID having ITEM_MANUFACTURER_SYS_ID = {menId}";
            var query2 = $"SELECT * FROM INV_ITEM_IMAGES ";
            DataSet items = await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);
            DataSet images = await OracleDQ.ExcuteGetQueryAsync(query2, null, authParms, CommandType.Text, _public: true);

            return GetItemsDataWithImages(items, images);
        }
        public async Task<DataSet> GetItemsWithName(string name, int Curr, string authParms)
        {
            name = name.ToLower();
            var query = $"SELECT  item_sys_id , item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n   ,  SUM(QTY_BASIC) AS STOCK_QTY, ITEM_SALE_PRICE, ITEM_DISCOUNT_PCT FROM(SELECT inv_item_master.item_sys_id, QTY_BASIC, item_name_ar, item_name_en, item_need_mdchl_desc_y_n, item_need_auth_y_n, NVL(fn_get_item_sale_price(inv_item_master.ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {Curr}), 0) AS ITEM_SALE_PRICE," +
                        $" fn_get_ITEM_DISCOUNT_PCT(inv_item_master.item_sys_id, 1) AS ITEM_DISCOUNT_PCT FROM inv_item_master   join  V_INV_ALL_QTY_SUM ON inv_item_master.ITEM_SYS_ID = V_INV_ALL_QTY_SUM.ITEM_SYS_ID  where lower(item_name_en) like '%{name}%' or item_name_ar like '%{name}%')" +
                      $"GROUP BY     item_sys_id , item_name_ar, item_name_en,item_need_mdchl_desc_y_n,item_need_auth_y_n    ,ITEM_SALE_PRICE,ITEM_DISCOUNT_PCT";


            var query2 = $"SELECT * FROM INV_ITEM_IMAGES ";

            DataSet items = await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);
            DataSet images = await OracleDQ.ExcuteGetQueryAsync(query2, null, authParms, CommandType.Text, _public: true);

            return GetItemsDataWithImages(items, images);
        }
    }

}
