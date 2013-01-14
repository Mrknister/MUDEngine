using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Nehmen : BasicCommand
    {
        public Nehmen(UserDialogue.WriteMethod write, DataContainer cont)
        {
            this.write = write;
            this.container = cont;
        }
        public override bool interpret(string userinput)
        {
            if (userinput.ToLower().StartsWith("nimm ") || userinput.ToLower().StartsWith("nehme "))
            {
                userinput = userinput.Remove(0, 5).Trim();
                take(userinput);
            }
            return false;
        }
        private void take(string command)
        {

            string object_name, from_name;

            if (command.Contains(" aus "))
            {


                object_name = command.Remove(command.IndexOf(" aus "));
                object_name = object_name.Trim();

                from_name = command.Remove(0, command.IndexOf(" aus ") + 6); // plus 6 because index of returns index and no the number of characters
                from_name = from_name.Trim();

                //Console.WriteLine(object_name + "|" + from_name); //debugging

            }
            else
            {

                TakeableGameObject obj = TakeableGameObject.getObjectByName(ref container, command);
                if (obj == null)
                {
                    write("Ich konnte den Gegenstand nicht finden.\n");
                    return;
                }
                if (obj.take())
                {
                    write("Der Gegenstand wurde aufgenommen.\n");
                }
                else
                {
                    write("Der Gegenstand konnte nicht aufgenommen werden.\n");
                }
            }
        }
    }
}
