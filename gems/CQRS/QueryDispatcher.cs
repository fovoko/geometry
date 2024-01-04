using Microsoft.Extensions.DependencyInjection;
using System;

namespace gems.CQRS
{
    /// <summary>
    /// Operates with query handlers
    /// </summary>
    /// <param name="serviceProvider"></param>
    public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
    {
        /// <summary>
        /// Gets correct handler by TQuery, executes it and returns result
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public TResult Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var handler = serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();

            if (handler == null) throw new Exception($"Cannot find handler for query {typeof(TQuery).Name}.");

            return handler.Execute(query);
        }
    }
}
