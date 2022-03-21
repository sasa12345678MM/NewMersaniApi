namespace Mersani.models.Administrator
{
    //[Table("GAS_CITY")]
    public class City
    {
        public int CITY_SYS_ID { get; set; }
        public int CITY_REGION_SYS_ID { get; set; }
        public int CITY_ID { get; set; }
        public string CITY_NAME_EN { get; set; }
        public string CITY_NAME_AR { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }
    }
}
