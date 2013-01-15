using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CommandEngine
    {
        List<BasicCommand> commands=new List<BasicCommand>();
        public CommandEngine(UserDialogue.WriteMethod write, DataContainer cont)
        {
            commands.Add(new Gehen(write, cont));
            commands.Add(new Angreifen(write, cont));
            commands.Add(new Betrachten(write, cont));
            commands.Add(new Statistiken(write, cont));
            commands.Add(new Nehmen(write, cont));
            commands.Add(new Benutzen(write, cont));
            commands.Add(new Hilfe(write, cont));
        }
        public void interpretCommand(string userinput)
        {
            userinput = userinput.Trim();
            foreach (BasicCommand tmp in commands)
            {
                if (tmp.interpret(userinput))
                    break;
            }
        }
    }
}
