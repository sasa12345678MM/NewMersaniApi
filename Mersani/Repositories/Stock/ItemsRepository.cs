using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;


namespace Mersani.Repositories.Stock
{
    public class ItemsRepository : IItemsRepo
    {
        public async Task<DataSet> GetItems(Items entity, string authParms)
        {
            var query = $"SELECT INV_ITEM_MASTER.*,INV_ITEM_GROUP.IIG_STK_SRV_S_V FROM INV_ITEM_MASTER " +
                $" left outer join INV_ITEM_GROUP on INV_ITEM_MASTER.ITEM_IIG_SYS_ID = INV_ITEM_GROUP.IIG_SYS_ID " +
                $" WHERE ITEM_SYS_ID = :pITEM_SYS_ID OR :pITEM_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITEM_SYS_ID", entity.ITEM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        
        public async Task<DataSet> GetItemsWithCriteria(Items criteria, string authParms)
        {
            var query = $"SELECT * FROM(SELECT a.*, ROWNUM r__ FROM" +
                $" (SELECT INV_ITEM_MASTER.*, INV_ITEM_GROUP.IIG_STK_SRV_S_V, FN_GET_TABLE_ROWS_COUNT('INV_ITEM_MASTER') AS ROWS_COUNT FROM INV_ITEM_MASTER " +
                $" LEFT OUTER JOIN INV_ITEM_GROUP ON INV_ITEM_MASTER.ITEM_IIG_SYS_ID = INV_ITEM_GROUP.IIG_SYS_ID) a " +
                $" WHERE ROWNUM < (({criteria.PAGE_NO} * {criteria.PAGE_SIZE}) + 1)) " +
                $" WHERE r__ >= ((({criteria.PAGE_NO} - 1) * {criteria.PAGE_SIZE}) +1)";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostItemMasterDetails(Items entity, string authParms)
        {
            if (entity.ITEM_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            
            entity.INS_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode.Value;
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document", new List<dynamic>() { entity });
            return await OracleDQ.ExcuteMasterDetailsXMLAsyncWithOutPut("PRC_INV_ITEM_MASTER_XML", parameters, authParms);
        }
        
        public async Task<DataSet> DeleteItemMasterDetails(Items entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document", new List<dynamic>() { entity });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_INV_ITEM_MASTER_XML", parameters, authParms);
        }

        public async Task<DataSet> PostItem(List<Items> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                //entities[i].V_Code = authP.User_Act_PH;
                entities[i].INS_USER = authP.UserCode.Value;
                if (entities[i].ITEM_SYS_ID > 0)
                    entities[i].STATE = (int)OperationType.Update;
                else entities[i].STATE = (int)OperationType.Add;
            }
            return await OracleDQ.ExcuteSelectizeProcAsync("PRC_INV_ITEM_XML", entities.ToList<dynamic>(), authParms);
        }
       
        public async Task<DataSet> GetLastItemCode(string authParms)
        {
            var query = $"SELECT NVL (MAX (TO_NUMBER (ITEM_SYS_ID)), 0) + 1 AS Code FROM INV_ITEM_MASTER";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        #region units
        public async Task<DataSet> GetItemUnits(ItemUnits entity, string authParms)
        {
            var query = $"SELECT INV_ITEM_UOM.*, INV_UOM.UOM_NAME_AR, INV_UOM.UOM_NAME_EN " +
                $"FROM INV_ITEM_UOM " +
                $"JOIN INV_UOM ON INV_ITEM_UOM.ITU_UOM_SYS_ID = INV_UOM.UOM_SYS_ID " +
                $"WHERE ITU_ITEM_SYS_ID = :pITU_ITEM_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITU_ITEM_SYS_ID", entity.ITU_ITEM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostItemUnits(List<ItemUnits> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].INS_USER = authP.UserCode;
                if (entities[i].ITU_SYS_ID > 0)
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
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_UNITS_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteItemUnits(ItemUnits entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_UNITS_XML", new List<dynamic>() { entity }, authParms);
        }
        #endregion

        #region alternatives
        public async Task<DataSet> GetInvItemAlternative(invItemAlternative entity, string authParms)
        {
            var query = $"SELECT IIA.*, ITEM.ITEM_NAME_AR, ITEM.ITEM_NAME_EN " +
                $" FROM INV_ITEM_ALTERNATIVE IIA, INV_ITEM_MASTER ITEM " +
                $" WHERE IIA.IIA_ITEM_ALTRNV_SYS_ID = ITEM.ITEM_SYS_ID AND IIA.IIA_ITEM_MSTR_SYS_ID =:pITEM_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITEM_SYS_ID", entity.IIA_ITEM_MSTR_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostItemAlternative(List<invItemAlternative> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].INS_USER = authP.UserCode;
                if (entities[i].IIA_ALTRNV_SYS_ID > 0)
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
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_ALTERNATIVE_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteItemAlternative(invItemAlternative entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_ALTERNATIVE_XML", new List<dynamic>() { entity }, authParms);
        }
        #endregion

        #region related
        public async Task<DataSet> GetInvItemRelated(invItemRelated entity, string authParms)
        {
            var query = $"SELECT RLTD.*, ITEM.ITEM_NAME_AR, ITEM.ITEM_NAME_EN " +
                $" FROM INV_ITEM_RELATED RLTD, INV_ITEM_MASTER ITEM " +
                $" WHERE RLTD.IIR_ITEM_RLTD_SYS_ID = ITEM.ITEM_SYS_ID AND RLTD.IIR_ITEM_MSTR_SYS_ID = :pITEM_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pITEM_SYS_ID", entity.IIR_ITEM_MSTR_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostItemRelated(List<invItemRelated> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].INS_USER = authP.UserCode;
                if (entities[i].IIR_RLTD_SYS_ID > 0)
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
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_RELATED_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteItemRelated(invItemRelated entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_RELATED_XML", new List<dynamic>() { entity }, authParms);
        }
        #endregion

        #region images
        public async Task<DataSet> GetInvItemImages(InvItemImages entity, string authParms)
        {
            var query = $"SELECT INV_ITEM_IMAGES.*FROM INV_ITEM_IMAGES where INV_ITEM_IMAGES.IMG_ITEM_SYS_ID =:PIMG_ITEM_SYS_ID";

            var parms = new List<OracleParameter>() {
                new OracleParameter("PIMG_ITEM_SYS_ID", entity.IMG_ITEM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostInvItemImages(List<InvItemImages> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].CURR_USER = authP.UserCode;
                if (entities[i].IMG_SYS_ID > 0)
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
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_IMAGES_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteInvItemImages(InvItemImages entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_IMAGES_XML", new List<dynamic>() { entity }, authParms);
        }
        #endregion

        #region prices
        public async Task<DataSet> GetInvItemMasterPrices(InvItemMasterPrices entity, string authParms)
        {
            var query = $"select iip.*,GC.CURR_NAME_AR,GC.CURR_NAME_EN from INV_ITEM_MASTER_PRICES iip " +
                $" inner join GAS_CURRENCY GC on iip.ITP_CURR_SYS_ID = GC.CURR_SYS_ID " +
                $" where iip.ITP_ITEM_SYS_ID =:pItemMasterId ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pItemMasterId", entity.ITP_ITEM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostInvItemMasterPrices(List<InvItemMasterPrices> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].INS_USER = authP.UserCode;
                if (entities[i].ITP_SYS_ID > 0)
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
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_MASTER_PRICES", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteInvItemMasterPrices(InvItemMasterPrices entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_MASTER_PRICES", new List<dynamic>() { entity }, authParms);
        }
        #endregion

        #region batches
        public async Task<DataSet> GetInvItemBatches(invitemMasterBatches entity, string authParms)
        {
            var query = $"select * from INV_ITEM_MASTER_BATCHES where IMB_ITEM_SYS_ID = :pItemMaster";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pItemMaster  ", entity.IMB_ITEM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> PostItemMasterBatches(List<invitemMasterBatches> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].INS_USER = authP.UserCode;
                if (entities[i].IMB_SYS_ID > 0)
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
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_M_BATCHES_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeleteItemMasterBatches(invitemMasterBatches entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_M_BATCHES_XML", new List<dynamic>() { entity }, authParms);
        }
        #endregion

        #region Assempld
        public async Task<DataSet> GetInvItemAssempld(InvItemAssempldItems entity, string authParms)
        {
            var query = $"SELECT IIAI.*, ITEM.ITEM_NAME_AR, ITEM.ITEM_NAME_EN " +
                $" FROM INV_ITEM_ASSPLD_ITEMS IIAI, INV_ITEM_MASTER ITEM " +
                $" WHERE IIAI.IIS_ITEM_ASSMPLD_SYS_ID = ITEM.ITEM_SYS_ID AND IIAI.IIS_ITEM_MSTR_SYS_ID = :pITEM_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pITEM_SYS_ID", entity.IIS_ITEM_MSTR_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostInvItemAssempld(List<InvItemAssempldItems> entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].INS_USER = authP.UserCode;
                if (entities[i].IIS_SYS_ID > 0)
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
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_ASSEMPLD_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteInvItemAssempld(InvItemAssempldItems entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_ITEM_ASSEMPLD_XML", new List<dynamic>() { entity }, authParms);
        }
        #endregion
    }
}
