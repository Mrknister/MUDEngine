using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CommandInterpreter
    {
        UserData _userdata;
        public CommandInterpreter(UserData data)
        {
            this._userdata = data;
        }
    }
}
