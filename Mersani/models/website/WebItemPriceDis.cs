using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.website
{
    public class WebItemPriceDis
    {
        public int item_sys_id { get; set; }
        public string item_name_ar { get; set; }
        public string item_name_en { get; set; }
        public decimal ITEM_SALE_PRICE { get; set; }
        public double ITEM_DISCOUNT_PCT { get; set; }
        public bool item_need_mdchl_desc_y_n { get; set; }
        public bool item_need_auth_y_n { get; set; }


    }
}
