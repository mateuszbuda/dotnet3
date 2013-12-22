using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WMS.ServicesInterface.DataContracts
{
    /// <summary>
    /// Genryczny kontrakt odpowiedzi z serwera
    /// </summary>
    /// <typeparam name="T">Typ danych w odpowiedzi</typeparam>
    [DataContract]
    public class Response<T>
    {
        /// <summary>
        /// Id zapytania, na które jest dana odpowiedź
        /// </summary>
        [DataMember]
        public Guid RequestId { get; protected set; }

        /// <summary>
        /// Dane odpowiedzi
        /// </summary>
        [DataMember]
        public T Data { get; set; }

        /// <summary>
        /// Konstruktor odpowiedzi
        /// </summary>
        /// <param name="requestId">Id zapytania, na które jest dana odpowiedź</param>
        /// <param name="data">Dane odpowiedzi</param>
        public Response(Guid requestId, T data)
        {
            RequestId = requestId;
            Data = data;
        }
    }
}
