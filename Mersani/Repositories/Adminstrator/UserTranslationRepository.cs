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
    public class UserTranslationsRepository : IUserTranslationRepo
    {

        public List<UserTranslation> GetUserTranslation(int id, string authParms)
        {
            var query = "SELECT * FROM GAS_LABEL  WHERE LABEL_CODE = :pCODE OR :pCODE = 0  order by MNU_CODE,LABEL_CODE";
            return OracleDQ.GetData<UserTranslation>(query, authParms, new { pCODE = id });
        }

        public List<UserTranslation> GetPageTranslation(int PageCode, string authParms)
        {
            var query = "SELECT * FROM GAS_LABEL  WHERE MNU_CODE = :pCODE OR :pCODE = 0 order by MNU_CODE";
            return OracleDQ.GetData<UserTranslation>(query, authParms, new { pCODE = PageCode });
        }
        public bool PostUserTranslationp(UserTranslation UserTranslation, string authParms)
        {
            var dyParam = GetDynamicParameters(UserTranslation, authParms, OperationType.Add);
            return OracleDQ.PostData("PRC_GAS_LABEL_INS", authParms, dyParam, commandType: CommandType.StoredProcedure);
            
        }

        public bool UpdateUserTranslation(int id, UserTranslation userTranslation, string authParms)
        {
            var dyParam = GetDynamicParameters(userTranslation, authParms, OperationType.Update);
            return OracleDQ.PostData("PRC_GAS_LABEL_UPD", authParms, dyParam, commandType: CommandType.StoredProcedure);
        }
        public bool DeleteUserTranslation(int id, string authParms)
        {
            var dyParam = GetDynamicParameters(new UserTranslation() { LABEL_CODE = id }, authParms, OperationType.Delete);
            return OracleDQ.PostData("PRC_GAS_LABEL_DEL", authParms, dyParam, commandType: CommandType.StoredProcedure);

        }


        private OracleDynamicParameters GetDynamicParameters(UserTranslation _UserTranslation, string authParms, OperationType operationType)
        {
            var dyParam = new OracleDynamicParameters();
            if (operationType != OperationType.Add && operationType != OperationType.Other)
            {
                dyParam.Add("P_LABEL_CODE", OracleDbType.Int32, ParameterDirection.Input, _UserTranslation.LABEL_CODE);
            }
            if (operationType != OperationType.Delete)
            {
                dyParam.Add("P_FORM_NAME", OracleDbType.Varchar2, ParameterDirection.Input, _UserTranslation.FORM_NAME != null ? _UserTranslation.FORM_NAME.ToUpper() : "");
                dyParam.Add("P_ITEM_NAME", OracleDbType.Varchar2, ParameterDirection.Input, _UserTranslation.ITEM_NAME.Replace(" ", String.Empty).ToUpper());
                dyParam.Add("P_ITEM_LABEL_EN", OracleDbType.Varchar2, ParameterDirection.Input, _UserTranslation.ITEM_LABEL_EN);
                dyParam.Add("P_ITEM_LABEL_AR", OracleDbType.Varchar2, ParameterDirection.Input, _UserTranslation.ITEM_LABEL_AR);
                dyParam.Add("P_ITEM_TYPE", OracleDbType.Varchar2, ParameterDirection.Input, _UserTranslation.ITEM_TYPE);
                dyParam.Add("P_MNU_CODE", OracleDbType.Int32, ParameterDirection.Input, _UserTranslation.MNU_CODE > 0 ? _UserTranslation.MNU_CODE : 0);
            }
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage);
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);
            return dyParam;
        }

        public object ExportPageTranslationAr(string authParms)
        {
            var query = " SELECT ITEM_NAME, ITEM_LABEL_AR, ITEM_LABEL_EN, MNU_CODE ,TO_CHAR(MNU_CODE) as PMNU_CODE FROM GAS_LABEL WHERE MNU_CODE IS NOT NULL AND item_label_ar IS NOT NULL AND item_label_en IS NOT NULL" +
                " union " +
                " SELECT GAM_MSG_CODE AS ITEM_NAME,GAM_AR_MESSAGE AS ITEM_LABEL_AR,GAM_EN_MESSAGE AS ITEM_LABEL_EN,0 AS MNU_CODE,'MSG' as PMNU_CODE FROM GAS_ALERT_MESSAGE";

            List<UserTranslation> usertranslation = OracleDQ.GetData<UserTranslation>(query, authParms, null);

            List<Translation> translations = new List<Translation>();
            for (int i = 0; i < usertranslation.Count; i++)
            {
                translations.Add(new Translation() { ParentCode = usertranslation[i].PMNU_CODE.ToString(), Keyword = usertranslation[i].ITEM_NAME, Value = usertranslation[i].ITEM_LABEL_AR });
            };
            object doneflagAr = OracleDQ.WriteTranslation(translations, 1);
            //object doneflagEn = OracleDQ.WriteTranslation(translations, 2);
            //dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            //MyDynamic.doneflagAr = doneflagAr;
            //MyDynamic.doneflagEn = doneflagEn;
            return doneflagAr;
        }
        public object ExportPageTranslationEn(string authParms)
        {
            var query = "SELECT DISTINCT ITEM_NAME, ITEM_LABEL_AR, ITEM_LABEL_EN, MNU_CODE ,TO_CHAR(MNU_CODE) as PMNU_CODE FROM GAS_LABEL WHERE MNU_CODE IS NOT NULL AND item_label_ar IS NOT NULL AND item_label_en IS NOT NULL" +
                " union " +
                " SELECT DISTINCT GAM_MSG_CODE AS ITEM_NAME,GAM_AR_MESSAGE AS ITEM_LABEL_AR,GAM_EN_MESSAGE AS ITEM_LABEL_EN,0 AS MNU_CODE,'MSG' as PMNU_CODE FROM GAS_ALERT_MESSAGE";

            List<UserTranslation> usertranslation = OracleDQ.GetData<UserTranslation>(query, authParms, null);
            List<Translation> translations = new List<Translation>();
            for (int i = 0; i < usertranslation.Count; i++)
            {
                translations.Add(new Translation() { ParentCode = usertranslation[i].PMNU_CODE.ToString(), Keyword = usertranslation[i].ITEM_NAME, Value = usertranslation[i].ITEM_LABEL_EN });
            };
            object doneflagEn = OracleDQ.WriteTranslation(translations, 2);

            return doneflagEn;
        }

    }
}


