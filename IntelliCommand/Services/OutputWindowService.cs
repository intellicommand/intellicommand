// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using EnvDTE;

    using Microsoft.VisualStudio;

    /// <summary>
    /// The output window service.
    /// </summary>
    internal class OutputWindowService : IOutputWindowService
    {
        private const string OutputWindowName = "IntelliCommand";

        private readonly IAppServiceProvider serviceProvider;
        private OutputWindowPane outputWindowPane;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputWindowService"/> class.
        /// </summary>
        /// <param name="package">The package instance.</param>
        public OutputWindowService(IAppServiceProvider package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.serviceProvider = package;
        }

        /// <inheritdoc />
        public void OutputLine(string messageFormat, params object[] arguments)
        {
            this.OutputString(false, messageFormat + Environment.NewLine, arguments);
        }

        /// <inheritdoc />
        public void OutputString(string messageFormat, params object[] arguments)
        {
            this.OutputString(false, messageFormat, arguments);
        }

        private void OutputString(bool fActivate, string messageFormat, params object[] arguments)
        {
            string message = arguments.Length == 0 ? messageFormat : string.Format(CultureInfo.InvariantCulture, messageFormat, arguments);

            try
            {
                var vsOutputWindowPane = this.GetOutputWindowPane();
                vsOutputWindowPane.OutputString(message);
                if (fActivate)
                {
                    this.ShowOutputPane();
                }
            }
            catch (Exception e)
            {
                if (ErrorHandler.IsCriticalException(e))
                {
                    throw;
                }

                Debug.WriteLine("Cannot write to output window: {0}. \n\tOriginal message: {1}", e, message);
            }
        }

        private OutputWindowPane GetOutputWindowPane()
        {
            if (this.outputWindowPane == null)
            {
                var outputWindow = (OutputWindow)this.GetOutputWindow().Object;
                this.outputWindowPane = outputWindow.OutputWindowPanes.Add(OutputWindowName);
            }

            return this.outputWindowPane;
        }

        private void ShowOutputPane()
        {
            this.GetOutputWindow().Visible = true;

            var vsOutputWindowPane = this.GetOutputWindowPane();
            vsOutputWindowPane.Activate();
        }

        private Window GetOutputWindow()
        {
            var dte = this.serviceProvider.GetService<DTE>();
            return dte.Windows.Item(Constants.vsWindowKindOutput);
        }
    }
}