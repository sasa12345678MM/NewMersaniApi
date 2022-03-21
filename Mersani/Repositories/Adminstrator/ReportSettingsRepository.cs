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
    public class ReportSettingsRepository : IReportSettingsRepo
    {

        public async Task<DataSet> getMenuData(Menu Menu, string authParms)
        {
            var query = $"select * from GAS_MNU where (GAS_MNU.MNU_TYPE='C' and GAS_MNU.MNU_PAGE_REPORT_P_R='R') and (MNU_CODE=:MNU_CODE or :MNU_CODE=0) ";
            var parms = new List<OracleParameter>() { new OracleParameter("MNU_CODE", Menu.MNU_CODE) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> getMenuReportsData(IMenuReports IMenuReports, string authParms)
        {
            var query = $" select * from GAS_MNU_REPORTS where GAS_MNU_REPORTS.MNURPT_MNU_CODE = :MNURPT_MNU_CODE ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("MNURPT_MNU_CODE", IMenuReports.MNURPT_MNU_CODE)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> geMenuReportParmData(IMenuReportParm IMenuReportParm, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $" select * from GAS_MNU_REPORT_PARM where GAS_MNU_REPORT_PARM.RDTL_MNU_CODE = :RDTL_MNU_CODE ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("RDTL_MNU_CODE", IMenuReportParm.RDTL_MNU_CODE)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> SaveMenuReportsData(List<IMenuReports> entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entity.Count; i++)
            {
                entity[i].CURR_USER = authP.UserCode;
                entity[i].MNURPT_CLASS_NAME = entity[i].MNURPT_CLASS_NAME.Trim();
                entity[i].MNURPT_ASSEMPLY_NAME = entity[i].MNURPT_ASSEMPLY_NAME.Trim();
                if (entity[i].MNURPT_SYS_ID > 0)
                    if (entity[i].STATE == 3)
                    {
                        entity[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entity[i].STATE = (int)OperationType.Update;
                    }
                else
                    entity[i].STATE = (int)OperationType.Add;
            }
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", entity.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_GAS_MNU_REPORTS_XML", parameters, authParms);

        }

        public async Task<DataSet> SaveMenuReportParmData(List<IMenuReportParm> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (IMenuReportParm entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.RDTL_SYS_ID > 0)
                    if (entity.STATE == 3)
                    {
                        entity.STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entity.STATE = (int)OperationType.Update;
                    }
                else
                    entity.STATE = (int)OperationType.Add;
            }
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_d", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_GAS_MNU_REPORT_PARM_XML", parameters, authParms);

        }

        public async Task<DataSet> getMenuReportUsers(IMenuReportUsers IMenuReportUsers, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $" select* from GAS_MNU_REPORT_USERS where GAS_MNU_REPORT_USERS.GMRU_MNURPT_SYS_ID=:MenuReportSysID ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("MenuReportSysID", IMenuReportUsers.GMRU_MNURPT_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> SaveIMenuReportUsersData(List<IMenuReportUsers> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (IMenuReportUsers entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.GMRU_SYS_ID > 0)
                    if (entity.STATE == 3)
                    {
                        entity.STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entity.STATE = (int)OperationType.Update;
                    }
                else
                    entity.STATE = (int)OperationType.Add;
            }
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_d", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_GAS_MNU_REPORT_USERS_XML", parameters, authParms);
        }

        public async Task<DataSet> GetMenuParamsByMenuCode(int menu_code, string authParms)
        {
            var query = $"SELECT * FROM V_MENU_PARAMS_DATA WHERE MENU_CODE = :pMNU_CODE ";
            var parms = new List<OracleParameter>() { new OracleParameter("pMNU_CODE", menu_code) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetMenuParamsByPath(string menu_path, string authParms)
        {
            var query = $"SELECT * FROM V_MENU_PARAMS_DATA WHERE MENU_PATH LIKE '%{menu_path}%' ";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
