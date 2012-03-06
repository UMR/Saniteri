using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using SaniteriWebService.DTO;
using SaniteriWebService.DAL;


namespace SaniteriWebService
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class SaniteriWebService
    {
        // TODO: Implement the collection resource that will contain the SampleItem instances

        [WebGet(UriTemplate = "LogIn?userID={userID}&password={password}", ResponseFormat = WebMessageFormat.Json)]
        public bool LogIn(string userID, string password)
        {
            try
            {
                return true;
            }

            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return false;
            }
        }

        [WebGet(UriTemplate = "GetMaintenanceInfo?CanId={canId}&ServiceDate={serviceDate}", ResponseFormat = WebMessageFormat.Json)]
        public MaintenanceDTO GetMaintenanceInfo(string canId, string serviceDate)
        {
            try
            {
                MaintenanceDTO maintenanceDTO = SaniteriDAL.GetMaintenanceInfo(Int64.Parse(canId), DateTime.Parse(serviceDate));
                return maintenanceDTO;
            }
            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return null;
            }
        }


        [WebGet(UriTemplate = "GetTransactionLog?CanId={canId}&EventTimeStamp={eventTimeStamp}", ResponseFormat = WebMessageFormat.Json)]
        public TransactionLogDTO GetTransactionLog(string canId, string eventTimeStamp)
        {
            try
            {
                TransactionLogDTO transactionLogDTO = SaniteriDAL.GetTransactionLog(Int64.Parse(canId), DateTime.Parse(eventTimeStamp));
                return transactionLogDTO;
            }
            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return null;
            }


        }
        
        [WebGet(UriTemplate = "GetInventoryInfo?Id={id}", ResponseFormat = WebMessageFormat.Json)]
        public InventoryDTO GetInventoryInfo(string id)
        {
            try
            {
                InventoryDTO inventoryDTO = SaniteriDAL.GetInventoryInfo(Int64.Parse(id));
                return inventoryDTO;
            }
            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return null;
            }
        }

        [WebGet(UriTemplate = "GetAllInventoryInfo", ResponseFormat = WebMessageFormat.Json)]
        public List<InventoryDTO> GetAllInventoryInfo()
        {
            try
            {
                List<InventoryDTO> inventoryDTO = SaniteriDAL.GetAllInventoryInfo();
                return inventoryDTO;
            }
            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return null;
            }
        }

        [WebGet(UriTemplate = "GetAllCanId", ResponseFormat = WebMessageFormat.Json)]

        public List<string> GetAllCanId()
        {
            List<string> listOfAllCanId = new List<string>();
            try
            {
                listOfAllCanId = SaniteriDAL.GetAllCanId();
                return listOfAllCanId;
            }
            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return null;
            }
        }


        [WebInvoke(UriTemplate = "InsertCanCommand", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public Boolean InsertCanCommand(string canId, string commandId)
        {
            try
            {
                if (SaniteriDAL.InsertCanCommand(Int64.Parse(canId), Convert.ToInt32(commandId)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return false;
            }

        }


        [WebGet(UriTemplate = "GetCanStatus?CanId={canId}&EventTime={eDate}", ResponseFormat = WebMessageFormat.Json)]
        public CanStatus GetCanStatus(string canId, string eDate)
        {
            CanStatus canStatus = new CanStatus();
            try
            {
                canStatus = SaniteriDAL.GetCanStatus(Int64.Parse(canId), DateTime.Parse(eDate));
                return canStatus;
            }

            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return null;
            }
        }

        [WebGet(UriTemplate = "GetCanLiveStatus?CanId={canId}", ResponseFormat = WebMessageFormat.Json)]
        public CanLiveStatus GetCanLiveStatus(string canID)
        {
            try
            {
                var _canLiveStatus = SaniteriDAL.GetCanLiveStatus(Int64.Parse(canID));
                return _canLiveStatus;
            }

            catch (Exception e)
            {
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.UnsupportedMediaType;
                response.StatusDescription = e.Message;
                return null;
            }
        }

    }
}
