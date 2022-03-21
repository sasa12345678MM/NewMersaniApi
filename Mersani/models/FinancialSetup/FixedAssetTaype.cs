

namespace Mersani.models.FinancialSetup
{
    public class FixedAssetType
    {
        public int? ASSET_TYPE_SYS_ID { get; set; }
        public int? ASSET_TYPE_CODE { get; set; }
        public string ASSET_TYPE_NAME_AR { get; set; }
        public string ASSET_TYPE_NAME_EN { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; internal set; }
    }
}

