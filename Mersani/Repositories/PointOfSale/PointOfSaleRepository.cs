using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Mersani.Repositories.PointOfSale
{
    public class PointOfSaleRepository : IPointOfSaleRepo
    {
        public async Task<DataSet> GetPointOfSaleMaster(PointOfSaleMASTER entity, string authParms)
        {
            var query = $"SELECT * FROM POS_CHASHER_HDR WHERE (PCH_SYS_ID = :pSYS_ID OR :pSYS_ID = 0) AND PCH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.PCH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetPointOfSaleDetails(PointOfSaleDetails entity, string authParms)
        {
            var query = $"SELECT DTL.*, ITEM.ITEM_NAME_AR, ITEM.ITEM_NAME_EN, UNT.UOM_NAME_AR, UNT.UOM_NAME_EN " +
                $" FROM POS_CHASHER_DTL DTL, INV_ITEM_MASTER ITEM, INV_UOM UNT " +
                $" WHERE DTL.PCD_ITEM_SYS_ID = ITEM.ITEM_SYS_ID AND DTL.PCD_UOM_SYS_ID = UNT.UOM_SYS_ID AND PCD_PCH_SYS_ID = :pSYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", entity.PCD_PCH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetItemDataByBarCode(string barcode, int stockId, string authParms)
        {
            var query = $"SELECT IIB_SYS_ID, IIB_BATCH_SYS_ID, IMB_BARCODE, III_INV_SYS_ID, ITEM_SYS_ID, ITEM_UOM_SYS_ID, IIB_OP_UNIT_AMOUNT AS ITEM_SALE_PRICE, " +
                $" NVL(fn_get_item_lpur_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS ITEM_LAST_PUR_PRICE, " +
                $" NVL (fn__item_btch_curr_stk(III_INV_SYS_ID, ITEM_SYS_ID, IIB_BATCH_SYS_ID, ITEM_UOM_SYS_ID), 0) AS CURR_STK, NVL(FN_GET_ITEM_VAT_PCT(ITEM_SYS_ID,{ OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) ITEM_VAT_PCT " +
                $" FROM inv_item_batches, INV_ITEM_MASTER_BATCHES, inv_inv_items, inv_item_master " +
                $" WHERE IIB_BATCH_SYS_ID = IMB_SYS_ID AND IIB_III_SYS_ID = III_SYS_ID AND III_ITEM_SYS_ID = ITEM_SYS_ID AND IMB_EXPR_DATE > SYSDATE " +
                $" AND NVL (fn__item_btch_curr_stk (III_INV_SYS_ID, ITEM_SYS_ID, IIB_BATCH_SYS_ID, ITEM_UOM_SYS_ID), 0) > 0 " +
                $" AND IMB_BARCODE = :pBarcode AND III_INV_SYS_ID = :pStockId";
            var parms = new List<OracleParameter>() { new OracleParameter("pBarcode", barcode), new OracleParameter("pStockId", stockId) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetPointOfSaleLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (PCH_VOUCHER_NO, '^[0-9]+') THEN PCH_VOUCHER_NO ELSE '0' END)), 0) + 1 AS Code FROM POS_CHASHER_HDR WHERE PCH_V_CODE  = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostPointOfSaleMASTERDetails(PointOfSaleData entities, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entities.MASTER.PCH_V_CODE = authData.User_Act_PH;
            entities.MASTER.CURR_USER = authData.UserCode.Value;
            if (entities.MASTER.PCH_SYS_ID > 0) entities.MASTER.STATE = (int)OperationType.Update;
            else entities.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entities.DETAILS.Count; i++)
            {
                entities.DETAILS[i].PCD_PCH_SYS_ID = entities.MASTER.PCH_SYS_ID;
                entities.DETAILS[i].CURR_USER = authData.UserCode;
                if (entities.DETAILS[i].PCD_SYS_ID > 0) entities.DETAILS[i].STATE = (int)OperationType.Update;
                else entities.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.MASTER });
            parameters.Add("xml_document_d", entities.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsyncWithOutPut("PRC_INV_POINT_OF_SALES_XML", parameters, authParms);
        }


        public async Task<DataSet> BulkPointOfSale(List<PointOfSaleMASTER> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Update;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.PCH_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_POINT_OF_SALES_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeletePointOfSaleMASTERDetails(PointOfSaleDetails entity, int type, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_POINT_OF_SALES_XML", parameters, authParms);
        }

        public async Task<DataSet> GetItemsByInventory(int stockId, string authParms)
        {
            var query = $"SELECT III_SYS_ID, ITEM_SYS_ID, III_INV_SYS_ID, ITEM_CODE, ITEM_NAME_AR, ITEM_NAME_EN, ITEM_IIG_SYS_ID, ITEM_FRZ_Y_N, ITEM_NOTES, ITEM_UOM_SYS_ID, ITEM_SUPP_SYS_ID, " +
                $" ITEM_SALE_Y_N, ITEM_BTCH_Y_N, ITEM_ASSPLD_Y_N, ITEM_NEED_AUTH_Y_N, ITEM_NEED_MDCHL_DESC_Y_N,  " +
                $" NVL(fn_get_item_sale_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS ITEM_SALE_PRICE, " +
                $" NVL(fn_get_item_lpur_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS ITEM_LAST_PUR_PRICE, " +
                $" NVL(FN_GET_ITEM_VAT_PCT(ITEM_SYS_ID,{OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) ITEM_VAT_PCT " +
                $" FROM inv_inv_items, inv_item_master WHERE III_ITEM_SYS_ID = ITEM_SYS_ID AND ITEM_SALE_Y_N = 'Y' AND ITEM_FRZ_Y_N <> 'Y' AND III_INV_SYS_ID = :pStockId";
            var parms = new List<OracleParameter>() { new OracleParameter("pStockId", stockId) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetDefaultAccounts(string authParms)
        {
            string v_code = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            var query = $"SELECT FN_GET_WH_ACC_CODE ('CSHC', FUN_GET_PARENT_V_CODE('{v_code}')) AS P_CUST_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('CASHCASH', FUN_GET_PARENT_V_CODE('{v_code}')) AS P_CASH_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('CSHVAT', FUN_GET_PARENT_V_CODE('{v_code}')) AS P_VAT_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('CSHADD', FUN_GET_PARENT_V_CODE('{v_code}')) AS P_ADDED_AMT_ACC_CODE, " +
                $"FN_GET_WH_ACC_CODE('CSHBNK', FUN_GET_PARENT_V_CODE('{v_code}')) AS P_BANK_ACC_CODE, " + 
                $"FN_GET_WH_ACC_CODE('EXPNSV', FUN_GET_PARENT_V_CODE('{v_code}')) AS P_EXP_ACC_CODE, " + 
                $"FN_GET_WH_ACC_CODE('CSHONTCR', FUN_GET_PARENT_V_CODE('{v_code}')) AS P_POINT_CR_ACC_CODE, " + 
                $"FN_GET_WH_ACC_CODE('CSHPNTDR', FUN_GET_PARENT_V_CODE('{v_code}')) AS P_POINT_DR_ACC_CODE " + 
                $"FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetDefaultPrinters(string authParms)
        {
            string v_code = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            var query = $"SELECT GPS_SYS_ID AS PRINTER_ID, GPS_PRINTER_NAME AS PRINTER_NAME, GPS_DEVICE_OS_NAME AS DEVICE_NAME, GPS_TYPE_RPT_RCT_LBL_BRC AS PRINTER_TYPE " +
                $" FROM GAS_PRINTER_SETUP WHERE GPS_V_CODE = '{v_code}' AND GPD_FRZ_Y_N <> 'Y' ";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSalesOrdersNotifactions(int pharmacyId, string authParms)
        {
            var query = $"SELECT SOH.TSOH_SYS_ID, SOH.TSOH_CODE, SOH.TSOH_DATE, SOH.TSOH_PH_SYS_ID, ph.PHARM_NAME_AR, ph.PHARM_NAME_EN, SOH.TSOH_CUST_SYS_ID," +
                $" fcust.CUST_NAME_AR, fcust.CUST_NAME_EN, SOH.TSOH_CURRENT_STATUS, SOH.TSOH_FILE_NAME " +
                $" FROM TKT_SALES_ORDER_HDR SOH, GAS_PHARMACY ph, FINS_CUSTOMER fcust " +
                $" WHERE ph.PHARM_SYS_ID = SOH.TSOH_PH_SYS_ID AND fcust.CUST_SYS_ID = SOH.TSOH_CUST_SYS_ID " +
                $" AND TSOH_PH_SYS_ID = {pharmacyId} AND SOH.TSOH_CURRENT_STATUS IN ('WIP', 'TOP', 'OPN')";
            //var parms = new List<OracleParameter>() { new OracleParameter("pPharamcyId", pharmacyId) };
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public List<dynamic> GetReminders(int pharmacyId, string authParms)
        {
            var query = $"SELECT SOH.TSOH_SYS_ID, SOH.TSOH_CODE, SOH.TSOH_DATE, SOH.TSOH_PH_SYS_ID, ph.PHARM_NAME_AR, ph.PHARM_NAME_EN, SOH.TSOH_CUST_SYS_ID," +
                $" fcust.CUST_NAME_AR, fcust.CUST_NAME_EN, SOH.TSOH_CURRENT_STATUS, SOH.TSOH_FILE_NAME " +
                $" FROM TKT_SALES_ORDER_HDR SOH, GAS_PHARMACY ph, FINS_CUSTOMER fcust " +
                $" WHERE ph.PHARM_SYS_ID = SOH.TSOH_PH_SYS_ID AND fcust.CUST_SYS_ID = SOH.TSOH_CUST_SYS_ID " +
                $" AND TSOH_PH_SYS_ID = {pharmacyId} AND SOH.TSOH_CURRENT_STATUS IN ('WIP', 'TOP', 'OPN')";
            return OracleDQ.GetData<dynamic>(query, authParms, null, CommandType.Text);
        }

        public async Task<DataSet> getPosShiftsExpenses(IPosShiftsExpenses entity, string authParms)
        {
            var query = $"select* from POS_SHIFTS_EXPENSES where POS_SHIFTS_EXPENSES.PSE_PSA_SYS_ID=:pPSE_PSA_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pPSE_PSA_SYS_ID", entity.PSE_PSA_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> SavePosShiftsExpenses(List<IPosShiftsExpenses> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_SHIFTS_EXPENSES_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> GetItemWithinNearbyPharamcies(int itemId, string authParms)
        {
            var query = $"SELECT * FROM V_ITEM_IN_NEARBY_PHARAMCIES WHERE item_sys_id = {itemId}";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPOSInvPromotions(int invId, string authParms)
        {
            var query = $"SELECT * FROM V_CURRENT_PROMOTIONS_DETAILS WHERE PCH_SYS_ID = {invId}";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> CheckItemDiscount(int itemId, decimal Qty, string authParms)
        {
            var query = $"SELECT fn_get_ITEM_DISCOUNT_PCT ({itemId}, {Qty}) AS DISCOUNT FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> CheckInvoiceDiscount(decimal total, string authParms)
        {
            var query = $"SELECT fn_get_POS_DISCOUNT_PCT ({total}) AS DISCOUNT FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> CheckLinkConnection()
        {
            bool online = false;
            TcpClient tc = new TcpClient();
            try
            {
                await tc.ConnectAsync("94.242.61.84", 8889);
                bool stat = tc.Connected;
                if (stat)
                {
                    online = true;
                }
                tc.Close();
            }
            catch (Exception)
            {
                online = false;
                tc.Close();
            }
            return await OracleDQ.handleOnlineOfflineDataSet(new DataSet(), new {message = online });
        }

        public async Task<DataSet> SyncDataFromLocalToLive(string authParms)
        {
            return await OracleDQ.ExcuteSyncLiveWithLocalProc("PRC_CASHIER_UPLOAD", authParms);
        }
    }
}
