using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    abstract class BasicCommand
    {
        protected UserDialogue.WriteMethod write;
        protected DataContainer container;
        public abstract bool interpret(string userinput);
    }
}
