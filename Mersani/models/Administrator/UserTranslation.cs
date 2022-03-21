using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class UserTranslation
    {
        public int LABEL_CODE { set; get; }
        public string FORM_NAME { set; get;}
        public string ITEM_NAME { set; get; }
        public string ITEM_LABEL_AR { set; get; }
        public string ITEM_LABEL_EN { set; get; }
        public string ITEM_TYPE { set; get; }
        public int? MNU_CODE { set; get; }
        public string PMNU_CODE { set; get; }

    }
}

