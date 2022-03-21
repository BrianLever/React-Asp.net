using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace FrontDesk
{
    public class State
    {
        #region public property

        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
	    public string StateCode {get; set;}

        public string CountryCode {get; set;}

        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
        public string Name {get; set;}

        #endregion

        #region constructor
        
        public State()
        {
            this.StateCode = String.Empty;
            this.CountryCode = String.Empty;
            this.Name = String.Empty;
        }

        public State(IDataReader reader)
        {
            this.StateCode = Convert.ToString(reader["StateCode"]);
            this.Name = Convert.ToString(reader["Name"]);
        }

        #endregion

        //private static List<State> _cachedStates = null;

        private static bool _useLocalDatabaseConnection = false;
        /// <summary>
        /// If true user SQL Compact database provider. False by default
        /// </summary>
        public static bool UseLocalDatabaseConnection
        {
            get { return _useLocalDatabaseConnection; }
            set { _useLocalDatabaseConnection = value; }
        }


        /// <summary>
        /// Get new database object
        /// </summary>
        private static Data.IStateDb DbObject
        {
            get
            {
                if (!_useLocalDatabaseConnection)
                {
                    return new Data.StateDatabase();
                }
                else
                {
                    return new Data.StateKioskDb();
                }
            }
        }

        #region GET static

        /// <summary>
        /// Get all state
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in ObjectDataSource by name
        public static DataSet GetAllState()
        {
            return DbObject.GetAllState();
        }

        /// <summary>
        /// Get the complete list of states
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in ObjectDataSource by name
        public static List<State> GetList()
        {
            return DbObject.GetList();
        }

        public static State GetByStateCode(string stateCode)
        {
            return DbObject.GetByStateCode(stateCode);
        }

        #endregion
    }
}
