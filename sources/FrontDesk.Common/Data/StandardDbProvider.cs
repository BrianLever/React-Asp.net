using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;

namespace FrontDesk.Common.Data
{
    public class StandardDbProvider : IDbProvider
    {
        protected DbProviderFactory providerFactory = null;
        private object _factoryLock = new object();

        /// <summary>
        /// Get database access factory name.
        /// Returns "System.Data.SqlClient" string be default
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual string DatabaseFactoryName
        {
            get { return "System.Data.SqlClient"; }
        }


        protected virtual void EnsureDbFactoryIsCreated()
        {
            if (providerFactory == null)
            {
                lock (_factoryLock)
                    if (providerFactory == null)
                    {
                        ///Get SQL PRovider Factory
                        ///this can be overriden from derived class 
                        providerFactory = DbProviderFactories.GetFactory(this.DatabaseFactoryName); //"System.Data.SqlClient"
                    }
            }
        }

        public virtual IDbConnection CreateConnection()
        {

            EnsureDbFactoryIsCreated();
            return providerFactory.CreateConnection();
        }

        public virtual IDbCommand CreateCommand()
        {
            EnsureDbFactoryIsCreated();
            return providerFactory.CreateCommand();
        }

        public virtual IDataAdapter CreateDataAdapter(IDbCommand selectCommand)
        {
            EnsureDbFactoryIsCreated();
            var adapter = providerFactory.CreateDataAdapter();
            adapter.SelectCommand = (DbCommand)selectCommand;
            return adapter;
        }

        public virtual IDataParameter CreateParameter()
        {
            EnsureDbFactoryIsCreated();
            return providerFactory.CreateParameter();
        }

        public virtual IDataParameter CreateParameter(int size)
        {
            var param = (DbParameter)CreateParameter();
            param.Size = size;
            return param;
        }
    }
}
