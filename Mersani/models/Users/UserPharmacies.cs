
namespace Mersani.models.Users
{
    public class UserPharmacies
    {
        public int? UBA_SYS_ID { get; set; }
        public int? UBA_USR_CODE { get; set; }
        public int? UBA_PH_SYS_ID { get; set; }
        public char? UBA_FRZ_Y_N { get; set; }
        public string UBA_FRZ_REASON { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
}
