using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class GameLoopDialogue : UserDialogue
    {
        long C_Id;
        DataContainer container;
        CommandEngine c_engine;
        public GameLoopDialogue(UserDialogue.WriteMethod write,long C_Id)
        {
            this.write = write;
            this.C_Id = C_Id;

            container = new DataContainer();
            container.c_data = new CharacterData(ref container, C_Id);
            container.r_data = new Room(ref container);

            c_engine = new CommandEngine(write, container);
            writeStartMessage();

        }
        public override bool interpretMessage(string message)
        {
            c_engine.interpretCommand(message);
            return false;
        }
        public override UserDialogue getNextDialogue()
        {
            return new StartDialogue(write);
        }
        private void writeStartMessage()
        {
            write(container.r_data.Name + "\n\n" + container.r_data.Description + "\n");
        }
    }
}
