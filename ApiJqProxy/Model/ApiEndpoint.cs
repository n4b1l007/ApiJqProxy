namespace ApiJqProxy.Model
{
    public class ApiEndpoint
    {
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public string ControllerName { get; set; }

        public ApiEndpoint(string url, string httpMethod, string controllerName)
        {
            Url = url;
            HttpMethod = httpMethod;
            ControllerName = controllerName;
        }
    }
}
