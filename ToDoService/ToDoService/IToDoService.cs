using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ToDoList
{
    /// <summary>
    /// This interface defines a collection of operations provided by ToDoService.svc.  Each method that 
    /// is annotated as an [OperationContract] will be exposed by the service.  The means by which the exposed method
    /// can be invoked is given in the [WebInvoke] annotation.
    /// </summary>
    [ServiceContract]
    public interface IToDoService
    {
        /// <summary>
        /// Returns a list of ToDoSummaries.  It returns a summary of just the incomplete
        /// tasks unless the includeCompleted parameter is true.  It returns summaries for all users,
        /// unless userName is provided.  The method and URL required to access the service appears
        /// in the WebInvoke annotation.  The response is encoded as JSON.
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "/GetAllItems?completed={includeCompleted}&user={userName}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        IList<ToDoItem> GetAllItems(bool includeCompleted, string userName);

        /// <summary>
        /// Adds an item to the ToDo list.  The item should be a ToDoItem encoded in
        /// JSON (the supplied Uid is ignored).  The response is the Uid, encoded as JSON.
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/AddItem",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string AddItem(ToDoItem data);

        /// <summary>
        /// Marks an item with the specified uid as completed.
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/MarkCompleted/{uid}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void MarkCompleted(string uid);

        /// <summary>
        /// Deletes the item with the specified uid.
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/DeleteItem/{uid}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void DeleteItem(string uid);
    }
}
