namespace Mersani.models.Stock
{
    public class InventoryLocations
    {
        public string IIL_LOC_NAME_AR { set; get; }
        public string IIL_LOC_NAME_EN { set; get; }
        public string IIL_LOC_CODE { set; get; }

        public int? IIL_LOC_SYS_ID { set; get; }
        public int? IIL_MST_INV_SYS_ID { set; get; }

        public float? IIL_LOC_AREA { set; get; }
        public float? IIL_LOC_LENGTH { set; get; }
        public float? IIL_LOC_WIDTH { set; get; }
        public float? IIL_LOC_NO_OF_SHELVES { set; get; }
        public int? IIL_LOC_PARENT_SYS_ID { set; get; }
        public string IIL_LOC_ARRANGE_TYPE { set; get; }
        public float? IIL_LOC_HEIGHT { set; get; }
        public char? IIL_LOC_REFRIGERATOR_Y_N { set; get; }
        public string IIL_LOC_ARRANGE_FROM { set; get; }
        public string IIL_LOC_ARRANGE_TO { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
}
