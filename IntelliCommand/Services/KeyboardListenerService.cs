// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;

    /// <summary>
    /// The KeyboardListenerService. Listens to the keyboard globally.
    /// </summary>
    /// <remarks>Uses WH_KEYBOARD_LL.</remarks>
    internal class KeyboardListenerService : IKeyboardListenerService, IDisposable
    {
        /// <summary>
        /// Hook ID
        /// </summary>
        private readonly IntPtr hookIdKeyboard = IntPtr.Zero;

        private readonly Action<NativeMethods.KeyEvent, int> hookedKeyboardCallbackAsync;

        /// <summary>
        /// Contains the hooked callback in runtime.
        /// </summary>
        private readonly NativeMethods.LowLevelProc hookedLowLevelKeyboardProc;

        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardListenerService"/> class.
        /// </summary>
        public KeyboardListenerService()
        {
            // We have to store the HookCallback so that it is not garbage collected
            this.hookedLowLevelKeyboardProc = this.LowLevelKeyboardProc;

            // Set the hook
            this.hookIdKeyboard = NativeMethods.SetKeyboardHook(this.hookedLowLevelKeyboardProc);

            // Assign the asynchronous callback event
            this.hookedKeyboardCallbackAsync = this.KeyboardListener_KeyboardCallbackAsync;
        }

        ~KeyboardListenerService()
        {
            this.DisposeInternal();
        }

        /// <summary>
        /// Fired when any key is pressed down.
        /// </summary>
        public event EventHandler<RawKeyEventArgs> KeyDown;

        /// <summary>
        /// Fired when any key is released.
        /// </summary>
        public event EventHandler<RawKeyEventArgs> KeyUp;

        /// <summary>
        /// Disposes the hook.
        /// <remarks>This call is required as it calls the UnhookWindowsHookEx.</remarks>
        /// </summary>
        public void Dispose()
        {
            this.DisposeInternal();
            GC.SuppressFinalize(this);
        }

        private void DisposeInternal()
        {
            if (!this.disposed)
            {
                NativeMethods.UnhookWindowsHookEx(this.hookIdKeyboard);
                this.disposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (this.IfProcessWindow())
                {
                    if (wParam.ToUInt32() == (int)NativeMethods.KeyEvent.WM_KEY_DOWN
                        || wParam.ToUInt32() == (int)NativeMethods.KeyEvent.WM_KEY_UP
                        || wParam.ToUInt32() == (int)NativeMethods.KeyEvent.WM_SYSKEY_DOWN
                        || wParam.ToUInt32() == (int)NativeMethods.KeyEvent.WM_SYSKEY_UP)
                    {
                        this.hookedKeyboardCallbackAsync.BeginInvoke(
                            (NativeMethods.KeyEvent)wParam.ToUInt32(), Marshal.ReadInt32(lParam), null, null);
                    }
                }
            }

            return NativeMethods.CallNextHookEx(this.hookIdKeyboard, nCode, wParam, lParam);
        }

        private bool IfProcessWindow()
        {
            IntPtr currentWindow = NativeMethods.GetForegroundWindow();

            IntPtr parent;
            while ((parent = NativeMethods.GetParent(currentWindow)) != IntPtr.Zero)
            {
                currentWindow = parent;
            }

            bool fVsWindow = 
                Application.Current.Windows != null 
                && Application.Current.Windows.Cast<Window>().Any(
                    window => new WindowInteropHelper(window).Handle == currentWindow);

            return fVsWindow;
        }

        /// <summary>
        /// HookCallbackAsync procedure that calls the KeyDown or KeyUp events.
        /// </summary>
        /// <param name="keyEvent">Keyboard event.</param>
        /// <param name="vkCode">The vkCode.</param>
        private void KeyboardListener_KeyboardCallbackAsync(NativeMethods.KeyEvent keyEvent, int vkCode)
        {
            switch (keyEvent)
            {
                // KeyDown events
                case NativeMethods.KeyEvent.WM_SYSKEY_DOWN:
                case NativeMethods.KeyEvent.WM_KEY_DOWN:
                    if (this.KeyDown != null)
                    {
                        this.KeyDown(this, new RawKeyEventArgs(this.KeyFromVirtualKey(vkCode)));
                    }

                    break;

                // KeyUp events
                case NativeMethods.KeyEvent.WM_KEY_UP:
                case NativeMethods.KeyEvent.WM_SYSKEY_UP:
                    if (this.KeyUp != null)
                    {
                        this.KeyUp(this, new RawKeyEventArgs(this.KeyFromVirtualKey(vkCode)));
                    }

                    break;
            }
        }

        private Key KeyFromVirtualKey(int vkCode)
        {
            var value = KeyInterop.KeyFromVirtualKey(vkCode);

            // based on table "OEM Keys" from http://technet.microsoft.com/en-us/library/hh273163(v=WinEmbedded.21).aspx
            switch (value)
            {
                case Key.Oem1:
                    return Key.OemSemicolon;
                case Key.Oem2:
                    return Key.OemBackslash;
                case Key.Oem3:
                    return Key.OemTilde;
                case Key.Oem4:
                    return Key.OemOpenBrackets;
                case Key.Oem5:
                    return Key.Separator;
                case Key.Oem6:
                    return Key.OemCloseBrackets;
                case Key.Oem7:
                    return Key.OemQuotes;
                default:
                    return value;
            }
        }
    }
}