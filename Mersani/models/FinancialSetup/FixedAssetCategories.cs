
namespace Mersani.models.FinancialSetup
{
    public class FixedAssetCategories
    {
        public int? ASSET_CTGRY_SYS_ID { get; set; }
        public int? ASSET_CTGRY_CODE { get; set; }
        public string ASSET_CTGRY_NAME_AR { get; set; }
        public string ASSET_CTGRY_NAME_EN { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; internal set; }
    }
}
