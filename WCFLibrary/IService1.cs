using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundFault))]
        Book GetBook(string bookName);

        [OperationContract]
        [FaultContract(typeof(DuplicatePrimaryKey))]
        void SaveBook(Book book);

        [OperationContract]
        [FaultContract(typeof(NotFoundFault))]
        void DeleteBook(int bookId);

        [OperationContract]
        [FaultContract(typeof(NotFoundFault))]
        void RentBook(User user,Book book);

        [OperationContract]
        [FaultContract(typeof(DuplicatePrimaryKey))]
        void SaveUser(User user);

        [OperationContract]
        List<Book> ListBooks();

        [OperationContract]
        [FaultContract(typeof(ServiceData))]
        ServiceData ConnectionOpen();

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Book
    {
        int book_id;
        int price;
        string name;
        [DataMember]
        public int Book_id
        {
            get { return book_id; }
            set { book_id = value; }
        }
        [DataMember]
        public int Price
        {
            get { return price; }
            set { price = value; }
        }   
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }    
    }
    [DataContract]
    public class User
    {
        int user_id;
        string name;
        string phone_number;
        string password;
        [DataMember]
        public int User_id { 
            get { return user_id; } 
            set { user_id = value; } 
        }
        [DataMember]
        public string Name { get { return name; } set { name = value; } }
        [DataMember]
        public string PhoneNumber { get { return phone_number; } set { phone_number = value; } }
        [DataMember]
        public string Password { get { return password; } set { password = value; } }
    }
    [DataContract]
    public class ServiceData
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string ErrorDetails { get; set; }
    }
    [DataContract]
    public class NotFoundFault
    {
        public NotFoundFault(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public bool Result { get; set; }
    }
    [DataContract]
    public class DuplicatePrimaryKey
    {
        public DuplicatePrimaryKey(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }
       [DataMember]
       public string ErrorMessage { get; set; }
    }
}
