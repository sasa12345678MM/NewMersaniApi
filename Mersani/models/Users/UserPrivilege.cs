namespace Mersani.models.Users
{
	public class UserPrivilege
	{
		public int? UCBF_SYS_ID { get; set; }
		public int? UBA_SYS_ID { get; set; }
		public int? MNU_CODE { get; set; }
		public string UCBF_INSERT_ALLOWED_Y_N { get; set; }
		public string UCBF_UPDATE_ALLOWED_Y_N { get; set; }
		public string UCBF_DELETE_ALLOWED_Y_N { get; set; }
		public string UCBF_QUERY_ALLOWED_Y_N { get; set; }
		public string UCBF_RUN_REP_ALLOWED_Y_N { get; set; }
		public string MNU_LABEL_AR { get; set; }
		public string MNU_LABEL_EN { get; set; }
		public string MNU_PATH { get; set; }
		public string MNU_NAME { get; set; }
		public string PARENT_AR { get; set; }
		public string PARENT_EN { get; set; }
		public int? PARENT_CODE { get; set; }


		public int? CURR_USER { get; set; }
		public int? STATE { get; set; }
	}
}
