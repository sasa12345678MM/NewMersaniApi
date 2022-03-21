using System;
using System.Collections.Generic;

namespace Mersani.models.Stock
{
    public class Items
    {
        public string ITEM_NAME_AR { set; get; }
        public string ITEM_NAME_EN { set; get; }
        public string ITEM_CODE { set; get; }
        public string ITEM_NOTES { set; get; }
        public int? ITEM_SYS_ID { set; get; }
        public int? ITEM_IIG_SYS_ID { set; get; }
        public int? ITEM_LAST_PUR_PRICE { set; get; }
        public int? ITEM_SALE_PRICE { set; get; }
        public int? ITEM_UOM_SYS_ID { set; get; }
        public int? ITEM_SUPP_SYS_ID { set; get; }
        public int? ITEM_TAG { set; get; }
        public string ITEM_FRZ_Y_N { set; get; }
        public int? INS_USER { set; get; }
        public int? STATE { set; get; }
        public string ITEM_SALE_Y_N { set; get; }
        public char? ITEM_BTCH_Y_N { set; get; }
        public char? ITEM_ASSPLD_Y_N { set; get; }

        public string ITEM_SHORT_NAME_AR { set; get; }
        public string ITEM_SHORT_NAME_EN { set; get; }
        public int? ITEM_MANUFACTURER_SYS_ID { set; get; }
        public int? ITEM_DOSAGE_FORM_SYS_ID { set; get; }
        public string ITEM_GENERIC_NAME { set; get; }

        public char? ITEM_NEED_MDCHL_DESC_Y_N { set; get; }
        public char? ITEM_NEED_AUTH_Y_N { set; get; }
        public char? ITEM_DLVRY_TO_SHRM_Y_N { set; get; }
        public int? ITEM_DELVRY_SHRM_MAXQTY { set; get; }
        public char? ITEM_REFRIGERATOR_Y_N { set; get; }
        ////////////////////////////////////////
        public int? PAGE_NO { set; get; }
        public int? PAGE_SIZE { set; get; }
        public int? ROWS_COUNT { set; get; }

    }
    public class invItemRelated
    {
        public int? IIR_RLTD_SYS_ID { set; get; }
        public int? IIR_ITEM_MSTR_SYS_ID { set; get; }
        public int? IIR_ITEM_RLTD_SYS_ID { set; get; }
        public string IIR_DESC { set; get; }
        public int? INS_USER { set; get; }
        public int? STATE { set; get; }

    }
    public class invItemAlternative
    {
        public int? IIA_ALTRNV_SYS_ID { set; get; }
        public int? IIA_ITEM_MSTR_SYS_ID { set; get; }
        public int? IIA_ITEM_ALTRNV_SYS_ID { set; get; }
        public string IIA_DESC { set; get; }
        public int? INS_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class invitemMasterBatches
    {
        public int? IMB_SYS_ID { set; get; }
        public int? IMB_ITEM_SYS_ID { set; get; }
        public string IMB_BATCH_CODE { set; get; }
        public string IMB_BARCODE { set; get; }

        public DateTime? IMB_PROD_DATE { set; get; }
        public DateTime? IMB_EXPR_DATE { set; get; }
        public char? IMB_ADDED_M_A { set; get; }

        public int? INS_USER { set; get; }
        public int? STATE { set; get; }

    }
    public class InvItemAssempldItems
    {
        public int? IIS_SYS_ID { set; get; }
        public int? IIS_ITEM_MSTR_SYS_ID { set; get; }
        public int? IIS_ITEM_ASSMPLD_SYS_ID { set; get; }
        public string IIS_DESC { set; get; }
        public int? IIS_UOM_SYS_ID { set; get; }
        public int? IIS_QTY { set; get; }
        public int? INS_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class InvItemMasterPrices
    {
        public int ITP_SYS_ID { set; get; }
        public int? ITP_ITEM_SYS_ID { set; get; }
        public int? ITP_CURR_SYS_ID { set; get; }
        public decimal? ITP_LAST_PUR_PRICE { set; get; }
        public decimal? ITP_DEF_SALE_PRICE { set; get; }
        public int? INS_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class IStockItemsData
    {
        public Items ISTOCKITEMS { set; get; }
        public List<InvItemAssempldItems> INVITEMASSEMPLDITEMS { set; get; }
    }


    public class StockItemDosage
    {
        public int? IIDF_SYS_ID { set; get; }
        public string IIDF_CODE { set; get; }
        public string IIDF_NAME_AR { set; get; }
        public string IIDF_NAME_EN { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }

    public class StockItemManufacturer
    {
        public int? IIMF_SYS_ID { set; get; }
        public string IIMF_CODE { set; get; }
        public string IIMF_NAME_AR { set; get; }
        public string IIMF_NAME_EN { set; get; }

        public string C_NAME_AR { set; get; }
        public string C_NAME_EN { set; get; }

        public int? IIMF_CNTRY_SYS_ID { set; get; }
        public char? IIMF_FRZ_Y_N { set; get; }
        public string IIMF_LOGO { set; get; }
        public char? IIMF_MOB_WEB_SHOW_Y_N { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }

    public class InvItemImages
    {

        public int? IMG_SYS_ID { set; get; }
        public int? IMG_ITEM_SYS_ID { set; get; }

        public string IMG_PATH { set; get; }

        public string IMG_DESC_AR { set; get; }
        public string IMG_DESC_EN { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }

    }



}
