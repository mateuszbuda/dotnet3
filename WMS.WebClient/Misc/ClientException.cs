using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.WebClient.Misc
{
    public class ClientException : Exception
    {
        public ClientException(string message) : base(message) { }
    }
}