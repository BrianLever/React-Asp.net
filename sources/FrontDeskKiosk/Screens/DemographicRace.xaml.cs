﻿using System;
using System.Windows.Controls;

using FrontDesk.Kiosk.Screens.Behaviors;
using FrontDesk.Kiosk.Services;

namespace FrontDesk.Kiosk.Screens
{
    public partial class DemographicsRace : UserControl, IVisualScreen, IDemographicsLookupVisualScreen
    {

        private DemographicsLookupValuesControlBehavior _behavior;


        public DemographicsRace()
        {
            InitializeComponent();
            _behavior = new DemographicsLookupValuesControlBehavior(this);
            _behavior.UpdateUI();
        }

        public event EventHandler<NextScreenClickedEventArg> NextScreenClicked;

        public ScreeningResult UserSessionResult { get; set; }
        public Button NextButton => btnNext;
        public WrapPanel AnswerOptionsPanel => pnlMultiLineAnswerOptions;
        public DemographicsLookupServiceBase LookupService => new DemographicsRaceService();
        

        public Button GetDefaultButtonForTesting()
        {
            return NextButton;
        }

        public void Reset()
        {
            _behavior.Reset();
        }

        public void Show(bool withAnimation)
        {
            _behavior.Show(withAnimation);
        }

        public void Hide()
        {
            _behavior.Hide();
        }

        public void TriggerNextScreenEvent(int selectedItemId)
        {
            NextScreenClicked?.Invoke(this, new NextScreenClickedEventArg(null, selectedItemId));
        }
    }
}
