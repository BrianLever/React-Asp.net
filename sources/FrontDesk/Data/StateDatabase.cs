using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Configuration;

using FrontDesk.Common.Data;

namespace FrontDesk.Data
{
    internal class StateDatabase : DBDatabase, IStateDb
    {
 
        #region constructor
        
        public StateDatabase(string connectionString)
            : base(connectionString) { }

        public StateDatabase()
            : base(0)
        { }

        public StateDatabase(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        #region GET

        /// <summary>
        /// Get all state
        /// </summary>
        public DataSet GetAllState()
        {
            DataSet ds = null;

            string sql = @"Select StateCode, Name FROM dbo.State Order by Name";
            CommandObject.Parameters.Clear();
            try
            {
                base.Disconnect();
                ds = GetDataSet(sql);
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }

            return ds;
        }

        /// <summary>
        /// Get the complete list of states
        /// </summary>
        public List<State> GetList()
        {
            List<State> states = new List<State>();

            string sql = @"Select StateCode, Name FROM dbo.State Order by Name";

            CommandObject.Parameters.Clear();

            try
            {
                Connect();

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        states.Add(new State(reader));
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                base.Disconnect();
            }

            return states;
        }

        /// <summary>
        /// Get state instance by state code
        /// </summary>
        public State GetByStateCode(string stateCode)
        {
            State state = null;

            string sql = @"Select * FROM dbo.State where StateCode=@StateCode";


            CommandObject.Parameters.Clear();
            AddParameter("@StateCode", DbType.String, 2).Value = stateCode;

            try
            {
                Connect();
                using (var r = RunSelectQuery(sql))
                {
                    if (r.Read())
                    {
                        state = new State(r);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

            return state;
        }

        #endregion

    }
}
