using System.Web;
using System.Web.Optimization;

namespace LearnLanguages.Mvc4
{
  public class BundleConfig
  {
    // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
    public static void RegisterBundles(BundleCollection bundles)
    {
      
      #region Jquery Scripts

      bundles.Add(new ScriptBundle(Mvc4Resources.ScriptVirtualPathJQuery).Include(
                  "~/Scripts/jquery-1.9*"));

      bundles.Add(new ScriptBundle(Mvc4Resources.ScriptVirtualPathJQueryUi).Include(
                  "~/Scripts/jquery-ui*"));

      bundles.Add(new ScriptBundle(Mvc4Resources.ScriptVirtualPathJQueryVal).Include(
                  "~/Scripts/jquery.unobtrusive*",
                  "~/Scripts/jquery.validate*"));

      bundles.Add(new ScriptBundle(Mvc4Resources.ScriptVirtualPathJQueryMobile).Include(
                  "~/Scripts/jquery.mobile*"));

      #endregion

      bundles.Add(new ScriptBundle(Mvc4Resources.ScriptVirtualPathModernizr).Include(
                  "~/Scripts/modernizr-*"));

      bundles.Add(new ScriptBundle(Mvc4Resources.ScriptVirtualPathCustom).Include(
                  "~/Scripts/custom*"));

      #region Styles

      bundles.Add(new StyleBundle(Mvc4Resources.StyleVirtualPathSite).Include(
                  "~/Content/site.css"));

      bundles.Add(new StyleBundle(Mvc4Resources.StyleVirtualPathJQueryMobile).Include(
                  "~/Content/jquery.mobile*"));

      bundles.Add(new StyleBundle(Mvc4Resources.StyleVirtualPathJQueryUi).Include(
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

    }
  }
}