using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DataContracts
{
    /// <summary>
    /// Wyjątek rzucany w przypadku błedu po stronie serwera
    /// </summary>
    [DataContract]
    public class ServiceException
    {
        /// <summary>
        /// Konstruktor wyjątku
        /// </summary>
        /// <param name="m">Wiadoność z informacją o błędzie</param>
        public ServiceException(string m)
        {
            Message = m;
        }

        /// <summary>
        /// Wiadomość z informacją o błędzie
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
