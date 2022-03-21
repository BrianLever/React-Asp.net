using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Deployment
{
    /// <summary>
    /// Product version that consists of 4 digits groups (i.e. 1.0.0.0)
    /// </summary>
    public class ProductVersion : IComparable<ProductVersion>, IEquatable<ProductVersion>, IEquatable<string>, IComparable<string>
    {
        private int[] _version = new int[4];

        public ProductVersion() { }

        public ProductVersion(string version)
        {
            _version = ParseVersionString(version);
        }
        /// <summary>
        /// parse version string
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        private int[] ParseVersionString(string version)
        {
            int[] ver = new int[4];
            if (!string.IsNullOrEmpty(version))
            {
                var groups = version.Split(new char[] { '.' });

                for (int i = 0; i < groups.Length && i < 4; i++)
                {
                    if (String.IsNullOrEmpty(groups[i]))
                        ver[i] = 0;
                    else
                        ver[i] = Int32.Parse(groups[i]);
                }
            }
            return ver;

        }

        #region Properties

        public int[] Version { get { return _version; } }
        public string VersionAsString
        {
            get
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < _version.Length; i++)
                {
                    str.AppendFormat("{0}.", _version[i]);
                }
                str.Length--;
                return str.ToString();
            }
            set
            {
                _version = ParseVersionString(value);
            }

        }

        /// <summary>
        /// is version equal to 0.0.0.0
        /// </summary>
        public bool IsEmpty
        {
            get { return _version == null || (_version[0] == 0 && _version[1] == 0 && _version[2] == 0 && _version[3] == 0); }
        }
        #endregion


        #region Operators

        public static bool operator ==(ProductVersion a, ProductVersion b)
        {
            if ((Object)a == null) a = new ProductVersion();
            return a.Equals(b);
        }

        public static bool operator !=(ProductVersion a, ProductVersion b)
        {
            if ((Object)a == null) a = new ProductVersion();
            return !a.Equals(b);
        }

        public static bool operator >(ProductVersion a, ProductVersion b)
        {
            if (a == null) a = new ProductVersion();
            if (b == null) b = new ProductVersion();
            return (a.CompareTo(b) > 0);
        }

        public static bool operator >=(ProductVersion a, ProductVersion b)
        {
            if (a == null) a = new ProductVersion();
            if (b == null) b = new ProductVersion();
            var cmpResult = a.CompareTo(b);
            return cmpResult == 0 || cmpResult > 0;
        }

        public static bool operator <(ProductVersion a, ProductVersion b)
        {
            if (a == null) a = new ProductVersion();
            if (b == null) b = new ProductVersion();
            var cmpResult = a.CompareTo(b);
            return cmpResult < 0;
        }

        public static bool operator <=(ProductVersion a, ProductVersion b)
        {
            if (a == null) a = new ProductVersion();
            if (b == null) b = new ProductVersion();
            var cmpResult = a.CompareTo(b);
            return cmpResult == 0 || cmpResult < 0;
        }

        #endregion

        #region IComparable<ProductVersion> Members
        /// <summary>
        /// Compare 2 version numbers
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ProductVersion other)
        {
            var a = this.Version;
            var b = other.Version;
            int result = 0;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                result = a[i].CompareTo(b[i]);
                if (result != 0) return result;
            }
            return result;
        }

        #endregion

        #region IEquatable<ProductVersion> Members


        public bool Equals(ProductVersion other)
        {
            if ((Object)other == null) other = new ProductVersion();

            return (this.CompareTo(other) == 0);
        }

        #endregion

        #region IEquatable<string> Members

        public bool Equals(string other)
        {
            return Equals(new ProductVersion(other));
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }


        #endregion

        #region IComparable<string> Members

        public int CompareTo(string other)
        {
            return CompareTo(new ProductVersion(other));
        }

        #endregion

        public static implicit operator ProductVersion(string version)
        {
            return new ProductVersion(version);
        }
       
        /// Need to override when overriding operator ==
        public override int GetHashCode()
        {
            int hash = 0;
            if (_version != null)
            {
                foreach (var i in _version)
                {
                    hash ^= i.GetHashCode();
                    
                }
            }
            return hash;
        }
      


    }
}
