using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class TblArchivesRepository : TblArchivesRepo
    {

        public async Task<DataSet> GetTblArchives(TblArchives entity, string authParms)
        {
            var query = $"select *  from TBL_ARCHIVES" +
             $" where  TBL_ARCHIVES.ARCH_PARENT_TBL_NAME ='"+ entity.ARCH_PARENT_TBL_NAME.ToUpper() + "' " +
             $"and TBL_ARCHIVES.ARCH_PARENT_TBL_SYS_ID =:pRCH_PARENT_TBL_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pRCH_PARENT_TBL_SYS_ID", entity.ARCH_PARENT_TBL_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostTblArchives(List<TblArchives> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].INS_USER = authP.UserCode;
                entities[i].ARCH_PARENT_TBL_NAME = entities[i].ARCH_PARENT_TBL_NAME.ToUpper();
                if (entities[i].ARCH_SYS_ID > 0)
                    if (entities[i].STATE == 3)
                    {
                        entities[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities[i].STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_TBL_ARCHIVES_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteTblArchives(TblArchives entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            
                entities.STATE = (int)OperationType.Delete;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_TBL_ARCHIVES_XML", new List<dynamic>() { entities }, authParms);
        }

        //public async Task<DataSet> getscanArchives(int id, string authParms)
        //{
        //    var query = $"select *  from TBL_ARCHIVES" ;
           
        //    return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        //}
    }
}
