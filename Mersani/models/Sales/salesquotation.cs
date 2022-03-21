using System;
using System.Collections.Generic;


namespace Mersani.models.Sales
{
    public class IsalesquotationMaster
    {
        public int? SQH_SYS_ID { set; get; }
        public string SQH_CODE { set; get; }
        public string SQH_V_CODE { set; get; }
        public DateTime? SQH_DATE { set; get; }
        public int SQH_VERSION { set; get; }
        public int? SQH_PRNT_SQH_SYS_ID { set; get; }
        public string SQH_NOTE { set; get; }
        public char? SQH_TO_OWNER_CUST_O_C { set; get; }
        public int? SQH_OWNER_CUST_SYS_ID { set; get; }
        public string SQH_RFQ_INFO { set; get; }
        public decimal? SQH_DISCOUNT_PCT { set; get; }
        public decimal? SQH_DISCOUNT_AMT { set; get; }
        public decimal? SQH_TOTAL { set; get; }
        
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }


    }
    public class IsalesquotationDetails
    {
        public int ? SQD_SYS_ID { get; set; }
        public int? SQD_SQH_SYS_ID { get; set; } 
        public int? SQD_ITEM_SYS_ID { get; set; }
        public int? SQD_ITEM_QTY { get; set; }
        public int? SQD_ITEM_UOM_SYS_ID { get; set; }
        public decimal? SQD_ITEM_UNIT_PRICE { get; set; }
        public string SQD_NOTES { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class IsalesquotationTerms
    {
        public int? SQT_SYS_ID { get; set; }
        public int? SQT_SQH_SYS_ID { get; set; } 
        public string SQT_TERM { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }


    public class ISalesQuotation
    {
        public IsalesquotationMaster SALESQUOTATIONMASTER { set; get; }
        public List<IsalesquotationDetails> SALESQUOTATIONDETAILES { set; get; }
        public List<IsalesquotationTerms> SALESQUOTATIONTERMS { set; get; }

    }




}
