using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Stock
{
    public class InventoryItemsRepository : IInventoryItemsRepo
    {
        public async Task<DataSet> GetInventoryItems(InventoryItems entity, string authParms)
        {
            var query = $"SELECT inv.*, mst.ITEM_NAME_AR, mst.ITEM_NAME_EN, mst.ITEM_BTCH_Y_N,  fn__item_btch_curr_stk(inv.III_INV_SYS_ID, inv.III_ITEM_SYS_ID, null,null) AS III_CURR_QTY, " +
                $" fn_get_ITEM_BASIC_UOM(inv.III_ITEM_SYS_ID) AS BASIC_UNIT_ID, bsc.UOM_NAME_AR AS BASIC_NAME_AR, bsc.UOM_NAME_EN AS BASIC_NAME_EN, " +
                $" NVL(fn_get_item_sale_price(mst.ITEM_SYS_ID, mst.ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS SALE_PRICE " +
                $" FROM INV_INV_ITEMS inv " +
                $" join INV_ITEM_MASTER mst on mst.ITEM_SYS_ID = inv.III_ITEM_SYS_ID " +
                $" LEFT JOIN INV_UOM bsc ON bsc.UOM_SYS_ID = fn_get_ITEM_BASIC_UOM(inv.III_ITEM_SYS_ID) " +
                $" WHERE III_INV_SYS_ID = :pSYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.III_INV_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInventoryItemsById(InventoryItems entity, string authParms)
        {
            var query = $"SELECT itms.*, unt.uom_name_ar, unt.uom_name_en FROM INV_INV_ITEMS itms join INV_UOM unt on unt.UOM_SYS_ID = itms.III_NOBTCH_UOM_SYS_ID " +
                $"WHERE III_SYS_ID = :pSYS_ID OR :pSYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pSYS_ID", entity.III_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkInventoryItems(List<InventoryItems> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

                if (entity.III_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_INV_ITEMS_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteInventoryItems(InventoryItems entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_INV_ITEMS_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetInventoryItemBatches(int stockId, int itemId, string authParms)
        {
            var query = $"SELECT btch.*, unit.UOM_NAME_AR, unit.UOM_NAME_EN, invm.III_INV_SYS_ID,invm.III_ITEM_SYS_ID, " +
                 $" NVL(fn_get_item_lpur_price(item.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) as ITEM_LAST_PUR_PRICE, " +
                 $" NVL(fn_get_item_sale_price(item.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) as ITEM_SALE_PRICE, " +
                 $" fn__item_btch_curr_stk(invm.III_INV_SYS_ID, invm.III_ITEM_SYS_ID, btch.IIB_BATCH_SYS_ID, null) CURRENT_QTY, " +
                 $" fn_get_ITEM_BASIC_UOM(invm.III_ITEM_SYS_ID) AS BASIC_UNIT_ID, bsc.UOM_NAME_AR AS BASIC_NAME_AR, bsc.UOM_NAME_EN AS BASIC_NAME_EN, " +
                 $" ('باتش ' || IMB_BATCH_CODE || ' ينتهي فى ' || to_char(IMB_EXPR_DATE, 'dd/MM/yyy')) AS BATCH_NAME_AR, " +
                 $" ('Batch ' || IMB_BATCH_CODE || ' expire on ' || to_char(IMB_EXPR_DATE, 'dd/MM/yyy')) AS BATCH_NAME_EN, " +
                 $" mbtch.IMB_BATCH_CODE AS IIB_BATCH_NO, mbtch.IMB_PROD_DATE AS IIB_BATCH_PROD_DATE, mbtch.IMB_EXPR_DATE AS IIB_BATCH_EXP_DATE, mbtch.IMB_BARCODE AS IIB_BARCODE " +
                 $" FROM INV_ITEM_BATCHES btch " +
                 $" JOIN inv_inv_items invm ON invm.III_SYS_ID = btch.IIB_III_SYS_ID " +
                 $" JOIN inv_item_master item ON item.ITEM_SYS_ID = invm.III_ITEM_SYS_ID " +
                 $" JOIN inv_uom unit ON btch.IIB_UOM_SYS_ID = unit.UOM_SYS_ID " +
                 $" JOIN INV_ITEM_MASTER_BATCHES mbtch ON btch.IIB_BATCH_SYS_ID = mbtch.IMB_SYS_ID " +
                 $" LEFT JOIN INV_UOM bsc ON bsc.UOM_SYS_ID = fn_get_ITEM_BASIC_UOM(invm.III_ITEM_SYS_ID) " +
                 $" WHERE mbtch.IMB_EXPR_DATE > SYSDATE and fn__item_btch_curr_stk (invm.III_INV_SYS_ID, invm.III_ITEM_SYS_ID, btch.IIB_BATCH_SYS_ID, NULL) > 0 " +
                 $" AND invm.III_ITEM_SYS_ID = {itemId} AND invm.III_INV_SYS_ID = {stockId}";
            var parms = new List<OracleParameter>() { new OracleParameter("pStockId", stockId), new OracleParameter("pItemId", itemId) };
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInventoryByPharmacyId(string authParms)
        {
            var query = $"SELECT * FROM inv_inventory_master WHERE IIM_INV_TYPE_I_S = 'S' AND IIM_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            //var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", id) };
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
