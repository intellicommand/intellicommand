// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// The scope service. Allows us to find current Visual Studio Scope (Text Editor / Global / Designer).
    /// </summary>
    internal class CommandScopeService : ICommandScopeService
    {
        private readonly IAppServiceProvider appServiceProvider;
        private readonly IOutputWindowService outputWindowService;
        private readonly Dictionary<Guid, string> scopeDefinitions = new Dictionary<Guid, string>();
        private readonly IList<__VSFPROPID> windowFrameScopeProperties = new List<__VSFPROPID>()
            {
                __VSFPROPID.VSFPROPID_CmdUIGuid,
                __VSFPROPID.VSFPROPID_GuidPersistenceSlot,
                __VSFPROPID.VSFPROPID_InheritKeyBindings
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandScopeService"/> class.
        /// </summary>
        /// <param name="appServiceProvider">
        /// The application service provider.
        /// </param>
        public CommandScopeService(IAppServiceProvider appServiceProvider)
        {
            this.appServiceProvider = appServiceProvider;
            this.outputWindowService = this.appServiceProvider.GetService<IOutputWindowService>();
            this.LoadScopes();
        }

        /// <inheritdoc />
        public IEnumerable<string> GetCurrentScopes()
        {
            // Global scope always active.
            IList<string> scopes = new List<string>() { "Global" };

            var vsMonitorSelection = this.appServiceProvider.GetService(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;

            if (vsMonitorSelection == null)
            {
                this.outputWindowService.OutputLine("Error Cannot get visual studio service SVsShellMonitorSelection.");
            }
            else
            {
                object value;
                if (ErrorHandler.Succeeded(vsMonitorSelection.GetCurrentElementValue((uint)VSConstants.VSSELELEMID.SEID_DocumentFrame, out value))
                    && value is IVsWindowFrame)
                {
                    var vsWindowFrame = (IVsWindowFrame)value;

                    string scope;
                    if (this.TryGetScope(vsWindowFrame, __VSFPROPID.VSFPROPID_CmdUIGuid, out scope))
                    {
                        scopes.Add(scope);
                    }
                }
                
                if (ErrorHandler.Succeeded(vsMonitorSelection.GetCurrentElementValue((uint)VSConstants.VSSELELEMID.SEID_WindowFrame, out value))
                    && value is IVsWindowFrame)
                {
                    var vsWindowFrame = (IVsWindowFrame)value;
                    foreach (__VSFPROPID propId in this.windowFrameScopeProperties)
                    {
                        string scope;
                        if (this.TryGetScope(vsWindowFrame, propId, out scope))
                        {
                            scopes.Add(scope);
                        }
                    }
                }
            }

            return scopes.Distinct();
        }

        private bool TryGetScope(IVsWindowFrame vsWindowFrame, __VSFPROPID propId, out string scope)
        {
            scope = default(string);
            Guid guid;
            return ErrorHandler.Succeeded(vsWindowFrame.GetGuidProperty((int)propId, out guid))
                   && this.scopeDefinitions.TryGetValue(guid, out scope);
        }

        private void LoadScopes()
        {
            var vsProfileDataManager = this.appServiceProvider.GetService<SVsProfileDataManager, IVsProfileDataManager>();
            if (vsProfileDataManager == null)
            {
                this.outputWindowService.OutputLine("Error: Cannot get visual studio service SVsProfileDataManager.");
                return;
            }
            
            string defaultSettingsLocation;
            if (ErrorHandler.Failed(vsProfileDataManager.GetDefaultSettingsLocation(out defaultSettingsLocation)))
            {
                this.outputWindowService.OutputLine("Error: Cannot get default settings location.");
                return;
            }

            IVsProfileSettingsFileCollection settingFilesCollection;
            if (ErrorHandler.Failed(vsProfileDataManager.GetSettingsFiles(uint.MaxValue, out settingFilesCollection)))
            {
                this.outputWindowService.OutputLine("Error: Cannot get visual studio settings files.");
                return;
            }
            
            int count;
            if (ErrorHandler.Failed(settingFilesCollection.GetCount(out count)))
            {
                this.outputWindowService.OutputLine("Error: Cannot get count of visual studio settings files.");
                return;
            }
            
            for (int i = 0; i < count; i++)
            {
                IVsProfileSettingsFileInfo fileInfo;
                if (ErrorHandler.Succeeded(settingFilesCollection.GetSettingsFile(i, out fileInfo)))
                {
                    string filePath;
                    if (ErrorHandler.Succeeded(fileInfo.GetFilePath(out filePath)))
                    {
                        this.ParseFile(filePath);
                    }
                }
            }

            if (this.scopeDefinitions.Count == 0)
            {
                this.outputWindowService.OutputLine("Warning: Cannot find any scope definitions.");
            }
        }

        private void ParseFile(string filePath)
        {
            XDocument xDocument = XDocument.Load(filePath);
            foreach (var xElement in xDocument.Descendants("ScopeDefinitions"))
            {
                foreach (var xScope in xElement.Elements("Scope"))
                {
                    var xIdAttribute = xScope.Attribute("ID");

                    if (xIdAttribute == null)
                    {
                        this.outputWindowService.OutputLine("Warning: Scope doesn't have ID attribute. File {0}.", filePath);
                        continue;
                    }

                    var xNameAttribute = xScope.Attribute("Name");

                    if (xNameAttribute == null)
                    {
                        this.outputWindowService.OutputLine("Warning: Scope doesn't have Name attribute. File {0}.", filePath);
                        continue;
                    }

                    Guid id;
                    if (!Guid.TryParse(xIdAttribute.Value, out id))
                    {
                        this.outputWindowService.OutputLine("Warning: Cannot parse Id attribute as Guid. File {0}.", filePath);
                        continue;
                    }
                    
                    var name = xNameAttribute.Value;

                    if (!this.scopeDefinitions.ContainsKey(id))
                    {
                        this.scopeDefinitions.Add(id, name);
                    }

                    Debug.Assert(this.scopeDefinitions[id] == name, "this.scopeDefinitions[id] == name");
                    Debug.Assert(this.scopeDefinitions.Values.Count(v => v == name) == 1, "this.scopeDefinitions.Values.Count(v => v == name) == 1");
                }
            }
        }
    }
}