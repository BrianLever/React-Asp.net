using System.Collections.Generic;

namespace FrontDeskServer
{
	public static class MainMenuItemsDescriptor
	{
		public static Dictionary<string, string[]> MainMenuItems = new Dictionary<string, string[]>
		{
			{"screen", new []{"PatientCheckIn.aspx", "EditPatientContact.aspx", "ExportWizard.aspx"}},
            {"follow-up", new []{"FollowUp.aspx"}},
            {"visit", new []{"BhsVisit.aspx", "BhsDemographics.aspx"}},
            {"my profile", new []{"userprofile.aspx", "ChangePassword.aspx", "ChangeSecurityQuestion.aspx"}},
			{"user list", new []{"UserDetails.aspx"}},
			{"branch locations", new []{"BranchLocationDetails.aspx"}},
			{"kiosks", new []{"KioskDetails.aspx"}},

		};
	}
}