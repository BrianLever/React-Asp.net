using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace FrontDeskServerInstall
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunFile(Path.Combine(Environment.CurrentDirectory, "SQLServerInstaller.exe"), string.Empty);
        }

        private void btnRunInstall_Click(object sender, EventArgs e)
        {
            RunFile(Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.System), "msiexec.exe"), "/i FrontDeskServerSetup.msi");
        }

        private void RunFile(string filename, string args)
        {
            try
            {
                var pathToFile = filename;
                ProcessStartInfo startInfo = new ProcessStartInfo(pathToFile);

                //MessageBox.Show(pathToFile);
                startInfo.Arguments = args;
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Operation failed. Error message: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
