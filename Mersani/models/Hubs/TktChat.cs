using System;

namespace Mersani.models.Hubs
{
    public class TktChat
    {
        public int? TC_SYS_ID { get; set; }
        public string clientuniqueid { get; set; }

        public string TC_MSG_TYPE { get; set; } 
        public string TC_SNDR_TYPE { get; set; }
        public string TC_RCVR_TYPE { get; set; }
        public string TC_TYPE { get; set; }
        public DateTime? TC_DATE { get; set; }
        public string TC_MESSAGE { get; set; }
        public string TC_ATTACHMENT { get;set;}

        public string TC_SENDER { get; set; }
        public string TC_RECEIVER { get; set; }



        public char? SNDR_CHAR_AR { get; set; }
        public char? SNDR_CHAR_EN { get; set; }
    }
}
