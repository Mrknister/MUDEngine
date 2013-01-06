using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CommandInterpreter
    {
        UserData _user_data;
        MessageInterpreter.writemethod write;


        public CommandInterpreter(UserData data,MessageInterpreter.writemethod method)
        {
            this._user_data = data;
        }
    }
}
