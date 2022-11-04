using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace DeveloperOperations.Commands
{
    public abstract class BaseCommand : ICommand 
    {
        [CommandOption("env", 'e')]
        public string Environment { get; set; }
        
        public virtual ValueTask ExecuteAsync(IConsole console)
        {
            return ValueTask.CompletedTask;
        }
    }
}