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

        [WebGet(UriTemplate = "GetMaintenanceInfo?CanId={canId}&ServiceDate={serviceDate}", ResponseFormat = WebMessageFormat.Json)]
        public MaintenanceDTO GetMaintenanceInfo(String canId, String serviceDate)
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
        public TransactionLogDTO GetTransactionLog(String canId, String eventTimeStamp)
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
        public InventoryDTO GetInventoryInfo(String id)
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

        [WebGet(UriTemplate = "GetAllCanId", ResponseFormat = WebMessageFormat.Json)]

        public List<String> GetAllCanId()
        {
            List<String> listOfAllCanId = new List<string>();
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
        public Boolean InsertCanCommand(String commandId, String canId, String canLidStatus, String commandTimeStamp)
        {
            try
            {
                if (SaniteriDAL.InsertCanCommand(Int64.Parse(canId), Convert.ToInt32(canLidStatus)))
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
        public CanStatus GetCanStatus(String canId, String eDate)
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

    }
}
