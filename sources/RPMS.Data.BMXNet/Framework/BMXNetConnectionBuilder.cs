using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndianHealthService.BMXNet;

namespace RPMS.Data.BMXNet.Framework
{
    public class BMXNetConnectionBuilder
    {
        public static ConnectionInfo FromConnectionString(string connectionString)
        {
            ConnectionInfo conn = new ConnectionInfo();
            if (!string.IsNullOrWhiteSpace(connectionString))
            {


                // Parse values from connect string
                string sSemi = ";";
                char[] trimChars = "' ".ToCharArray();
                string sEq = "=";
                string sU = "^";
                char[] cSemi = sSemi.ToCharArray();
                char[] cEq = sEq.ToCharArray();
                char[] cU = sU.ToCharArray();
                string[] saSplit = connectionString.Split(cSemi, StringSplitOptions.RemoveEmptyEntries);
                string[] saProp;
                string[] saTemp;
                string sPropName;
                string sPropVal;
                for (int j = 0; j < saSplit.Length; j++)
                {
                    saProp = saSplit[j].Split(cEq);
                    if (saProp.Length != 2)
                    {
                        //throw invalid parameter exception								
                    }
                    sPropName = saProp[0].ToUpperInvariant();
                    sPropVal = saProp[1];
                    if (!string.IsNullOrEmpty(sPropName))
                    {
                        sPropName = sPropName.Trim(trimChars);
                    }

                    if (sPropName == "SERVER" || sPropName == "DATA SOURCE")
                    {
                        conn.ServerAddress = sPropVal;
                    }
                    else if (sPropName == "NAMESPACE" || sPropName == "INITIAL CATALOG" || sPropName == "DATABASE")
                    {
                        conn.Namespace = sPropVal;
                    }
                    else if (sPropName == "PASSWORD")
                    {
                        saTemp = sPropVal.Split(cU);
                        if (saTemp.Length != 2)
                        {
                            //throw invalid parameter exception								
                        }

                        conn.AccessCode = saTemp[0];
                        conn.VerifyCode = saTemp[1];
                    }
                    else if (sPropName == "ACCESS CODE")
                    {
                        conn.AccessCode = sPropVal;
                    }
                    else if (sPropName == "VERIFY CODE")
                    {
                        conn.VerifyCode = sPropVal;
                    }
                    else if ((sPropName == "LOCATION") || (sPropName == "PORT"))
                    {
                        Int16 port = 0;
                        if (Int16.TryParse(sPropVal, out port))
                        {
                            conn.Port = port;
                        }
                    }

                    else if (sPropName == "EXTENDED PROPERTIES")
                    {
                        conn.AppContext = sPropVal;
                    }

                }


            }
            return conn;
        }
    }
}
