using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace FrontDeskKioskUILocker
{
    public static class HotKeyManager
    {
        public static void SetStateCtrlAltDelete(bool enabled)
        {
            if (enabled)
            {
                EnabledTaskManager();
            }
            else
            {
                DisableTaskManager();
            }
        }

        private static void DisableTaskManager()
        {
            int keyValueInt = 1;
            string subKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";

            try
            {
                using (var regkey = Registry.CurrentUser.CreateSubKey(subKey))
                {
                    regkey.SetValue("DisableTaskMgr", keyValueInt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to disable Task Manager. Error: {ex}.");
                throw;
            }
        }

        private static void EnabledTaskManager()
        {
            string subKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";

            try
            {
                using (var regkey = Registry.CurrentUser.CreateSubKey(subKey))
                {
                    if (regkey.GetValueNames().Contains("DisableTaskMgr"))
                    {
                        regkey.DeleteValue("DisableTaskMgr");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to enable Task Manager. Error: {ex}.");
                throw;
            }
        }


        /// <summary>
        /// Get status of Task Manager has been disabled
        /// </summary>
        /// <returns>True if Task Manager is enabled</returns>
        public static bool GetStateCtrlAltDelete()
        {
            RegistryKey regKey;
            string subKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";

            try
            {
                regKey = Registry.CurrentUser.OpenSubKey(subKey);
                if (regKey == null)
                {
                    return true; // enabled
                }

                try
                {
                    return !regKey.GetValueNames().Contains("DisableTaskMgr");

                }
                finally
                {
                    regKey.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to get the Task Manager policy status. Error: {ex}.");
                throw;
            }
        }
    }
}
