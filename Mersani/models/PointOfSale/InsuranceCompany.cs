namespace Mersani.models.PointOfSale
{
    public class InsuranceCompany
    {
        public int? PIC_SYS_ID { get; set; }

        public string PIC_CODE { get; set; }
        public string PIC_NAME_AR { get; set; }
        public string PIC_NAME_EN { get; set; }
        public string PIC_ADDRESS { get; set; }
        public string PIC_EMAIL { get; set; }
        public string PIC_TEL { get; set; }
        public string PIC_FAX { get; set; }

        public string PIC_CNTC_PERSN { get; set; }
        public string PIC_PERSN_TEL { get; set; }
        public string PIC_PERSN_EMAIL { get; set; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
}
