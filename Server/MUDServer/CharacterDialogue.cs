using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CharacterDialogue : UserDialogue
    {
        bool next_dialogue_type = false;
        long U_Id;
        public CharacterDialogue(UserDialogue.WriteMethod write,long U_Id)
        {
            this.write = write;
            writeStartMessage();
            this.U_Id = U_Id;
        }
        public override bool interpretMessage(string message)
        {
            message=message.Trim().ToLower();
            if (message == "neu")
            {
                next_dialogue_type = true;
                return true;
            }
            else if (message == "wahl")
            {
                return true;
            }
            else
            {
                writeStartMessage();
                return false;
            }
        }
        public override UserDialogue getNextDialogue()
        {
            if (next_dialogue_type)
            {
                return new CharacterBuildDialogue(write, U_Id);
            }
            else
            {
                return new CharacterSelectionDialogue(write, U_Id);
            }
        }

        private void writeStartMessage()
        {
            write("Um einen neuen Character zu erstellen gebe 'neu' ein.\nUm einen bereits bestehenden Character zu waehlen gebe 'wahl' ein\n");
        }
    }
}
