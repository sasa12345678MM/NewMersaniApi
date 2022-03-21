namespace Mersani.models.Administrator
{
    //[Table("GAS_GROUP")]
    public class Group
    {
        public int? GROUP_ID { get; set; }
        public int? GROUP_SYS_ID { get; set; }
        public string GROUP_NAME_EN { get; set; }
        public string GROUP_NAME_AR { get; set; }

        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }
    }
}
