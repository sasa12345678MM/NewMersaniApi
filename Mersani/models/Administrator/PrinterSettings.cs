namespace Mersani.models.Administrator
{
    public class PrinterSetup
    {

        public int GPS_SYS_ID { get; set; }
        public string GPS_V_CODE { get; set; }
        public string GPS_PRINTER_NAME { get; set; }
        public string GPS_DEVICE_OS_NAME { get; set; }
        public string GPS_TYPE_RPT_RCT_LBL_BRC { get; set; }
        public char? GPD_FRZ_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
}