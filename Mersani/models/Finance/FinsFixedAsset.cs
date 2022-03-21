using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Finance
{
    public class FinsFixedAsset
    {
        public int? ASSET_SYS_ID { set; get; }
        public string ASSET_CODE { set; get; }
        public string ASSET_NAME_EN { set; get; }
        public string ASSET_NAME_AR { set; get; }
        public int? ASSET_TYPE_ID { set; get; }
        public DateTime? ASSET_PURCHASE_DATE { set; get; }
        public int? ASSET_SUPP_SYS_ID { set; get; }
        public int? ASSET_PURCHASE_PRICE { set; get; }
        public string ASSET_SERIAL_NO { set; get; }
        public int? ASSET_COST_CENTER_SYS_ID { set; get; }
        public string ASSET_V_CODE { set; get; }
        public string ASSET_PARENT_V_CODE { set; get; }
        
        public int? ASSET_YEAR { set; get; }
        public string ASSET_FINSHED_Y_N { set; get; }
        public int? ASSET_CATEGORY_ID { set; get; }
        public int? ASSET_RECIPT_CR_ACC_SYS_ID { set; get; }
        public int? ASSET_RECIPT_DR_ACC_SYS_ID { set; get; }
        public int? ASSET_PARENT_ASSET_SYS_ID { set; get; }
        public int? ASSET_INSURANCE_AMOUNT { set; get; }
        public DateTime? ASSET_INSURANCE_START_DATE { set; get; }
        public int? ASSET_INSUR_SUPP_SYS_ID { set; get; }
        public string ASSET_POSTED_Y_N { set; get; }
        public DateTime? ASSET_SALE_DATE { set; get; }
        public decimal? ASSET_SALE_VALUE { set; get; }
        public string ASSET_SALE_Y_N { set; get; }
        public int? INS_USER { set; get; }
        public int? UP_USER { set; get; }
        public int? STATE { set; get; }
        //////////////////////////////////////
        public DateTime? POST_DATE { set; get; }
        public char? ASSET_SALVAGED_Y_N { set; get; }
        public decimal? ASSET_SALVAGE_VALUE { set; get; }
        public int? ASSET_LIVE_YEARS { set; get; }
        public decimal? ASSET_BOOK_VAL { set; get; }
        public DateTime? ASSET_BOOK_VAL_DATE { set; get; }



    //public decimal? ASSET_DEPRE_VALUE { set; get; }
    //public int? ASSET_NOU { set; get; }
    //public decimal? ASSET_BOOK_VALUE { set; get; }
    //public int? ASSET_BOOK_FORMULA_ID { set; get; }
    //public int? ASSET_DEPRE_PERCENT { set; get; }
    //public decimal? ASSET_ACCUMULATED_VALUE { set; get; }
    //public int? ASSET_NET_BOOK { set; get; }
    //public int? ASSET_DEPRE_EXP_ACC_SYS_ID { set; get; }
    //public int? ASSET_ACCUMULATED_ACC_SYS_ID { set; get; }
    //public int? ASSET_PROFIT_LOSS_ACC_SYS_ID { set; get; }
    //public int? ASSET_STATUS { set; get; }
    //public int? ASSET_CLOSED_YEAR { set; get; }
    //public int? ASSET_CLOSED_PERIOD_ID { set; get; }
    //public int? ASSET_SALE_ACCUMULTE { set; get; }
    //public int? ASSET_PENDING { set; get; }
    //public decimal? ASSET_YTD_VALUE { set; get; }
    //public int? ASSET_DEPR_YEARLY { set; get; }
    //public int? ASSET_ACCUMULATED_UP { set; get; }
    public int? ASSET_DEP_CR_ACC_SYS_ID { set; get; }
        public int? ASSET_DEP_DR_ACC_SYS_ID { set; get; }
        public int? ASSET_SALE_CR_ACC_SYS_ID{ set; get; }
        public int? ASSET_SALE_DR_ACC_SYS_ID { set; get; }

    }
    public class FinsFixedAssetDepr
    {
        public int? ASSETD_SYS_ID { set; get; }
        public int? ASSETD_ASSET_SYS_ID { set; get; }
        public int? ASSETD_YEAR { set; get; }
        public int? ASSETD_PERIOD_SYS_ID { set; get; }
        public string ASSETD_V_CODE { set; get; }
        public int? ASSETD_DEP_VAL { set; get; }
        public int? ASSETD_ACCUMLTD_DEP_VAL { set; get; }
        public int? ASSETD_BOOK_VAL { set; get; }
        public char ASSETD_POSTED_Y_N { set; get; }
        public string ASSETD_NOTES { set; get; }
        public int? INS_USER { set; get; }
        public int? UP_USER { set; get; }
        public int? STATE { set; get; }

    }
}
