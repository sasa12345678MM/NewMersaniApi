using Mersani.Interfaces.Website.CusAuth;
using Mersani.models.FinancialSetup;
using Mersani.models.website;
using Mersani.Oracle;
using Mersani.Utility;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Website.CustRepo
{
    public class WebAuthRepository : IWebAuthRepo
    {


        public async Task<DataSet> Login(WebLoginModel user)
        {
            DataSet res = await OracleDQ.WebLoginAuthCheck("Web_LOGIN_AUTH_CHECK", user);
            return GetDataTableWithToken(res);
        }




        public async Task<DataSet> Register(Customer customer, string authParms)
        {
          
            await OracleDQ.ExcuteXmlProcAsync("PRC_Web_Auth_CUSTOMER_Register_XML", new List<dynamic>() { customer }, authParms, true);
            string query = $"SELECT * FROM FINS_CUSTOMER USR WHERE CUST_ATT_EMAIL = '{customer.CUST_ATT_EMAIL}' AND CUST_PASSWORD = '{customer.CUST_PASSWORD}'";
            DataSet res = await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, "result", true);
            return GetDataTableWithToken(res);
        }


        public async Task<DataSet> UpdateCustomer(Customer customer, string authParms)
        {

            await OracleDQ.ExcuteXmlProcAsync("PRC_POS_CUSTOMER_UPDATE_XML", new List<dynamic>() { customer }, authParms, true);
            string query = $"SELECT * FROM FINS_CUSTOMER USR WHERE CUST_ATT_EMAIL = '{customer.CUST_ATT_EMAIL}' AND CUST_PASSWORD = '{customer.CUST_PASSWORD}'";
            DataSet res = await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, "result", true);
            return GetDataTableWithToken(res);
        }



        public DataSet GetDataTableWithToken(DataSet res)
        {
            DataTable resTable = res.Tables["result"];
            for (int i = 0; i < res.Tables["result"].Rows.Count; i++)
            {
                DataRow row = res.Tables["result"].Rows[i];
                decimal UserCode = row.Field<decimal>("CUST_SYS_ID");
                decimal UserGroup = row.Field<decimal>("CUST_CLASS_SYS_ID");
                string UserLogin = row.Field<string>("CUST_ATT_EMAIL");
                string User_V_Code = row.Field<string>("CUST_V_CODE");
                string data = "UserCode" + "/" + UserCode
                            + "," + "UserGroup" + "/" + UserGroup
                            + "," + "ForDebug" + "/" + 0
                            + "," + "UserLogin" + "/" + UserLogin
                            + "," + "User_V_Code" + "/" + User_V_Code
                            + "," + "UserCurrency" + "/" + 7
                            + "," + "UserType" + "/" + UserGroup
                            + "," + "UserLanguage" + "/" + "AR"
                            + "," + "User_Parent_V_Code" + "/" + "OW102";
                string token = CustomAuth.encodingToken(data);
                if (!resTable.Columns.Contains("USER_TOKEN"))
                {
                    resTable.Columns.Add("USER_TOKEN", typeof(string));
                }
                row["USER_TOKEN"] = token;
            }
            return res;
        }

      
    }
}
