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
    internal class StateKioskDb : DBCompactDatabase, IStateDb
    {
 
        #region constructor
        
        public StateKioskDb(string connectionString)
            : base(connectionString) { }

        public StateKioskDb()
            : base()
        { }

       

        #endregion

        #region GET

        /// <summary>
        /// Get all states
        /// </summary>
        public DataSet GetAllState()
        {
            DataSet ds = null;

            string sql = @"Select StateCode, Name FROM State Order by Name";
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

            string sql = @"Select StateCode, Name FROM State Order by Name";

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


        public State GetByStateCode(string stateCode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
