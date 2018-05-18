using System.Web;
using System.Web.Optimization;

namespace TimeSheet
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/logincss").Include("~/Content/Login.css"));

            bundles.Add(new StyleBundle("~/allStyle").Include(
               "~/bower_components/bootstrap/dist/css/bootstrap.css",
                "~/bower_components/bootstrap/dist/css/bootstrap-theme.css",
                "~/bower_components/jquery-ui/themes/base/all.css",
                "~/bower_components/angular-loading-bar/src/loading-bar.css",
                "~/bower_components/angular-dialog-service/dist/dialogs.css",
                "~/Content/Themes/SbAdmin/css/sb-admin.css",
                "~/Content/Genghis.css",
                "~/Content/alerts.css",
                "~/Scripts/Vendor/SbAdmin/plugins/datepicker/bootstrap-datetimepicker.css",
                "~/Scripts/Vendor/tiptps/tipTip.css"
                ));

            bundles.Add(new ScriptBundle("~/SystemScript").Include(

                "~/bower_components/jquery/dist/jquery.js",
                "~/bower_components/jquery-ui/jquery-ui.js",
                "~/bower_components/jquery-cookie/jquery.cookie.js",
                "~/bower_components/bootstrap/dist/js/bootstrap.js",
                "~/bower_components/angular/angular.js",
                "~/bower_components/angular-route/angular-route.js",
                "~/bower_components/angular-loading-bar/src/loading-bar.js",
                "~/bower_components/angular-bootstrap/ui-bootstrap.js",
                "~/bower_components/angular-bootstrap/ui-bootstrap-tpls.js",
                "~/bower_components/angular-sanitize/angular-sanitize.js",
                "~/bower_components/angular-dialog-service/dist/dialogs.js",
                "~/bower_components/angular-dialog-service/dist/dialogs-default-translations.js"
                ));

            bundles.Add(new ScriptBundle("~/CustomerScript").Include(
                "~/Scripts/app.js",
                "~/Scripts/directives.js",
                "~/Scripts/factories.js",
                "~/Scripts/services.js",
                "~/Scripts/controllers/controllers.js",
                "~/Scripts/controllers/logincontroller.js",
                "~/Scripts/controllers/DashBoardController.js",
                "~/Scripts/controllers/StatisticsController.js",
                "~/Scripts/controllers/ExceptionController.js",
                "~/Scripts/controllers/sharedcontroller.js",
                "~/Scripts/datatable/t2v-datatable.js",
                "~/Scripts/lib/t2v-lib.js",
                "~/Scripts/lib/t2v-core.js",
                "~/Scripts/Vendor/SbAdmin/plugins/metisMenu/jquery.metisMenu.js",
                "~/Scripts/Vendor/SbAdmin/sb-admin.js",
                "~/Scripts/Vendor/Splitter/Splitter.js",
                "~/Scripts/Vendor/tiptps/jquery.tipTip.js",
                 "~/Scripts/Vendor/bootstrap-treeview.js",
                "~/Scripts/Vendor/bootstrap-typeahead.js",
                "~/Scripts/Vendor/SbAdmin/plugins/datepicker/bootstrap-datetimepicker.js",
                "~/Scripts/webworker/StatisticsAnalysis.js",
                "~/Scripts/controllers/DetailController.js",
                "~/Scripts/controllers/SettingController.js",
                "~/Scripts/lib/t2v_StorageData.js",
                "~/Scripts/Vendor/Ueditor/ueditor.config.js",
                "~/Scripts/Vendor/Ueditor/ueditor.all.min.js",
                "~/Scripts/Vendor/Ueditor/lang/zh-cn.js"
                ));

            BundleTable.EnableOptimizations = false;
        }
    }
}