using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka z informacjami na temat produktu
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Id produktu
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nazwa produktu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Data produkcji produktu
        /// </summary>
        public DateTime ProductionDate { get; set; }
        /// <summary>
        /// Cena produktu
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Wersja
        /// </summary>
        public byte[] Version { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Paczka na informacje o produkcie i jego ilości w partii.
    /// </summary>
    public class ProductDetailsDto : ProductDto  // do użycia w podglądzie Partii (okno 5)
    {
        /// <summary>
        /// Ilość produktu w partii
        /// </summary>
        public int Count { get; set; }
    }
}
