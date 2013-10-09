// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Shell
{
    using System;

    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Zombie Property Event Change Handler.
    /// </summary>
    public class ZombiePropertyEventChangeHandler : IVsShellPropertyEvents
    {
        private readonly Action callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZombiePropertyEventChangeHandler"/> class.
        /// </summary>
        /// <param name="callback">The callback for zombie changed event.</param>
        public ZombiePropertyEventChangeHandler(Action callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            this.callback = callback;
        }

        /// <inheritdoc />
        public int OnShellPropertyChange(int propid, object var)
        {
            if ((int)__VSSPROPID.VSSPROPID_Zombie == propid)
            {
                this.callback();
            }

            return VSConstants.S_OK;
        }
    }
}