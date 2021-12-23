using System.Web;
using System.Web.Optimization;

namespace OnlineLearningEnglish
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           
            bundles.Add(new StyleBundle("~/content/header").Include(
                      "~/Content/bootstrap.min.css",
                       "~/Content/font-awesome.min.css",
                       "~/Content/fontawesome.min.css",
                       "~/Content/fontawesome-all.min.css",
                       "~/Content/v4-shims.css",
                       "~/Content/css/ims-stylesheet.css",
                       "~/Content/css/ims-navbar-stylesheet.css",
                       "~/Content/css/ims-menu-stylesheet.css",
                       "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                       "~/Content/DataTables/css/dataTables.bootstrap4.min.css",
                       "~/Content/DataTables/css/responsive.dataTables.css",
                       "~/Content/css/gijgo.min.css",
                       "~/Content/css/jquery.easing.min.js",
                       "~/Content/css/select2.css",
                       "~/Content/css/editor.css",
                        "~/Content/w2ui-1.5.min.css",
                       "~/Content/css/select2-bootstrap4.css"));

            bundles.Add(new ScriptBundle("~/scripts/header").Include(
                        "~/Scripts/jquery-3.6.0.js",
                        "~/Scripts/jquery-ui-1.13.0.js",
                      //  "~/Scripts/editor.js"
                        "~/Content/w2ui-1.5.js"
                       ));
            bundles.Add(new ScriptBundle("~/scripts/footer").Include(
                       "~/Scripts/bootstrap.bundle.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.bootstrap4.js",
                        "~/Scripts/DataTables/dataTables.responsive.min.js"));
            bundles.Add(new ScriptBundle("~/scripts/blob").Include(
                       "~/Scripts/js/blob.js"));
        }
    }
}
