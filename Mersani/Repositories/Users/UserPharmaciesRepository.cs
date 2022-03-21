using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using Mersani.Interfaces.Users;
using System.Threading.Tasks;
using Mersani.models.Users;
using Mersani.Oracle;
using System.Data;
using System.Linq;

namespace Mersani.Repositories.Users
{
    public class UserPharmaciesRepository : IUserPharmaciesRepo
    {
        public async Task<DataSet> GetUserPharmciesByUserId(int id, string authParms)
        {
            var query = $"SELECT usrph.*, ph.PHARM_NAME_AR, ph.PHARM_NAME_EN " +
                $" FROM GAS_USR_PHARMACY usrph " +
                $" JOIN GAS_PHARMACY ph ON ph.PHARM_SYS_ID = usrph.UBA_PH_SYS_ID " +
                $" WHERE usrph.UBA_USR_CODE = :pUBA_USR_CODE";
            var parms = new List<OracleParameter>() { new OracleParameter("pUBA_USR_CODE", id) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
         
        public async Task<DataSet> BulkUserPharmcies(List<UserPharmacies> pharmacies, string authParms)
        {
            foreach (var entity in pharmacies)
            {
                if (entity.UBA_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_USR_PHARMACY_XML", pharmacies.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteUserPharmcy(int id, string authParms)
        {
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_USR_PHARMACY_XML", new List<dynamic>() {
                new UserPharmacies() { UBA_SYS_ID = id, STATE = (int)OperationType.Delete }
            }, authParms);
        }
    }
}
