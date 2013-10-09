// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Threading;

    using EnvDTE80;

    using IntelliCommand.Presentation;
    using IntelliCommand.Services;

    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    /// <remarks>
    /// This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    /// a package.
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]

    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.GuidIntelliCommandPkgString)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    [ProvideOptionPage(typeof(IntelliCommandOptionsDialogPage), "Intelli Command", "General", 0, 0, true)]
    public sealed class IntelliCommandPackage : Package, IAppServiceProvider
    {
        private static IAppServiceProvider appServiceProvider;

        private readonly Dictionary<Type, object> registeredServices = new Dictionary<Type, object>();

        private CommandsInfoWindow mainWindow;

        /// <inheritdoc />
        public TService GetService<TService>()
        {
            return this.GetService<TService, TService>();
        }

        /// <inheritdoc />
        public TServiceImpl GetService<TService, TServiceImpl>()
        {
            object service;
            if (this.registeredServices.TryGetValue(typeof(TService), out service))
            {
                Debug.Assert(service is TService, "service is TService");
                return (TServiceImpl)service;
            }

            return (TServiceImpl)this.GetService(typeof(TService));
        }

        internal static IAppServiceProvider GetAppServiceProvider()
        {
            return IntelliCommandPackage.appServiceProvider;
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();
            
            var shellService = this.GetService(typeof(SVsShell)) as IVsShell;
            if (shellService != null)
            {
                uint stateChangedCookie = 0;
                ErrorHandler.ThrowOnFailure(shellService.AdviseShellPropertyChanges(
                    new ZombiePropertyEventChangeHandler(() =>
                        {
                            ErrorHandler.ThrowOnFailure(shellService.UnadviseShellPropertyChanges(stateChangedCookie));
                            this.InitializeCommandServices();
                        }), 
                    out stateChangedCookie));
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                foreach (var registeredService in this.registeredServices.Values)
                {
                    var disposable = registeredService as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                
                if (this.mainWindow != null)
                {
                    this.mainWindow.Close();
                }
            }
        }

        private void InitializeCommandServices()
        {
            var dte = this.GetService<SDTE>() as DTE2;
            var outputWindowService = new OutputWindowService(this);
            this.registeredServices.Add(typeof(IOutputWindowService), outputWindowService);

            Debug.Assert(dte != null, "dte != null");
            if (dte != null)
            {
                this.registeredServices.Add(typeof(ICommandScopeService), new CommandScopeService(this));
                this.registeredServices.Add(typeof(Dispatcher), Dispatcher.CurrentDispatcher);
                this.registeredServices.Add(typeof(IKeyboardListenerService), new KeyboardListenerService());
                this.registeredServices.Add(typeof(IPackageSettings), this.GetDialogPage(typeof(IntelliCommandOptionsDialogPage)));

                IntelliCommandPackage.appServiceProvider = this;

                this.mainWindow = new CommandsInfoWindow(this) { Owner = Application.Current.MainWindow };
                this.mainWindow.Show();
            }
            else
            {
                outputWindowService.OutputLine("Cannot get a DTE servoce.");
            }
        }
    }
}
