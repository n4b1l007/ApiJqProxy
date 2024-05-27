using ApiJqProxy.Model;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ApiJqProxy
{
    public class ControllerLoader
    {
        public static List<HttpMethodInfoModel> LoadControllers()
        {
            List<HttpMethodInfoModel> controllerInfos = new List<HttpMethodInfoModel>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if(type.IsSubclassOf(typeof(ControllerBase)))
                    {
                        MethodInfo[] methods = type.GetMethods();

                        foreach (MethodInfo method in methods)
                        {
                            if (Attribute.IsDefined(method, typeof(HttpGetAttribute)) || 
                                Attribute.IsDefined(method, typeof(HttpPostAttribute)) ||
                                Attribute.IsDefined(method, typeof(HttpPatchAttribute)) ||
                                Attribute.IsDefined(method, typeof(HttpPutAttribute)) ||
                                Attribute.IsDefined(method, typeof(HttpDeleteAttribute))
                                )
                            {
                                string? route = GetRouteFromMethod(method);
                                controllerInfos.Add(new HttpMethodInfoModel
                                {
                                    Route = route,
                                    Name = method.Name,
                                    ControllerName = type.Name.Replace("Controller", string.Empty).ToLower(),
                                });
                            }
                        }
                    }
                }
            }
            return controllerInfos;
        }
        public static string? GetRouteFromMethod(MethodInfo method)
        {
            if (Attribute.IsDefined(method, typeof(RouteAttribute)))
            {
                RouteAttribute? routeAttribute = method?.GetCustomAttribute<RouteAttribute>();

                return routeAttribute?.Template;
            }
            else
            {
                return GetControllerAndActionName(method);
            }
        }

        private static string GetControllerAndActionName(MethodInfo method)
        {
            string? controllerName = GetControllerName(method);
            string? ActionName = GetActionName(method?.Name);
            return $"{controllerName}/{ActionName}";
        }

        private static string? GetControllerName(MethodInfo method)
        {
            string? controllerFullName = method?.DeclaringType?.Name;
            string? controllerName = controllerFullName?.Substring(0, controllerFullName.Length - "controller".Length);
            return controllerName;
        }

        private static string? GetActionName(string methodName)
        {
            string[] httpVerbs = new[] { "GET", "POST", "PUT", "PATCH", "DELETE" };


            if (httpVerbs.Any(a=> a == methodName.ToUpper()))
            {
                return string.Empty;
            }
            else { return methodName; }
        }
    }
}
