using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CommandInterpreter
    {
        public UserData _user_data;
        public EnviromentData _enviroment_data;
        public MessageInterpreter.writemethod write;
        public void InterpretCommand(string command)
        {
            command = command.Trim();
            if(command.ToLower().StartsWith("inventar"))
            {
                inventar(command);
            }
        }
        private void  inventar(string command)
        {
            List<string> items = _user_data.loadItems();
            if (items.Count == 0)
            {
                write("Du besitzt momentan nichts");
            }
            foreach (string item in items)
            {
                write(item + "\n");
            }

        }


        
    }
}
