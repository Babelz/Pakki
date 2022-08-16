namespace Inviscan.Sync.Commands
{
    /// <summary>
    /// Interface for wrapping certain functionality behind a command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command. Executes the next command and passes the results from this command
        /// to the next command if the supplied argument is not null.
        /// </summary>
        void Execute();
    }
}