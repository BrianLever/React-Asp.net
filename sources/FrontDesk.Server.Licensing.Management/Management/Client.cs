using System;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace FrontDesk.Server.Licensing.Management
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed - need for datasources on pages
    public class Client
    {
        public int ClientID;
        public string CompanyName;
        public string StateCode;
        public string City;
        public string AddressLine1;
        public string AddressLine2;
        public string PostalCode;

        public string Email;
        public string ContactPerson;
        public string ContactPhone;

        public string Notes;
        public DateTimeOffset LastModified;

        private static ClientDb DbObject
        {
            get
            {
                return new ClientDb();
            }
        }

        public Client()
        {
        }

        public Client(IDataReader reader)
        {
            this.ClientID = Convert.ToInt32(reader["ClientID"]);
            this.CompanyName = Convert.ToString(reader["CompanyName"]);
            this.StateCode = Convert.ToString(reader["StateCode"]);
            this.City = Convert.ToString(reader["City"]);
            this.AddressLine1 = Convert.ToString(reader["AddressLine1"]);
            this.AddressLine2 = Convert.ToString(reader["AddressLine2"]);
            this.PostalCode = Convert.ToString(reader["PostalCode"]);
            this.Email = Convert.ToString(reader["Email"]);
            this.ContactPerson = Convert.ToString(reader["ContactPerson"]);
            this.ContactPhone = Convert.ToString(reader["ContactPhone"]);
            this.Notes = Convert.ToString(reader["Notes"]);

            this.LastModified = (DateTimeOffset)reader["LastModified"];
        }

        [Obfuscation(Feature = "renaming", Exclude = true)] // used in ObjectDataSource by name
        public static DataSet GetAll()
        {
            return DbObject.GetAll();
        }

        /// <summary>
        /// Get all clients with paging
        /// </summary>
        public static DataSet GetAllWithPaging(int startRowIndex, int maximumRows, string orderBy)
        {
            return DbObject.GetAllWithPaging(startRowIndex, maximumRows, orderBy);
        }

        public static Client GetByID(int clientID)
        {
            return DbObject.GetByID(clientID);
        }

        public static int Add(Client client)
        {
            return DbObject.Add(client);
        }

        public bool Update()
        {
            return DbObject.Update(this);
        }

        public static void Delete(int clientId)
        {
            DbObject.Delete(clientId);
        }

        /// <summary>
        /// Get client count
        /// </summary>
        public static int GetCount()
        {
            return DbObject.GetCount();
        }

    }
}
