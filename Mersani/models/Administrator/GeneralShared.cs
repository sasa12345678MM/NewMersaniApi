namespace Mersani.models.Administrator
{
    public class GeneralShared
    {
    }
    public class PrintParms
    {
        public int? PrintType { get; set; }
        public string HtmlContent { get; set; }
        public int? TranSerial { get; set; }
        public string PreparePrintPath { get; set; }
        public string TransPrintPath { get; set; }
    }
}
