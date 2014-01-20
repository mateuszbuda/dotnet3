using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.WebClient.Models
{
    /// <summary>
    /// Model dla błędów
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// Tutuł błedu
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Szczegóły błedu
        /// </summary>
        public string Message { get; set; }

        public ErrorMessage(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}