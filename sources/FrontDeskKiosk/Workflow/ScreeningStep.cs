namespace FrontDesk.Kiosk.Workflow
{
	/// <summary>
	/// Steps of the screening application
	/// </summary>
	public enum ScreeningStep
    {
        Home = 0,
        Welcome,
        PatientFirstName,
        PatientMiddleName,
        PatientLastName,
        PatientDateOfBirth,
        PatientStreet,
        PatientCity,
        PatientState, 
        PatientZipCode,
        PatientPrimaryPhone,
        ScreeningSection,
        SendResult,
        DemographicsMessage,
        DemographicsGender,
        DemographicsRace,
        DemographicsTribe,
        DemographicsOnOffReservation,
        DemographicsCountyName,
        DemographicsEducationLevel,
        DemographicsMaritalStatus,
        DemographicsSexualOrientation,
        DemographicsMilitaryExperience,
        DemographicsMilitaryCombat,
        DemographicsSendResult,
        ThankYou,
        DemographicsCountyState
    }
}
