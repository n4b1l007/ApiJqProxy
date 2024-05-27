using ApiJqProxy.Model;

namespace ApiJqProxy
{
    public class ApiJqProxyMiddleware
    {
        public RequestDelegate Next { get; set; }
        public string JsFileName { get; set; }

        public ApiJqProxyMiddleware(RequestDelegate next, string jsFileName)
        {
            Next = next;
            JsFileName = jsFileName;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == $"/{JsFileName}.js")
            {
                var hostUrl = $"{context.Request.Scheme}://{context.Request.Host}";

                string result = string.Empty;
                List<HttpMethodInfoModel> controllerInfos = ControllerLoader.LoadControllers();

                IEnumerable<ApiEndpoint> endpoints = controllerInfos
                    .Select(s=> new ApiEndpoint(s.Route, s.Name, s.ControllerName));
                
                AjaxFunctionGenerator generator = new AjaxFunctionGenerator();
                string jsCode = generator.GenerateAjaxFunctions(endpoints, hostUrl);

                // Write the JavaScript file to the response.
                context.Response.ContentType = "text/javascript";
                await context.Response.WriteAsync(jsCode);
            }
            else
            {
                await Next(context);
            }
        }
    }
}
