using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka dla przesunięcia
    /// </summary>
    public class ShiftDto
    {
        /// <summary>
        /// Id przesunięcia
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id orzesuwanej partii
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// Id magazynu nadawcy
        /// </summary>
        public int SenderId { get; set; }
        /// <summary>
        /// Nazwa magazynu nadawcy
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// Data przesunięcia
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Id magazynu odbiorcy
        /// </summary>
        public int WarehouseId { get; set; }
        /// <summary>
        /// Nazwa magazynu odbiorcy
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// Id docelowego sektora
        /// </summary>
        public int RecipientSectorId { get; set; }
        /// <summary>
        /// Czy docelowy magazyn jest magazynem wewnętrznym czy partnera
        /// </summary>
        public bool Internal { get; set; }
        /// <summary>
        /// Wersja
        /// </summary>
        public byte[] Version { get; set; }
    }

    /// <summary>
    /// Paczka dla grupy
    /// </summary>
    public class GroupDto // do użycia w oknie 5 jako info o grupie
    {
        /// <summary>
        /// Id grupy
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nazwa magazynu w którym aktualnie znajduje się partia
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// Id sektora w którym aktualnie znajduje się partia
        /// </summary>
        public int SectorId { get; set; }
        /// <summary>
        /// Numer sektora w którym aktualnie znajduje się partia
        /// </summary>
        public int SectorNumber { get; set; }
        /// <summary>
        /// Czy partia znajduje się w wewnętrznym magazynie czy w w magazynie partnera
        /// </summary>
        public bool Internal { get; set; }
        /// <summary>
        /// Wersja
        /// </summary>
        public byte[] Version { get; set; }
    }

    /// <summary>
    /// Paczka dla grupy wraz z informacjami o przesyłanych produktach i ich ilościach
    /// </summary>
    public class GroupDetailsDto : GroupDto
    {
        /// <summary>
        /// Lista produktów i ich liczba w danym przesunięciu
        /// </summary>
        public List<ProductDetailsDto> Products { get; set; }
    }

    /// <summary>
    /// Paczka dla historii przesunięć
    /// </summary>
    public class ShiftHistoryDto // okno 6, 9
    {
        /// <summary>
        /// Id przesunięcia
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id magazynu nadawcy
        /// </summary>
        public int SenderId { get; set; }
        /// <summary>
        /// Nazwa magazynu nadawcy
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// Id magazynu odbiorcy
        /// </summary>
        public int RecipientId { get; set; }
        /// <summary>
        /// Nazwa magazynu odbiorcy
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// Data przesunięcia
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Czy docelowy magazyn jest magazynem wewnętrznym czy partnera
        /// </summary>
        public bool Internal { get; set; }
    }
}
