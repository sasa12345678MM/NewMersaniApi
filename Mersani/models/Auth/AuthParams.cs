namespace Mersani.models.Auth
{
    public class AuthParams
    {
        public int? UserCode { get; set; }
        public int? UserGroup { get; set; }
        public int? ForDebug { get; set; }
        public int? UserCurrency { set; get; }
        public string UserLogin { get; set; }
        public string User_Act_PH { get; set; }
        public string User_Parent_V_Code { get; set; }
        public string UserType { get; set; }
        public string UserLanguage { get; set; }
    }
}
