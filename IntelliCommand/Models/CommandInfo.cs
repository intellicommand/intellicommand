// -----------------------------------------------------------------------------
// License: Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

namespace IntelliCommand.Models
{
    using EnvDTE;

    /// <summary>
    /// The command info class. Contains information about command and combination of keys to invoke this command.
    /// </summary>
    public class CommandInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInfo"/> class.
        /// </summary>
        /// <param name="vsCommand">The VS instance of a command.</param>
        /// <param name="commandBinding">The command bindings.</param>
        public CommandInfo(Command vsCommand, CommandBinding commandBinding)
        {
            this.Name = this.GetCommandName(vsCommand);
            this.VsCommand = vsCommand;
            this.CommandBinding = commandBinding;
        }

        /// <summary>
        /// Gets the command bindings.
        /// </summary>
        public CommandBinding CommandBinding { get; private set; }

        /// <summary>
        /// Gets the localized name of the command.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the VS instance of the command.
        /// </summary>
        public Command VsCommand { get; private set; }

        private string GetCommandName(Command vsCommand)
        {
            return string.IsNullOrWhiteSpace(vsCommand.LocalizedName) ? vsCommand.Name : vsCommand.LocalizedName;
        }
    }
}