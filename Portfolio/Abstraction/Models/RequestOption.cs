namespace Service.Model
{
    public class RequestOption
    {
        public RequestOption()
        {
            QueryStringItems = new List<KeyValuePair<string, string>>);
            HeaderParameters = new Dictionary<string, string>();
        }
        public string Url { get; set; }
        public object Body { get; set; }
        public string ContentType { get; set; } = "application/json";
        public string Token { get; set; }
        public TimeSpan? Timeout { get; set; } = null;
        public List<KeyValuePair<string, string>>? QueryStringItems { get; set; } = null;
        public Dictionary<string, string>? HeaderParameters = null;
    }
}
