using ApiJqProxy.Model;
using System.Text;

namespace ApiJqProxy
{
    public class AjaxFunctionGenerator
    {
        public string GenerateAjaxFunctions(IEnumerable<ApiEndpoint> endpoints, string host)
        {
            StringBuilder jsCode = new StringBuilder();

            // Add the generic ajax function
            jsCode.AppendLine("var apijqproxy = apijqproxy || {};");
            jsCode.AppendLine("(function(){");
            jsCode.AppendLine($"let baseUrl = '{host}'");

            jsCode.AppendLine(@"
            function genericAjaxCall(url, method, data) {
                const options = {
                    method: method,
                    headers: { 'Content-Type': 'application/json' }
                };
            
                if (method !== 'GET' && method !== 'HEAD') {
                    options.body = JSON.stringify(data);
                }
            
                return fetch(url, options)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .catch(error => {
                        console.error('There has been a problem with your fetch operation:', error);
                        throw error;
                    });
            }");

            foreach (var controller in endpoints.GroupBy(g => g.ControllerName))
            {
                jsCode.AppendLine($"apijqproxy.{controller.Key} = apijqproxy.{controller.Key} ||" + " {};");
            }

            // Generate specific functions for each endpoint
            foreach (var endpoint in endpoints)
            {
                string functionName = GenerateFunctionName($"{endpoint.ControllerName}/{endpoint.HttpMethod}");
                jsCode.AppendLine($"apijqproxy.{endpoint.ControllerName}.{functionName} = function {functionName}(data, onSuccess, onError) {{");
                //jsCode.AppendLine($"    return genericAjaxCall('https://localhost:7017/{endpoint.Url}', '{endpoint.HttpMethod.ToUpper()}', data, onSuccess, onError);");
                jsCode.AppendLine($"    return genericAjaxCall('https://localhost:7017/{endpoint.Url}', '{endpoint.HttpMethod.ToUpper()}', data);");
                jsCode.AppendLine("};");
            }

            jsCode.AppendLine("})();");

            return jsCode.ToString();
        }

        private string GenerateFunctionName(string url)
        {
            return url.Replace('/', '_').Replace(' ', '_').Trim('_');
        }
    }
}