using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka na statystyki o magazynach przedsiębiorstwa
    /// </summary>
    public class StatisticsDto
    {
        /// <summary>
        /// Liczba magazynów wewnętrzych wprowadzonych do systemu
        /// </summary>
        public int WarehousesCount { get; set; }
        /// <summary>
        /// Liczba produktów wprowadzonych do systemu
        /// </summary>
        public int ProductsCount { get; set; }
        /// <summary>
        /// Liczba partnerów wprowadzonych do systemu
        /// </summary>
        public int PartnersCount { get; set; }
        /// <summary>
        /// Liczba aktualnie przetwarzanych partii (tych w magazynach wewnętrznych)
        /// </summary>
        public int GroupsCount { get; set; }
        /// <summary>
        /// Liczba wszystkich wykonanych przesunięć
        /// </summary>
        public int ShiftsCount { get; set; }
        /// <summary>
        /// Wskaźnik zapełnienie magazynów
        /// Stosunek liczby zapełnionych sektórów w nieusuniętych wewnętrznych magazynach
        /// do liczby wszystkich sektórów w nieusuniętych wewnętrznych magazynach
        /// </summary>
        public int FIllRate { get; set; }
    }
}
