using FrontDesk.Deploy.Server.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
namespace FrontDesk_UnitTest
{
    
    
    /// <summary>
    ///This is a test class for FrontDeskServerSetupCustonActionTest and is intended
    ///to contain all FrontDeskServerSetupCustonActionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FrontDeskServerSetupCustonActionTest
    {

        /// <summary>
        ///A test for ShowConfigurationWizard
        ///</summary>
        [TestMethod()]
        [Ignore]
        public void ShowConfigurationWizardTest()
        {
            //ConfigureConnectionStringForm target = new ConfigureConnectionStringForm();
            //target.InstallerPath = System.Environment.CurrentDirectory;
            //target.IsDemoMode = true;
            //target.ShowDialog();

            //Assert.AreNotEqual<DialogResult>(DialogResult.Cancel, target.DialogResult);
        }



        /// <summary>
        ///A test for CreateLocalGroup
        ///</summary>
        [TestMethod()]
        [Ignore]
        [DeploymentItem("FrontDesk.Deploy.Server.Actions.dll")]
        public void CreateLocalGroupTest()
        {
            FrontDeskServerSetupCustonAction target = new FrontDeskServerSetupCustonAction();
            target.CreateLocalGroup();
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
            // no exception == passed
        }
    }
}
