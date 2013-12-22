using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DataContracts
{
    /// <summary>
    /// Bazowy kontrakt zapytania
    /// </summary>
    [DataContract]
    public class Request
    {
        /// <summary>
        /// Id zapytania
        /// </summary>
        [DataMember]
        private readonly Guid _id = Guid.NewGuid();

        public Guid Id
        {
            get { return _id; }
        }
    }

    /// <summary>
    /// Generyczny kontrakt zapytania z danymi do przekazania
    /// </summary>
    /// <typeparam name="T">Typ danych przesyłanych w zapytaniu</typeparam>
    [DataContract]
    public class Request<T> : Request
    {
        /// <summary>
        /// Konstruktor zapytania z danymi do przesłania
        /// </summary>
        /// <param name="content">Dane do przesłania w zapytaniu</param>
        public Request(T content)
        {
            Content = content;
        }

        /// <summary>
        /// Dane przekazywane w zapytaniu
        /// </summary>
        [DataMember]
        public T Content { get; set; }
    }
}
