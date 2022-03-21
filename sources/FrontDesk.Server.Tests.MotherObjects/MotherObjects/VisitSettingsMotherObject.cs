using FrontDesk.Configuration;
using FrontDesk.Server.Screening.Models;

using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Tests.MotherObjects
{
    public static class VisitSettingsMotherObject
    {
        public static List<VisitSettingItem> GetAllOn()
        {
            return new List<VisitSettingItem>
            {
                new VisitSettingItem() { Id = VisitSettingsDescriptor.SmokerInHome, IsEnabled = true, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.TobaccoUseCeremony, IsEnabled = true, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.TobaccoUseSmoking, IsEnabled = true, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.TobaccoUseSmokeless, IsEnabled = true, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.Alcohol, IsEnabled = true, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.SubstanceAbuse, IsEnabled = true, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.Depression, IsEnabled = true, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.DepressionThinkOfDeath, IsEnabled = true, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.PartnerViolence, IsEnabled = true, CutScore = 1 },

            };
        }

        public static List<VisitSettingItem> GetAllOff()
        {
            return new List<VisitSettingItem>
            {
                new VisitSettingItem() { Id = VisitSettingsDescriptor.SmokerInHome, IsEnabled = false, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.TobaccoUseCeremony, IsEnabled = false, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.TobaccoUseSmoking, IsEnabled = false, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.TobaccoUseSmokeless, IsEnabled = false, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.Alcohol, IsEnabled = false, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.SubstanceAbuse, IsEnabled = false, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.Depression, IsEnabled = false, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.DepressionThinkOfDeath, IsEnabled = false, CutScore = 1 },
                new VisitSettingItem() { Id = VisitSettingsDescriptor.PartnerViolence, IsEnabled = false, CutScore = 1 },

            };
        }

        public static List<VisitSettingItem> GetDepressionThreshold21()
        {
            var settings = GetAllOff();

            var item = settings.First(x => x.Id == VisitSettingsDescriptor.Depression);

            item.IsEnabled = true;
            item.CutScore = 21;

            return settings;
        }


        public static List<VisitSettingItem> GetSucideThreshold3()
        {
            var settings = GetAllOff();

            var item = settings.First(x => x.Id == VisitSettingsDescriptor.DepressionThinkOfDeath);

            item.IsEnabled = true;
            item.CutScore = 3;

            return settings;
        }
    }
}
