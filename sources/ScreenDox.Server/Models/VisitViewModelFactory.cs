using ScreenDox.Server.Models;

namespace ScreenDox.Server.Screening
{
    public static class VisitViewModelFactory
    {
        public static VisitViewModel Create(VisitViewModelBase model)
        {
            return new VisitViewModel
            {
                ID = model.ID,
                ScreeningResultID = model.ScreeningResultID,
                ScreeningDate = model.ScreeningDate,
                CreatedDate = model.CreatedDate,
                CompletedDate = model.CompletedDate,
                HasAddress = model.HasAddress,
                IsVisitRecordType = model.IsVisitRecordType,
                LocationName = model.LocationName
            };
        }
    }
}