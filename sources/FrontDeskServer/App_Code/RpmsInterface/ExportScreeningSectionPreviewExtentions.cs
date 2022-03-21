using FrontDesk.Configuration;

using RPMS.Common.Models;


namespace EhrInterface
{
    public static class ExportScreeningSectionPreviewExtentions
    {
        public static string GetName(this ExportScreeningSectionPreview value)
        {
            if (value == null) return string.Empty;

            return ScreeningFrequencyDescriptor.GetName(value.ScreeningSectionID);
        }
    }
}