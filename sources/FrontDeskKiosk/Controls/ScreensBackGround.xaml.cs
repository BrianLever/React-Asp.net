using System.Windows.Controls;

namespace FrontDesk.Kiosk.Controls
{
	/// <summary>
	/// Interaction logic for ScreensBackGround.xaml
	/// </summary>
	public partial class ScreensBackGround : UserControl
    {
        public ScreensBackGround()
        {
            InitializeComponent();
            txtCopyright.Text = string.Empty;
        }

        /// <summary>
        /// Get or set copyright text
        /// </summary>
        public string CopyrightText
        {
            get { return this.txtCopyright.Text; }
            set { this.txtCopyright.Text = value; }
        }
    }
}
