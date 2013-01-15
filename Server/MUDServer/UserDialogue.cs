using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    abstract class UserDialogue
    {
        public delegate void WriteMethod(string message);
        protected WriteMethod write;
        public abstract bool interpretMessage(string message);
        public abstract UserDialogue getNextDialogue();
    }
}
