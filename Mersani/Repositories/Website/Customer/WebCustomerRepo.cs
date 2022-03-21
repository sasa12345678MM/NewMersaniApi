using Mersani.Interfaces.Website.Customer_;
using Mersani.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Website.Customer_
{
    public class WebCustomerRepo : IWebCustomer
    {
        public async Task<DataSet> GetCustomerDetailedAdresses(int customerid, string authParms)
        {
           string query = $" SELECT FINS_CUSTOMER_ADDRESSES.FCA_SYS_ID as Code , " +
                  $"                      GAS_REGION.R_NAME_AR || '_' || GAS_COUNTRY.C_NAME_AR || '_' || GAS_CITY.CITY_NAME_AR AS NAMEAR, " +
                  $"                      GAS_REGION.R_NAME_EN || '_' || GAS_COUNTRY.C_NAME_EN || '_' || GAS_CITY.CITY_NAME_EN AS NAMEEN ," +
                  $"                      FINS_CUSTOMER_ADDRESSES.FCA_NEAREST_PHARM_SYS_ID " +
                  $"                 FROM FINS_CUSTOMER_ADDRESSES " +
                  $"                      INNER JOIN GAS_REGION " +
                  $"                         ON FINS_CUSTOMER_ADDRESSES.FCA_REGION_SYS_ID = GAS_REGION.R_SYS_ID" +
                  $"                      INNER JOIN GAS_COUNTRY" +
                  $"                         ON FINS_CUSTOMER_ADDRESSES.FCA_CONTERY_SYS_ID = GAS_COUNTRY.C_SYS_ID" +
                  $"                      INNER JOIN GAS_CITY ON FINS_CUSTOMER_ADDRESSES.FCA_CITY_SYS_ID = GAS_CITY.CITY_SYS_ID" +
                  $"                WHERE (FINS_CUSTOMER_ADDRESSES.FCA_CUST_SYS_ID = { customerid} ) and FCA_ACTIVE_Y_N='Y'";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text, _public: true);

        }
    }
}
