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
            writeStartMessage();
        }
        public override bool interpretMessage(string message)
        {
            message=message.Trim().ToLower();
            if (message == "login")
            {
                next_dialogue_type=true;
                return true;
            }
            else if (message == "neu")
            {
                return true;
            }
            else
            {
                write("Ich hae dich nicht verstanden.\n");
                writeStartMessage();
                return false;
            }
        }
        public override UserDialogue getNextDialogue()
        {
            if (next_dialogue_type)
            {
                return new LoginDialogue(write);
            }
            else
            {
                return new RegisterDialogue(write);
            }
        }
        private void writeStartMessage()
        {
            write("Wenn du ein neuer Nutzer bist tippe 'neu' ein. \nWenn du schon mal hier warst, tippe 'login' ein.\n");
        }
    }
}
