using System.Data;
using Mersani.Oracle;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mersani.models.FinancialSetup;
using Oracle.ManagedDataAccess.Client;
using Mersani.Interfaces.FinancialSetup;

namespace Mersani.Repositories.FinancialSetup
{
    public class CustomerRepository : ICustomerRepo
    {
        public async Task<DataSet> BulkInsertUpdateCustomer(Customer entity, string authParms)
        {
            var Auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            if (entity.CUST_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = Auth.UserCode;
            entity.CUST_V_CODE = Auth.User_Act_PH;
            entity.CUST_FRZ_BY = entity.CUST_FRZ_Y_N == 'Y' ? Auth.UserCode : null;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_CUSTOMER_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteCustomer(Customer entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_CUSTOMER_XML", new List<dynamic>() { entity }, authParms);
        }


        public async Task<DataSet> GetCustomerDataList(Customer entity, string authParms)
        {
            var query = $"select cust.*,acnt.ACC_NO from FINS_CUSTOMER cust left JOIN fins_account acnt ON cust.CUST_ACC_CODE = acnt.ACC_CODE " +
                $"WHERE (CUST_SYS_ID = :pCUST_SYS_ID OR :pCUST_SYS_ID = 0)" +
                $" AND ('OW'||cust.CUST_OWNR_SYS_ID = FUN_GET_PARENT_V_CODE('{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') " +
                $" or cust.CUST_OWNR_SYS_ID is null)";
                //$"CUST_V_CODE = FUN_GET_PARENT_V_CODE('{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}')";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pCUST_SYS_ID", entity.CUST_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> getFinsCustomerAddresses(FinsCustomerAddresses FinsCustomerAddresses, int parentId, string authParms)
        {
            var query = $" SELECT FINS_CUSTOMER_ADDRESSES.*, " +
                $"       GAS_REGION.R_NAME_AR, " +
                $"       GAS_REGION.R_NAME_EN, " +
                $"       GAS_COUNTRY.C_NAME_AR, " +
                $"       GAS_COUNTRY.C_NAME_EN, " +
                $"       GAS_CITY.CITY_NAME_AR, " +
                $"       GAS_CITY.CITY_NAME_EN " +
                $"  FROM FINS_CUSTOMER_ADDRESSES " +
                $"       INNER JOIN GAS_REGION " +
                $"          ON FINS_CUSTOMER_ADDRESSES.FCA_REGION_SYS_ID = GAS_REGION.R_SYS_ID " +
                $"       INNER JOIN GAS_COUNTRY " +
                $"          ON FINS_CUSTOMER_ADDRESSES.FCA_CONTERY_SYS_ID = GAS_COUNTRY.C_SYS_ID " +
                $"       INNER JOIN GAS_CITY ON FINS_CUSTOMER_ADDRESSES.FCA_CITY_SYS_ID = GAS_CITY.CITY_SYS_ID " +
                $" WHERE (FINS_CUSTOMER_ADDRESSES.FCA_CUST_SYS_ID = {parentId} OR {parentId} = 0) " +
                $"       AND (FINS_CUSTOMER_ADDRESSES.FCA_SYS_ID = {FinsCustomerAddresses.FCA_SYS_ID} OR {FinsCustomerAddresses.FCA_SYS_ID} = 0)";                    
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text,_public:true);
        }
        public async Task<DataSet> PostFinsCustomerAddresses(FinsCustomerAddresses FinsCustomerAddresses, string authParms)
        {
            var Auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            if (FinsCustomerAddresses.FCA_SYS_ID > 0) FinsCustomerAddresses.STATE = (int)OperationType.Update;
            else FinsCustomerAddresses.STATE = (int)OperationType.Add;
            FinsCustomerAddresses.CURR_USER = Auth.UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_CUST_ADDREESS_XML", new List<dynamic>() { FinsCustomerAddresses }, authParms,puplic:true);
        }
        public async Task<DataSet> DeleteFinsCustomerAddresses(FinsCustomerAddresses FinsCustomerAddresses, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            FinsCustomerAddresses.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_CUST_ADDREESS_XML", new List<dynamic>() { FinsCustomerAddresses }, authParms);
        }
        public async Task<DataSet> getFinsCustomerRelatives(FinsCustomerRelatives CustomerRelatives, int parentId, string authParms)
        {
            var query = $" SELECT custR.*, dtl.GSD_NAME_AR, dtl.GSD_NAME_EN" +
                $"  FROM FINS_CUSTOMER_RELATIVES custR" +
                $"       INNER JOIN GAS_GNRL_SET_DTL dtl" +
                $"          ON custR.FCR_TYPE_CODE = dtl.GSD_CODE AND dtl.GSD_GSH_SYS_ID = 81" +
                $" WHERE (custR.FCR_CUST_SYS_ID = {parentId} OR {parentId} = 0)" +
                $"       AND(custR.FCR_SYS_ID = {CustomerRelatives.FCR_SYS_ID} OR {CustomerRelatives.FCR_SYS_ID} = 0)";
            //var parms = new List<OracleParameter>() {
            //    new OracleParameter("pCode", CustomerRelatives.FCR_SYS_ID),
            //    new OracleParameter("ParenId",parentId)

            //};
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostFinsCustomerRelatives(FinsCustomerRelatives CustomerRelatives, string authParms)
        {
            var Auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            if (CustomerRelatives.FCR_SYS_ID > 0) CustomerRelatives.STATE = (int)OperationType.Update;
            else CustomerRelatives.STATE = (int)OperationType.Add;
            CustomerRelatives.CURR_USER = Auth.UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_CUST_RELAT_XML", new List<dynamic>() { CustomerRelatives }, authParms);
        }
        public async Task<DataSet> DeleteFinsCustomerRelatives(FinsCustomerRelatives CustomerRelatives, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            CustomerRelatives.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_CUST_RELAT_XML", new List<dynamic>() { CustomerRelatives }, authParms);
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (CUST_CODE, '^[0-9]+') THEN CUST_CODE ELSE '0' END)), 0) + 1 AS Code FROM FINS_CUSTOMER WHERE CUST_V_CODE = FUN_GET_PARENT_V_CODE('{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}')";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPOSLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (CUST_CODE, '^[0-9]+') THEN CUST_CODE ELSE '0' END)), 0) + 1 AS Code FROM FINS_CUSTOMER WHERE CUST_V_CODE IS NULL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async  Task<DataSet> getCustomerByMobile(string mobile, string authParms)
        {
            var query = $"SELECT FINS_CUSTOMER.*, fn_get_Default_Adress(FINS_CUSTOMER.CUST_SYS_ID) AS FCA_SYS_ID " +
                $" FROM FINS_CUSTOMER WHERE(FINS_CUSTOMER.CUST_ATT_MOBILE = '{mobile}')";
           
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> SavePOSCustomer(Customer customer, string authParms)
        {
            var Auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            customer.STATE = (int)OperationType.Add;
            customer.CURR_USER = Auth.UserCode;
            customer.CUST_V_CODE = Auth.User_Act_PH;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_CUSTOMER_ADD_XML", new List<dynamic>() { customer }, authParms);
        }
    }

}

