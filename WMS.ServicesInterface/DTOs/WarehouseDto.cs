using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka z danymi ogólnymi na temat magazynu
    /// </summary>
    public class WarehouseInfoDto
    {
        /// <summary>
        /// Id magazynu
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Czy jest wewnętrznym magazynem czy zewnętrznym (partnera)
        /// </summary>
        public bool Internal { get; set; }
        /// <summary>
        /// Telefon kontaktowy do magazynu
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// Mail do magazynu
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// Nazwa magazynu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ulica magazynu
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// Numer lokalu magazynu
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// Miasto w którym jest magazyn
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Kod pocztowy magazynu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Czy magazyn został usunięty
        /// </summary>
        public bool Deleted { get; set; }
        public byte[] Version { get; set; }
    }

    /// <summary>
    /// Paczka z informacjami o magazynie i jego strukturze
    /// </summary>
    public class WarehouseDetailsDto
    {
        /// <summary>
        /// Id magazynu
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nazwa magazynu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Czy jest wewnętrznym magazynem czy zewnętrznym (partnera)
        /// </summary>
        public bool Internal { get; set; }
        /// <summary>
        /// Liczba nieusuniętych sektorów w magazynie
        /// </summary>
        public int SectorsCount { get; set; }
        /// <summary>
        /// Liczba wolnych sektorów w magazynie
        /// </summary>
        public int FreeSectorsCount { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
