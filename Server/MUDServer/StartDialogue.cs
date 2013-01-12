using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class StartDialogue : UserDialogue
    {
        bool next_dialogue_type=false;
        public StartDialogue(UserDialogue.WriteMethod write)
        {
            this.write = write;
        }
        public override void interpretMessage(string message)
        {
        }
        public override UserDialogue getNextDialogue()
        {
            if (next_dialogue_type)
            {
                return new RegisterDialogue(write);
            }
            else
            {
                return new LoginDialogue(write);
            }
        }
        private void writeStartMessage()
        {
        }
    }
}
