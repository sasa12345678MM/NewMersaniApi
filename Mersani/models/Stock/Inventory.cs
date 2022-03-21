namespace Mersani.models.Stock
{
    public class Inventory
    {
        public string IIM_CODE { get; set; }
        public string IIM_NAME_AR { get; set; }
        public string IIM_NAME_EN { get; set; }

        public int? IIM_SYS_ID { get; set; }
        public int? IIM_OWNER_SYS_ID { get; set; }
        public int? IIM_MGR_USR_CODE { get; set; }
        public int? IIM_CITY_SYS_ID { get; set; }
        public int? IIM_AREA { get; set; }
        public int? IIM_LENGTH { get; set; }
        public int? IIM_WIDTH { get; set; }
        public int? IIM_NO_OF_SHELVES { get; set; }
        public int? IIM_INV_S_PHARM_SYS_ID { get; set; }

        public int? IIM_DR_ACCOUNT_CODE { get; set; }
        public int? IIM_CR_ACCOUNT_CODE { get; set; }
        public int? IIM_OP_ITEMS_AMOUNT { get; set; }

        // toggle columns
        public char? INV_FRZ_Y_N { get; set; }
        public char? IIM_OP_APPROVED_Y_N { get; set; }
        public char? IIM_INV_TYPE_I_S { get; set; }

        // log columns
        public string IIM_V_CODE { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
}
