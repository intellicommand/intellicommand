// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        private const int WhKeyboardLl = 13;

        public delegate IntPtr LowLevelProc(int nCode, UIntPtr wParam, IntPtr lParam);

        public enum KeyEvent : int
        {
            WM_KEY_DOWN = 256,

            WM_KEY_UP = 257,

            WM_SYSKEY_UP = 261,

            WM_SYSKEY_DOWN = 260
        }

        public static IntPtr SetKeyboardHook(LowLevelProc proc)
        {
            return SetHook(WhKeyboardLl, proc);
        }

        [DllImport("user32.dll", ExactSpelling = true, EntryPoint = "VkKeyScanW", CharSet = CharSet.Auto)]
        public static extern short IntVkKeyScan(char key);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, UIntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr windowHandle);

        private static IntPtr SetHook(int idHook, LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(idHook, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
    }
}