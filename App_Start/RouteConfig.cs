using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudentFinanceSupport
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //define our route login this will go straight to administrator controller and admin action
            routes.MapRoute(
                name: "Login",
                //null,
                url: "Administrators/Login",
                //url: "{controller}/{action}/{id}",
                defaults: new { controller = "Administrators", action = "Login", id = UrlParameter.Optional }
            );
            

            //set our default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Administrators", action = "Index", id = UrlParameter.Optional }
            );

            

            
        }
    }
}
