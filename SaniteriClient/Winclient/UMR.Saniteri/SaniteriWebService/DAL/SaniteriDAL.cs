using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaniteriWebService.DTO;
using System.Data;
using System.Data.SqlClient;
using UMR.Saniteri.Data;
using UMR.Saniteri.DataFactory;

namespace SaniteriWebService.DAL
{
    public class SaniteriDAL
    {
        public static InventoryDTO GetInventoryInfo(Int64 id)
        {
            try
            {
                using (var context = DatabaseManager.server.GetMainEntities())
                {
                    var inventoryDTO = new InventoryDTO();
                    var _canDetails = context.can_inventory.Where(r => r.can_id == id).FirstOrDefault();
                    if (_canDetails == null) return null;
                    inventoryDTO.CanId = (Int64)_canDetails.can_id;
                    inventoryDTO.ProductionDate = _canDetails.production_date;
                    inventoryDTO.InServiceDate = _canDetails.in_service_date;
                    inventoryDTO.Street = _canDetails.street;
                    inventoryDTO.Additional = _canDetails.additional;
                    inventoryDTO.City = _canDetails.city;
                    inventoryDTO.Zip = _canDetails.zip;
                    inventoryDTO.Floor = _canDetails.floor;
                    inventoryDTO.Room = _canDetails.room;
                    inventoryDTO.Custom1 = _canDetails.custom_1;
                    inventoryDTO.Custom2 = _canDetails.custom_2;
                    inventoryDTO.Custom3 = _canDetails.custom_3;
                    inventoryDTO.IpAddress = _canDetails.ip_address;
                    return inventoryDTO;
                }
            }
            catch
            {
                return null;
            }
        }

        public static List<InventoryDTO> GetAllInventoryInfo()
        {
            try
            {
                using (var context = DatabaseManager.server.GetMainEntities())
                {
                    var inventoryDTO = context.can_inventory.Select(inv => new InventoryDTO
                    {
                        CanId = inv.can_id,
                        ProductionDate = inv.production_date,
                        InServiceDate = inv.in_service_date,
                        Street = inv.street,
                        Additional = inv.additional,
                        City = inv.city,
                        Zip = inv.floor,
                        Room = inv.room,
                        Custom1 = inv.custom_1,
                        Custom2 = inv.custom_2,
                        Custom3 = inv.custom_3
                    }).ToList();
                    return inventoryDTO;
                }
            }
            catch
            {
                return null;
            }
        }

        public static MaintenanceDTO GetMaintenanceInfo(Int64 canId, DateTime serviceDate)
        {
            try
            {
                using (var context = DatabaseManager.server.GetMainEntities())
                {

                    var maintenanceDTO = new MaintenanceDTO();
                    var _canMaintenance = context.can_maintenance.Where(r => r.can_id == canId && r.service_date == serviceDate).FirstOrDefault();
                    maintenanceDTO.CanId = _canMaintenance.can_id;
                    maintenanceDTO.ServiceDate = _canMaintenance.service_date;
                    maintenanceDTO.ServicePerformed = _canMaintenance.service_performed;
                    return maintenanceDTO;
                }
            }
            catch
            {
                return null;
            }
        }

        public static TransactionLogDTO GetTransactionLog(Int64 canId, DateTime eventTimeStamp)
        {
            try
            {
                using (var context = DatabaseManager.server.GetMainEntities())
                {
                    var transactionLogDTO = new TransactionLogDTO();
                    var _canEventLog = context.can_transaction_log.Where(r => r.can_id == canId && r.event_time_stamp == eventTimeStamp).FirstOrDefault();
                    if (_canEventLog == null) return null;

                    transactionLogDTO.CanId = _canEventLog.can_id;
                    transactionLogDTO.EventTimeStamp = _canEventLog.event_time_stamp;
                    transactionLogDTO.EventType = _canEventLog.event_type;
                    transactionLogDTO.EventDescription = _canEventLog.event_description;
                    transactionLogDTO.EventData = _canEventLog.event_data;
                    return transactionLogDTO;
                }
            }
            catch 
            {
                return null;
            }
        }


        public static Boolean InsertCanCommand(Int64 canId, int commandId)
        {
            try
            {
                using (var context = DatabaseManager.server.GetMainEntities())
                {
                    var count = context.can_command.Count();
                    var newCommand = new can_command();
                    newCommand.seqno = count + 1;
                    newCommand.command_id = commandId;
                    newCommand.can_id = canId;
                    context.can_command.AddObject(newCommand);
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static List<String> GetAllCanId()
        {
            List<String> listOfAllCanId = new List<string>();
            using (var context = DatabaseManager.server.GetMainEntities())
            {
                var canIdList = context.can_inventory;
                foreach (var item in canIdList)
                    listOfAllCanId.Add(item.can_id.ToString());
            }
            return listOfAllCanId;           
        }


        public static CanStatus  GetCanStatus(Int64 canId,DateTime eDate)
        {           
            CanStatus canStatus = new CanStatus();
            using (var context = DatabaseManager.server.GetMainEntities())
            {
                var _canStatus = context.can_status.Where(c => c.can_id == canId && c.edate == eDate).FirstOrDefault();
                if (_canStatus == null) return null;

                canStatus.CanId = _canStatus.can_id;
                canStatus.StatusType = _canStatus.status_type;
                canStatus.eDate = _canStatus.edate;
                canStatus.StatusDescription = _canStatus.status_description;
                return canStatus;
            }
        }

        public static CanLiveStatus GetCanLiveStatus(Int64 canId)
        {
            var canLiveStatus = new CanLiveStatus();
            using (var context = DatabaseManager.server.GetMainEntities())
            {
                var _canStatus = context.can_livestatus.Where(c => c.can_id == canId).FirstOrDefault();
                if (_canStatus == null) return canLiveStatus;

                canLiveStatus.CanID = _canStatus.can_id;
                canLiveStatus.NeedService = _canStatus.need_service;
                canLiveStatus.LidOpen = _canStatus.lid_open;
                canLiveStatus.DoorOpen = _canStatus.door_open;
                canLiveStatus.Fault = _canStatus.fault;
                canLiveStatus.Weight = _canStatus.weight;
                canLiveStatus.BagInfo = _canStatus.bag_info;
                canLiveStatus.PowerStatus = _canStatus.power_status;
                canLiveStatus.CommunicationStatus = _canStatus.comm_status;
                return canLiveStatus;
            }
        }
    }
}