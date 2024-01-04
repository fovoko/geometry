using System.Threading.Tasks;

namespace gems.CQRS
{
    /// <summary>
    /// Represents handler for IQuery 
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// IQuery executor, it accepts IQuery argument query and returns TResult
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        TResult Execute(TQuery query);
    }
}
