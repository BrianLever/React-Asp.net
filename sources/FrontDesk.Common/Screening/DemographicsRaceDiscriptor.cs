namespace FrontDesk.Common.Screening
{
    public static class DemographicsRaceDiscriptor
    {
        public static int AmericanIndian = 1;
        public static int AlaskaNative = 7;

        public static bool IsNativeRace(int raceId)
        {
            return raceId == AmericanIndian || raceId == AlaskaNative;
        }
    }
}
