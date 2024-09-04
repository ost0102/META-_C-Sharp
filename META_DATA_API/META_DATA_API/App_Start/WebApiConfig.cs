using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace META_DATA_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //config.MapHttpAttributeRoutes();
            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);
            // Web API 경로                                    
            //config.EnableCors(cors);

            // Web API 구성 및 서비스
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
