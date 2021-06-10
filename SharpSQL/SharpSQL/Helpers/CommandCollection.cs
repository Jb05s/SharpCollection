using System;
using System.Collections.Generic;
using SharpSQL.Commands;

namespace SharpSQL.Helpers
{
    public class CommandCollection
    {
        private readonly Dictionary<string, Func<ICommand>> _availableCommands = new Dictionary<string, Func<ICommand>>();

        // How To Add A New Command:
        //  1. Create your command class in the Commands Folder
        //      a. That class must have a CommandName static property that has the Command's name
        //              and must also Implement the ICommand interface
        //      b. Put the code that does the work into the Execute() method
        //  2. Add an entry to the _availableCommands dictionary in the Constructor below.

        public CommandCollection()
        {
            _availableCommands.Add(getlinked.CommandName, () => new getlinked());
            _availableCommands.Add(getdbuser.CommandName, () => new getdbuser());
            _availableCommands.Add(gethash.CommandName, () => new gethash());
            _availableCommands.Add(getlogin.CommandName, () => new getlogin());
            _availableCommands.Add(xp.CommandName, () => new xp());
            _availableCommands.Add(ole.CommandName, () => new ole());
            _availableCommands.Add(linkedquery.CommandName, () => new linkedquery());
            _availableCommands.Add(linkedxp.CommandName, () => new linkedxp());
            _availableCommands.Add(dbllinkedxp.CommandName, () => new dbllinkedxp());
            _availableCommands.Add(rpc.CommandName, () => new rpc());
        }

        public bool ExecuteCommand(string commandName, Dictionary<string, string> arguments)
        {
            bool commandWasFound;

            if (string.IsNullOrEmpty(commandName) || _availableCommands.ContainsKey(commandName) == false)
                commandWasFound = false;
            else
            {
                // Create the command object 
                var command = _availableCommands[commandName].Invoke();

                // and execute it with the arguments from the command line
                command.Execute(arguments);

                commandWasFound = true;
            }

            return commandWasFound;
        }
    }
}