namespace Mersani.models.Administrator
{
    //[Table("GAS_COUNTRY")]
    public class Country 
    {
        public int C_SYS_ID { get; set; }
        public int C_COUNTRY_ID { get; set; }
        public int C_CURR_SYS_ID { get; set; }
        public string C_NAME_EN { get; set; }
        public string C_NAME_AR { get; set; }
        public char? C_POINTS_Y_N { get; set; }
        public char? C_DISCOUNT_Y_N { get; set; }
        public int? C_DISCOUNT_RESET_MTHS { get; set; }
        public char? C_PROMOTION_Y_N { get; set; }
        public int? C_HOME_DELEVERY_FEES { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }

        // for currency
        public string CURR_NAME_AR { get; set; }
        public string CURR_NAME_EN { get; set; }

        
    }
}
