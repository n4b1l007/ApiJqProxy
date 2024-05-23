﻿using ApiJqProxy.Model;

namespace ApiJqProxy
{
    public class ApiJqProxyMiddleware
    {
        public RequestDelegate Next { get; set; }

        public ApiJqProxyMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/my-middleware.js")
            {
                string result = string.Empty;
                List<HttpMethodInfoModel> controllerInfos = ControllerLoader.LoadControllers();

                List<ApiEndpoint> endpoints = new List<ApiEndpoint>();
                // Iterate over the list of controller information.
                foreach (HttpMethodInfoModel controllerInfo in controllerInfos)
                {
                    result += controllerInfo.Route + " " + controllerInfo.Name + @"
                    " ;
                    // Do something with the controller information.

                    endpoints.Add(new ApiEndpoint(controllerInfo.Route, controllerInfo.Name));
                }

                // Generate the JavaScript file dynamically.
                string javascript = @"alert('Hello, world!'); 

                    https://localhost:7239/swagger/index.html

                    " + result;

                AjaxFunctionGenerator generator = new AjaxFunctionGenerator();
                string jsCode = generator.GenerateAjaxFunctions(endpoints);
                javascript += @"
" + jsCode;

                // Write the JavaScript file to the response.
                context.Response.ContentType = "text/javascript";
                await context.Response.WriteAsync(javascript);
            }
            else
            {
                await Next(context);
            }
        }
    }
}
