using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaniteriWebService.DTO;
using System.Data;
using System.Data.SqlClient;
using UMR.Saniteri.Data;

namespace SaniteriWebService.DAL
{
    public class SaniteriDAL
    {
        public static InventoryDTO GetInventoryInfo(Guid id)
        {
            try
            {
                using (var context = DBManager.GetMainEntities())
                {
                    var inventoryDTO = new InventoryDTO();
                    var _canDetails = context.can_inventory.Where(r => r.can_id == id).FirstOrDefault();
                    if (_canDetails == null) return null;
                    inventoryDTO.CanId = (Guid)_canDetails.can_id;
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

        public static MaintenanceDTO GetMaintenanceInfo(Guid canId, DateTime serviceDate)
        {
            try
            {
                using (var context = DBManager.GetMainEntities())
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

        public static TransactionLogDTO GetTransactionLog(Guid canId, DateTime eventTimeStamp)
        {
            try
            {
                using (var context = DBManager.GetMainEntities())
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


        public static Boolean InsertCanCommand(Guid canId, int canLidStatus)
        {
            try
            {
                using (var context = DBManager.GetMainEntities())
                {
                    var newCommand = new can_command();
                    newCommand.command_id = Guid.NewGuid();
                    newCommand.can_id = canId;
                    newCommand.can_lid_status = Convert.ToByte(canLidStatus);
                    newCommand.command_timestamp = DateTime.Now;
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
            using (var context = DBManager.GetMainEntities())
            {
                var canIdList = context.can_inventory;
                foreach (var item in canIdList)
                    listOfAllCanId.Add(item.can_id.ToString());
            }
            return listOfAllCanId;           
        }


        public static CanStatus  GetCanStatus(Guid canId,DateTime eDate)
        {           
            CanStatus canStatus = new CanStatus();
            using (var context = DBManager.GetMainEntities())
            {
                var _canStatus = context.can_status.Where(c=>c.can_id==canId&&c.edate==eDate).FirstOrDefault();
                if (_canStatus == null) return null;

                canStatus.CanId = _canStatus.can_id;
                canStatus.StatusType = _canStatus.status_type;
                canStatus.eDate = _canStatus.edate;
                canStatus.StatusDescription = _canStatus.status_description;

                return canStatus;
            }
        
        }

    }
}