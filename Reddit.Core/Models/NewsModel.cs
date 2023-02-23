namespace Reddit.Core.Models
{
    public class NewsModel
    {
        public string Kind { get; set; }
        public DataModel Data { get; set; }


        public class DataModel
        {
            public string After { get; set; }
            public int Dist { get; set; }
            public object Modash { get; set; }
            public string geo_filter { get; set; }
            public Child[] Children { get; set; }
            public object Before { get; set; }
        }

        public class Child
        {
            public string Kind { get; set; }
            public DataChildModel Data { get; set; }
        }

        public class DataChildModel
        {
            public string Title { get; set; }
        }

    }
}
