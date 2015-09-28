using System;
using System.Web.Optimization;

namespace EasyWeixin.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region 重新定义排除那些js，css类型
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);
            #endregion
            //设置这个可以覆盖Web.config文件中compilption 节点的debug属性
            //启用压缩
            BundleTable.EnableOptimizations = true;
            #region
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/christmas").Include(
                       "~/js/Activity/christmas.js"));
            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            #endregion
            //add zr
            #region ~/css/adminlayout
            bundles.Add(new StyleBundle("~/css/adminlayout").Include(
                "~/js/kindeditor-4.1.7/plugins/code/prettify.css",
                "~/css/bootstrap.css",
                "~/css/jquery.gritter.css",
                "~/css/bootstrap-responsive.css"));
            #endregion
            #region 
            bundles.Add(new ScriptBundle("~/js/kindeditor").Include(
                "~/js/kindeditor-4.1.7/plugins/code/prettify.js",
                "~/js/kindeditor-4.1.7/kindeditor.js",
                "~/js/kindeditor-4.1.7/lang/zh_CN.js"
            ));
            #endregion
            #region ~/js/adminlayout
            bundles.Add(new ScriptBundle("~/js/adminlayout").Include(
                
                ));
            #endregion
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}