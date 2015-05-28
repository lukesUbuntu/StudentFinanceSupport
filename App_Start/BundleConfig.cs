using System.Web;
using System.Web.Optimization;

namespace StudentFinanceSupport
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            /*
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            */
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/Plugins/bootstrap/dist/js/bootstrap.js",
                      //"~/Scripts/jquery-ui.js",
                      "~/Scripts/respond.js"));

           

            //content/core
            bundles.Add(new StyleBundle("~/Content/core").Include(
                      "~/Content/Plugins/bootstrap/dist/css/bootstrap.css",
                      "~/Content/Plugins/metisMenu/dist/metisMenu.css",
                      "~/Content/site.css"
                      )
            );

            //content/font-awesome (plugin)
            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                      "~/Content/Plugins/font-awesome/css/font-awesome.css"));

            //**MORRIS PLUGIN**/
            //content/morris (css)
            bundles.Add(new StyleBundle("~/Content/morris").Include(
                      "~/Content/Plugins/morrisjs/morris.css"));

            //bundles/morris (js)
            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                      "~/Content/Plugins/raphael/raphael.js",
                      "~/Content/Plugins/morrisjs/morris.js"
                     
                      
                      ));


            bundles.Add(new ScriptBundle("~/bundles/adminScripts").Include(
                        "~/Content/Plugins/metisMenu/dist/metisMenu.js",
                        "~/Scripts/site.js"));
        


            //wizardSteps
            bundles.Add(new ScriptBundle("~/bundles/wizardSteps").Include(
                    "~/Content/Plugins/datepicker/datepicker.js",
                     "~/Scripts/wizardSteps.js"));

            bundles.Add(new StyleBundle("~/Content/Wizard").Include(
                    "~/Content/Plugins/datepicker/datepicker.css",
                     "~/Content/Wizard.css")
            );

         
            
            //datatables
            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                     "~/Content/Plugins/datatables/datatables-responsive/css/dataTables.responsive.css",
                     "~/Content/Plugins/datatables/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.css"
                      )
             );
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                    "~/Content/Plugins/datatables/media/js/jquery.dataTables.min.js",
                    "~/Content/Plugins/datatables/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"));

            //password strength metor
            bundles.Add(new ScriptBundle("~/bundles/pwstrength").Include(
                   "~/Content/Plugins/password/pwstrength.js"));
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
