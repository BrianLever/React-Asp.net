using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPMS.Data.BMXNet.Framework
{
    public struct ConnectionInfo
    {
        public string ServerAddress { get; set; }
        public short Port { get; set; }
        public string Namespace { get; set; }
        public string AccessCode { get; set; }
        public string VerifyCode { get; set; }
        public string Division { get; set; }
        public string AppContext { get; set; }


        public override string ToString()
        {
            return string.Format("{0}:{1}/{2}. P: {3}^{4} Division: {5}",
                this.ServerAddress,
                this.Port,
                this.Namespace,
                this.AccessCode,
                this.VerifyCode,
                this.Division
                );
        }
    }
}
