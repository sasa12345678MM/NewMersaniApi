using System;
using System.Collections.Generic;

namespace Mersani.models.Purchase
{
    public class PurchaseRequestMaster
    {
        public int? IPRH_SYS_ID { set; get; }
        public string IPRH_CODE { set; get; }
        public string IPRH_V_CODE { set; get; }
        public string IPRH_DESC { set; get; }
        public DateTime? IPRH_DATE { set; get; }
        public DateTime? IPRH_REQ_DELVRY_DATE { set; get; }
        public char? IPRH_MIN_QTY_Y_N { set; get; }
        public char? IPRH_REORDER_Y_N { set; get; }

        public char? IPRH_APPROVED_Y_N { set; get; }
        public int? IPRH_APPROVED_BY { set; get; }
        public DateTime? IPRH_APPROVED_DATE { set; get; }
        public string IPRH_APPROVED_NOTES { set; get; }

        public char? IPRH_CPM_APPROVED_Y_N { set; get; }
        public int? IPRH_CPM_APPROVED_BY { set; get; }
        public DateTime? IPRH_CPM_APPROVED_DATE { set; get; }
        public string IPRH_CPM_APPROVED_NOTES { set; get; }

        public char? IPRH_OWNR_APPROVED_Y_N { set; get; }
        public int? IPRH_OWNR_APPROVED_BY { set; get; }
        public DateTime? IPRH_OWNR_APPROVED_DATE { set; get; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class PurchaseRequestDetails
    {
        public int? IPRD_SYS_ID { get; set; }
        public int? IPRD_IPRH_SYS_ID { get; set; }
        public int? IPRD_ITEM_SYS_ID { get; set; }
        public int? IPRD_UOM_SYS_ID { get; set; }
        public int? IPRD_QTY { get; set; }
        public char? IPRD_M_N_R { get; set; }
        public string IPRD_NOTES { get; set; }

        // new updates
        public int? IPRD_OWNR_APPROVED_QTY { get; set; }
        public int? IPRD_CPM_APPROVED_QTY { get; set; }
        public char? IPRD_CPM_FRM_SP_OWNR_S_O { get; set; }
        public int? IPRD_CPM_FRM_OWNR_SYS_ID { get; set; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }

    public class PurchaseRequest
    {
        public PurchaseRequestMaster MASTER { set; get; }
        public List<PurchaseRequestDetails> DETAILS { set; get; }
    }


    public class PurchaseRequestDashboard
    {
        public string ITEM_NAME_AR { get; set; }
        public string ITEM_NAME_EN { get; set; }
        public string IPRD_CPM_FRM_SP_OWNR { get; set; }
        public string ITEM_BASIC_UOM_CODE { get; set; }
        public string ITEM_IIG_NAME_EN { get; set; }
        public string ITEM_REQ_UOM_CODE { get; set; }
        public string IPRD_OWNR_NAME_EN { get; set; }
        public string ITEM_BASIC_SUPP_NAME { get; set; }
        
        public string ITEM_CODE { get; set; }
        public string IPRH_CODE { get; set; }
        public string IPRH_DESC { get; set; }
        public string IPRH_CPM_APPROVED_NOTES { get; set; }
        public string IPRH_APPROVED_NOTES { get; set; }
        public string IPRD_NOTES { get; set; }
        public string IPRH_V_CODE { get; set; }

        public int? IPRH_SYS_ID { get; set; }
        public int? IPRH_APPROVED_BY { get; set; }
        public int? IPRH_CPM_APPROVED_BY { get; set; }
        public int? IPRH_OWNR_APPROVED_BY { get; set; }
        public int? IPRD_SYS_ID { get; set; }
        public int? IPRD_IPRH_SYS_ID { get; set; }
        public int? IPRD_ITEM_SYS_ID { get; set; }
        public int? ITEM_IIG_SYS_ID { get; set; }
        public int? ITEM_BASIC_UOM_SYS_ID { get; set; }
        public int? ITEM_BASIC_SUPP_SYS_ID { get; set; }
        public int? ITEM_GROUP { get; set; }
        public int? ITEM_LAST_PUR_PRICE { get; set; }
        public int? ITEM_SALE_PRICE { get; set; }
        public int? ITEM_REQ_UOM_SYS_ID { get; set; }
        public int? REQ_QTY { get; set; }
        public int? REQ_BASIC_UOM_QTY { get; set; }
        public int? IPRD_OWNR_APPROVED_QTY { get; set; }
        public int? OWNR_APRV_BASIC_UOM_QTY { get; set; }
        public int? IPRD_CPM_APPROVED_QTY { get; set; }
        public int? CPM_APRV_BASIC_UOM_QTY { get; set; }
        public int? IPRD_CPM_FRM_OWNR_SYS_ID { get; set; }

        public DateTime? IPRH_DATE { get; set; }
        public DateTime? IPRH_REQ_DELVRY_DATE { get; set; }
        public DateTime? IPRH_APPROVED_DATE { get; set; }
        public DateTime? IPRH_CPM_APPROVED_DATE { get; set; }
        public DateTime? IPRH_OWNR_APPROVED_DATE { get; set; }

        public char? IPRH_MIN_QTY_Y_N { get; set; }
        public char? IPRH_REORDER_Y_N { get; set; }
        public char? IPRH_APPROVED_Y_N { get; set; }
        public char? IPRH_CPM_APPROVED_Y_N { get; set; }
        public char? IPRH_OWNR_APPROVED_Y_N { get; set; }
        public char? IPRD_AUTO_MNUL { get; set; }
        public char? IPRD_CPM_FRM_SP_OWNR_S_O { get; set; }
    }
}



