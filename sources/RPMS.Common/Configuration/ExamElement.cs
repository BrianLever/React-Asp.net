using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace RPMS.Common.Configuration
{
    public class ExamElement : ConfigurationElement, IRpmsConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)base["key"]; }
        }


        [ConfigurationProperty("id", IsRequired = true)]
        [IntegerValidator()]
        public int Id
        {
            get { return (int)base["id"]; }
        }
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        [ConfigurationProperty("code", IsRequired = true)]
        public string Code
        {
            get { return (String)base["code"]; }
        }


        #region IRpmsConfigurationElement Members

        public object GetKey()
        {
            return this.Key;
        }

        #endregion
    }
}
