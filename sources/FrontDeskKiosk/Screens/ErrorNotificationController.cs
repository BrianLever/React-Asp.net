using System;
using System.Windows.Controls;
using System.Windows;
using System.Threading;

namespace FrontDesk.Kiosk.Screens
{
	public class ErrorNotificationController
    {
        private readonly IUserSessionTimeoutController _userSessionTimeoutController;

        public ErrorNotificationController(IUserSessionTimeoutController userSessionTimeoutController) 
        {
            if (userSessionTimeoutController == null)
            {
                throw new ArgumentNullException("userSessionTimeoutController");

            }
            _userSessionTimeoutController = userSessionTimeoutController;

            _userSessionTimeoutController.UserSessionExpired += new EventHandler(Instance_UserSessionExpired);
        }


        private FrameworkElement _container;
        private TextBlock _label;


        /// <summary>
        /// Initialize error controller
        /// </summary>
        /// <param name="container">Error place holder</param>
        /// <param name="label">Label that represents error text</param>
        public void Initialize(FrameworkElement container, TextBlock label)
        {
            this._container = container;
            this._label = label;

            ClearErrors();
        }

        /// <summary>
        /// Show error message
        /// </summary>
        /// <param name="errorMessage">error text</param>
        public void ShowError(string errorMessage)
        {
            _label.Text = errorMessage;
            _container.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hide errors
        /// </summary>
        public void ClearErrors()
        {
            _label.Text = String.Empty;
            _container.Visibility = Visibility.Hidden;
        }

        private void Instance_UserSessionExpired(object sender, EventArgs e)
        {
            _container.Dispatcher.BeginInvoke((ThreadStart)delegate
            {
                ClearErrors();
            });
        }

    }
}
