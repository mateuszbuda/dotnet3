using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.WebClient.Misc
{
    public class WCFProvider : Controller
    {
        protected IWarehousesService WarehousesService { get; set; }

        public WCFProvider()
        {
            //try
            var warehouseChannelFactory = new ChannelFactory<IWarehousesService>("SecureBinding_IWarehousesService");
            warehouseChannelFactory.Credentials.UserName.UserName = "admin";
            warehouseChannelFactory.Credentials.UserName.Password = "test";
            WarehousesService = warehouseChannelFactory.CreateChannel();
        }
    }
}