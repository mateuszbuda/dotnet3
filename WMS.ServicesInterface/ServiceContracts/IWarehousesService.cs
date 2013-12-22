using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.ServicesInterface.ServiceContracts
{
    /// <summary>
    /// Interfejs do wymiany ifnormacji o magazynach i sektorach
    /// </summary>
    [ServiceContract]
    public interface IWarehousesService
    {
        /// <summary>
        /// Zwraza listę wszystkich wewnętrznych i nieusuniętych magazynów
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą żądanych magazynów</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<WarehouseDetailsDto>> GetWarehouses(Request request);

        /// <summary>
        /// Zwraca listę magazynów partnerów.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Lista partnerów</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<WarehouseDetailsDto>> GetPartnersWarehouses(Request request);

        /// <summary>
        /// Usuwa magazyn jeśli jest pusty
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu do usunięcia</param>
        /// <returns>Odpowiedź z informację czy operacja się powiodła (czy magazyn był pusty)</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> DeleteIfEmpty(Request<int> warehouseId);

        /// <summary>
        /// Usuwa sektor jeśli jest pusty
        /// </summary>
        /// <param name="sectorId">Zapytanie z id sektora do usunięcia</param>
        /// <returns>Odpowiedź z informację czy operacja się powiodła (czy sektor był pusty)</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> DeleteSectorIfEmpty(Request<int> sectorId);

        /// <summary>
        /// Pobiera statyski związane z systemem
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź ze statysykami</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<StatisticsDto> GetStatistics(Request request);

        /// <summary>
        /// Pobiera informacjie o zadanym przez id magazynie
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu</param>
        /// <returns>Odpowiedź z żądanym magazynem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<WarehouseInfoDto> GetWarehouse(Request<int> warehouseId);

        /// <summary>
        /// Pobiera informacjie o magazynie w którym znajduje się grupa o zadanym id
        /// </summary>
        /// <param name="groupId">Zapytanie z id grupy</param>
        /// <returns>Odpowiedź z żądanym magazynem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<WarehouseInfoDto> GetWarehouseByGroup(Request<int> groupId);

        /// <summary>
        /// Pobiera sektory dla zadanego magazynu
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu</param>
        /// <returns>Odpowiedź z listą sektorów</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<SectorDto>> GetSectors(Request<int> warehouseId);

        /// <summary>
        /// Pobiera informacje o zadanym sektorze
        /// </summary>
        /// <param name="sectorId">apytanie z id sektora</param>
        /// <returns>Odpowiedź z żądanym sektorem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<SectorDto> GetSector(Request<int> sectorId);

        /// <summary>
        /// Dodaje nowy sektor
        /// </summary>
        /// <param name="sector">Zapytanie z sektorem do dodania</param>
        /// <returns>Odpowiedź z dodanym sektorem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<SectorDto> AddSector(Request<SectorDto> sector);

        /// <summary>
        /// Dodaje nowy magazyn
        /// </summary>
        /// <param name="warehouse">Zapytanie z magazynem do dodania</param>
        /// <returns>Odpowiedź z dodanym magazynem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<WarehouseInfoDto> AddNew(Request<WarehouseInfoDto> warehouse);

        /// <summary>
        /// Edycja danych o magazynie
        /// </summary>
        /// <param name="warehouse">Zapytanie z wyedytowanym magazynem</param>
        /// <returns>Odpowiedź z wyedytowanym magazynem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<WarehouseInfoDto> Edit(Request<WarehouseInfoDto> warehouse);

        /// <summary>
        /// Zwraca numer kolejnego wolnego numeru sektora dla zadanego magazynu
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu</param>
        /// <returns>Odpowiedź z kolejnym numerem wolnego sektora</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<int> GetNextSectorNumber(Request<int> warehouseId);

        /// <summary>
        /// Edycja istniejącego sektora
        /// </summary>
        /// <param name="sector">Zapytanie z wyedytowanym sektorem</param>
        /// <returns>Odpowiedź z wyedytowanym sektorem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<SectorDto> EditSector(Request<SectorDto> sector);
    }
}
