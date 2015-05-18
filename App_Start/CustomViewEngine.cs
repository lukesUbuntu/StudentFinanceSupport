using System.Web.Mvc;

public class CustomViewEngine : RazorViewEngine
{
    public CustomViewEngine()
    {
        var viewLocations =  new[] {  
            "~/Views/{1}/{0}.aspx",  
            "~/Views/{1}/{0}.cshtml",  
            "~/Views/Shared/{0}.aspx",  
            "~/Views/Shared/{0}.cshtml",  
            "~/AnotherPath/Views/{0}.ascx"
            // etc
        };

        this.PartialViewLocationFormats = viewLocations;
        this.ViewLocationFormats = viewLocations;
    }
    
}