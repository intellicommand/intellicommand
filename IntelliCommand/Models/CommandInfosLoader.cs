// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Models
{
#if DEBUG
    using System.Text;
#endif

    using EnvDTE;
    using EnvDTE80;
    using IntelliCommand.Services;

    /// <summary>
    /// The CommandInfosLoader class.
    /// </summary>
    internal class CommandInfosLoader
    {
        private readonly IAppServiceProvider appServiceProvider;
        private readonly CommandBindingParser commandBindingParser = new CommandBindingParser();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInfosLoader"/> class.
        /// </summary>
        /// <param name="appServiceProvider">
        /// The app service provider.
        /// </param>
        public CommandInfosLoader(IAppServiceProvider appServiceProvider)
        {
            this.appServiceProvider = appServiceProvider;
        }

        /// <summary>
        /// Load commands.
        /// </summary>
        /// <returns>The list of command infos.</returns>
        public CommandsContainer LoadCommands()
        {
            var commands = new CommandsContainer(this.appServiceProvider, keyCombinationLevel: 0);

            var dte = this.appServiceProvider.GetService<DTE>() as DTE2; 
            if (dte != null)
            {
                var outputWindowService = this.appServiceProvider.GetService<IOutputWindowService>();

                foreach (Command vsCommand in dte.Commands)
                {
                    var objects = vsCommand.Bindings as object[];
                    if (objects != null)
                    {
                        foreach (var o in objects)
                        {
#if DEBUG
                            outputWindowService.OutputLine(o.ToString());
#endif
                            CommandBinding commandBinding = this.commandBindingParser.ParseBinding(o.ToString());
                            var commandInfo = new CommandInfo(vsCommand, commandBinding);

                            commands.Add(commandInfo);
#if DEBUG
                            var sb = new StringBuilder();
                            sb.AppendFormat("Scope: {0}, Name: {1}", commandInfo.CommandBinding.Scope, commandInfo.Name);

                            foreach (var keycombination in commandInfo.CommandBinding.KeyCombinations)
                            {
                                sb.AppendFormat("  {0}-{1},", keycombination.Modifiers, keycombination.Key);
                            }

                            outputWindowService.OutputLine(sb.ToString());
#endif
                        }
                    }
                }
            }

            return commands;
        }
    }
}