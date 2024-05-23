namespace ApiJqProxy.Model
{
    public class ApiEndpoint
    {
        public string Url { get; set; }
        public string HttpMethod { get; set; }

        public ApiEndpoint(string url, string httpMethod)
        {
            Url = url;
            HttpMethod = httpMethod;
        }
    }
}
