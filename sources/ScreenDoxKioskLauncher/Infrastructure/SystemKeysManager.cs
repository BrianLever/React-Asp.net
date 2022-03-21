using Common.Logging;

using System;
using System.Runtime.InteropServices;

#pragma warning disable CS1591, CA1901, CA2101

namespace ScreenDoxKioskLauncher.Infrastructure
{



    public static class NativeMethods
    {
        #region import from Win32

        [DllImport("user32", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr  SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);


        [DllImport("user32", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern int UnhookWindowsHookEx(IntPtr hHook);
        internal delegate IntPtr LowLevelKeyboardProcDelegate(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);


        [DllImport("user32", EntryPoint = "CallNextHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

        internal const int WH_KEYBOARD_LL = 13;

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(string lpFileName);


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0")]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int command);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;


        #endregion

        internal struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
    }

    public class SystemKeysManager: IDisposable
    {
        private readonly ILog _logger = LogManager.GetLogger("SystemKeysManager");

        /*code needed to disable start menu*/


        private NativeMethods.LowLevelKeyboardProcDelegate _proc;
        private IntPtr _hookID;
        private IntPtr _intLLKey;
        private bool disposedValue;

        public SystemKeysManager()
        {
            _proc = new NativeMethods.LowLevelKeyboardProcDelegate(LowLevelKeyboardProc);
        }

        private IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, ref NativeMethods.KBDLLHOOKSTRUCT lParam)
        {
            bool ignoreKey = false;

            int iParam = (int)wParam;

            switch (iParam)
            {
                case 256:
                case 257:
                case 260:
                case 261:
                    if (_logger.IsDebugEnabled)
                    {
                        _logger.Debug($"Handling Keyboard event. lParam.vkCode: {lParam.vkCode}. lParam.flags: {lParam.flags}.");
                    }
                    //Alt+Tab, Alt+Esc, Ctrl+Esc, Windows Key, Alt + Space
                    ignoreKey =
                        (lParam.vkCode == 9 && lParam.flags == 32)
                        | (lParam.vkCode == 27 /* ESC */ && lParam.flags == 32 /* ALT */)
                        | (lParam.vkCode == 27 && lParam.flags == 0)
                        | (lParam.vkCode == 91 && lParam.flags == 1)
                        | (lParam.vkCode == 92 && lParam.flags == 1)
                        | (lParam.vkCode == 73 && lParam.flags == 0)
                        | (lParam.vkCode == 32 && lParam.flags == 32);
                    break;
            }

            if (ignoreKey)
            {
                return (IntPtr)1;
            }
            else
            {
                _logger.Trace($"Handling Keyboard event. Ignored key. vkCode: {lParam.vkCode}. flags: {lParam.flags}.");

                return NativeMethods.CallNextHookEx((IntPtr)0, nCode, wParam, ref lParam);
            }
        }

        public void DisableSystemKeys()
        {
            _hookID = NativeMethods.LoadLibrary("user32.dll");
            _intLLKey = NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL, _proc, _hookID, 0);
        }

        public void EnableSystemKeys()
        {
            NativeMethods.UnhookWindowsHookEx(_intLLKey);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   
                }

             
                disposedValue = true;
            }
        }

        ~SystemKeysManager()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

#pragma warning restore CS1591, CA1901, CA2101

