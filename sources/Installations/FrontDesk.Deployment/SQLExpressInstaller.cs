using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Configuration.Install;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;
using Microsoft.Win32;


namespace FrontDesk.Deployment
{
    /// <summary>
    /// Install SQL 2008 Express
    /// </summary>
    public class SQLExpressInstaller
    {
        #region SQL Server Instance Names

        const string ColumnInstanceName = "InstanceName";
        const string ColumnFullServerName = "FullServerName";
        const string ColumnVersionName = "Version";
        const string FullServerNameExpression = "IIF(LEN(InstanceName) > 0, ServerName + '\\' + InstanceName, ServerName)";


        /// <summary>
        /// Enumerates all SQL Server instances on the machine.
        /// </summary>
        /// <returns></returns>
        public static bool CheckSQLInstanceExists(string instanceName, string version)
        {
            ProductVersion searchedVersion = new ProductVersion(version);

            instanceName = instanceName.ToUpper();
            string correctNamespace = GetCorrectWmiNameSpace();
            if (string.Equals(correctNamespace, string.Empty))
            {
                return false;
            }
            string query = string.Format("select * from SqlServiceAdvancedProperty where SQLServiceType = 1 and PropertyName = 'instanceID'");
            ManagementObjectSearcher getSqlEngine = new ManagementObjectSearcher(correctNamespace, query);
            if (getSqlEngine.Get().Count == 0)
            {
                return false;
            }
            Debug.WriteLine("SQL Server database instances discovered :");
            string instance = string.Empty;
            string serviceName = string.Empty;
            ProductVersion ver = new ProductVersion();
            string edition = string.Empty;
            Debug.WriteLine("Instance Name \t ServiceName \t Edition \t Version \t");
            foreach (ManagementObject sqlEngine in getSqlEngine.Get())
            {
                serviceName = sqlEngine["ServiceName"].ToString();
                instance = GetInstanceNameFromServiceName(serviceName);
                ver.VersionAsString = GetWmiPropertyValueForEngineService(serviceName, correctNamespace, "Version");
                edition = GetWmiPropertyValueForEngineService(serviceName, correctNamespace, "SKUNAME");

                if (instance == instanceName && ver > searchedVersion)
                {
                    return true;
                }


                Debug.Write("{0} \t", instance);
                Debug.Write("{0} \t", serviceName);
                Debug.Write("{0} \t", edition);
                Debug.WriteLine("{0} \t", ver.VersionAsString);
            }
            return false;
        }



        public static List<string> BrowseSQlServers(ProductVersion minVersion)
        {
            List<string> list = new List<string>();
            var version = new ProductVersion();

            DataTable sqlServers = SqlDataSourceEnumerator.Instance.GetDataSources();
            sqlServers.Columns.Add(ColumnFullServerName, typeof(string), FullServerNameExpression);
            foreach (DataRow row in sqlServers.Rows)
            {

                version.VersionAsString = Convert.ToString(row[ColumnVersionName]);

                if (version >= minVersion) list.Add(Convert.ToString(row[ColumnFullServerName]));
            }

            return list;
        }

        /// <summary>
        /// Method returns the correct SQL namespace to use to detect SQL Server instances.
        /// </summary>
        /// <returns>namespace to use to detect SQL Server instances</returns>
        public static string GetCorrectWmiNameSpace()
        {
            String wmiNamespaceToUse = "root\\Microsoft\\sqlserver";
            List<string> namespaces = new List<string>();
            try
            {
                // Enumerate all WMI instances of
                // __namespace WMI class.
                ManagementClass nsClass =
                    new ManagementClass(
                    new ManagementScope(wmiNamespaceToUse),
                    new ManagementPath("__namespace"),
                    null);
                foreach (ManagementObject ns in
                    nsClass.GetInstances())
                {
                    namespaces.Add(ns["Name"].ToString());
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("Exception = " + e.Message);
            }
            if (namespaces.Count > 0)
            {
                if (namespaces.Contains("ComputerManagement10"))
                {
                    //use katmai+ namespace
                    wmiNamespaceToUse = wmiNamespaceToUse + "\\ComputerManagement10";
                }
                else if (namespaces.Contains("ComputerManagement"))
                {
                    //use yukon namespace
                    wmiNamespaceToUse = wmiNamespaceToUse + "\\ComputerManagement";
                }
                else
                {
                    wmiNamespaceToUse = string.Empty;
                }
            }
            else
            {
                wmiNamespaceToUse = string.Empty;
            }
            return wmiNamespaceToUse;
        }

        /// <summary>
        /// method extracts the instance name from the service name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static string GetInstanceNameFromServiceName(string serviceName)
        {
            if (!string.IsNullOrEmpty(serviceName))
            {
                if (string.Equals(serviceName, "MSSQLSERVER", StringComparison.OrdinalIgnoreCase))
                {
                    return serviceName;
                }
                else
                {
                    return serviceName.Substring(serviceName.IndexOf('$') + 1, serviceName.Length - serviceName.IndexOf('$') - 1);
                }
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Returns the WMI property value for a given property name for a particular SQL Server service Name
        /// </summary>
        /// <param name="serviceName">The service name for the SQL Server engine serivce to query for</param>
        /// <param name="wmiNamespace">The wmi namespace to connect to </param>
        /// <param name="propertyName">The property name whose value is required</param>
        /// <returns></returns>
        public static string GetWmiPropertyValueForEngineService(string serviceName, string wmiNamespace, string propertyName)
        {
            string propertyValue = string.Empty;
            string query = String.Format("select * from SqlServiceAdvancedProperty where SQLServiceType = 1 and PropertyName = '{0}' and ServiceName = '{1}'", propertyName, serviceName);
            ManagementObjectSearcher propertySearcher = new ManagementObjectSearcher(wmiNamespace, query);
            foreach (ManagementObject sqlEdition in propertySearcher.Get())
            {
                propertyValue = sqlEdition["PropertyStrValue"].ToString();
            }
            return propertyValue;
        }
        #endregion

        #region Run Custom Installation command
        private const string SqlInstallCommandFileName = @"install_sqlexpress.cmd";
        private const string SqlInstallCommandFilePath = @"SQLExpress\SQLEXPR_x{0}_ENU";



        public static string GetPathToSqlInstallationx86()
        {
            return GetPathToSqlInstallationx86(System.Environment.CurrentDirectory);

        }

        public static string GetPathToSqlInstallationx86(string currentDirectory)
        {
            var fullCommandPath = Path.Combine(currentDirectory, string.Format(SqlInstallCommandFilePath, "86"));
            fullCommandPath = Path.Combine(fullCommandPath, SqlInstallCommandFileName);
            return fullCommandPath;

        }

        public static void RunSqlInstallationx86(string suPassword, string instanceName)
        {
            RunSqlInstallationx86(suPassword, instanceName, GetPathToSqlInstallationx86());
        }


        public static void RunSqlInstallationx86(string suPassword, string instanceName, string commandFilePath)
        {


            if (!File.Exists(commandFilePath))
            {
                new InstallException(string.Format(Properties.Resources.Installation_SQLFileNotFound, commandFilePath));
            }


            //check that we have installed sql express with the right version and the same name
            if (!CheckSQLInstanceExists(instanceName, "10."))
            {

                //                var cmd = string.Format(@"/QS /action=install 
                ///features=SQLEngine 
                ///Instancename=""{0}""
                ///INDICATEPROGRESS 
                ///SQLSVCACCOUNT=""NT AUTHORITY\Network Service""
                ///SQLSVCSTARTUPTYPE=Automatic 
                ///SQLSYSADMINACCOUNTS=""Builtin\Administrators""
                ///SECURITYMODE=SQL
                ///SAPWD=""{1}""
                ///TCPENABLED=1 
                ///x86", instanceName, suPassword);
                ProcessStartInfo startinfo = new ProcessStartInfo(commandFilePath);
                startinfo.CreateNoWindow = true;
                startinfo.Arguments = string.Format("\"{0}\" \"{1}\"", suPassword, instanceName);
                startinfo.CreateNoWindow = false;
                startinfo.UseShellExecute = false;
                //startinfo.Arguments = cmd;



                startinfo.WorkingDirectory = Path.GetDirectoryName(commandFilePath);


                using (Process svnExecute = Process.Start(startinfo))
                {

                    svnExecute.WaitForExit();
                    if (svnExecute.ExitCode != 0)
                    {
                        new InstallException(Properties.Resources.Installation_SQLServerInstallFailed);
                    }
                }
            }
        }

        #endregion

        #region Create database from script
        /// <summary>
        /// Create database from script
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="sqlFileName"></param>
        public static void CreateDatabase(string serverName, string username, string password, string sqlFileName)
        {
            SqlConnectionStringBuilder connStr = new SqlConnectionStringBuilder();
            connStr.DataSource = serverName;
            connStr.IntegratedSecurity = true;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connStr.ToString();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                StringBuilder sqlBatchFile = new StringBuilder();

                //read batch file
                using (System.IO.StreamReader sr = new StreamReader(sqlFileName))
                {
                    sqlBatchFile.Append(sr.ReadToEnd());
                }

                //replace tags
                sqlBatchFile.Replace("{PASSWORD}", password);


                //SqlTransaction tran = null;

                string[] sqlCmds = sqlBatchFile.ToString().Replace("GO", "~").Split(new Char[] { '~' });


                try
                {
                    conn.Open();
                    //tran = conn.BeginTransaction();
                    //cmd.Transaction = tran;
                    foreach (var sql in sqlCmds)
                    {
                        cmd.CommandText = sql;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            throw new InstallException(string.Format(Properties.Resources.Installation_SQLServerInstallFailedOnCommand, cmd.CommandText), ex);
                        }
                    }

                    //tran.Commit();

                }
                catch (Exception ex)
                {
                    //if(tran != null)
                    //    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Test if application can connect to SQL Server using Windows Authentication
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="connectionError"></param>
        /// <returns></returns>
        public static bool TestConnection(string serverName, out string connectionError)
        {
            bool connected = false;
            connectionError = string.Empty;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = serverName;
            connectionString.IntegratedSecurity = true;


            conn.ConnectionString = connectionString.ToString();
            try
            {
                conn.Open();
                connected = true;
            }
            catch (Exception ex)
            {
                connectionError = ex.Message;
                connected = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return connected;
        }
        #endregion


        #region MSI 4.5 Installer

        public static ProductVersion GetInstalledMsiVersion()
        {
            ProductVersion version = new ProductVersion();
            //1. find msi.dll in the systemroot
            string pathToMsi = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "msi.dll");

            FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(pathToMsi);
            version.VersionAsString = info.FileVersion;

            return version;
        }

        public static string GetMsi45InstallationFile()
        {

            var osInfo = OSInfo.GetOsInfo();


            //var os = System.Environment.OSVersion;
            string fileName = string.Empty;

            if (osInfo.Platform == PlatformArchitecture.IA64)
            {
                if (osInfo.Version >= "6.0")
                {
                    //Vista RTM and later
                    //Windows Server 2008 RTM and later

                    fileName = "Windows6.0-KB942288-v2-ia64.MSU";
                }
                else if (osInfo.Version >= "5.2.1")
                {
                    //Windows 2003 Service Pack 1 or later
                    fileName = "WindowsServer2003-KB942288-v4-ia64.exe";
                }
            }
            else if (osInfo.Platform == PlatformArchitecture.x86)
            {
                if (osInfo.Version >= "6.0")
                {
                    //Vista RTM and later
                    //Windows Server 2008 RTM and later
                    fileName = "Windows6.0-KB942288-v2-x86.MSU";
                }
                else if (osInfo.Version < "5.2.0" &&
                    osInfo.Version >= "5.1.2")
                {
                    //Windows XP Service Pack 2 and later
                    fileName = "WindowsXP-KB942288-v3-x86.exe";
                }
                else if (osInfo.Version > "5.2.0")
                {
                    //Windows 2003 Service Pack 1 or later
                    fileName = "WindowsServer2003-KB942288-v4-x86.exe";
                }
            }

            else if (osInfo.Platform == PlatformArchitecture.x64)
            {
                if (osInfo.Version >= "6.0")
                {
                    //Vista RTM and later
                    //Windows Server 2008 RTM and later
                    fileName = "Windows6.0-KB942288-v2-x64.MSU";

                }
                else if (osInfo.Version >= "5.1.2"
                    && !(osInfo.Version >= "5.2.0" && osInfo.Version < "5.2.1"))
                {
                    //Windows XP Service Pack 2 and later
                    //Windows 2003 Service Pack 1 or later
                    fileName = "WindowsServer2003-KB942288-v4-x64.exe";
                }

            }


            return fileName;
        }

        /// <summary>
        /// Start installation for Msi 4.5
        /// </summary>
        /// <param name="rootDirectory">Directory, where installation files are placed</param>
        /// <returns></returns>
        public static int RunMsi45Installation(string rootDirectory)
        {

            var msiFile = GetMsi45InstallationFile();

            var pathToExec = Path.Combine(rootDirectory, msiFile);


            if (!File.Exists(pathToExec))
            {
                throw new FileNotFoundException(string.Format(Properties.Resources.Installation_Msi45FileNotFound, pathToExec));
            }



            ProcessStartInfo startinfo = new ProcessStartInfo(pathToExec);
            startinfo.CreateNoWindow = false;
            startinfo.UseShellExecute = true;



            using (Process svnExecute = Process.Start(startinfo))
            {

                svnExecute.WaitForExit();
                return svnExecute.ExitCode;
            }

        }


        #endregion


        #region PowerShell 1.0 Installer
        /// <summary>
        /// Check if Powershell is installed
        /// </summary>
        /// <returns></returns>
        public static bool GetIfPowershellIsInstalled()
        {
            bool exists = false;
            string powerShellRegKey = @"SOFTWARE\Microsoft\PowerShell\1";
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(powerShellRegKey, false);
            if (regKey != null)
            {
                var version = regKey.GetValue("Install");
                if (version != null)
                {
                    exists = true;
                }
                regKey.Close();
            }

            return exists;
        }

        public static string GetPowershellInstallationFile()
        {

            var osInfo = OSInfo.GetOsInfo();


            //var os = System.Environment.OSVersion;
            string fileName = string.Empty;

            if (osInfo.Platform == PlatformArchitecture.IA64)
            {
                if (osInfo.Version >= "6.0")
                {
                    //nothing
                }
                else if (osInfo.Version >= "5.2")
                {
                    //Windows 2003 Service Pack 1 or later
                    fileName = "WindowsServer2003-KB926139-v2-ia64-ENU.exe";
                }
            }
            else if (osInfo.Platform == PlatformArchitecture.x86)
            {
                if (osInfo.Version >= "6.0")
                {
                    //Vista RTM and later
                    //Windows Server 2008 RTM and later
                    fileName = "Windows6.0-KB928439-x86.msu";
                }
                else if (osInfo.Version < "5.2.0" &&
                    osInfo.Version >= "5.1.2")
                {
                    //Windows XP Service Pack 2 and later
                    fileName = "WindowsXP-KB926139-v2-x86-ENU.exe";
                }
                else if (osInfo.Version > "5.2.0")
                {
                    //Windows 2003 Service Pack 1 or later
                    fileName = "WindowsServer2003-KB926139-v2-x86-ENU.exe";
                }
            }

            else if (osInfo.Platform == PlatformArchitecture.x64)
            {
                if (osInfo.Version >= "6.0")
                {
                    //Vista RTM and later
                    //Windows Server 2008 RTM and later
                    fileName = "Windows6.0-KB928439-x64.msu";

                }
                else
                {
                    //Windows XP Service Pack 2 and later
                    //Windows 2003 Service Pack 1 or later
                    fileName = "WindowsServer2003.WindowsXP-KB926139-v2-x64-ENU.exe";
                }

            }


            return fileName;
        }

        /// <summary>
        /// Start installation for powershell
        /// </summary>
        /// <param name="rootDirectory">Directory, where installation files are placed</param>
        /// <returns></returns>
        public static int RunPowershellInstallation(string rootDirectory)
        {

            var installFile = GetPowershellInstallationFile();

            var pathToExec = Path.Combine(rootDirectory, installFile);


            if (!File.Exists(pathToExec))
            {
                throw new FileNotFoundException(string.Format(Properties.Resources.Installation_PowershellFileNotFound, pathToExec));
            }



            ProcessStartInfo startinfo = new ProcessStartInfo(pathToExec);
            startinfo.CreateNoWindow = false;
            startinfo.UseShellExecute = true;



            using (Process svnExecute = Process.Start(startinfo))
            {

                svnExecute.WaitForExit();
                return svnExecute.ExitCode;
            }

        }


        #endregion



        public static bool IsDatabaseExists(string serverName, string userName, string password, string dbName)
        {
            SqlConnectionStringBuilder connStr = new SqlConnectionStringBuilder();
            connStr.DataSource = serverName;
            connStr.InitialCatalog = "master";
            connStr.IntegratedSecurity = true;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connStr.ToString();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"
IF DB_ID('FrontDesk') IS NULL
    SET @IsExists = 0
ELSE 
    SET @IsExists = 1
";

                SqlParameter parameter = new SqlParameter("@IsExists", SqlDbType.Bit);
                parameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parameter);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return Convert.ToBoolean(parameter.Value);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
