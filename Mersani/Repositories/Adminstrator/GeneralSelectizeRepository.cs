using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.models.FinancialSetup;
using Mersani.Oracle;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class GeneralSelectizeRepository : IGeneralSelectize
    {
        public async Task<List<dynamic>> GetUserActivities(int id, string authParms)
        {
            string query = $"SELECT DISTINCT GAM.FAC_CODE AS CODE, GAM.FAC_NAME_AR AS NAMEAR, GAM.FAC_NAME_EN AS NAMEEN " +
                "FROM GAS_ACTIVITY_MASTER GAM " +
                "JOIN GAS_BR_ACTV BRACTV ON BRACTV.FAC_ACTIVITY_CODE = GAM.FAC_CODE " +
                "JOIN GAS_USR_BR_ACTV UBA ON BRACTV.FAC_SYS_ID = UBA.UBA_ACV_SYS_ID " +
                "WHERE UBA.UBA_USR_CODE = :pUserCode";
            int userCode = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode.Value;
            return await OracleDQ.GetDataAsync<dynamic>(query, authParms, new { pUserCode = userCode }, CommandType.Text);
        }

        public async Task<List<dynamic>> GetBranchesByActivity(int activityId, string authParms)
        {
            var query = $"SELECT DISTINCT branch.CB_SYS_ID AS CODE, branch.CB_NAME_AR AS NAMEAR, branch.CB_NAME_EN AS NAMEEN " +
                $"FROM GAS_COMPANY_BRANCHES branch " +
                $"JOIN GAS_BR_ACTV brActv ON branch.CB_SYS_ID = brActv.FAC_BR_SYS_ID " +
                $"JOIN GAS_USR_BR_ACTV UBA ON UBA.UBA_ACV_SYS_ID = brActv.FAC_ACTIVITY_CODE " +
                $"WHERE UBA.UBA_USR_CODE = :pUserCode and UBA.UBA_ACV_SYS_ID = :pACV_SYS_ID";
            int userCode = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode.Value;
            return await OracleDQ.GetDataAsync<dynamic>(query, authParms, new { pUserCode = userCode, pACV_SYS_ID = activityId });
        }

        public Task<List<dynamic>> GetCompaniesByBranch(int branchId, string authParms)
        {
            var query = $"SELECT DISTINCT COMP.COMP_SYS_ID AS CODE, COMP.COMP_NAME_AR AS NAMEAR, COMP.COMP_NAME_EN AS NAMEEN " +
                $"FROM GAS_COMPANY COMP " +
                $"JOIN GAS_COMPANY_BRANCHES BR ON BR.CB_COMPANY_SYS_ID = COMP.COMP_SYS_ID " +
                $"JOIN GAS_BR_ACTV BRACTV ON BRACTV.FAC_BR_SYS_ID = BR.CB_SYS_ID " +
                $"JOIN GAS_USR_BR_ACTV UBA ON BRACTV.FAC_SYS_ID = UBA.UBA_ACV_SYS_ID " +
                $"WHERE UBA.UBA_USR_CODE = :pUserCode and BR.CB_SYS_ID = :pCB_SYS_ID";
            int userCode = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode.Value;
            return OracleDQ.GetDataAsync<dynamic>(query, authParms, new { pUserCode = userCode, pCB_SYS_ID = branchId });
        }

        public List<FinsAcountClass> GetFinsAccountClass(int AccountClassCode, string authParms)
        {
            if (AccountClassCode > 0)
            {
                var query = "SELECT * FROM FINS_ACC_CLASS where ACC_CLASS_CODE= :pACC_CODE :pACC_CODE = 0";
                return OracleDQ.GetData<FinsAcountClass>(query, authParms, new { pACC_CODE = AccountClassCode });
            }
            else
            {
                var query = "SELECT * FROM FINS_ACC_CLASS";
                return OracleDQ.GetData<FinsAcountClass>(query, authParms);
            }
        }

        public List<FinsAccountLevel> GetFinsAccountLevel(int AccountLevelCode, string authParms)
        {
            if (AccountLevelCode > 0)
            {
                var query = "SELECT * FROM FINS_ACC_LEVEL where ACC_LEVEL_CODE = :pACC_CODE ";
                return OracleDQ.GetData<FinsAccountLevel>(query, authParms, new { pACC_CODE = AccountLevelCode });
            }
            else
            {
                var query = "SELECT * FROM FINS_ACC_LEVEL";
                return OracleDQ.GetData<FinsAccountLevel>(query, authParms);
            }
        }

        public List<FinsCostCeneter> GetFinsCostCenter(int CostCenterCode, string authParms)
        {
            if (CostCenterCode > 0)
            {
                var query = "SELECT * FROM FINS_COST_CENTER where COST_CENTER_CODE= :pACC_CODE";
                return OracleDQ.GetData<FinsCostCeneter>(query, authParms, new { pACC_CODE = CostCenterCode });
            }
            else
            {
                var query = "SELECT * FROM FINS_COST_CENTER";
                return OracleDQ.GetData<FinsCostCeneter>(query, authParms);
            }
        }

        private OracleDynamicParameters GetDynamicParameters(int companyId, int branchId, string authParms)
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("P_ACTV_ID", OracleDbType.RefCursor, ParameterDirection.Output);
            dyParam.Add("P_BR_ID", OracleDbType.Int32, ParameterDirection.Input, branchId);
            dyParam.Add("P_COMP_ID", OracleDbType.Int32, ParameterDirection.Input, companyId);
            dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserCode);
            dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, "AR");
            dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
            dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);

            return dyParam;
        }

        public async Task<List<dynamic>> getMirsaniSelectData(ISelectSearch selectSearch, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = "";
            var key = selectSearch.key;
            object search = null;
            var dyParam = new OracleDynamicParameters();
            string json = selectSearch.filterKeyes;
            JObject myJObject = null;
            if (selectSearch.filterKeyes != null)
            {
                myJObject = JObject.Parse(json);

            }
            CommandType commandType = CommandType.Text;
            if (key > 5000)
            {
                int parent = key - 5000;
                search = new { pCODE = parent };
                if (parent == 1)
                {
                    query = "SELECT gen.GSD_SYS_ID AS CODE, gen.GSD_NAME_AR AS NAMEAR, gen.GSD_NAME_EN AS NAMEEN FROM GAS_GNRL_SET_DTL gen WHERE gen.GSD_GSH_SYS_ID = :pCODE order by gen.GSD_SORT";
                }
                else
                {
                    query = "SELECT gen.GSD_CODE AS CODE, gen.GSD_NAME_AR AS NAMEAR, gen.GSD_NAME_EN AS NAMEEN FROM GAS_GNRL_SET_DTL gen WHERE gen.GSD_GSH_SYS_ID = :pCODE order by gen.GSD_SORT";

                }
            }

            if (key == 1) // ACC_CLASS
            {
                search = new { pCODE = 0 };
                query = "SELECT  ACC_CLASS_CODE As Code  ,ACC_CLASS_CODE As PCode  ," +
                            $"NVL(ACC_CLASS_NAME_AR,ACC_CLASS_NAME_AR) as NameAr," +
                            $"NVL(ACC_CLASS_NAME_EN,ACC_CLASS_NAME_EN) as NameEn " +
                            $"FROM FINS_ACC_CLASS  where ACC_CLASS_CODE = :pCODE or :pCODE=0 order by ACC_CLASS_CODE";
            }
            if (key == 2) // CostCenter
            {
                search = new { pCODE = 0 };
                query = "SELECT  COST_CENTER_CODE As Code,COST_CENTER_CODE As PCode" +
                             $",COST_CENTER_CODE||'-'||NVL(COST_CENTER_NAME_AR,COST_CENTER_NAME_EN) as NameAr " +
                             $",COST_CENTER_CODE||'-'||NVL(COST_CENTER_NAME_EN,COST_CENTER_NAME_AR) as NameEn " +
                             $", COST_CENTER_NO " +
                             $" FROM FINS_COST_CENTER where (COST_CENTER_CODE = :pCODE or :pCODE = 0 )" +
                             //$" and COST_CENTER_V_CODE= '{auth.User_Act_PH}' " +
                             $" order by COST_CENTER_CODE";
            }
            if (key == 3) // Account Level
            {
                search = new { pCODE = 0 };
                query = "SELECT  ACC_LEVEL_CODE As Code, ACC_LEVEL_CODE As PCode" +
                             $", ACC_LEVEL_NAME_AR as NameAr" +
                             $", ACC_LEVEL_NAME_EN as NameEn" +
                             $", ACC_LEVEL_DIGITS" +
                             $" FROM FINS_ACC_LEVEL" +
                             $" where  ACC_LEVEL_CODE = :pCODE or :pCODE = 0" +
                             $" order by ACC_LEVEL_CODE";
            }

            if (key == 4) // menues
            {
                int userGroupId = 0;
                if (myJObject != null && myJObject.SelectToken("UserGroupId") != null)
                {
                    userGroupId = myJObject.SelectToken("UserGroupId").Value<int>();
                }
                search = new { pUSRGRP_CODE = userGroupId };
                query = $"SELECT UGMNU.MNU_CODE AS CODE, UGMNU.MNU_CODE AS PCODE, MNU.MNU_LABEL_AR AS NAMEAR, MNU.MNU_LABEL_EN AS NAMEEN " +
                    $"FROM GAS_USRGRP_MNU UGMNU JOIN GAS_MNU MNU ON MNU.MNU_CODE = UGMNU.MNU_CODE " +
                    $"WHERE UGMNU.USRGRP_CODE = :pUSRGRP_CODE AND MNU.MNU_TYPE = 'C'";
            }
            if (key == 5) // countries
            {
                search = new { pCODE = 0 };
                query = $"SELECT C_SYS_ID AS CODE, C_SYS_ID AS PCODE, C_NAME_EN AS NAMEEN, C_NAME_AR AS NAMEAR" +
                    $" FROM GAS_COUNTRY" +
                    $" WHERE C_SYS_ID = :pCODE or :pCODE = 0";
            }

            if (key == 6) // regions
            {
                int CountryId = 0;
                if (myJObject != null && myJObject.SelectToken("CountryId") != null)
                {
                    CountryId = myJObject.SelectToken("CountryId").Value<int>();
                }
                search = new { pCODE = 0, PCountryId = CountryId };
                query = $"SELECT R_SYS_ID AS CODE,R_SYS_ID AS PCODE, R_NAME_EN AS NAMEEN, R_NAME_AR AS NAMEAR " +
                    $"FROM GAS_REGION " +
                    $"WHERE (R_SYS_ID = :pCODE or :pCODE = 0)";
                if (CountryId > 0)
                {
                    query += "and GAS_REGION.R_COUNTRY_SYS_ID= " + CountryId + " ";
                }
            }

            if (key == 7) // cities
            {
                int CountryId = 0;
                int RegionId = 0;
                if (myJObject != null && myJObject.SelectToken("CountryId") != null)
                {
                    CountryId = myJObject.SelectToken("CountryId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("RegionId") != null)
                {
                    RegionId = myJObject.SelectToken("RegionId").Value<int>();
                }
                search = new { pCODE = CountryId };
                query = $"SELECT city.CITY_SYS_ID AS CODE,city.CITY_SYS_ID AS PCODE, city.CITY_NAME_AR AS NAMEAR, city.CITY_NAME_EN AS NAMEEN FROM GAS_CITY city " +
                    $"join GAS_REGION region ON region.R_SYS_ID = city.CITY_REGION_SYS_ID " +
                    $"join GAS_COUNTRY ctry on ctry.C_SYS_ID = region.R_COUNTRY_SYS_ID " +
                    $"where ( ctry.C_SYS_ID = :pCODE or :pCODE = 0 )";
                if (RegionId > 0)
                {
                    query += " and city.CITY_REGION_SYS_ID= " + RegionId + " ";
                }
            }
            if (key == 8) // activities
            {
                search = new { pCODE = 0 };
                query = $"SELECT FAC_CODE AS CODE,FAC_CODE AS PCODE, FAC_NAME_AR AS NAMEAR, FAC_NAME_EN AS NAMEEN" +
                    $" FROM GAS_ACTIVITY_MASTER " +
                    $"WHERE FAC_CODE = :pCODE or :pCODE = 0";
            }

            if (key == 9) // groups
            {
                search = new { pCODE = 0 };
                query = $"SELECT GROUP_SYS_ID AS CODE ,GROUP_SYS_ID AS PCODE, GROUP_NAME_EN AS NAMEEN, GROUP_NAME_AR AS NAMEAR " +
                    $"FROM GAS_GROUP" +
                    $" WHERE GROUP_SYS_ID = :pCODE or :pCODE = 0";
            }

            if (key == 10) // companies
            {
                int GroupId = 0;
                int CountryId = 0;
                if (myJObject != null && myJObject.SelectToken("GroupId") != null)
                {
                    GroupId = myJObject.SelectToken("GroupId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("CountryId") != null)
                {
                    CountryId = myJObject.SelectToken("CountryId").Value<int>();
                }
                search = new { pCODE = 0, pGroupId = GroupId, pCountryId = CountryId };

                query = $"SELECT COMP_SYS_ID AS CODE,COMP_SYS_ID AS PCODE," +
                    $" COMP_NAME_EN AS NAMEEN," +
                    $" COMP_NAME_AR AS NAMEAR ," +
                    $" COMP_GROUP_SYS_ID ," +
                    $" COMP_COUNTRY_SYS_ID " +
                    $" FROM GAS_COMPANY " +
                    $" WHERE (COMP_SYS_ID = :pCODE or :pCODE = 0 ) " +
                    $" and (COMP_GROUP_SYS_ID=:pGroupId or :pGroupId = 0)" +
                    $" and (COMP_COUNTRY_SYS_ID= :pCountryId or:pCountryId = 0)";
            }

            if (key == 11) // branches
            {
                int CompanyId = 0;
                if (myJObject != null && myJObject.SelectToken("CompanyId") != null)
                {
                    CompanyId = myJObject.SelectToken("CompanyId").Value<int>();
                }
                search = new { pCODE = 0, pCompanyId = CompanyId };
                query = $"SELECT CB_SYS_ID AS CODE,CB_SYS_ID AS PCODE, CB_NAME_EN AS NAMEEN, CB_NAME_AR AS NAMEAR" +
                    $" FROM GAS_COMPANY_BRANCHES " +
                    $"WHERE (CB_SYS_ID = :pCODE or :pCODE = 0) " +
                    $"and CB_COMPANY_SYS_ID = :pCompanyId or :pCompanyId =0 ";
            }
            if (key == 12) // Menus
            {
                string MenuType = "C";
                if (myJObject != null && myJObject.SelectToken("MenuType") != null)
                {
                    MenuType = myJObject.SelectToken("MenuType").Value<string>();
                }
                search = new { pCODE = 0, pType = MenuType };
                query = $"SELECT MNU_CODE AS CODE, MNU_CODE AS PCODE " +
                    $",MNU_LABEL_EN AS NAMEEN " +
                    $",MNU_LABEL_AR AS NAMEAR " +
                    $" FROM GAS_MNU WHERE (MNU_CODE = :pCODE OR :pCODE = 0) AND MNU_TYPE = :pType";
            }

            if (key == 13) // user company branch privilege
            {
                search = new { pUCB_SYS_ID = 0 };
                query = $"SELECT UCB.UCB_SYS_ID AS CODE,UCB.UCB_SYS_ID AS PCODE, " +
                $"(USR.USR_FULL_NAME_AR || '-' || CMP.COMP_NAME_AR || '-' || CB.CB_NAME_AR) AS NAMEAR, " +
                $"(USR.USR_FULL_NAME_EN || '-' || CMP.COMP_NAME_EN || '-' || CB.CB_NAME_EN) AS NAMEEN " +
                $"FROM GAS_USR_COM_BR UCB " +
                $"JOIN GAS_USR USR ON USR.USR_CODE = UCB.UCB_USR_CODE " +
                $"JOIN GAS_COMPANY CMP ON CMP.COMP_SYS_ID = UCB.UCB_COMP_SYS " +
                $"JOIN GAS_COMPANY_BRANCHES CB ON CB.CB_SYS_ID = UCB.UCB_BRANCH_SYS " +
                $"WHERE UCB.UCB_SYS_ID = :pUCB_SYS_ID OR :pUCB_SYS_ID = 0";
            }

            if (key == 14) // activities with branches
            {
                search = new { pFAC_SYS_ID = 0 };
                query = $"SELECT FACB.FAC_SYS_ID AS CODE,FACB.FAC_SYS_ID AS PCODE, " +
                    $"(GAM.FAC_NAME_AR || '-' || GCB.CB_NAME_AR) AS NAMEAR, " +
                    $"(GAM.FAC_NAME_EN || '-' || GCB.CB_NAME_EN) AS NAMEEN " +
                    $"FROM GAS_BR_ACTV FACB " +
                    $"JOIN GAS_ACTIVITY_MASTER GAM ON GAM.FAC_CODE = FACB.FAC_ACTIVITY_CODE " +
                    $"JOIN GAS_COMPANY_BRANCHES GCB ON GCB.CB_SYS_ID = FACB.FAC_BR_SYS_ID " +
                    $"WHERE FACB.FAC_SYS_ID = :pFAC_SYS_ID OR :pFAC_SYS_ID = 0";
            }
            if (key == 15) // user-comp-brach-actv
            {
                int usr_code = 0;
                if (myJObject != null && myJObject.SelectToken("usr_code") != null)
                {
                    usr_code = myJObject.SelectToken("usr_code").Value<int>();
                }

                search = new { pCODE = usr_code };
                query = $"SELECT DISTINCT DATA.V_CODE         AS CODE," +
                    $"       DATA.V_CODE AS PCODE," +
                    $"       DATA.V_DESC_NAME_AR AS NAMEAR," +
                    $"       DATA.V_DESC_NAME_EN AS NAMEEN," +
                    $"       DATA.V_FRZ_Y_N," +
                    $"       DATA.V_PARENT_FRZ_Y_N," +
                    $"       DATA.V_UBA_FRZ_Y_N" +
                    $"  FROM USR_ACTV_VIEW DATA" +
                    $" WHERE (DATA.V_USR_CODE = :pCODE or :pCODE=0)" +
                    $" and V_FRZ_Y_N = 'N' and V_PARENT_FRZ_Y_N = 'N'";
            }

            if (key == 16) // CUSTOMER CLASSES
            {
                search = new { pCODE = 0 };
                query = $"SELECT FCUC_SYS_ID AS CODE, FCUC_SYS_ID AS PCODE, FCUC_NAME_EN AS NAMEEN, FCUC_NAME_AR AS NAMEAR FROM FINS_CUST_CLASS " +
                    $"WHERE FCUC_SYS_ID = :pCODE OR :pCODE = 0";
            }
            if (key == 17) // SUPPLIER CLASSES
            {
                search = new { pCODE = 0 };
                query = $"SELECT FSUC_SYS_ID AS CODE, FSUC_SYS_ID AS PCODE, FSUC_NAME_EN AS NAMEEN, FSUC_NAME_AR AS NAMEAR FROM FINS_SUPP_CLASS " +
                    $"WHERE FSUC_SYS_ID = :pCODE OR :pCODE = 0";
            }
            if (key == 18) // PHARMACIES
            {
                int OwnerId = 0;
                if (myJObject != null && myJObject.SelectToken("OwnerId") != null)
                {
                    OwnerId = myJObject.SelectToken("OwnerId").Value<int>();
                }
                search = new { pCODE = 0, pOwnerId = OwnerId };
                query = $"SELECT PHARM_SYS_ID AS CODE,PHARM_SYS_ID AS PCODE, PHARM_NAME_AR AS NAMEAR, PHARM_NAME_EN AS NAMEEN, IIM_SYS_ID AS INVENTORY_ID FROM GAS_PHARMACY " +
                    $" LEFT JOIN inv_inventory_master ON PHARM_SYS_ID = IIM_INV_S_PHARM_SYS_ID AND IIM_INV_TYPE_I_S = 'S' " +
                    $" WHERE (PHARM_SYS_ID = :pCODE OR :pCODE = 0) AND (OWNER_SYS_ID = :pOwnerId OR nvl(:pOwnerId, 0) = 0)";
            }
            if (key == 19) ////  owner
            {
                search = new { pCODE = 0 };
                query = $"SELECT OWNER_SYS_ID as Code," +
                    $"OWNER_CODE as PCODE," +
                    $"OWNER_NAME_AR As NameAr," +
                    $"OWNER_NAME_EN  as NameEn ," +
                    $"OWNER_MOB  " +
                    $"FROM gas_owner " +
                    $"where OWNER_SYS_ID = :pCODE or :pCODE = 0";
            }
            if (key == 20) ////  Currencies 
            {
                search = new { pCODE = 0 };
                query = $"SELECT CURR_SYS_ID AS CODE,CURR_SYS_ID AS PCODE," +
                    $" CURR_NAME_AR AS NAMEAR," +
                    $" CURR_NAME_EN AS NAMEEN," +
                    $" get_currency_ex_rate(" + auth.UserCurrency + ", GAS_CURRENCY.CURR_SYS_ID) AS currr_rate" +
                    $"  FROM GAS_CURRENCY order by CURR_SYS_ID";

            }
            if (key == 21) ////  FTS_TXN_CODE     VoucherTypes 
            {
                search = new { pCODE = 0 };
                query = $"SELECT" +
                    $" FTS_TXN_CODE AS CODE,FTS_TXN_CODE AS PCODE," +
                    $" FTS_NAME_AR  AS NAMEAR," +
                    $" FTS_NAME_EN  AS NAMEEN," +
                    $" fn_get_vchr_code (FTS_TXN_CODE,'{auth.User_Parent_V_Code}') AS VCHR_CODE " +
                    $" FROM FINS_TXN_SETUP " +
                    $" where FINS_TXN_SETUP.FTS_TXN_TYPE ='MVCR' ";
            }

            if (key == 22) ////  FINS_PERIOD     
            {
                var Year = DateTime.Now.Year;
                if (myJObject != null && myJObject.SelectToken("Year") != null)
                {
                    Year = myJObject.SelectToken("Year").Value<int>();
                }

                search = new { pCODE = 0, pYear = Year };
                query = $"SELECT PERIOD_SYS_ID                    AS CODE," +
                        $"       PERIOD_SYS_ID                    AS PCODE," +
                        $"       PERIOD_YEAR||'-'||TO_CHAR (PERIOD_END_DT, 'Month') AS NAMEAR," +
                        $"       PERIOD_YEAR||'-'||TO_CHAR (PERIOD_END_DT, 'Month') AS NAMEEN," +
                        $"       PERIOD_YEAR," +
                        $"       PERIOD_START_DT," +
                        $"       PERIOD_END_DT " +
                        $"  FROM FINS_PERIOD" +
                        $" WHERE     (PERIOD_SYS_ID = :pCODE OR :pCODE = 0)" +
                        $" AND FINS_PERIOD.PERIOD_Y_N = 'Y' " +
                        $" and PERIOD_V_CODE = '{ auth.User_Act_PH}'" +
                        $" order by PERIOD_SYS_ID desc  ";
                //$"     and (FINS_PERIOD.PERIOD_YEAR= :pYear or :pYear=0 ) " +

            }
            if (key == 23) ////  FINS_ACCOUNT lvl3     
            {
                int AccountLevel = 3;
                string AccClass = "";
                bool NotInOtherComp = false;
                if (myJObject != null && myJObject.SelectToken("AccountLevel") != null)
                {
                    AccountLevel = myJObject.SelectToken("AccountLevel").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("AccClass") != null)
                {
                    AccClass = myJObject.SelectToken("AccClass").Value<string>();
                }

                if (myJObject != null && myJObject.SelectToken("NotInOtherComp") != null)
                {
                    NotInOtherComp = myJObject.SelectToken("NotInOtherComp").Value<bool>();
                }

                search = new { pCODE = 0, pAccountLevel = AccountLevel, pAccClass = AccClass };
                query = $"  SELECT FINS_ACCOUNT.ACC_CODE AS CODE," +
                    $"         FINS_ACCOUNT.ACC_NO AS PCODE," +
                    $"         FINS_ACCOUNT.ACC_NO || '-' || FINS_ACCOUNT.ACC_NAME_AR AS NAMEAR," +
                    $"         FINS_ACCOUNT.ACC_NO || '-' || FINS_ACCOUNT.ACC_NAME_En AS NAMEEN," +
                    $"         FINS_ACCOUNT.ACC_CODE," +
                    $"         FINS_ACCOUNT.ACC_OLD_NO," +
                    $"         ACC_V_CODE" +
                    $"    FROM FINS_ACCOUNT" +
                    $"   WHERE(FINS_ACCOUNT.ACC_STATUS = 'A')" +
                    $"         AND(FINS_ACCOUNT.ACC_LEVEL_CODE = :pAccountLevel" +
                    $"              OR: pAccountLevel = 0)" +
                    $"         AND ACC_V_CODE = FUN_GET_PARENT_V_CODE('{ auth.User_Act_PH}')" +
                    $"         AND(FINS_ACCOUNT.ACC_CODE = :PCODE OR: PCODE = 0) ";
                if (AccClass.Length > 0)
                {
                    query += $"  AND FINS_ACCOUNT.ACC_CLASS_CODE in(" + AccClass + ")  ";
                }

                if (NotInOtherComp) query += $"  AND FINS_ACCOUNT.ACC_CODE NOT IN (SELECT GOIC_ACC_CODE FROM GAS_OWNER_INS_CO)  ";

                query += $" ORDER BY ACC_NO";

            }

            if (key == 24) //// MODULES
            {
                search = new { pCODE = 0 };
                query = $"SELECT MNU_CODE AS CODE, MNU_CODE AS PCODE, MNU_LABEL_AR AS NAMEAR, MNU_LABEL_EN AS NAMEEN FROM GAS_MNU " +
                    $"WHERE MNU_TYPE = 'P'";

            }

            if (key == 25) //// COMPONENTS 
            {
                int ModuleId = 0;
                if (myJObject != null && myJObject.SelectToken("ModuleId") != null)
                {
                    ModuleId = myJObject.SelectToken("ModuleId").Value<int>();
                }
                search = new { pCODE = ModuleId };
                query = $"SELECT MNU_CODE AS CODE,MNU_CODE AS PCODE, MNU_LABEL_AR AS NAMEAR, MNU_LABEL_EN AS NAMEEN FROM GAS_MNU " +
                    $"WHERE MNU_TYPE = 'C' AND MNU_PARENT = :pCODE or :pCODE = 0 ";

            }

            if (key == 26) //// User Groups
            {
                search = new { pCODE = 0 };
                query = $"SELECT USRGRP_CODE AS CODE,USRGRP_CODE AS PCODE, USRGRP_NAME_AR AS NAMEAR, USRGRP_NAME_EN AS NAMEEN FROM GAS_USRGRP";

            }

            if (key == 27) //// User Menus
            {
                search = null;
                int UserId = 0;
                if (myJObject != null && myJObject.SelectToken("UserId") != null)
                {
                    UserId = myJObject.SelectToken("UserId").Value<int>();
                }
                dyParam.Add("P_MNU_ID", OracleDbType.RefCursor, ParameterDirection.Output);
                dyParam.Add("P_USER_ID", OracleDbType.Int32, ParameterDirection.Input, UserId);
                dyParam.Add("P_LANG", OracleDbType.Varchar2, ParameterDirection.Input, OracleDQ.GetAuthenticatedUserObject(authParms).UserLanguage.ToUpper());
                dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.InputOutput);
                dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.InputOutput);

                query = "PRC_GET_MNU_USR";
            }
            if (key == 28) //// Fins Bank
            {
                search = new { pCODE = 0 };
                query = $"select FB_BANK_CODE AS CODE" +
                    $", FB_BANK_CODE AS PCODE" +
                    $", FB_BANK_NAME_AR AS NAMEAR " +
                    $", FB_BANK_NAME_EN AS NAMEEN" +
                    $", FB_BANK_IBAN, FB_BANK_TEL" +
                    $", FB_BANK_FAX " +
                    $" from FINS_BANKS ";

            }
            if (key == 29) // SUPPLIERS DATA
            {
                search = new { pCODE = 0 };
                query = $"SELECT supp.*," +
                    $"       supp.SUPP_SYS_ID AS CODE," +
                    $"       supp.SUPP_NAME_AR AS NAMEAR," +
                    $"       supp.SUPP_NAME_EN AS NAMEEN," +
                    $"       supp.SUPP_CODE AS PCODE," +
                    $"       cls.FSUC_DISC_PERC AS MAX_DISCOUNT" +
                    $"  FROM FINS_SUPPLIER supp" +
                    $"       JOIN fins_supp_class cls ON supp.SUPP_CLASS_SYS_ID = cls.FSUC_SYS_ID" +
                    $" WHERE(SUPP_SYS_ID = :pCODE OR: pCODE = 0) AND supp.SUPP_FRZ_Y_N = 'N' ";
            }
            if (key == 30) // Items By Supplier
            {
                int SupplierId = 0;
                string ItemSale = "";
                string StockType = "";
                string AssemplyType = "";
                int InventoryCode = 0;
                int PurchaseInvoiceId = 0;
                int SalesInvoiceId = 0;
                int PurchaseOrderId = 0;
                int PurchaseRtrnOrderId = 0;
                if (myJObject != null && myJObject.SelectToken("SupplierId") != null)
                {
                    SupplierId = myJObject.SelectToken("SupplierId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("StockType") != null)
                {
                    StockType = myJObject.SelectToken("StockType").Value<string>();
                }
                if (myJObject != null && myJObject.SelectToken("AssemplyType") != null)
                {
                    AssemplyType = myJObject.SelectToken("AssemplyType").Value<string>();
                }
                if (myJObject != null && myJObject.SelectToken("ItemSale") != null)
                {
                    ItemSale = myJObject.SelectToken("ItemSale").Value<string>();
                }
                if (myJObject != null && myJObject.SelectToken("InventoryCode") != null)
                {
                    InventoryCode = myJObject.SelectToken("InventoryCode").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("PurchaseInvoiceId") != null)
                {
                    PurchaseInvoiceId = myJObject.SelectToken("PurchaseInvoiceId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("SalesInvoiceId") != null)
                {
                    SalesInvoiceId = myJObject.SelectToken("SalesInvoiceId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("PurchaseOrderId") != null)
                {
                    PurchaseOrderId = myJObject.SelectToken("PurchaseOrderId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("PurchaseRtrnOrderId") != null)
                {
                    PurchaseRtrnOrderId = myJObject.SelectToken("PurchaseRtrnOrderId").Value<int>();
                }
                search = new
                {
                    pSupplierId = SupplierId,
                    pItemSale = ItemSale,
                    pStockType = StockType,
                    PInventoryCode = InventoryCode,
                    pInvoiceId = PurchaseInvoiceId,
                    sInvoiceId = SalesInvoiceId,
                    POrderId = PurchaseOrderId,
                    PROrderId = PurchaseRtrnOrderId
                };

                query = $"SELECT ITEM.ITEM_SYS_ID  AS CODE," +
                    $"       ITEM.ITEM_SYS_ID AS PCODE," +
                    $"       ITEM.ITEM_NAME_AR AS NAMEAR," +
                    $"       ITEM.ITEM_NAME_EN AS NAMEEN," +
                    $"       ITEM.ITEM_IIG_SYS_ID," +
                    $"       ITEM.ITEM_FRZ_Y_N," +
                    $"       ITEM.ITEM_BTCH_Y_N," +
                    $"       NVL(fn_get_item_lpur_price(ITEM.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {auth.UserCurrency}), 0) as ITEM_LAST_PUR_PRICE," +
                    $"       NVL(fn_get_item_sale_price(ITEM.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {auth.UserCurrency}), 0) as ITEM_SALE_PRICE," +
                    $"       ITEM.ITEM_UOM_SYS_ID," +
                    $"       ITEM.ITEM_SUPP_SYS_ID," +
                    $"       ITEM.ITEM_SALE_Y_N," +
                    $"       UNIT.UOM_NAME_AR," +
                    $"       UNIT.UOM_NAME_EN,";
                if (PurchaseInvoiceId > 0)
                {
                    query += $"PIItem.INVI_ITEM_DISCOUNT_AMT," +
                        $"       PIItem.INVI_ITEM_DISCOUNT_PCT," +
                        $"       PIItem.INVI_ITEM_QTY," +
                        $"       PIItem.INVI_ITEM_UNIT_PRICE," +
                        $"       PIItem.INVI_ITEM_UOM_SYS_ID, ";
                };
                if (SalesInvoiceId > 0)
                {
                    query += $"SIItem.INVSI_ITEM_DISCOUNT_AMT," +
                        $"       SIItem.INVSI_ITEM_DISCOUNT_PCT," +
                        $"       SIItem.INVSI_ITEM_QTY," +
                        $"       SIItem.INVSI_ITEM_UNIT_PRICE," +
                        $"       SIItem.INVSI_ITEM_UOM_SYS_ID,";
                };
                if (PurchaseOrderId > 0)
                {
                    query += $"ipod.IPOD_DISCOUNT_AMNT," +
                        $"       ipod.IPOD_DISCOUNT_PERC," +
                        $"       ipod.IPOD_QTY," +
                        $"       ipod.IPOD_PRCH_PRICE," +
                        $"       ipod.IPOD_UOM_SYS_ID,";
                };
                if (PurchaseRtrnOrderId > 0)
                {
                    query += $"IPROD.IPROD_DISCOUNT_AMNT," +
                        $"       IPROD.IPROD_DISCOUNT_PERC," +
                        $"       IPROD.IPROD_QTY," +
                        $"       IPROD.IPROD_PRCH_PRICE," +
                        $"       IPROD.IPROD_UOM_SYS_ID,";
                };
                query += $"       IIG.IIG_STK_SRV_S_V" +
                    $"  FROM INV_ITEM_MASTER ITEM" +
                    $"       JOIN INV_UOM UNIT ON ITEM.ITEM_UOM_SYS_ID = UNIT.UOM_SYS_ID" +
                    $"       left outer join INV_ITEM_GROUP IIG on ITEM.ITEM_IIG_SYS_ID = IIG.IIG_SYS_ID ";
                if (InventoryCode > 0) { query += $"inner join INV_INV_ITEMS III on III.III_ITEM_SYS_ID = ITEM.ITEM_SYS_ID"; }
                if (PurchaseInvoiceId > 0)
                {
                    query += $"inner join P_INVOICE_ITEM PIItem on ITEM.ITEM_SYS_ID= PIItem.INVI_ITEM_SYS_ID " +
                             $"inner join P_INVOICE_HEAD PIHEAD on PIHEAD.INVH_SYS_ID = PIItem.INVI_INVH_SYS_ID";
                }
                if (SalesInvoiceId > 0)
                {
                    query += $"INNER JOIN S_INVOICE_ITEM SIItem ON ITEM.ITEM_SYS_ID = SIItem.INVSI_ITEM_SYS_ID" +
                        $"       INNER JOIN s_INVOICE_HEAD SIHEAD ON SIHEAD.INVSH_SYS_ID = SIItem.INVSI_INVSH_SYS_ID";
                }
                if (PurchaseOrderId > 0)
                {
                    query += $"inner  JOIN   INV_PRCH_ORDR_DTL ipod ON ipod.IPOD_ITEM_SYS_ID = ITEM.ITEM_SYS_ID ";
                }
                if (PurchaseRtrnOrderId > 0)
                {
                    query += $"inner JOIN   INV_PRCH_RETURN_ORDR_DTL IPROD ON IPROD.IPROD_ITEM_SYS_ID = ITEM.ITEM_SYS_ID ";
                }
                query += $" where 1=1 ";
                if (InventoryCode > 0) { query += $"AND  (III.III_INV_SYS_ID=:PInventoryCode)"; }
                if (PurchaseInvoiceId > 0) { query += $"AND  (PIItem.INVI_INVH_SYS_ID =:pInvoiceId)"; }
                if (PurchaseOrderId > 0) { query += $"and (ipod.IPOD_IPOH_SYS_ID=:POrderId )"; }
                if (PurchaseRtrnOrderId > 0) { query += $"and (IPROD.IPROD_IPROH_SYS_ID=:PROrderId)"; }
                if (SalesInvoiceId > 0) { query += $"AND (SIItem.INVSI_INVSH_SYS_ID = :sInvoiceId)"; }
                if (SupplierId > 0) { query += $"AND ITEM.ITEM_SUPP_SYS_ID = :pSupplierId"; }
                if (ItemSale.Length == 1) { query += $"AND ITEM.ITEM_SALE_Y_N = :pItemSale"; }
                if (StockType.Length == 1) { query += $"AND IIG.IIG_STK_SRV_S_V = '" + StockType + "'"; }
                if (AssemplyType.Length == 1) { query += $"AND ITEM.ITEM_ASSPLD_Y_N = '" + AssemplyType + "'"; }
            }
            if (key == 31) // Units By Item
            {
                int ItemCode = 0;
                if (myJObject != null && myJObject.SelectToken("ItemCode") != null)
                {
                    ItemCode = myJObject.SelectToken("ItemCode").Value<int>();
                }
                search = new { pItemCode = ItemCode };
                query = $"SELECT itemunits.ITU_SYS_ID," +
                    $"       itemunits.ITU_ITEM_SYS_ID," +
                    $"       itemunits.ITU_UOM_SYS_ID AS CODE," +
                    $"       itemunits.TU_CONV_FACTOR," +
                    $"       items.ITEM_NAME_AR," +
                    $"       items.ITEM_NAME_EN," +
                    $"       units.UOM_NAME_AR AS NAMEAR," +
                    $"       units.UOM_NAME_EN AS NAMEEN," +
                    $"       units.UOM_CODE AS PCODE," +
                    $"       items.ITEM_UOM_SYS_ID As ItemMainUnitCode " +
                    $"  FROM INV_ITEM_UOM itemunits" +
                    $"       inner JOIN INV_ITEM_MASTER items" +
                    $"          ON itemunits.ITU_ITEM_SYS_ID = items.ITEM_SYS_ID" +
                    $"       inner JOIN INV_UOM units ON units.UOM_SYS_ID = itemunits.ITU_UOM_SYS_ID" +
                    $" WHERE itemunits.ITU_ITEM_SYS_ID = :pItemCode ";
            }
            if (key == 32) // FixedAssetCategory 
            {
                search = new { pCODE = 0 };
                query = $"SELECT cat.ASSET_CTGRY_SYS_ID as Code " +
                    $", cat.ASSET_CTGRY_NAME_AR as NameAr" +
                    $", cat.ASSET_CTGRY_NAME_EN as NameEn " +
                    $"FROM FINS_FIXED_ASSET_CATEGORIES cat " +
                    $"where cat.ASSET_CTGRY_SYS_ID =:pCODE or :pCODE = 0 ";
            }
            if (key == 33) //FixedAssetType
            {
                search = new { pCODE = 0 };
                query = $"SELECT typ.ASSET_TYPE_SYS_ID As Code" +
                    $", typ.ASSET_TYPE_NAME_AR As NameAr" +
                    $", typ.ASSET_TYPE_NAME_EN as NameEn" +
                    $"  FROM FINS_FIXED_ASSET_TYPES typ " +
                    $"where typ.ASSET_TYPE_SYS_ID =:pCODE or :pCODE = 0 ";
            }
            if (key == 34) //FINS_YEARS
            {

                search = new { pCODE = 0, VCode = auth.User_Act_PH };
                query = $" SELECT FINS_YEARS.PERIOD_YEAR AS Code" +
                    $", FINS_YEARS.PERIOD_YEAR AS NameAr" +
                    $", FINS_YEARS.PERIOD_YEAR AS NameEn  " +
                    $"  FROM FINS_YEARS  where (FINS_YEARS.PERIOD_YEAR =:pCODE OR :pCODE =0) " +
                    $" and YEAR_V_CODE = '{ auth.User_Act_PH}'";

            }
            if (key == 35) // CUSTOMERS DATA
            {
                string mobile = "";
                if (myJObject != null && myJObject.SelectToken("mobile") != null)
                {
                    mobile = myJObject.SelectToken("mobile").Value<string>();
                }
                search = new { pCODE = 0 };
                query = $"SELECT cust.CUST_SYS_ID   AS CODE," +
                    $"       cust.CUST_NAME_AR AS NAMEAR," +
                    $"       cust.CUST_NAME_EN AS NAMEEN," +
                    $"       cust.CUST_CODE AS PCODE," +
                    $"       cls.FCUC_DISC_PERC AS MAX_DISCOUNT," +
                    $"       acnt.ACC_NO AS ACCOUNT_NO," +
                    $"       acnt.ACC_CODE AS ACCOUNT_CODE," +
                    $"       cust.CUST_ATT_MOBILE," +
                    $"       fn_get_Default_Adress(cust.CUST_SYS_ID) AS ADDRSS_SYS_ID ," +
                    $"       fn_get_CUST_CURR_POS_POINTS(cust.CUST_SYS_ID) as POS_POINTS " +
                    $"  FROM FINS_CUSTOMER cust" +
                    $"       JOIN fins_cust_class cls ON cust.CUST_CLASS_SYS_ID = cls.FCUC_SYS_ID" +
                    $"       LEFT JOIN fins_account acnt ON cust.CUST_ACC_CODE = acnt.ACC_CODE" +
                    $" WHERE (CUST_SYS_ID = :pCODE OR :pCODE = 0) AND cust.CUST_FRZ_Y_N = 'N' ";
                if (mobile.Length >= 1) { query += $" and (cust.CUST_ATT_MOBILE like'%{mobile}%')"; };
            }
            if (key == 36) // FINS_FIXED_ASSET DATA
            {
                search = new { pCODE = 0 };
                query = $"SELECT ASSET_SYS_ID AS Code, ASSET_NAME_EN AS NameAr, ASSET_NAME_AR AS NameEn" +
                    $"  FROM FINS_FIXED_ASSET " +
                    $" WHERE (ASSET_SYS_ID = :pCODE OR :pCODE = 0 ) " +
                    $" and ASSET_SALE_Y_N!='Y' " +
                    $" and ASSET_POSTED_Y_N !='Y'" +
                    $" and ASSET_V_CODE = '{ auth.User_Act_PH}'";
            }
            if (key == 37) ////  FINS_ACCOUNT for account settings   
            {
                search = new { pCODE = 0 };
                query = $"SELECT " +
                    $"FINS_ACCOUNT.ACC_CODE as CODE, FINS_ACCOUNT.ACC_NO as PCODE, " +
                    $"FINS_ACCOUNT.ACC_NO||'-'||FINS_ACCOUNT.ACC_NAME_AR AS NAMEAR, " +
                    $"FINS_ACCOUNT.ACC_NO||'-'||FINS_ACCOUNT.ACC_NAME_En AS NAMEEN, " +
                    $"FINS_ACCOUNT.ACC_CODE, FINS_ACCOUNT.ACC_OLD_NO, FINS_ACCOUNT.ACC_V_CODE " +
                    $"FROM FINS_ACCOUNT " +
                    $"WHERE FINS_ACCOUNT.ACC_STATUS = 'A' " +
                    $"AND FINS_ACCOUNT.ACC_LEVEL_CODE in (2,3) " +
                    $"order by FINS_ACCOUNT.ACC_NO";

            }
            if (key == 38) ////  INV_UOM 
            {
                search = new { pCODE = 0 };
                query = $" SELECT UOM_SYS_ID," +
                    $" UOM_SYS_ID as Code ," +
                    $" UOM_NAME_AR As NameAR," +
                    $" UOM_NAME_EN as NameEn " +
                    $" FROM INV_UOM ";

            }
            if (key == 39) ////  INV_ITEM_GROUP   
            {
                search = new { pCODE = 0 };
                query = $"SELECT IIG_SYS_ID as Code ," +
                    $"       IIG_CODE ||'-'||IIG_NAME_AR As NameAr," +
                    $"       IIG_CODE ||'-'||IIG_NAME_EN as NameEn," +
                    $"       IIG_STK_SRV_S_V ," +
                    $"       IIG_MDCHN_Y_N," +
                    $"       IIG_NEED_MDCHL_DESC_Y_N," +
                    $"       IIG_NEED_AUTH_Y_N" +
                    $"  FROM INV_ITEM_GROUP ";

            }
            if (key == 40) ////  Inventory   
            {
                int OwnerId = 0;
                if (myJObject != null && myJObject.SelectToken("OwnerId") != null)
                {
                    OwnerId = myJObject.SelectToken("OwnerId").Value<int>();
                }
                search = new { pSYS_ID = auth.UserCode };
                //query = $"SELECT IIM_SYS_ID AS CODE, IIM_NAME_AR AS NAMEAR, IIM_NAME_EN AS NAMEEN, IIM_OWNER_SYS_ID AS OWNER_ID, " +
                //    $" IIM_CR_ACCOUNT_CODE AS INV_CR_ACCOUNT, IIM_DR_ACCOUNT_CODE AS INV_DR_ACCOUNT " +
                //    $" FROM INV_INVENTORY_MASTER WHERE (IIM_OWNER_SYS_ID=:pOwnerId or :pOwnerId=0) ";
                query = $"SELECT distinct  IIM_SYS_ID AS CODE, IIM_NAME_AR AS NAMEAR, IIM_NAME_EN AS NAMEEN, IIM_OWNER_SYS_ID AS OWNER_ID, " +
                   $" IIM_CR_ACCOUNT_CODE AS INV_CR_ACCOUNT, IIM_DR_ACCOUNT_CODE AS INV_DR_ACCOUNT " +
              $" FROM USR_INV_VIEW WHERE V_CODE = '{auth.User_Act_PH}'";
            }
            if (key == 41) ////  ITEMS BATCHES   
            {
                int ItemCode = 0;
                int InventoryCode = 0;
                if (myJObject != null && myJObject.SelectToken("ItemCode") != null)
                {
                    ItemCode = myJObject.SelectToken("ItemCode").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("InventoryCode") != null)
                {
                    InventoryCode = myJObject.SelectToken("InventoryCode").Value<int>();
                }
                search = new { pItemCode = ItemCode, PInventoryCode = InventoryCode };
                query = $"SELECT " +
                    $"III_INV_SYS_ID," +
                    $"btch.IMB_SYS_ID AS CODE  ," +
                    $"(btch.IMB_BATCH_CODE || '_' || ItemMaster.ITEM_SHORT_NAME_AR) AS NAMEAR," +
                    $" ( btch.IMB_BATCH_CODE || '_' || ItemMaster.ITEM_SHORT_NAME_En) AS NAMEEN," +
                    $"  IMB_ITEM_SYS_ID," +
                    $"  IMB_BATCH_CODE," +
                    $"  IMB_BARCODE," +
                    $"  IMB_PROD_DATE," +
                    $"  IMB_EXPR_DATE," +
                    $"  IIB_OP_QTY," +
                    $"  IIB_OP_UNIT_AMOUNT," +
                    $"  IIB_UOM_SYS_ID," +
                    $"  IIB_CURR_STK  as STK_QTY ," +
                    $" round(nvl(fn__item_btch_curr_stk(III_INV_SYS_ID, IMB_ITEM_SYS_ID, IMB_SYS_ID, IIB_UOM_SYS_ID),0)) AS batch_curr_stk," +
                    $" round(nvl(fn__item_btch_curr_stk(III_INV_SYS_ID, IMB_ITEM_SYS_ID, IMB_SYS_ID, fn_get_ITEM_BASIC_UOM(IMB_ITEM_SYS_ID) ),0)) AS batch_curr_stk_basic_uom" +
                    $" FROM INV_ITEM_MASTER_BATCHES btch ,INV_ITEM_BATCHES ItemBtch, INV_INV_ITEMS iii,INV_ITEM_MASTER ItemMaster," +
                    $" INV_INVENTORY_MASTER IIM" +
                    $" WHERE ItemBtch.IIB_BATCH_SYS_ID = btch.IMB_SYS_ID" +
                    $" AND ItemBtch.IIB_III_SYS_ID = iii.III_SYS_ID" +
                    $" and ItemMaster.ITEM_SYS_ID = btch.IMB_ITEM_SYS_ID" +
                    $" and IIM.IIM_SYS_ID = iii.III_INV_SYS_ID " +
                    ////////////////////////////////
                    $" and (iii.III_ITEM_SYS_ID = :pItemCode or: pItemCode = 0)";
                // $"  and IIM.IIM_V_CODE ='{auth.User_Act_PH}'";
                if (InventoryCode > 0) { query += $"  and (iii.III_INV_SYS_ID = :PInventoryCode )  "; }

            }
            if (key == 42)
            {
                search = new { pCode = 0 };
                query = $"SELECT USR_CODE AS CODE, USR_FULL_NAME_AR AS NAMEAR, USR_FULL_NAME_EN AS NAMEEN FROM GAS_USR WHERE USR_FRZ_Y_N = 'N'";
            }

            if (key == 43)
            {
                int OwnerCode = 0;
                if (myJObject != null && myJObject.SelectToken("OwnerCode") != null)
                {
                    OwnerCode = myJObject.SelectToken("OwnerCode").Value<int>();
                }
                search = new { pOwnerCode = OwnerCode };
                query = $"select mstr.ITM_SYS_ID AS CODE, (inv.IIM_NAME_AR || ' - ' || 'رقم التحويل: ' || mstr.ITM_CODE) AS NAMEAR, mstr.ITM_DESC, " +
                    $" (inv.IIM_NAME_EN || ' - ' || 'Transfer No.: ' || mstr.ITM_CODE) AS NAMEEN, mstr.ITM_TO_INV_SYS_ID, mstr.ITM_FRM_INV_SYS_ID, mstr.ITM_OTHER_CHARGES_AMT, mstr.ITM_OTHER_CHARGES_DESC " +
                    $" from INV_TRANFER_MSTR mstr join INV_INVENTORY_MASTER inv on inv.IIM_SYS_ID = mstr.ITM_FRM_INV_SYS_ID " +
                    $" where FUN_GET_PARENT_V_CODE(mstr.ITM_V_CODE) = FUN_GET_PARENT_V_CODE('{auth.User_Act_PH}') and mstr.ITM_APPROVED_Y_N = 'Y' " +
                    $" AND mstr.ITM_SYS_ID NOT IN (SELECT ITM_RELATED_SYS_ID FROM INV_TRANFER_MSTR WHERE ITM_TYPE_LTO_LTI = 'LTI' AND ITM_APPROVED_Y_N = 'Y') " +
                    $"and mstr.ITM_TYPE_LTO_LTI = 'LTO' and mstr.ITM_OWNER_SYS_ID = :pOwnerCode";
            }

            if (key == 44)
            {
                search = new { pCode = 0 };
                query = $"SELECT DISTINCT V_USR_CODE AS CODE, V_USER_NAME_AR AS NAMEAR, V_USER_NAME_EN AS NAMEEN FROM USR_ACTV_VIEW WHERE PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{auth.User_Act_PH}')";
            }

            if (key == 45)
            {
                search = new { pCode = 0 };
                query = $"SELECT ITRH_SYS_ID AS CODE, ITRH_RQSTR_INV_SYS_ID, ITRH_RQSTD_INV_SYS_ID,  ITRH_DESC, ITRH_OWNER_SYS_ID, " +
                    $" ('طلب تحويل رقم ' || ITRH_CODE || ' - بتاريخ ' || ITRH_DATE) AS NAMEAR, ('Transfer Request No' || ITRH_CODE ||' with date ' || ' - ' || ITRH_DATE) AS NAMEEN " +
                    $" FROM (SELECT rqst.*, rqstr.IIM_NAME_AR AS Stock_To_Ar, rqstr.IIM_NAME_EN AS Stock_To_En, rqstd.IIM_NAME_AR AS Stock_From_Ar, rqstd.IIM_NAME_EN AS Stock_From_En, " +
                    $" ownr.OWNER_NAME_AR, ownr.OWNER_NAME_EN, rqstr.IIM_V_CODE rqstr_v_code, rqstd.IIM_V_CODE rqstd_v_code " +
                    $" FROM INV_TRNSR_REQST_HDR  rqst, INV_INVENTORY_MASTER rqstr, INV_INVENTORY_MASTER rqstd, GAS_OWNER ownr " +
                    $" WHERE rqst.ITRH_RQSTR_INV_SYS_ID = rqstr.IIM_SYS_ID AND rqst.ITRH_RQSTd_INV_SYS_ID = rqstd.IIM_SYS_ID AND ownr.OWNER_SYS_ID = rqst.ITRH_OWNER_SYS_ID " +
                    $" AND rqstr.IIM_V_CODE = '{auth.User_Act_PH}' " +
                    $" UNION ALL " +
                    $" SELECT rqst.*, rqstr.IIM_NAME_AR AS Stock_To_Ar, rqstr.IIM_NAME_EN AS Stock_To_En, rqstd.IIM_NAME_AR AS Stock_From_Ar, rqstd.IIM_NAME_EN AS Stock_From_En, " +
                    $" ownr.OWNER_NAME_AR, ownr.OWNER_NAME_EN, rqstr.IIM_V_CODE rqstr_v_code, rqstd.IIM_V_CODE rqstd_v_code " +
                    $" FROM INV_TRNSR_REQST_HDR  rqst, INV_INVENTORY_MASTER rqstr, INV_INVENTORY_MASTER rqstd, GAS_OWNER ownr " +
                    $" WHERE rqst.ITRH_RQSTR_INV_SYS_ID = rqstr.IIM_SYS_ID AND rqst.ITRH_RQSTd_INV_SYS_ID = rqstd.IIM_SYS_ID AND ownr.OWNER_SYS_ID = rqst.ITRH_OWNER_SYS_ID " +
                    $" AND rqstd.IIM_V_CODE = '{auth.User_Act_PH}' AND rqst.ITRH_APPROVED_Y_N = 'Y') RQSTS " +
                    $" WHERE (RQSTS.ITRH_SYS_ID = :pCode OR :pCode = 0) ";
            }
            if (key == 46)
            {
                search = new { pCode = 0 };
                query = $"SELECT IPRH_SYS_ID AS CODE, ('طلب مشتريات رقم ' || IPRH_CODE || ' - بتاريخ  ' || IPRH_DATE) AS NAMEAR, " +
                    $" ('Purchase Request No ' || IPRH_CODE || ' with date ' || ' - ' || IPRH_DATE) AS NAMEEN, " +
                    $" IPRH_DATE, IPRH_REQ_DELVRY_DATE, IPRH_DESC, IPRH_MIN_QTY_Y_N, IPRH_REORDER_Y_N " +
                    $" FROM INV_PRCH_REQST_HDR " +
                    $" WHERE IPRH_V_CODE = '{auth.User_Act_PH}' AND IPRH_OWNR_APPROVED_Y_N = 'Y' AND FN_CHK_PRCH_RQST_N_P_C(IPRH_SYS_ID) IN ('N', 'P') ";
            }
            if (key == 47)
            {
                search = new { pCode = 0 };
                query = $"SELECT IPOH_SYS_ID AS CODE,  ('أمر شراء رقم ' || IPOH_CODE ||' - بتاريخ '|| IPOH_DATE)   AS NAMEAR, " +
                    $" ('Purchase Order No ' || IPOH_CODE || ' with date '||' - '|| IPOH_DATE)   AS NAMEEN, IPOH_DATE, IPOH_SUPP_SYS_ID " +
                    $" FROM INV_PRCH_ORDR_HDR  WHERE IPOH_V_CODE = '{auth.User_Act_PH}' AND IPOH_APPRVD_Y_N = 'Y'";
            }
            if (key == 48)
            {
                search = new { pCode = 0 };
                query = $"SELECT INVH_SYS_ID AS CODE, ('Ivoice No' || INVH_CODE || ' With Date ' || INVH_DATE) AS NAMEAR, " +
                    $" ('Ivoice No' || INVH_CODE || ' With Date ' || INVH_DATE) AS NAMEEN, " +
                    $" P_INVOICE_HEAD.* " +
                    $" FROM P_INVOICE_HEAD WHERE INVH_V_CODE='{auth.User_Act_PH}' " +
                    $" order by INVH_SYS_ID DESC ";
            }
            if (key == 49) // item MANUFACTURERS
            {
                search = new { pCode = 0 };
                query = $"SELECT IIMF_SYS_ID AS CODE, IIMF_CODE ||'-'||IIMF_NAME_AR AS NAMEAR,  IIMF_CODE ||'-'||IIMF_NAME_EN AS NAMEEN FROM INV_ITEM_MANUFACTURER";
            }

            if (key == 50) // item DOSAGE
            {
                search = new { pCode = 0 };
                query = $"SELECT IIDF_SYS_ID AS CODE, IIDF_CODE ||'-'||IIDF_NAME_AR AS NAMEAR,  IIDF_CODE ||'-'||IIDF_NAME_EN AS NAMEEN FROM INV_ITEM_DOSAGE_FORM";
            }
            if (key == 51)
            {
                search = new { pCode = 0 };
                query = $"SELECT INVSH_SYS_ID AS CODE, ('Sales Ivoice No' || INVSH_CODE || ' With Date ' || INVSH_DATE) AS NAMEAR," +
                    $"('Sales Ivoice No' || INVSH_CODE || ' With Date ' || INVSH_DATE) AS NAMEEN," +
                    $"  S_INVOICE_HEAD.*" +
                    $"  FROM S_INVOICE_HEAD WHERE INVSH_V_CODE='{auth.User_Act_PH}' " +
                    $"order by INVSH_SYS_ID DESC ";
            }

            if (key == 52)
            {
                string NotInSDN = "";
                if (myJObject != null && myJObject.SelectToken("NotInSDN") != null)
                {
                    NotInSDN = myJObject.SelectToken("NotInSDN").Value<string>();
                }
                string NotInSRO = "";
                if (myJObject != null && myJObject.SelectToken("NotInSRO") != null)
                {
                    NotInSRO = myJObject.SelectToken("NotInSRO").Value<string>();
                }

                search = new { pCode = 0, NotInSDN = NotInSDN, NotInSRO = NotInSRO };
                query = $"SELECT ordr.SOH_SYS_ID AS CODE, ordr.SOH_DISCOUNT_AMT, ordr.SOH_DISCOUNT_PCT, " +
                    $" ('أمر بيع رقم ' || ordr.SOH_CODE || ' بتاريخ ' || ordr.SOH_DATE) AS NAMEAR, " +
                    $" ('Sales Order No. ' || ordr.SOH_CODE || ' With Date ' || ordr.SOH_DATE) AS NAMEEN " +
                    $" FROM SALES_ORDER_HDR ordr " +
                    $" WHERE (ordr.SOH_SYS_ID = :pCode OR :pCode = 0)  ";
                if (NotInSDN == "Y".ToString()) { query += $" AND ordr.SOH_SYS_ID NOT IN (SELECT ISDH_SOH_SYS_ID FROM INV_SALES_DN_HDR)"; }
                if (NotInSRO == "Y".ToString()) { query += $" AND ordr.SOH_SYS_ID NOT IN (SELECT SROH_SOH_SYS_ID FROM SALES_RTRN_ORDER_HDR)"; }
                query += $" AND ordr.SOH_V_CODE = '{auth.User_Act_PH}'";
            }

            if (key == 53)
            {
                search = new { pCode = 0 };
                query = $"SELECT qth.SQH_SYS_ID AS CODE, qth.SQH_DISCOUNT_AMT, qth.SQH_DISCOUNT_PCT, qth.SQH_NOTE, qth.SQH_TO_OWNER_CUST_O_C, qth.SQH_OWNER_CUST_SYS_ID, " +
                    $" ('عرض أسعار رقم ' || qth.SQH_CODE || ' بتاريخ ' || qth.SQH_DATE) AS NAMEAR, " +
                    $" ('Quotation No. ' || qth.SQH_CODE || ' With Date ' || qth.SQH_DATE)  AS NAMEEN " +
                    $" FROM SALES_QUOTATION_HDR qth " +
                    $" WHERE qth.SQH_V_CODE = '{auth.User_Act_PH}' AND qth.SQH_SYS_ID NOT IN (SELECT SOH_SQH_SYS_ID FROM SALES_ORDER_HDR WHERE SOH_V_CODE = 'AC1' AND SOH_APPROVED_Y_N = 'Y' AND SOH_PULLED_FROM_D_Q = 'Q')";
            }

            if (key == 54)
            {
                string NotInSRDN = "";
                if (myJObject != null && myJObject.SelectToken("NotInSRDN") != null)
                {
                    NotInSRDN = myJObject.SelectToken("NotInSRDN").Value<string>();
                }
                search = new { pCode = 0 };
                query = $"SELECT ordr.SROH_SYS_ID AS CODE, ordr.SROH_DISCOUNT_AMT, ordr.SROH_DISCOUNT_PCT, " +
                    $" ('أمر إرتجاع رقم ' || ordr.SROH_CODE || ' بتاريخ ' || ordr.SROH_DATE) AS NAMEAR, " +
                    $" ('Order Return No. ' || ordr.SROH_CODE || ' With Date ' || ordr.SROH_DATE) AS NAMEEN " +
                    $" FROM SALES_RTRN_ORDER_HDR ordr " +
                    $" WHERE (ordr.SROH_SYS_ID = :pCode OR :pCode = 0) ";
                if (NotInSRDN == "Y".ToString()) { query += $" and (ordr.SROH_SYS_ID not in (select distinct ISRDH.ISRDH_SROH_SYS_ID from INV_SALES_RTRN_DN_HDR ISRDH  ))"; }
                query += $" AND ordr.SROH_V_CODE = '{auth.User_Act_PH}'";
            }
            if (key == 55)
            {
                search = new { pCode = 0 };
                query = $"  SELECT SQH.SQH_SYS_ID AS CODE, SQH.SQH_DISCOUNT_AMT, SQH.SQH_DISCOUNT_PCT, " +
                    $"                     ('عرض رقم  ' || SQH.SQH_CODE || ' بتاريخ ' || SQH.SQH_DATE) AS NAMEAR," +
                    $"                     ('Sales Quotation No. ' || SQH.SQH_CODE || ' With Date ' || SQH.SQH_DATE) AS NAMEEN" +
                    $"                     FROM SALES_QUOTATION_HDR SQH  " +
                    $" WHERE (SQH.SQH_SYS_ID = :pCode OR :pCode = 0) AND SQH.SQH_V_CODE = '{auth.User_Act_PH}' " +
                    $"order by  SQH.SQH_SYS_ID desc";
            }
            if (key == 57)
            {
                search = new { pCode = 0 };
                query = $"SELECT IPROH.IPROH_SYS_ID AS CODE, " +
                    $"                     ('أمر إرتجاع رقم ' || IPROH.IPROH_CODE || ' بتاريخ ' || IPROH.IPROH_DATE) AS NAMEAR," +
                    $"                     ('Order Return No. ' || IPROH.IPROH_CODE || ' With Date ' || IPROH.IPROH_DATE) AS NAMEEN" +
                    $"                     FROM INV_PRCH_RETURN_ORDR_HDR IPROH " +
                    $"                     WHERE(IPROH.IPROH_SYS_ID = :pCode OR: pCode = 0) and IPROH.IPROH_APPRVD_Y_N='Y' AND IPROH.IPROH_V_CODE = '{auth.User_Act_PH}'";
            }
            if (key == 58) // Items By  Invoice and Order 
            {

                int PurchaseInvoiceId = 0;
                int SalesInvoiceId = 0;
                int PurchaseOrderId = 0;
                int PurchaseRtrnOrderId = 0;
                int SalesOrderId = 0;
                int SalesRtrnOrderId = 0;
                if (myJObject != null && myJObject.SelectToken("PurchaseInvoiceId") != null)
                {
                    PurchaseInvoiceId = myJObject.SelectToken("PurchaseInvoiceId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("SalesInvoiceId") != null)
                {
                    SalesInvoiceId = myJObject.SelectToken("SalesInvoiceId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("PurchaseOrderId") != null)
                {
                    PurchaseOrderId = myJObject.SelectToken("PurchaseOrderId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("PurchaseRtrnOrderId") != null)
                {
                    PurchaseRtrnOrderId = myJObject.SelectToken("PurchaseRtrnOrderId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("SalesOrderId") != null)
                {
                    SalesOrderId = myJObject.SelectToken("SalesOrderId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("SalesRtrnOrderId") != null)
                {
                    SalesRtrnOrderId = myJObject.SelectToken("SalesRtrnOrderId").Value<int>();
                }
                search = new
                {
                    pInvoiceId = PurchaseInvoiceId,
                    sInvoiceId = SalesInvoiceId,
                    POrderId = PurchaseOrderId,
                    PROrderId = PurchaseRtrnOrderId,
                    SOrderId = SalesOrderId,
                    SROrderId = SalesRtrnOrderId
                };

                query = $"SELECT ITEM.ITEM_SYS_ID  AS CODE," +
                    $"       ITEM.ITEM_SYS_ID AS PCODE," +
                    $"       ITEM.ITEM_NAME_AR AS NAMEAR," +
                    $"       ITEM.ITEM_NAME_EN AS NAMEEN," +
                    $"       ITEM.ITEM_IIG_SYS_ID," +
                    $"       ITEM.ITEM_FRZ_Y_N," +
                    $"       ITEM.ITEM_BTCH_Y_N," +
                    $"       NVL(fn_get_item_lpur_price(ITEM.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {auth.UserCurrency}), 0) as ITEM_LAST_PUR_PRICE, " +
                    $"       NVL(fn_get_item_sale_price(ITEM.ITEM_SYS_ID, ITEM.ITEM_UOM_SYS_ID, 1, {auth.UserCurrency}), 0) AS ITEM_SALE_PRICE, " +
                    $"       ITEM.ITEM_UOM_SYS_ID," +
                    $"       ITEM.ITEM_SUPP_SYS_ID," +
                    $"       ITEM.ITEM_SALE_Y_N," +
                    $"       UNIT.UOM_NAME_AR," +
                    $"       UNIT.UOM_NAME_EN,";
                if (PurchaseInvoiceId > 0)
                {
                    query += $"PIItem.INVI_ITEM_DISCOUNT_AMT," +
                        $"       PIItem.INVI_ITEM_DISCOUNT_PCT," +
                        $"       PIItem.INVI_ITEM_QTY," +
                        $"       PIItem.INVI_ITEM_UNIT_PRICE," +
                        $"       PIItem.INVI_ITEM_UOM_SYS_ID, ";
                };
                if (SalesInvoiceId > 0)
                {
                    query += $"SIItem.INVSI_ITEM_DISCOUNT_AMT," +
                        $"       SIItem.INVSI_ITEM_DISCOUNT_PCT," +
                        $"       SIItem.INVSI_ITEM_QTY," +
                        $"       SIItem.INVSI_ITEM_UNIT_PRICE," +
                        $"       SIItem.INVSI_ITEM_UOM_SYS_ID,";
                };
                if (PurchaseOrderId > 0)
                {
                    query += $"ipod.IPOD_DISCOUNT_AMNT," +
                        $"       ipod.IPOD_DISCOUNT_PERC," +
                        $"       ipod.IPOD_QTY," +
                        $"       ipod.IPOD_PRCH_PRICE," +
                        $"       ipod.IPOD_UOM_SYS_ID,";
                };
                if (PurchaseRtrnOrderId > 0)
                {
                    query += $" IPROD.IPROD_DISCOUNT_AMNT," +
                        $"       IPROD.IPROD_DISCOUNT_PERC," +
                        $"       IPROD.IPROD_QTY," +
                        $"       IPROD.IPROD_PRCH_PRICE," +
                        $"       IPROD.IPROD_UOM_SYS_ID,";
                };
                query += $"       IIG.IIG_STK_SRV_S_V" +
                    $"  FROM INV_ITEM_MASTER ITEM" +
                    $"       JOIN INV_UOM UNIT ON ITEM.ITEM_UOM_SYS_ID = UNIT.UOM_SYS_ID" +
                    $"       left outer join INV_ITEM_GROUP IIG on ITEM.ITEM_IIG_SYS_ID = IIG.IIG_SYS_ID ";
                if (PurchaseInvoiceId > 0)
                {
                    query += $"inner join P_INVOICE_ITEM PIItem on ITEM.ITEM_SYS_ID= PIItem.INVI_ITEM_SYS_ID " +
                             $"inner join P_INVOICE_HEAD PIHEAD on PIHEAD.INVH_SYS_ID = PIItem.INVI_INVH_SYS_ID";
                }
                if (SalesInvoiceId > 0)
                {
                    query += $"INNER JOIN S_INVOICE_ITEM SIItem ON ITEM.ITEM_SYS_ID = SIItem.INVSI_ITEM_SYS_ID" +
                        $"       INNER JOIN s_INVOICE_HEAD SIHEAD ON SIHEAD.INVSH_SYS_ID = SIItem.INVSI_INVSH_SYS_ID";
                }
                if (PurchaseOrderId > 0)
                {
                    query += $"inner  JOIN   INV_PRCH_ORDR_DTL ipod ON ipod.IPOD_ITEM_SYS_ID = ITEM.ITEM_SYS_ID ";
                }
                if (PurchaseRtrnOrderId > 0)
                {
                    query += $"inner JOIN  INV_PRCH_RETURN_ORDR_DTL IPROD ON IPROD.IPROD_ITEM_SYS_ID = ITEM.ITEM_SYS_ID ";
                }
                if (SalesOrderId > 0)
                {
                    query += $" INNER JOIN SALES_ORDER_DTL SOD ON ITEM.ITEM_SYS_ID = SOD.SOD_ITEM_SYS_ID ";
                }

                if (SalesRtrnOrderId > 0)
                {
                    query += $" INNER JOIN SALES_RTRN_ORDER_DTL SROD ON ITEM.ITEM_SYS_ID = SROD.SROD_ITEM_SYS_ID ";
                }
                query += $" where 1=1 ";
                if (PurchaseInvoiceId > 0) { query += $" AND  (PIItem.INVI_INVH_SYS_ID =:pInvoiceId)"; }
                if (SalesInvoiceId > 0) { query += $" AND (SIItem.INVSI_INVSH_SYS_ID = :sInvoiceId)"; }
                if (PurchaseOrderId > 0) { query += $" and (ipod.IPOD_IPOH_SYS_ID=:POrderId )"; }
                if (PurchaseRtrnOrderId > 0) { query += $" and (IPROD.IPROD_IPROH_SYS_ID=:PROrderId)"; }
                if (SalesOrderId > 0) { query += $" AND (SOD.SOD_SOH_SYS_ID = :SOrderId) "; }
                if (SalesRtrnOrderId > 0) { query += $" AND (SROD.SROD_SROH_SYS_ID = :SROrderId)"; }

            }


            if (key == 56)
            {
                search = new { pCode = 0 };
                query = $"SELECT IPOH_SYS_ID AS CODE,  ('أمر شراء رقم ' || IPOH_CODE ||' - بتاريخ '|| IPOH_DATE)   AS NAMEAR, " +
                    $" ('Purchase Order No ' || IPOH_CODE || ' with date '||' - '|| IPOH_DATE)   AS NAMEEN, IPOH_DATE, IPOH_SUPP_SYS_ID " +
                    $" FROM INV_PRCH_ORDR_HDR  WHERE IPOH_V_CODE = '{auth.User_Act_PH}' AND IPOH_APPRVD_Y_N = 'Y' " +
                    $" AND IPOH_SYS_ID NOT IN (SELECT INVH_PO_SYS_ID FROM P_INVOICE_HEAD WHERE INVH_PULLED_DT_PO = 'PO' AND INVH_POSTED_Y_N = 'Y') AND (SELECT COUNT (*) FROM INV_PRCH_ORDR_DTL WHERE IPOD_IPOH_SYS_ID = IPOH_SYS_ID) > 0";
            }

            if (key == 59)
            {
                int ItemCode = 0;
                if (myJObject != null && myJObject.SelectToken("ItemCode") != null)
                {
                    ItemCode = myJObject.SelectToken("ItemCode").Value<int>();
                }
                search = new { pItemCode = ItemCode };
                query = $"SELECT IMB_SYS_ID AS CODE, ('باتش ' || IMB_BATCH_CODE || ' ينتهي فى ' || to_char(IMB_EXPR_DATE, 'dd/MM/yyy')) AS NAMEAR," +
                    $" ('Batch ' || IMB_BATCH_CODE || ' expire on ' || to_char(IMB_EXPR_DATE, 'dd/MM/yyy')) AS NAMEEN, IMB_BATCH_CODE, IMB_PROD_DATE, IMB_EXPR_DATE, IMB_BARCODE " +
                    $"FROM INV_ITEM_MASTER_BATCHES WHERE IMB_EXPR_DATE IS NOT NULL AND IMB_ITEM_SYS_ID = :pItemCode ";
            }

            if (key == 60)
            {
                search = new { pCODE = 0 };
                query = $"SELECT DISTINCT V_CODE AS CODE, V_CODE AS PCODE, V_DESC_NAME_AR AS NAMEAR, V_DESC_NAME_EN AS NAMEEN,V_FRZ_Y_N, V_PARENT_FRZ_Y_N FROM ACTV_PHRM_VIEW WHERE V_FRZ_Y_N = 'N' AND V_PARENT_FRZ_Y_N = 'N'";
            }
            if (key == 61)
            {
                search = new { pCODE = 0 };
                query = $"SELECT GRP_SYS_ID as CODE," +
                    $"       GRP_NAME_AR as NameAr," +
                    $"       GRP_NAME_en as NameEn," +
                    $"       GRP_TYPE_N_T_D_L," +
                    $"       GRP_APPCODE" +
                    $"  FROM GAS_GNRL_REPORTS_PARM" +
                    $"  where(GRP_SYS_ID =:pCODE or: pCODE = 0)" +
                    $"  order by GRP_SYS_ID";
            }

            //if (key == 62) // SUPPLIERS DATA Without V_Code ==> for items
            //{
            //    search = new { pCODE = 0 };
            //    query = $"SELECT supp.SUPP_SYS_ID AS CODE, supp.SUPP_NAME_AR AS NAMEAR, supp.SUPP_NAME_EN AS NAMEEN, supp.SUPP_CODE AS PCODE, cls.FSUC_DISC_PERC AS MAX_DISCOUNT, " +
            //        $"acnt.ACC_NO AS ACCOUNT_NO, acnt.ACC_CODE AS ACCOUNT_CODE FROM FINS_SUPPLIER supp " +
            //        $"JOIN fins_supp_class cls ON supp.SUPP_CLASS_SYS_ID = cls.FSUC_SYS_ID " +
            //        $"JOIN fins_account acnt ON supp.SUPP_ACC_CODE = acnt.ACC_CODE " +
            //        $"WHERE (SUPP_SYS_ID = :pCODE OR :pCODE = 0) AND supp.SUPP_FRZ_Y_N = 'N'";
            //}

            if (key == 63) // امر بيع بعد تاكيد اخراجه
            {
                search = new { pCODE = 0 };
                query = $"SELECT ordr.SOH_SYS_ID AS CODE, ordr.SOH_DISCOUNT_AMT, ordr.SOH_DISCOUNT_PCT, " +
                   $" ('أمر بيع رقم ' || ordr.SOH_CODE || ' بتاريخ ' || ordr.SOH_DATE) AS NAMEAR, " +
                   $" ('Sales Order No. ' || ordr.SOH_CODE || ' With Date ' || ordr.SOH_DATE) AS NAMEEN " +
                   $" FROM SALES_ORDER_HDR ordr " +
                   $" WHERE (ordr.SOH_SYS_ID = :pCode OR :pCode = 0) AND ordr.SOH_APPROVED_Y_N = 'Y'  AND ordr.SOH_V_CODE = '{auth.User_Act_PH}' " +
                   $" AND ordr.SOH_SYS_ID IN (SELECT ISDH_SOH_SYS_ID FROM INV_SALES_DN_HDR) " +
                   $" AND ordr.SOH_SYS_ID NOT IN (SELECT SROH_SOH_SYS_ID FROM SALES_RTRN_ORDER_HDR where SROH_APPROVED_Y_N = 'Y') ";
            }
            if (key == 64) // FINS_OWNER_suppLIER
            {
                search = new { pCODE = 0 };
                query = $"SELECT FOS.*," +
                    $"       DECODE (FOS_OWN_CMP,  'OWN', 'OW',  'CMP', 'CM') || FOS.FOS_OWNER_COMP_SYS_ID        AS OWNER_V_CODE," +
                    $"       FOS.FOS_SYS_ID AS CODE," +
                    $"       FOS.FOS_NAME_AR AS NAMEAR," +
                    $"       FOS.FOS_NAME_EN AS NAMEEN," +
                    $"       FOS.FOS_CODE AS PCODE," +
                    $"       GET_ACC_NO_FROM_CODE(FOS_ACC_CODE) AS ACCOUNT_NO," +
                    $"       FOS_ACC_CODE                        AS ACCOUNT_CODE" +
                    $"  FROM FINS_OWNER_suppLIER FOS" +
                    $" WHERE(FOS_SYS_ID = :pCODE OR: pCODE = 0)" +
                    $"       AND FOS.FOS_FRZ_Y_N = 'N'" +
                    $"       AND DECODE (FOS_OWN_CMP,  'OWN', 'OW',  'CMP', 'CM') || FOS.FOS_OWNER_COMP_SYS_ID = FUN_GET_PARENT_V_CODE('{auth.User_Act_PH}')";
            }
            if (key == 65) // USER ROLES
            {
                search = new { pCODE = 0 };
                query = $"SELECT GUR_SYS_ID AS CODE, GUR_DESC_AR AS NAMEAR, GUR_DESC_EN AS NAMEEN FROM GAS_USR_ROLE WHERE (GUR_SYS_ID = :pCode OR :pCode = 0) ";
            }
            if (key == 66) // l_ARCHIVE_HEAD
            {
                search = new { pCODE = 0 };
                query = $"SELECT l_ARCHIVE_HEAD.AH_NAME_AR AS NameAr," +
                    $"       l_ARCHIVE_HEAD.AH_NAME_En AS NameEn," +
                    $"       l_ARCHIVE_HEAD.AH_SYS_ID as Code" +
                    $"  FROM l_ARCHIVE_HEAD ";
            }

            if (key == 67)
            {
                int archiveHeadId = 0;
                if (myJObject != null && myJObject.SelectToken("ArchiveHeadId") != null)
                {
                    archiveHeadId = myJObject.SelectToken("ArchiveHeadId").Value<int>();
                }
                search = new { pCODE = archiveHeadId };
                query = $"SELECT AD_NAME_AR AS NameAr, AD_NAME_EN AS NameEn, AD_CODE AS Code FROM l_ARCHIVE_DETAIL WHERE (AD_AH_CODE = :pCode OR :pCode = 0)";
            }


            if (key == 68) // Insurance Companies
            {
                search = new { pCODE = 0 };
                query = $"SELECT PIC_SYS_ID AS CODE, PIC_NAME_AR AS NAMEAR, PIC_NAME_EN AS NAMEEN FROM POS_INSURANCE_CMP WHERE (PIC_SYS_ID = :pCode OR :pCode = 0) ";
            }
            if (key == 69) // Company Contracts
            {
                int CompanyId = 0, CustomerId = 0;
                if (myJObject != null && myJObject.SelectToken("CompanyId") != null)
                {
                    CompanyId = myJObject.SelectToken("CompanyId").Value<int>();
                }
                if (myJObject != null && myJObject.SelectToken("CustomerId") != null)
                {
                    CustomerId = myJObject.SelectToken("CustomerId").Value<int>();
                }
                search = new { pCompany = CompanyId, pCustomer = CustomerId };
                query = $"SELECT PICNT_SYS_ID AS CODE, PICNT_CUST_SYS_ID, PICNT_START_DATE, PICNT_END_DATE, PICNT_DISCOUNT_PCT, PICNT_DEDUCT_PCT, PICNT_MAX_DEDUCT_VAL, PICNT_UNAPPROV_LIMIT_VAL, " +
                    $" ('عقد رقم #'|| PICNT_CONTRACT_NO || ' ينتهي بتاريخ '|| to_char(PICNT_END_DATE, 'yyyy/mm/dd')) AS NAMEAR, " +
                    $" ('Contract No #'|| PICNT_CONTRACT_NO || ' With End Date '|| to_char(PICNT_END_DATE, 'yyyy/mm/dd')) AS NAMEEN " +
                    $" FROM POS_INSURANCE_CONTRACT WHERE (PICNT_PIC_SYS_ID = :pCompany OR :pCompany = 0) AND (PICNT_CUST_SYS_ID = :pCustomer OR :pCustomer = 0)";
            }
            if (key == 70) // Contract Classes
            {
                int ContractId = 0;
                if (myJObject != null && myJObject.SelectToken("ContractId") != null)
                {
                    ContractId = myJObject.SelectToken("ContractId").Value<int>();
                }
                search = new { pCode = ContractId };
                query = $"SELECT PICNTC_SYS_ID AS CODE,GSD_NAME_AR AS NAMEAR,GSD_NAME_EN AS NAMEEN,PICNTC_CLASS_CODE, PICNTC_DISCOUNT_PCT, " +
                    $" PICNTC_DEDUCT_RATIO, PICNTC_MAX_DEDUCT, PICNTC_UNAPPROV_LIMIT " +
                    $" FROM POS_INSURANCE_CONTRACT_CLASS, GAS_GNRL_SET_DTL WHERE PICNTC_CLASS_CODE = GSD_CODE AND GSD_GSH_SYS_ID = 61 " +
                    $" AND (PICNTC_PICNT_SYS_ID = :pCode OR :pCode = 0) ";
            }

            if (key == 71) // batches for Point Of Sale
            {
                int ItemCode = 0;
                if (myJObject != null && myJObject.SelectToken("ItemCode") != null)
                {
                    ItemCode = myJObject.SelectToken("ItemCode").Value<int>();
                }
                search = new { pItemCode = ItemCode };
                query = $"SELECT IMB_SYS_ID AS CODE, ('باتش ' || IMB_BATCH_CODE || ' ينتهي فى ' || to_char(IMB_EXPR_DATE, 'dd/MM/yyy')) AS NAMEAR, IIB_OP_UNIT_AMOUNT AS IMB_ITEM_PRICE," +
                    $" ('Batch ' || IMB_BATCH_CODE || ' expire on ' || to_char(IMB_EXPR_DATE, 'dd/MM/yyy')) AS NAMEEN, IMB_BATCH_CODE, IMB_PROD_DATE, IMB_EXPR_DATE, IMB_BARCODE " +
                    $"FROM INV_ITEM_MASTER_BATCHES, INV_ITEM_BATCHES WHERE IIB_BATCH_SYS_ID = IMB_SYS_ID " +
                    $" AND IMB_EXPR_DATE IS NOT NULL AND IMB_EXPR_DATE > SYSDATE AND IMB_ITEM_SYS_ID = :pItemCode ";
            }
            if (key == 72)
            {
                int StockId = 0;
                if (myJObject != null && myJObject.SelectToken("StockId") != null)
                {
                    StockId = myJObject.SelectToken("StockId").Value<int>();
                }
                search = new { pStockId = StockId };
                query = $"SELECT III_SYS_ID, ITEM_SYS_ID AS CODE, III_INV_SYS_ID, ITEM_CODE, ITEM_NAME_AR AS NAMEAR, ITEM_NAME_EN AS NAMEEN, ITEM_IIG_SYS_ID, ITEM_FRZ_Y_N, ITEM_NOTES, ITEM_UOM_SYS_ID, ITEM_SUPP_SYS_ID, " +
                $" ITEM_SALE_Y_N, ITEM_BTCH_Y_N, ITEM_ASSPLD_Y_N, ITEM_NEED_AUTH_Y_N, ITEM_NEED_MDCHL_DESC_Y_N,  " +
                $" NVL(fn_get_item_sale_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS ITEM_SALE_PRICE, " +
                $" NVL(fn_get_item_lpur_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS ITEM_LAST_PUR_PRICE, " +
                $" NVL(FN_GET_ITEM_VAT_PCT(ITEM_SYS_ID,{OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) ITEM_VAT_PCT,uom_name_ar, uom_name_en " +
                $" FROM inv_inv_items, inv_item_master, inv_uom WHERE III_ITEM_SYS_ID = ITEM_SYS_ID AND item_uom_sys_id = uom_sys_id AND ITEM_SALE_Y_N = 'Y' AND ITEM_FRZ_Y_N <> 'Y' AND III_INV_SYS_ID = :pStockId";
            }
            if (key == 73) ////  Inventory   
            {
                search = new { pSYS_ID = 0 };
                query = $"SELECT IIM_SYS_ID AS CODE, IIM_NAME_AR AS NAMEAR, IIM_NAME_EN AS NAMEEN, IIM_OWNER_SYS_ID AS OWNER_ID, " +
                    $" IIM_CR_ACCOUNT_CODE AS INV_CR_ACCOUNT, IIM_DR_ACCOUNT_CODE AS INV_DR_ACCOUNT " +
                    $" FROM INV_INVENTORY_MASTER WHERE FUN_GET_PARENT_V_CODE(IIM_V_CODE) = FUN_GET_PARENT_V_CODE('{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            }
            if (key == 74)
            {
                search = new { pCode = 0 };
                query = $"SELECT PCH_SYS_ID AS CODE, ('فاتورة رقم #' || PCH_VOUCHER_NO || ' بتاريخ ' || to_char(PCH_DATE, 'dd/MM/yyy')) AS NAMEAR, " +
                    $" ('Invoice No. #' || PCH_VOUCHER_NO || ' With Date ' || to_char(PCH_DATE, 'dd/MM/yyy')) AS NAMEEN " +
                    $" FROM POS_CHASHER_HDR where PCH_V_CODE = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}' ";
            }
            if (key == 75) //customer address
            {
                int Cust_sys_id = 0;
                if (myJObject != null && myJObject.SelectToken("Cust_sys_id") != null)
                {
                    Cust_sys_id = myJObject.SelectToken("Cust_sys_id").Value<int>();
                }
                search = new { pCust_sys_id = Cust_sys_id };
                query = $" SELECT FINS_CUSTOMER_ADDRESSES.FCA_SYS_ID as Code , " +
                    $"                      GAS_REGION.R_NAME_AR || '_' || GAS_COUNTRY.C_NAME_AR || '_' || GAS_CITY.CITY_NAME_AR AS NAMEAR, " +
                    $"                      GAS_REGION.R_NAME_EN || '_' || GAS_COUNTRY.C_NAME_EN || '_' || GAS_CITY.CITY_NAME_EN AS NAMEEN ," +
                    $"                      FINS_CUSTOMER_ADDRESSES.FCA_NEAREST_PHARM_SYS_ID " +
                    $"                 FROM FINS_CUSTOMER_ADDRESSES " +
                    $"                      INNER JOIN GAS_REGION " +
                    $"                         ON FINS_CUSTOMER_ADDRESSES.FCA_REGION_SYS_ID = GAS_REGION.R_SYS_ID" +
                    $"                      INNER JOIN GAS_COUNTRY" +
                    $"                         ON FINS_CUSTOMER_ADDRESSES.FCA_CONTERY_SYS_ID = GAS_COUNTRY.C_SYS_ID" +
                    $"                      INNER JOIN GAS_CITY ON FINS_CUSTOMER_ADDRESSES.FCA_CITY_SYS_ID = GAS_CITY.CITY_SYS_ID" +
                    $"                WHERE (FINS_CUSTOMER_ADDRESSES.FCA_CUST_SYS_ID = { Cust_sys_id} ) and FCA_ACTIVE_Y_N='Y'";
     
            }
            if (key == 76) // Customerrelative
            {
                int Cust_sys_id = 0;
                if (myJObject != null && myJObject.SelectToken("Cust_sys_id") != null)
                {
                    Cust_sys_id = myJObject.SelectToken("Cust_sys_id").Value<int>();
                }
                search = new { pCust_sys_id = Cust_sys_id };
                query = $"SELECT custR.FCR_SYS_ID AS Code," +
                    $"       FCR_SYS_ID || '_' || dtl.GSD_NAME_AR," +
                    $"       FCR_SYS_ID || '_' || dtl.GSD_NAME_EN" +
                    $"  FROM FINS_CUSTOMER_RELATIVES custR" +
                    $"       INNER JOIN GAS_GNRL_SET_DTL dtl" +
                    $"          ON custR.FCR_TYPE_CODE = dtl.GSD_CODE AND dtl.GSD_GSH_SYS_ID = 81" +
                    $" WHERE (custR.FCR_CUST_SYS_ID =   { Cust_sys_id} )";
            }


            if (key == 77) // services items
            {
                search = new { pCode = 0 };
                query = $"SELECT ITEM_SYS_ID AS CODE, ITEM_NAME_AR AS NAMEAR, ITEM_NAME_EN AS NAMEEN, ITEM_UOM_SYS_ID, " +
                    $" NVL(fn_get_item_lpur_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, 7), 0) as ITEM_LAST_PUR_PRICE, " +
                    $" NVL(fn_get_item_sale_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, 7), 0) as ITEM_SALE_PRICE " +
                    $" FROM INV_ITEM_MASTER, INV_ITEM_GROUP WHERE ITEM_IIG_SYS_ID = IIG_SYS_ID AND IIG_STK_SRV_S_V = 'V' ";
            }

            if (key == 78) // CUSTOMERS DATA
            {
                string mobile = "";
                if (myJObject != null && myJObject.SelectToken("mobile") != null)
                {
                    mobile = myJObject.SelectToken("mobile").Value<string>();
                }
                search = new { pCODE = 0 };
                query = $"SELECT cust.CUST_SYS_ID AS CODE, cust.CUST_NAME_AR AS NAMEAR, cust.CUST_NAME_EN AS NAMEEN, cust.CUST_CODE AS PCODE, " +
                    $" cls.FCUC_DISC_PERC AS MAX_DISCOUNT, acnt.ACC_NO AS ACCOUNT_NO, acnt.ACC_CODE AS ACCOUNT_CODE, cust.CUST_ATT_MOBILE, " +
                    $" fn_get_Default_Adress (cust.CUST_SYS_ID) AS ADDRSS_SYS_ID, fn_get_CUST_CURR_POS_POINTS (cust.CUST_SYS_ID) AS POS_POINTS" +
                    $"  FROM FINS_CUSTOMER cust" +
                    $"  JOIN fins_cust_class cls ON cust.CUST_CLASS_SYS_ID = cls.FCUC_SYS_ID " +
                    $"  LEFT JOIN fins_account acnt ON cust.CUST_ACC_CODE = acnt.ACC_CODE " +
                    $" WHERE (CUST_SYS_ID = :pCODE OR :pCODE = 0) AND cust.CUST_FRZ_Y_N = 'N' AND CUST_V_CODE IS NULL ";
            }
            if (key == 79) // Insurance Companies WITH OWNER
            {
                int OwnerId = 0;
                if (myJObject != null && myJObject.SelectToken("OwnerId") != null)
                {
                    OwnerId = myJObject.SelectToken("OwnerId").Value<int>();
                }
                search = new { pCODE = OwnerId };
                query = $"SELECT PIC.PIC_SYS_ID AS CODE, PIC.PIC_NAME_AR AS NAMEAR, PIC.PIC_NAME_EN AS NAMEEN, GOIC.GOIC_ACC_CODE AS ACC_CODE " +
                    $" FROM GAS_OWNER_INS_CO GOIC, POS_INSURANCE_CMP PIC " +
                    $" WHERE GOIC_INS_CO_SYS_ID = PIC_SYS_ID AND GOIC.GOIC_FRZ_Y_N <> 'Y' AND GOIC.GOIC_OWNER_SYS_ID = :pCode";
            }

            if (key == 80) // ITEMS IN PHARMACY
            {
                int PharamcyId = 0;
                if (myJObject != null && myJObject.SelectToken("PharamcyId") != null)
                {
                    PharamcyId = myJObject.SelectToken("PharamcyId").Value<int>();
                }
                search = new { pCODE = PharamcyId };
                //query = $"SELECT * FROM V_ITEMS_OF_PHARMACY WHERE ( PHARMACY_ID = :pCode or :pCode=0 )";
                query = $"SELECT * FROM V_ITEMS ";

            }
            if (key == 81)
            {
                int PharamcyId = 0;
                if (myJObject != null && myJObject.SelectToken("PharamcyId") != null)
                {
                    PharamcyId = myJObject.SelectToken("PharamcyId").Value<int>();
                }
                search = new { PharamcyId = PharamcyId };
                query = $"SELECT SOH.TSOH_SYS_ID AS Code, SOH.TSOH_PH_SYS_ID, SOH.TSOH_CUST_SYS_ID,SOH.TSOH_CURRENT_STATUS, " +
                    $" ('أمر بيع رقم ' || SOH.TSOH_CODE || ' بتاريخ ' || SOH.TSOH_DATE) AS NAMEAR," +
                    $" ('Sales Order No. ' || SOH.TSOH_CODE || ' With Date ' || SOH.TSOH_DATE) AS NAMEEN, " +
                    $" ph.PHARM_NAME_AR, ph.PHARM_NAME_EN, fcust.CUST_NAME_AR, fcust.CUST_NAME_EN" +
                    $"  FROM TKT_SALES_ORDER_HDR SOH, GAS_PHARMACY ph, FINS_CUSTOMER fcust " +
                    $"  where ph.PHARM_SYS_ID = SOH.TSOH_PH_SYS_ID AND fcust.CUST_SYS_ID = SOH.TSOH_CUST_SYS_ID and (SOH.TSOH_PH_SYS_ID=:PharamcyId or :PharamcyId=0)";
            }

            if (key == 82) // shifts list
            {
                search = new { pCode = 0 };
                query = $"SELECT DTL.PSD_SYS_ID AS CODE, ('وردية بين ' || DTL.PSD_SHIFT_START_TIME || ' و ' || DTL.PSD_SHIFT_END_TIME) AS NAMEAR, " +
                    $" ('Shift Between ' || DTL.PSD_SHIFT_START_TIME || ' and ' || DTL.PSD_SHIFT_END_TIME) AS NAMEEN " +
                    $" FROM POS_SHIFTS_SETUP_HDR HDR, POS_SHIFTS_SETUP_DTL DTL WHERE HDR.PSH_SYS_ID = DTL.PSD_PSH_SYS_ID AND 'PH' || HDR.PSH_PHARM_SYS_ID = '{auth.User_Act_PH}'";
            }
            if (key == 83) // CUSTOMERS DATA
            {
                string mobile = "";
                if (myJObject != null && myJObject.SelectToken("mobile") != null)
                {
                    mobile = myJObject.SelectToken("mobile").Value<string>();
                }
                search = new { pCODE = 0 };
                query = $"SELECT cust.CUST_SYS_ID   AS CODE," +
                    $"       cust.CUST_NAME_AR AS NAMEAR," +
                    $"       cust.CUST_NAME_EN AS NAMEEN," +
                    $"       cust.CUST_CODE AS PCODE," +
                    $"       cls.FCUC_DISC_PERC AS MAX_DISCOUNT," +
                    $"       acnt.ACC_NO AS ACCOUNT_NO," +
                    $"       acnt.ACC_CODE AS ACCOUNT_CODE," +
                    $"       cust.CUST_ATT_MOBILE," +
                    $"       fn_get_Default_Adress(cust.CUST_SYS_ID) AS ADDRSS_SYS_ID ," +
                    $"       fn_get_CUST_CURR_POS_POINTS(cust.CUST_SYS_ID) as POS_POINTS " +
                    $"  FROM FINS_CUSTOMER cust" +
                    $"       JOIN fins_cust_class cls ON cust.CUST_CLASS_SYS_ID = cls.FCUC_SYS_ID" +
                    $"       LEFT JOIN fins_account acnt ON cust.CUST_ACC_CODE = acnt.ACC_CODE" +
                    $" WHERE (CUST_SYS_ID = :pCODE OR :pCODE = 0) AND cust.CUST_FRZ_Y_N = 'N' ";
                if (mobile.Length >= 1) { query += $" and (cust.CUST_ATT_MOBILE like'%{mobile}%')"; };
            }

            if (key == 84) // Loyalty Customers
            {
                search = new { pCODE = 0 };
                query = $"SELECT cust.CUST_SYS_ID AS CODE, cust.CUST_NAME_AR AS NAMEAR, cust.CUST_NAME_EN AS NAMEEN, cust.CUST_CODE AS PCODE, " +
                    $" cls.FCUC_DISC_PERC AS MAX_DISCOUNT, acnt.ACC_NO AS ACCOUNT_NO, acnt.ACC_CODE AS ACCOUNT_CODE, cust.CUST_ATT_MOBILE, " +
                    $" fn_get_Default_Adress (cust.CUST_SYS_ID) AS ADDRSS_SYS_ID, fn_get_CUST_CURR_POS_POINTS (cust.CUST_SYS_ID) AS POS_CURR_POINTS, " +
                    $" fn_get_CUST_WORTHY_POS_POINTS_AMNT(cust.CUST_SYS_ID) AS POS_POINTS_AMT " +
                    $" FROM FINS_CUSTOMER cust " +
                    $" JOIN fins_cust_class cls ON cust.CUST_CLASS_SYS_ID = cls.FCUC_SYS_ID " +
                    $" LEFT JOIN fins_account acnt ON cust.CUST_ACC_CODE = acnt.ACC_CODE " +
                    $" WHERE (CUST_SYS_ID = :pCODE OR :pCODE = 0) AND cust.CUST_FRZ_Y_N = 'N' AND CUST_LOYALTY_Y_N = 'Y' ";
            }

            if (key == 85) // company Customers
            {
                search = new { pCODE = 0 };
                query = $"SELECT cust.CUST_SYS_ID AS CODE, cust.CUST_NAME_AR AS NAMEAR, cust.CUST_NAME_EN AS NAMEEN, cust.CUST_CODE AS PCODE, " +
                    $" cls.FCUC_DISC_PERC AS MAX_DISCOUNT, acnt.ACC_NO AS ACCOUNT_NO, acnt.ACC_CODE AS ACCOUNT_CODE, cust.CUST_ATT_MOBILE, " +
                    $" fn_get_Default_Adress (cust.CUST_SYS_ID) AS ADDRSS_SYS_ID, fn_get_CUST_CURR_POS_POINTS (cust.CUST_SYS_ID) AS POS_CURR_POINTS, " +
                    $" fn_get_CUST_WORTHY_POS_POINTS_AMNT(cust.CUST_SYS_ID) AS POS_POINTS_AMT " +
                    $" FROM FINS_CUSTOMER cust " +
                    $" JOIN fins_cust_class cls ON cust.CUST_CLASS_SYS_ID = cls.FCUC_SYS_ID " +
                    $" LEFT JOIN fins_account acnt ON cust.CUST_ACC_CODE = acnt.ACC_CODE " +
                    $" WHERE (CUST_SYS_ID = :pCODE OR :pCODE = 0) AND cust.CUST_FRZ_Y_N = 'N' AND CUST_COMP_OWNR_C_O = 'C' ";
            }

            if (key == 86) // ordinary Customers
            {
                search = new { pCODE = 0 };
                query = $"SELECT cust.CUST_SYS_ID AS CODE, cust.CUST_NAME_AR AS NAMEAR, cust.CUST_NAME_EN AS NAMEEN, cust.CUST_CODE AS PCODE, " +
                    $" cls.FCUC_DISC_PERC AS MAX_DISCOUNT, acnt.ACC_NO AS ACCOUNT_NO, acnt.ACC_CODE AS ACCOUNT_CODE, cust.CUST_ATT_MOBILE, " +
                    $" fn_get_Default_Adress (cust.CUST_SYS_ID) AS ADDRSS_SYS_ID, fn_get_CUST_CURR_POS_POINTS (cust.CUST_SYS_ID) AS POS_CURR_POINTS, " +
                    $" fn_get_CUST_WORTHY_POS_POINTS_AMNT(cust.CUST_SYS_ID) AS POS_POINTS_AMT " +
                    $" FROM FINS_CUSTOMER cust " +
                    $" JOIN fins_cust_class cls ON cust.CUST_CLASS_SYS_ID = cls.FCUC_SYS_ID " +
                    $" LEFT JOIN fins_account acnt ON cust.CUST_ACC_CODE = acnt.ACC_CODE " +
                    $" WHERE (CUST_SYS_ID = :pCODE OR :pCODE = 0) AND cust.CUST_FRZ_Y_N = 'N' AND CUST_LOYALTY_Y_N = 'N' ";
            }
            if(key == 87)
            {
                double latitude = 0;
                double longitude = 0;
                if (myJObject != null && myJObject.SelectToken("latitude") != null && myJObject.SelectToken("longitude") != null)
                {
                    latitude = myJObject.SelectToken("latitude").Value<double>();
                    longitude = myJObject.SelectToken("longitude").Value<double>();
                }
                search = new { pLatitude = latitude, pLongitude = longitude };
                query = $"SELECT PHARM_SYS_ID AS CODE, PHARM_NAME_AR AS NAMEAR, PHARM_NAME_EN AS NAMEEN, PHARM_LOCATION, PHARM_LAT, PHARM_LONG, DISTANCE, DISTANCE_KM, URL FROM TABLE (get_ph_loc2 (:pLatitude, :pLongitude))";
            }
            if(key == 88)
            {
                int itemCode = 0, stockId = 0;
                if (myJObject != null && myJObject.SelectToken("StockId") != null && myJObject.SelectToken("ItemCode") != null)
                {
                    stockId = myJObject.SelectToken("StockId").Value<int>();
                    itemCode = myJObject.SelectToken("ItemCode").Value<int>();
                }
                search = new { pCODE = 0 };
                query = $"SELECT IMB_SYS_ID AS CODE, IMB_BATCH_CODE, IMB_PROD_DATE, IMB_EXPR_DATE, IMB_BARCODE, IIB_OP_UNIT_AMOUNT AS IMB_ITEM_PRICE, " +
                    $" ('باتش ' || IMB_BATCH_CODE || ' ينتهي فى ' || TO_CHAR(IMB_EXPR_DATE, 'dd/MM/yyy')) AS NAMEAR, " +
                    $" ('Batch ' || IMB_BATCH_CODE || ' expire on ' || TO_CHAR(IMB_EXPR_DATE, 'dd/MM/yyy')) AS NAMEEN " +
                    $" FROM INV_ITEM_BATCHES btch, inv_inv_items invm, INV_ITEM_MASTER_BATCHES mbtch " +
                    $" WHERE invm.III_SYS_ID = btch.IIB_III_SYS_ID and btch.IIB_BATCH_SYS_ID = mbtch.IMB_SYS_ID and mbtch.IMB_EXPR_DATE > SYSDATE " +
                    $" AND fn__item_btch_curr_stk(invm.III_INV_SYS_ID, invm.III_ITEM_SYS_ID, btch.IIB_BATCH_SYS_ID, NULL) > 0 " +
                    $" AND invm.III_ITEM_SYS_ID = {itemCode} AND invm.III_INV_SYS_ID = {stockId}";
            }

            if(key == 89)
            {
                search = new { pCODE = 0 };
                query = $"SELECT ITEM_SYS_ID AS CODE, ITEM_CODE, ITEM_NAME_AR AS NAMEAR, ITEM_NAME_EN AS NAMEEN, ITEM_IIG_SYS_ID, ITEM_FRZ_Y_N, ITEM_NOTES, " +
                    $" ITEM_UOM_SYS_ID, ITEM_SUPP_SYS_ID, ITEM_SALE_Y_N, ITEM_BTCH_Y_N, ITEM_ASSPLD_Y_N, ITEM_NEED_AUTH_Y_N, ITEM_NEED_MDCHL_DESC_Y_N, " +
                    $" NVL(fn_get_item_sale_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS ITEM_SALE_PRICE, " +
                    $" NVL (fn_get_item_lpur_price(ITEM_SYS_ID, ITEM_UOM_SYS_ID, 1, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) AS ITEM_LAST_PUR_PRICE, " +
                    $" NVL (FN_GET_ITEM_VAT_PCT(ITEM_SYS_ID, {OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency}), 0) ITEM_VAT_PCT, uom_name_ar, uom_name_en " +
                    $" FROM inv_item_master, inv_uom WHERE item_uom_sys_id = uom_sys_id AND ITEM_SALE_Y_N = 'Y' AND ITEM_FRZ_Y_N<> 'Y'";
            }
            if (key == 90)
            {
                search = new { pCODE = 0 };
                query = $"SELECT HR.HR_DEPARTMENT.HRD_SYS_ID AS CODE," +
                    $"       HR.HR_DEPARTMENT.HRD_NAME_AR AS NAMEAR," +
                    $"       HR.HR_DEPARTMENT.HRD_NAME_EN AS NAMEEN," +
                    $"       HR.HR_DEPARTMENT.HRD_PARENT_SYS_ID AS PARENTCODE" +
                    $"  FROM HR.HR_DEPARTMENT ";
            }
            if (key == 91)
            {
                search = new { pCODE = 0 };
                query = $"SELECT HRJG_SYS_ID AS Code, " +
                    $" HRJG_NAME_AR AS NameAR, " +
                    $" HRJG_NAME_EN AS nameEn " +
                    $" FROM HR_JOBS_GROUPS ";
            }
            if (key == 92)
            {
                search = new { pCODE = 0 };
                query = $"SELECT HRCC_SYS_ID AS Code," +
                    $" HRCC_NAME_AR AS NameAR," +
                    $" HRCC_NAME_EN AS nameEn " +
                    $" FROM HR_COST_CENTERS ";


            }
          

            var res = new List<dynamic>();
            var result = new List<dynamic>();
            if (key == 37)
            {
                string V_Code = "AC";
                if (myJObject != null && myJObject.SelectToken("V_Code") != null)
                {
                    V_Code = myJObject.SelectToken("V_Code").Value<string>();
                }
                result = await OracleDQ.GetDataAsync<dynamic>(query, authParms, search == null ? dyParam : search, commandType);

                var x = result.FindAll(el => el.ACC_V_CODE == V_Code);
                var y = result.FindAll(el => el.ACC_V_CODE == null || el.ACC_V_CODE == "");
                for (int i = 0; i < x.Count; i++)
                {
                    res.Add(x[i]);
                }
                for (int n = 0; n < y.Count; n++)
                {
                    res.Add(y[n]);
                }
            }

            else
                res = await OracleDQ.GetDataAsync<dynamic>(query, authParms, search == null ? dyParam : search, commandType);
            return res;
        }

        public async Task<DataSet> GetDynamicDataByAppCode(ISelectSearch selectSearch, string authParms)
        {
            string json = selectSearch.filterKeyes;
            dynamic myJObject = null;
            if (selectSearch.filterKeyes != null)
            {
                myJObject = JObject.Parse(json);
            }
            return await OracleDQ.ExcuteSelectizeProcAsync("PRC_GET_GENERAL_SELECTIZE", new List<dynamic>() { selectSearch }, authParms);
        }
    }
}
