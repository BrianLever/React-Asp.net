using System;
using FrontDesk.Kiosk.Screens;
using System.Windows.Controls;

namespace FrontDesk.Kiosk.Workflow
{
	public class VisualScreenFactory : FrontDesk.Kiosk.Workflow.IVisualScreenFactory
    {
        public IVisualScreen CreateScreenForStep(ScreeningStep step, IScreeningResultState resultState)
        {
            return CreateScreenForStep(step, null, resultState);
        }


        public IVisualScreen CreateScreenForStep(ScreeningStep step, ScreeningSection section, IScreeningResultState resultState)
        {
            IVisualScreen screen = null;
            switch (step)
            {
                case ScreeningStep.Home:
                    screen = new Screens.Home();
                    break;
                case ScreeningStep.Welcome:
                    screen = new Screens.Welcome();
                    break;
                case ScreeningStep.ScreeningSection:
                    screen = new Screens.ScreeningSectionPanel();
                    break;
                case ScreeningStep.SendResult:
                    screen = new Screens.SendResult();
                    break;
                case ScreeningStep.ThankYou:
                    screen = new Screens.ThankYou();
                    break;
                case ScreeningStep.PatientCity:
                    screen = new Screens.City();
                    break;
                case ScreeningStep.PatientDateOfBirth:
                    screen = new Screens.Birthday();
                    break;
                case ScreeningStep.PatientFirstName:
                    screen = new FirstName();
                    break;
                case ScreeningStep.PatientLastName:
                    screen = new Screens.LastName();
                    break;
                case ScreeningStep.PatientMiddleName:
                    screen = new Screens.MiddleName();
                    break;
                case ScreeningStep.PatientPrimaryPhone:
                    screen = new Screens.Phone();
                    break;
                case ScreeningStep.PatientState:
                    screen = new Screens.State();
                    break;
                case ScreeningStep.PatientStreet:
                    screen = new Screens.Address();
                    break;
                case ScreeningStep.PatientZipCode:
                    screen = new Screens.ZipCode();
                    break;
                case ScreeningStep.DemographicsMessage:
                    screen = new Screens.DemographicsMessage();
                    break;

                case ScreeningStep.DemographicsCountyName:
                    screen = new Screens.DemographicsCountyName();
                    break;
                case ScreeningStep.DemographicsCountyState:
                    screen = new Screens.DemographicsCountyState();
                    
                    break;
                case ScreeningStep.DemographicsTribe:
                    screen = new Screens.DemographicsTribe();
                    break;
                case ScreeningStep.DemographicsGender:
                    screen = new Screens.DemographicsGender();
                    break;
                case ScreeningStep.DemographicsRace:
                    screen = new Screens.DemographicsRace();
                    break;
                case ScreeningStep.DemographicsEducationLevel:
                    screen = new Screens.DemographicsEducationLevel();
                    break;
                case ScreeningStep.DemographicsSexualOrientation:
                    screen = new Screens.DemographicsSexualOrientation();
                    break;
                case ScreeningStep.DemographicsMaritalStatus:
                    screen = new Screens.DemographicsMaritalStatus();
                    break;
                case ScreeningStep.DemographicsOnOffReservation:
                    screen = new Screens.DemographicsOnOffReservation();
                    break;
                case ScreeningStep.DemographicsMilitaryExperience:
                    screen = new Screens.DemographicsMilitaryExperience();
                    break;
                case ScreeningStep.DemographicsMilitaryCombat:
                    screen = new Screens.DemographicsMilitaryCombat();
                    break;
                case ScreeningStep.DemographicsSendResult:
                    screen = new Screens.DemographicsSendResult();
                    break;
            }

            if (screen != null)
            {

                var txtInputWithAutosuggestControl = screen as IStatefulVisualScreen;
                if(txtInputWithAutosuggestControl != null)
                {
                    txtInputWithAutosuggestControl.ResultState = resultState;
                }

                var control = screen as UserControl;
                control.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                control.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                control.Width = Double.NaN;
            }
            return screen;
        }
    }
}
