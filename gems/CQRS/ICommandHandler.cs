using System.Threading.Tasks;

namespace gems.CQRS
{
    /// <summary>
    /// Represents handler for IQuery 
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// ICommand executor, it accepts ICommand argument query
        /// </summary>
        /// <param name="command"></param>
        Task<long> Execute(TCommand command);
    }
}
