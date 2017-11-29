namespace Models.Base
{
    public class ListPaging
    {
        public int records { get; set; }
        public string message { get; set; }
        public int pages { get; set; }
        public int page { get; set; }
        public string sort { get; set; }
        public string dir { get; set; }
        public int rows { get; set; }
        public object data { get; set; }
    }
    public class ListFilter
    {
        public int rows { get; set; }
        public int page { get; set; }
        public string keywords { get; set; }
        public string sort { get; set; }
        public string dir { get; set; }
    }
}
