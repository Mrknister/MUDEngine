using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class LoginDialogue : UserDialogue
    {
        public LoginDialogue(UserDialogue.WriteMethod write)
        {
            this.write = write;
            writeStartMessage();
        }
        public override bool interpretMessage(string message)
        {
            return false;
        }
        private void writeStartMessage()
        {
            write("Gebe bitte deinen Nutzernamen an\n");
        }
    }
}
