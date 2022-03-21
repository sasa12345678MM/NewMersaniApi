namespace Mersani.models.Administrator
{
    public class UserGroupMenu
    {
        public int USGRMN_SYS_ID { get; set; }
        public int USRGRP_CODE { get; set; }
        public int MNU_CODE { get; set; }
        public int MNU_PARENT { get; set; }

        // for view only
        public string USRGRP_NAME_AR { get; set; }
        public string USRGRP_NAME_EN { get; set; }
        public string MNU_LABEL_AR { get; set; }
        public string MNU_LABEL_EN { get; set; }
        public string MNU_PARENT_AR { get; set; }
        public string MNU_PARENT_EN { get; set; }
        public int? MNU_ORD { get; set; }

    }
}
