using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace gems.CQRS
{
    /// <summary>
    /// Iplementation of the class to work with query handlers
    /// </summary>
    /// <param name="serviceProvider"></param>
    public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
    {
        /// <summary>
        /// Gets correct handler by TQuery and executes it
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<long> Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var handler = serviceProvider.GetService<ICommandHandler<TCommand>>();

            if (handler == null) throw new Exception($"Cannot find handler for command {typeof(TCommand).Name}");

            return handler.Execute(command);
        }
    }
}
