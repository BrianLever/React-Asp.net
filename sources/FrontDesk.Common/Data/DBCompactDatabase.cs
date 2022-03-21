using System;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlTypes;


namespace FrontDesk.Common.Data
{



    /// <summary>
    /// Provide basic interface for MS SQL database Compact edition 
    /// </summary>
    /// <remarks>
    /// Create derived class to implement Database Access Logic in your application
    /// </remarks>
    public abstract class DBCompactDatabase : DBDatabase
    {
        /// <summary>
        /// Get database access factory name.
        /// Returns "System.Data.SqlClient" string be default
        /// </summary>
        protected override string DatabaseFactoryName
        {
            get { return "System.Data.SqlServerCe.4.0"; }
        }

        protected override int GetDefaultCommandTimeout()
        {
            return 0;
        }

        /// <summary>
        /// Create new object
        /// </summary>
        /// <param name="sConnectionString">Connection string</param>
        public DBCompactDatabase(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public DBCompactDatabase()
            : base()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["KioskConnection"].ConnectionString;
            ConnectionObject.ConnectionString = connectionString;


            //string connectionString = string.Format(@"Data Source={0}\Data\FrontDeskKiosk.sdf;Persist Security Info=false",
            //    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

            this.ConnectionObject.ConnectionString = connectionString;

        }

    }



}
