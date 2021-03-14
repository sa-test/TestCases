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
        int AddUser(string name, string descr);

        [OperationContract]
        int DeleteUser(int id);

        [OperationContract]
        int UpdateUser(int id, string name, string descr);

        [OperationContract]
        List<User> GetData();


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
