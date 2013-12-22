using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.Entity;
using WMS.Services.Assemblers;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.ServiceContracts;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface;

namespace WMS.Services
{
    /// <summary>
    /// Serwis obsługujący zapytania związane z partnerami
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class PartnersService : ServiceBase, IPartnersService
    {
        /// <summary>
        /// Pobiera informacje o wszyskich partnerach z systemu, co ciekawe potencjalnie tych usuniętych też
        /// </summary>
        /// <param name="request">Puste zapytanie</param>
        /// <returns>Odpowiedź z listą partnerów</returns>
        public Response<List<PartnerSimpleDto>> GetPartners(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<PartnerSimpleDto>>(request.Id, Transaction(tc =>
                tc.Entities.Partners.Include(x => x.Warehouse).
                Select(partnerAssembler.ToSimpleDto).ToList()));
        }

        /// <summary>
        /// Pobiera dane o partnerze o zadanym id, nawet jeśli został usunięty
        /// </summary>
        /// <param name="partnerId">Zapytanie z id partnera</param>
        /// <returns>Odpowiedź z danymi o partnerze z zadanym id</returns>
        public Response<PartnerDto> GetPartner(Request<int> partnerId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<PartnerDto>(partnerId.Id, Transaction(tc =>
                tc.Entities.Partners.Where(x => x.Id == partnerId.Content).Include(x => x.Warehouse).
                Select(partnerAssembler.ToDto).FirstOrDefault()));
        }

        /// <summary>
        /// Pobiera dane o partnerze z magazynem o zadanym id, nawet jeśli ten partner został usunięty
        /// </summary>
        /// <param name="warehouseId">Zapytanie z id magazynu partnera</param>
        /// <returns>Odpowiedź z danymi o partnerze z zadanym id</returns>
        public Response<PartnerDto> GetPartnerByWarehouse(Request<int> warehouseId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<PartnerDto>(warehouseId.Id, Transaction(tc =>
                tc.Entities.Partners.Where(x => x.WarehouseId == warehouseId.Content).Include(x => x.Warehouse).
                Select(partnerAssembler.ToDto).FirstOrDefault()));
        }

        /// <summary>
        /// Pobiera listę przesunięć, których nadawcą lub odbiorcą był dany partner
        /// </summary>
        /// <param name="partnerId">Zapytanie z id partnera</param>
        /// <returns>Odpowiedź z listą żądanych przesunięć</returns>
        public Response<List<ShiftHistoryDto>> GetPartnerHistory(Request<int> partnerId)
        {
            CheckPermissions(PermissionLevel.Manager);
            return new Response<List<ShiftHistoryDto>>(partnerId.Id, Transaction(tc =>
                {
                    int wId = tc.Entities.Partners.Where(x => x.Id == partnerId.Content).Select(x => x.WarehouseId).FirstOrDefault();
                    return tc.Entities.Shifts.Where(s => (s.SenderId == wId || s.Group.Sector.WarehouseId == wId)).
                    Include(x => x.Sender).Include(x => x.Group.Sector.Warehouse).Select(groupAssembler.ToShiftHistoryDto).ToList();
                }));
        }

        /// <summary>
        /// Dodaje nowego partnera, lub rzuca wjątek w przypadku niepowodzenia
        /// </summary>
        /// <param name="partner">Zapytanie z partnerem do dodania</param>
        /// <returns>Odpowiedź z dodanym partnerem</returns>
        public Response<PartnerDto> AddNew(Request<PartnerDto> partner)
        {
            CheckPermissions(PermissionLevel.Manager);
            Partner p = null;
            Transaction(tc => p = tc.Entities.Partners.Add(partnerAssembler.ToEntity(partner.Content)));
            return new Response<PartnerDto>(partner.Id, partnerAssembler.ToDto(p));
        }

        /// <summary>
        /// Etytuje dane o istniejącym partnerze
        /// </summary>
        /// <param name="partner">Zapytanie z wyedytowanym partnerem</param>
        /// <returns>Odpowiedź z wyedytowanym partnerem</returns>
        public Response<PartnerDto> Update(Request<PartnerDto> partner)
        {
            CheckPermissions(PermissionLevel.Manager);
            PartnerDto ret = null;

            Transaction(tc =>
                {
                    var p = tc.Entities.Partners.Where(x => x.Id == partner.Content.Id).Include(x => x.Warehouse).FirstOrDefault();
                    if (p == null)
                        throw new FaultException<ServiceException>(new ServiceException("Ten partner nie istnieje!"));

                    partnerAssembler.ToEntity(partner.Content, p);
                    tc.Entities.SaveChanges();
                    ret = partnerAssembler.ToDto(p);
                });

            return new Response<PartnerDto>(partner.Id, ret);
        }

    }
}
