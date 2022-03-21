using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using FrontDesk.Deploy.Server.Actions.Properties;
using FrontDesk.Deployment;

namespace FrontDesk.Deploy.Server.Actions
{
    public partial class SetupWizardForm : Form
    {
        public event EventHandler Completed;
        public event EventHandler<NextStepEventArgs> Next;
        public event EventHandler Previous;
        public event EventHandler<StepShownEventArgs> BeforeStepShown;


        public List<IWizardStep> Steps { get; private set; }

        private int _currentStepIndex = -1;

        public IWizardStep CurrentStep
        {
            get
            {
                return Steps[_currentStepIndex];
            }
        }

        public SetupWizardForm()
        {
            InitializeComponent();

            Steps = new List<IWizardStep>();
        }

        public void ShowInstallProgress(string title)
        {
            ucInstallProgress.Visible = true;
            ucInstallProgress.Title = title;

            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            //btnCancel.Enabled = false;
        }

        private void WizardForm_Shown(object sender, EventArgs e)
        {
            if (Steps != null && Steps.Count > 0)
            { 
                //display first step
                _currentStepIndex = 0;
                ShowStep(Steps[_currentStepIndex]);
                lblTitle.Text = CurrentStep.Title;
            }

            SetWizardButtonsState();

        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            string errorMessage = "";
            if (!CurrentStep.Validate(out errorMessage))
            {
                MessageBox.Show(errorMessage);
            }
            else
            {
                bool isLastStep = _currentStepIndex + 1 == Steps.Count;

                bool isCanceled = false;
                if (Next != null)
                {
                    //Raise Next event
                    NextStepEventArgs args = new NextStepEventArgs(Steps[_currentStepIndex],
                        isLastStep ? null : Steps[_currentStepIndex + 1]);

                    Next(this, args);
                    isCanceled = args.IsCanceled;
                }

                if (!isCanceled) //check is action canceled from event handler
                {
                    if (!isLastStep)
                    {
                        //Proceed to the next step
                        Steps[_currentStepIndex].HideStep();
                        ShowStep(Steps[++_currentStepIndex]);

                        lblTitle.Text = CurrentStep.Title;
                    }
                    else if(Completed != null)
                    {
                        //hide last step
                        CurrentStep.HideStep();

                        //Raise Completed event
                        Completed(this, new EventArgs());
                    }
                }
            }

            SetWizardButtonsState();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_currentStepIndex != 0)
            {
                Steps[_currentStepIndex].HideStep();
                ShowStep(Steps[--_currentStepIndex]);
                lblTitle.Text = CurrentStep.Title;
            }

            SetWizardButtonsState();
            
        }


        public void AddStep(IWizardStep step)
        {
            step.HideStep();
            Steps.Add(step);

            UserControl control = step as UserControl;
            control.Dock = DockStyle.Top & DockStyle.Left;
            pnlContainer.Controls.Add(control);
        }

        private void ShowStep(IWizardStep step)
        {
            if (BeforeStepShown != null)
            {
                BeforeStepShown(this, new StepShownEventArgs(step));
            }

            step.ShowStep();
        }

        private void SetWizardButtonsState()
        {
            btnPrevious.Enabled = _currentStepIndex != 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        internal void CompleteProgress()
        {
            ucInstallProgress.Stop();
        }
    }

    public class NextStepEventArgs: EventArgs
    {
        public IWizardStep CurrentStep { get; set; }
        public IWizardStep NextStep { get; set; }
        public bool IsCanceled { get; set; }

        public NextStepEventArgs(IWizardStep currentStep, IWizardStep nextStep)
        {
            CurrentStep = currentStep;
            NextStep = nextStep;
        }
    }


    public class StepShownEventArgs : EventArgs
    {
        public IWizardStep StepToDisplay { get; set; }

        #region Constructors

        public StepShownEventArgs() { }

        public StepShownEventArgs(IWizardStep stepToDisplay) 
        {
            this.StepToDisplay = stepToDisplay;
        }

        #endregion
    }
        


}
