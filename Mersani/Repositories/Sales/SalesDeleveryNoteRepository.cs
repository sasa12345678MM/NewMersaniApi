using Mersani.Interfaces.Sales;
using Mersani.models.Sales;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Sales
{
    public class SalesDeleveryNoteRepository : SalesDeleveryNoteRepo
    {

        public async Task<DataSet> GetDeleveryNoteHdr(InvSalesDnHdr entity, string PostedType, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"select invm.IIM_NAME_AR as InvNameAr,invm.IIM_NAME_EN as InvNameEn , sdnh.* from INV_SALES_DN_HDR sdnh " +
                $"inner join INV_INVENTORY_MASTER invm on invm.IIM_SYS_ID = sdnh.ISDH_INV_SYS_ID " +
                $" where(sdnh.ISDH_SYS_ID=:PSSDH_SYS_ID or :PSSDH_SYS_ID=0 )" +
                $" and sdnh.ISDH_V_CODE  ='{auth.User_Act_PH}' ";
            if (PostedType.Length > 0) { query += " AND( sdnh.ISDH_APPROVED_Y_N in('" + PostedType + "') or '" + PostedType + "'='ALL' )"; }
            query += $"order by sdnh.ISDH_SYS_ID DESC";

            var parms = new List<OracleParameter>() {
                new OracleParameter("PSSDH_SYS_ID", entity.ISDH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInvSalesDnDtl(InvSalesDnDtl entity, string authParms)
        {
            var query = $" select* from INV_SALES_DN_DTL where INV_SALES_DN_DTL.ISDD_ISDH_SYS_ID = :PISDD_ISDH_SYS_ID ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PISDD_ISDH_SYS_ID", entity.ISDD_ISDH_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleveryNotePosting(List<InvSalesDnHdr> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            foreach (InvSalesDnHdr entity in entities)
            {
                entity.CURR_USER = authP.UserCode.Value;
                entity.ISDH_APPROVED_BY = authP.UserCode.Value;
                entity.ISDH_V_CODE = authP.User_Act_PH;

            }
            parameters.Add("xml_document_Mstr", entities.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_SALES_DN_APPROVAL_XML", parameters, authParms);
        }

        public async Task<DataSet> SaveDeleveryNoteHdrandItem(InvSalesDeleveryNote entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.INVSALESDNHDR.CURR_USER = authP.UserCode.Value;
            entities.INVSALESDNHDR.ISDH_V_CODE = authP.User_Act_PH;

            if (entities.INVSALESDNHDR.ISDH_SYS_ID > 0)
                entities.INVSALESDNHDR.STATE = (int)OperationType.Update;
            else entities.INVSALESDNHDR.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.INVSALESDNDTL.Count; i++)
            {
                entities.INVSALESDNDTL[i].CURR_USER = authP.UserCode;
                if (entities.INVSALESDNDTL[i].ISDD_SYS_ID > 0)
                    if (entities.INVSALESDNDTL[i].STATE == 3)
                    {
                        entities.INVSALESDNDTL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.INVSALESDNDTL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.INVSALESDNDTL[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.INVSALESDNHDR });
            parameters.Add("xml_document_d", entities.INVSALESDNDTL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_SALES_DN_XML", parameters, authParms);
        }
        public async Task<DataSet> GetDeleveryNoteLastCode(string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (ISDH_CODE, '^[0-9]+') THEN ISDH_CODE ELSE '0' END)), 0) + 1 AS Code " +
                $"FROM INV_SALES_DN_HDR where(ISDH_V_CODE ='"+ authP .User_Act_PH+ "') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteDeleveryNote(InvSalesDnHdr entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_SALES_DN_XML", parameters, authParms);
        }

        public async Task<DataSet> getInvSalesOrderDtl(int id, string authParms)
        {
            var query = $"select * from SALES_RTRN_ORDER_DTL where SALES_RTRN_ORDER_DTL.SROD_SROH_SYS_ID=:pCode";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pCode",id)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> getInvItemcurrStk(int invSysId, int itemSysId, int batchSysId, int uomSysId, string authParms)
        {
            var query = $"select fn__item_btch_curr_stk(:p_inv_sys_id ,:p_item_sys_id ,:p_batch_sys_id ,:p_uom_sys_id ) as curr_stk_qty from dual ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("p_inv_sys_id",invSysId),
                new OracleParameter("p_item_sys_id", itemSysId),
                new OracleParameter("p_batch_sys_id", batchSysId),
                new OracleParameter("p_uom_sys_id", uomSysId)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}