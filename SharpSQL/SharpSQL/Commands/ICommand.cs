using System.Collections.Generic;

namespace SharpSQL.Commands
{
    public interface ICommand
    {
        void Execute(Dictionary<string, string> arguments);
    }
}