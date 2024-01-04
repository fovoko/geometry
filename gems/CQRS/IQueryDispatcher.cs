using System.Threading.Tasks;

namespace gems.CQRS
{
    /// <summary>
    /// Interface to operate with IQuery handler
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Gets correct handler by TQuery, executes it and returns result
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        TResult Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
