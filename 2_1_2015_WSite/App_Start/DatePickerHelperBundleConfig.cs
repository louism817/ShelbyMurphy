using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(_2_1_2015_WSite.App_Start.DatePickerHelperBundleConfig), "RegisterBundles")]

namespace _2_1_2015_WSite.App_Start
{
	public class DatePickerHelperBundleConfig
	{
		public static void RegisterBundles()
		{
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
            "~/Scripts/bootstrap-datepicker.js",
            "~/Scripts/locales/bootstrap-datepicker.*"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/datepicker").Include(
            "~/Content/bootstrap-datepicker.css"));
		}
	}
}