using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.models.Stock;
using Mersani.Interfaces.Stock;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Mersani.Oracle;

namespace Mersani.Repositories.Stock
{
    public class ItemBatchesRepository : IItemBatchesRepo
    {
        public async Task<DataSet> GetItemBatches(ItemBatches entity, string authParms)
        {
            var query = $"SELECT btch.*, item.ITEM_NAME_AR, item.ITEM_NAME_EN, unit.UOM_NAME_AR, unit.UOM_NAME_EN, " +
                $" fn__item_btch_curr_stk(invm.III_INV_SYS_ID, invm.III_ITEM_SYS_ID, btch.IIB_BATCH_SYS_ID,null) CURRENT_QTY, " +
                $" fn_get_ITEM_BASIC_UOM(item.ITEM_SYS_ID) AS BASIC_UNIT_ID, bsc.UOM_NAME_AR AS BASIC_NAME_AR, bsc.UOM_NAME_EN AS BASIC_NAME_EN," +
                $"('باتش رقم ' || mbtch.IMB_BATCH_CODE || ' بتاريخ ' || TO_CHAR (mbtch.IMB_PROD_DATE, 'FMMonth DD, YYYY')) AS BATCH_NAME_AR, " +
                $"('Batch Code ' || mbtch.IMB_BATCH_CODE || ' With Date ' || TO_CHAR (mbtch.IMB_PROD_DATE, 'FMMonth DD, YYYY')) AS BATCH_NAME_EN, " +
                $"mbtch.IMB_BATCH_CODE AS IIB_BATCH_NO, mbtch.IMB_PROD_DATE AS IIB_BATCH_PROD_DATE, mbtch.IMB_EXPR_DATE AS IIB_BATCH_EXP_DATE, mbtch.IMB_BARCODE AS IIB_BARCODE " +
                $" FROM INV_ITEM_BATCHES btch " +
                $" JOIN inv_inv_items invm ON invm.III_SYS_ID = btch.IIB_III_SYS_ID " +
                $" JOIN inv_item_master item ON item.ITEM_SYS_ID = invm.III_ITEM_SYS_ID " +
                $" JOIN inv_uom unit ON btch.IIB_UOM_SYS_ID = unit.UOM_SYS_ID " +
                $" JOIN INV_ITEM_MASTER_BATCHES mbtch ON btch.IIB_BATCH_SYS_ID = mbtch.IMB_SYS_ID " +
                $" LEFT JOIN INV_UOM bsc ON bsc.UOM_SYS_ID = fn_get_ITEM_BASIC_UOM (item.ITEM_SYS_ID) " +
                $" WHERE IIB_III_SYS_ID = :pSYS_ID OR :pSYS_ID = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.IIB_III_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkItemBatches(List<ItemBatches> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

                if (entity.IIB_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_BATCHES_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteItemBatches(ItemBatches entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_BATCHES_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}
