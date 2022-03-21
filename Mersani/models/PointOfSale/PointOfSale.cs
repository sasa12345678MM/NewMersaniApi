using System;
using System.Collections.Generic;

namespace Mersani.models.PointOfSale
{
    public class PointOfSaleMASTER
    {

        public int? PCH_SYS_ID { get; set; }
        public string PCH_VOUCHER_NO { get; set; }
        public string PCH_V_CODE { get; set; }
        public string PCH_BANK_CONFIRM_NO { get; set; }
        public string PCH_CUSTOMER { get; set; }
        public DateTime? PCH_DATE { get; set; }
        public int? PCH_CUST_TYPE { get; set; }
        public int? PCH_PIC_SYS_ID { get; set; }
        public int? PCH_PICNT_SYS_ID { get; set; }
        public int? PCH_PICNTC_SYS_ID { get; set; }
        public int? PCH_CUST_SYS_ID { get; set; }
        public int? PCH_PAYMENT_TYPE { get; set; }
        public decimal? PCH_TOTAL_AMOUNT { get; set; }
        public decimal? PCH_DISCOUNT_AMOUNT { get; set; }
        public decimal? PCH_DISCOUNT_PRC { get; set; }
        public decimal? PCH_VAT_VALUE { get; set; }
        public decimal? PCH_CASH_PAYMENT { get; set; }
        public decimal? PCH_CARD_PAYMENT { get; set; }
        public decimal? PCH_PONITS_PAYMENT { get; set; }
        
        public decimal? PCH_ISURANCE_PAYMENT { get; set; }
        public int? PCH_CR_ACC_CODE { get; set; }
        public int? PCH_CASH_DR_ACC_CODE { get; set; }
        public int? PCH_BANK_DR_ACC_CODE { get; set; }
        public int? PCH_INS_CO_CR_ACC_CODE { get; set; }
        public string PCH_PATIENT_NAME { get; set; }
        public string PCH_CUST_INSURANCE_NO { get; set; }
        public string PCH_OFFLINE_ONLINE { get; set; }
        public string PCH_CUST_MOB_NO { get; set; }
        public int? PCH_VAT_ACC_CODE { get; set; }
        public int? PCH_ADDED_VALUE { get; set; }
        public int? PCH_ADDED_VALUE_ACC_CODE { get; set; }
        public string PCH_ADDED_VALUE_DESC { get; set; }
        public int? PCH_PSA_SYS_ID { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }

    public class PointOfSaleDetails
    {

        public int? PCD_SYS_ID { get; set; }
        public int? PCD_PCH_SYS_ID { get; set; }
        public int? PCD_ITEM_SYS_ID { get; set; }
        public int? PCD_ITEM_BATCH_SYS_ID { get; set; }
        public int? PCD_UOM_SYS_ID { get; set; }
        public int? PCD_QTY { get; set; }
        public decimal? PCD_SALES_PRICE { get; set; }
        public decimal? PCD_ITEM_AVG_COST { get; set; }
        public decimal? PCD_DISCOUNT_AMOUNT { get; set; }
        public decimal? PCD_VAT_AMOUNT { get; set; }
        public int? PCD_DOSE_QTY { get; set; }
        public int? PCD_DOSE_TYPE { get; set; }
        public int? PCD_FREQ_QTY { get; set; }
        public int? PCD_FREQ_TYPE { get; set; }
        public int? PCD_PERIOD_QTY { get; set; }
        public int? PCD_PERIOD_TYPE { get; set; }
        public int? PCD_INTERACTION_TYPE { get; set; }
        public string PCD_TOTAL_DESC_AR { get; set; }
        public string PCD_TOTAL_DESC_EN { get; set; }
        public string PCD_DOSE_NOTES { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
    public class IPosShiftsExpenses
    {
        public int? PSE_SYS_ID { get; set; }
        public int? PSE_PSA_SYS_ID { get; set; }
        public DateTime? PSE_DATE { get; set; }
        public int? PSE_EXPENS_TYPE { get; set; }
        public Decimal? PSE_EXPENS_VALUE { get; set; }
        public Char? PSE_APPROVED_Y_N { get; set; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
    public class PointOfSaleData
    {
        public PointOfSaleMASTER MASTER { get; set; }
        public List<PointOfSaleDetails> DETAILS { get; set; }
    }

}
