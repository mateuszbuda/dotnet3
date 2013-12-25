using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.WebClient.Models
{
    public class ErrorMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public ErrorMessage(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}