namespace Mersani.models.Administrator
{
    public class GeneralSelectize
    {
        public int key { get; set; }
        public int code { get; set; }
        public int name { get; set; }
    }
    public class ISelectSearch
    {
        public int key { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int? parentCode { get; set; }
        public string filterKeyes { get; set; }

    }
}
