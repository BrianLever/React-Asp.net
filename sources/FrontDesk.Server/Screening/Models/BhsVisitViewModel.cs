using FrontDesk.Server.Descriptors;

using ScreenDox.Server.Models;

namespace FrontDesk.Server.Screening
{
    public class BhsVisitViewModel : VisitViewModelBase
    {
        public string DetailsPageUrl
        {
            get
            {
                return string.Format(IsVisitRecordType ? BhsWebPagesDescriptor.VisitPageUrlTemplate : BhsWebPagesDescriptor.DemographicsPageUrlTemplate, ID);
            }
        }
    }
}