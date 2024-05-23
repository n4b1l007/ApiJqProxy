using ApiJqProxy.Model;
using System.Text;

namespace ApiJqProxy
{

    public class AjaxFunctionGenerator
    {
        public string GenerateAjaxFunctions(List<ApiEndpoint> endpoints)
        {
            StringBuilder jsCode = new StringBuilder();

            // Add the generic ajax function
            jsCode.AppendLine("(function(){");
            jsCode.AppendLine("function genericAjaxCall(url, method, data, onSuccess, onError) {");
            jsCode.AppendLine("    $.ajax({");
            jsCode.AppendLine("        url: url,");
            jsCode.AppendLine("        method: method,");
            jsCode.AppendLine("        data: data,");
            jsCode.AppendLine("        success: onSuccess,");
            jsCode.AppendLine("        error: onError");
            jsCode.AppendLine("    });");
            jsCode.AppendLine("}");

            // Generate specific functions for each endpoint
            foreach (var endpoint in endpoints)
            {
                string functionName = GenerateFunctionName(endpoint.Url);
                jsCode.AppendLine($"function {functionName}(data, onSuccess, onError) {{");
                jsCode.AppendLine($"    genericAjaxCall('{endpoint.Url}', '{endpoint.HttpMethod.ToUpper()}', data, onSuccess, onError);");
                jsCode.AppendLine("}");
            }

            jsCode.AppendLine("})();");

            return jsCode.ToString();
        }

        private string GenerateFunctionName(string url)
        {
            return "ajax_" + url.Replace('/', '_').Replace(' ', '_').Trim('_');
        }
    }
}
