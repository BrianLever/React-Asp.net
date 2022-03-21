using ScreenDox.Server.Descriptors;
using ScreenDox.Server.Models;

using System;

namespace ScreenDox.Server.Screening
{
    public class VisitViewModel : VisitViewModelBase
    {
        public string Href
        {
            get
            {
                return string.Format(IsVisitRecordType ? RestApiDescriptor.VisitUrlTemplate : RestApiDescriptor.DemographicsUrlTemplate, ID);
            }
        }
    }
}