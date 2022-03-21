using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;

namespace RPMS.Common
{
    public class RpmsExportException : Exception
    {
        public ExportFault Fault { get; set; }

        public string ActionName { get; set; }

        public RpmsExportException() { }

        public RpmsExportException(string actionName, ExportFault fault) {
            this.ActionName = actionName;
            this.Fault = fault;
        }

    }
}
