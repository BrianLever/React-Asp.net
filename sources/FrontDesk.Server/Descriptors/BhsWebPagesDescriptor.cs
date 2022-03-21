using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Descriptors
{
    public static class BhsWebPagesDescriptor
    {
        public const string VisitPageUrlTemplate = "~/bhs/BhsVisit.aspx?ID={0}";
        public const string DemographicsPageUrlTemplate = "~/bhs/BhsDemographics.aspx?ID={0}";
        public static string FollowUpPageUrlTemplate = "~/bhs/FollowUp.aspx?ID={0}";
    }
}
