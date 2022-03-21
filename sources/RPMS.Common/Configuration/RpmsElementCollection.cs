using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace RPMS.Common.Configuration
{
    public class RpmsElementCollection<TElement> : ConfigurationElementCollection
         where TElement : ConfigurationElement, IRpmsConfigurationElement, new()
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new TElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TElement)element).GetKey();
        }

        public TElement this[int index]
        {
            get { return (TElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new TElement this[string key]
        {
            get { return (TElement)BaseGet(key); }
            set
            {
                if (BaseGet(key) != null)
                {
                    BaseRemove(key);
                }
                BaseAdd(value);
            }
        }
    }


    public interface IRpmsConfigurationElement
    {
        object GetKey();
    }
}
