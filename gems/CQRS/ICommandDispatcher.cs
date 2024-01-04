using System.Threading.Tasks;

namespace gems.CQRS
{
    /// <summary>
    /// Interface to operate with ICommand handler
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Gets correct handler by TQuery and executes it
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        Task<long> Execute<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
