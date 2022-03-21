using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrontDesk.Deploy.Server.Actions
{
    public partial class InstallingExpressProgress : UserControl
    {
        public InstallingExpressProgress()
        {
            InitializeComponent();
        }

        public void Start()
        {
            _progress = 0;
            progressBar1.Value = 0;
            timer1.Start();
        }

        public void Stop()
        {
            progressBar1.Value = 100;
            timer1.Stop();
        }

        int _progress = 0;
        int _increment = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            _progress += _increment;
            if (_progress > 100) _progress = _progress % 100;
            progressBar1.Value = _progress;

        }

        private void InstallingExpressProgress_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) Start();
            else Stop();
        }
    }
}
