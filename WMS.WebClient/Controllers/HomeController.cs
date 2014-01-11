using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.ServicesInterface.DataContracts;
using WMS.WebClient.Misc;

namespace WMS.WebClient.Controllers
{
    /// <summary>
    /// Menu główne aplikacji
    /// </summary>
    public class HomeController : WCFProvider
    {
        /// <summary>
        /// Menu główne i statystyki
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        public ActionResult Index()
        {
            return Execute(() => WarehousesService.GetStatistics(new Request()).Data);
        }
    }
}
