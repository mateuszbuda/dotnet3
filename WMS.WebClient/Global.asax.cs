using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WMS.ServicesInterface.Misc;

namespace WMS.WebClient
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            //DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(CurrencyAttribute), typeof(RegularExpressionAttributeAdapter));
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pl-PL");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat = new CultureInfo(EnGBCultureKey).DateTimeFormat;
        }
    }
}