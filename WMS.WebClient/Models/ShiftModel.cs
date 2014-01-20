using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.WebClient.Models
{
    /// <summary>
    /// Model dla produktu w przesunięciu (zawiera informacje o ilości)
    /// </summary>
    public class ProductInShift
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Model dla przesunięcia
    /// </summary>
    public class ShiftModel
    {
        public int SenderId { get; set; }
        public int WarehouseId { get; set; }
        public int SectorId { get; set; }
        public IEnumerable<ProductInShift> Products { get; set; }
    }
}