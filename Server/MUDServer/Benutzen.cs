using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Benutzen : BasicCommand
    {
        public Benutzen(UserDialogue.WriteMethod write ,DataContainer cont)
        {
            this.write = write;
            this.container = cont;
        }
        public override bool interpret(string userinput)
        {
            if (userinput.ToLower().Trim().StartsWith("rueste "))
            {
                ausruesten(userinput);
                return true;
            }
            if (userinput.ToLower().Trim().StartsWith("trinke "))
            {
                
                drink(userinput);
                return true;
            }
            if (userinput.ToLower().Trim().StartsWith("esse "))
            {
                eat(userinput);
                return true;
            }
            return false;
        }
        private void ausruesten(string command)
        {
            if (command.StartsWith("rueste "))
            {
                command = command.Remove(0, 7);
            }
            if (command.EndsWith(" aus"))
            {
                command = command.Remove(command.Length - 4);

            }
            command = command.Trim();
            Item to_equip = Item.getItemByName(ref container,command);

            if (to_equip == null)
            {
                write("Ich kann diesen Gegenstand leider nicht finden.\n");
                return;
            }

            if (to_equip.equip())
            {
                write("Du hast den Gegenstand erfolgreich ausgeruestet.\n");
            }
            else
            {
                write("Den Gegenstand kannst du nicht ausruesten.\n");
            }
        }
        private void eat(string to_eat)
        {
            to_eat = to_eat.Remove(0, 5).Trim();

            Item i = Item.getItemByName(ref container, to_eat);
            if (i == null)
            {
                write("Ich kann diesen Gegenstand nicht finden.\n");
                return;
            }
            int status = i.consume(false);
            Console.WriteLine(status);
            if (status == 0)
            {
                write("Lecker!\n");
            }
            else if (status == 1)
            {
                write("Ich habe \"" + to_eat + "\" nicht gefunden\n");
            }
            else if (status == 2)
            {
                write("Das zu essen waere nicht gerade angenehm.\n");
            }
            else if (status == 3)
            {
                write("Willst du das vieleicht nicht lieber trinken?\n");
            }
            else
            {
                write("Es ist nicht dein Fehler. Es ist meiner. Ich schäme mich :( .\n");
            }
        }
        private void drink(string to_drink)
        {
            to_drink = to_drink.Remove(0, 7).Trim();
            Item i = Item.getItemByName(ref container, to_drink);
            if (i == null)
            {
                write("Ich kann diesen Gegenstand nicht finden.\n");
                return;
            }
            int status = i.consume(true);
            Console.WriteLine(status);
            if (status == 0)
            {
                write("Lecker!\n");
            }
            else if (status == 1)
            {
                write("Ich habe \"" + to_drink + "\" nicht gefunden\n");
            }
            else if (status == 2)
            {
                write("Das zu trinken waere nicht gerade angenehm.\n");
            }
            else if (status == 3)
            {
                write("Willst du das vieleicht nicht lieber essen?\n");
            }
            else
            {
                write("Es ist nicht dein Fehler. Es ist meiner. Ich schäme mich :( .\n");
            }
        }
    }
}
