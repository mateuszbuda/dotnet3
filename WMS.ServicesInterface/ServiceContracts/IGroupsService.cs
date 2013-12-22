using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.ServicesInterface.ServiceContracts
{
    /// <summary>
    /// Interfejs do wymiany ifnormacji o partiach i przesunięciach
    /// </summary>
    [ServiceContract]
    public interface IGroupsService
    {
        /// <summary>
        /// Pobiera partie, znajdujące się aktualnie w zadanym sektorze.
        /// </summary>
        /// <param name="sectorId">Zapytanie z id sektora dla którego pobieramy znajdujące się w nim partie</param>
        /// <returns>Odpowiedź z listą partii</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<ShiftDto>> GetSectorGroups(Request<int> sectorId);

        /// <summary>
        /// Pobiera informacja o zadanej grupie
        /// </summary>
        /// <param name="groupId">Zapytanie z id grupy, o której informacje chcemy pobrać</param>
        /// <returns>Odpowiedź z informacją o grupie</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<GroupDto> GetGroupInfo(Request<int> groupId);

        /// <summary>
        /// Pobiera historię grupy jako listę przesunięć, jakie były wykonywane na tej grupie.
        /// </summary>
        /// <param name="groupId">Zapytanie z id grupy, o której informacje chcemy pobrać</param>
        /// <returns>Odpowiedź z informacją o grupie</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<ShiftDto>> GetGroupHistory(Request<int> groupId);

        /// <summary>
        /// Pobiera wszystkie przesunięcie, któr są ostatnimi dla swoich partii.
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą przesunięć</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<ShiftDto>> GetShifts(Request request);

        /// <summary>
        /// Pobiera informacje o produktach znajdujących się w partii.
        /// </summary>
        /// <param name="groupId">Zapytanie z id partii</param>
        /// <returns>Odpowiedź z listą szczegułów produktów przesyłanych w partii wraz z ich lościami</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<ProductDetailsDto>> GetGroupDetails(Request<int> groupId);

        /// <summary>
        /// Dodaje nowe przesunięcie i przesuwa partię aktualizując jej położenie
        /// i ustawiając ostatnie przesunięcie danej partii na bierzące.
        /// Jeśli jest to próba przesunięcia partii, która już została wydana
        /// do magazynu zewnętrznego (partnera), to rzucany jest wyjątek.
        /// </summary>
        /// <param name="shift">Zapytanie z przesunięciem, które ma być dodane</param>
        /// <returns>Odpowiedź z wykonanym przesunięciem</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> AddNewShift(Request<ShiftDto> group);

        /// <summary>
        /// Dodaje nową grupę wraz z pierwszym przesunięciem. Ilości produktów w tworzonej partii muszą być nieujemne,
        /// w przeciwnym przypadku rzucany jest wyjątek.
        /// </summary>
        /// <param name="group">Zapytanie z dodawaną grupą</param>
        /// <returns>Odpowiedź z dodaną grupą</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> AddNewGroup(Request<Tuple<GroupDetailsDto, ShiftDto>> group);

        /// <summary>
        /// Sprawdzenie, czy nadawca przesunięcia jest magazynem wewnętrznym.
        /// </summary>
        /// <param name="shift">Zapytanie z przesunięciem do sprawdzenia</param>
        /// <returns>Odpowiedź true albo false</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> IsSenderInternal(Request<ShiftDto> group);
    }
}
