using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Entity;


namespace TestWCFserviceApp
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ITestDBService
    {

        [OperationContract]
        [WebInvoke(Method = "POST",
      UriTemplate = "/AddUser/{name}/{descr}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int AddUser(string name, string descr);

        [OperationContract]
        int DeleteUser(int id);

        [OperationContract]
        [WebInvoke(Method = "GET",
UriTemplate = "/DelUser/{id}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int DeleteUserWeb(string id);


 
        [OperationContract]
        [WebInvoke(Method = "PUT",
UriTemplate = "/UpdateUser/{name}/{descr}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int UpdateUser(int id, string name, string descr);

        [OperationContract]
        [WebInvoke(Method = "GET",
UriTemplate = "/UpdateUserWeb/{id}/{name}/{descr}", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]

        int UpdateUserWeb(string id, string name, string descr);


        [OperationContract]
        [WebInvoke(Method = "GET",
      UriTemplate = "/GetData", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<User> GetData();

        //GetDataArray is not work as I want. It still return the object, not array. But I found the way how to 
        //work with object inside frontend vue application
        // so this Contract can be deleted.
        [OperationContract]
        [WebInvoke(Method = "GET",
UriTemplate = "/GetDataArray", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        User[] GetDataArray();


        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.



    [DataContract]
    public class User
    {

        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }

        /*
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
        */
    }
}
