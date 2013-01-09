using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CommandInterpreter
    {
        public UserData _user_data;
        public EnviromentData _enviroment_data;
        public MessageInterpreter.writemethod write;
        public void InterpretCommand(string command)
        {
            command = command.Trim();
            if (command.ToLower().StartsWith("inventar"))
            {
                inventar(command);
            }
            else if (command.StartsWith("a ")) 
                angriff(command);
            else if (command == "n")
            {
                goNorth();
            }
            else if (command == "s")
            {
                goSouth();
            }
            else if (command == "w")
            {
                goWest();
            }
            else if (command == "o")
            {
                goEast();
            }
            else if (command == "h")
            {
                goUp();
            }
            else if (command == "r")
            {
                goDown();
            }
            else if (command.StartsWith("nimm "))
            {
                take(command);
            }
            else if (command.StartsWith("nimm_aus"))
            {
                prepareTakeFrom(command);
            }
        }
        private void inventar(string command)
        {
            List<string> items = _user_data.loadItems();
            if (items.Count == 0)
            {
                write("Du besitzt momentan nichts");
            }
            foreach (string item in items)
            {
                write(item + "\n");
            }
        }
        private void angriff(string command)
        {
            if (command.StartsWith("a "))
            {
                command = command.Remove(0, 2);
                command = command.Trim();
            }
            long curr_health = _user_data.Health;
            long monster_lost = _user_data.attackMonster(command);
            if (_user_data.Health <= 0)
            {
                write("Du bist tod!");
            }
            if (monster_lost == -1)
            {
                write("Ich kann das Monster "+command+" nicht finden.");
            }
            else if (monster_lost == 0)
                write("Du hast das Monster bezwungen.");
            else
                write("Das Monster hat noch " + Convert.ToString(monster_lost) + " Lebenspunkte\n");
        }
        private void goNorth()
        {

            if (!_enviroment_data.changeRoom("North"))
            {
                write("Hier geht es nicht weiter!\n");
            }
            else
            {
                write("Du gehst nach Norden\n");
            }

        }
        private void goSouth()
        {
            if (!_enviroment_data.changeRoom("South"))
            {
                write("Hier geht es nicht weiter!\n");
            }
            else
            {
                write("Du gehst nach Süden\n");
            }
        }
        private void goWest()
        {
            if (!_enviroment_data.changeRoom("West"))
            {
                write("Hier geht es nicht weiter!\n");
            }
            else
            {
                write("Du gehst nach Westen\n");
            }
        }
        private void goEast()
        {
            if (!_enviroment_data.changeRoom("East"))
            {
                write("Hier geht es nicht weiter!\n");
            }
            else
            {
                write("Du gehst nach Osten\n");
            }
        }
        private void goUp()
        {
            if (!_enviroment_data.changeRoom("Up"))
            {
                write("Hier geht es nicht weiter!\n");
            }
            else
            {
                write("Du gehst hoch\n");
            }
        }
        private void goDown()
        {
            if (!_enviroment_data.changeRoom("Down"))
            {
                write("Hier geht es nicht weiter!\n");
            }
            else
            {
                write("Du gehst runter\n");
            }
        }
        private void take(string command) 
        {
            command = command.Remove(0, 4);
            command = command.Trim();
            if (_enviroment_data.takeFrom("", command))
            {
                write("Der Gegenstand wurde aufgenommen.");
            }
            else
            {
                write("Der Gegenstand konnte nicht aufgenommen werden.");
            }
        }
        private void prepareTakeFrom(string command)
        {
            command = command.Remove(0, 8);
            command = command.Trim();
            write("Wähle nun den Gegenstand");


        }
        private void executeTakeFrom(string command)
        {
            string command2 = command.Trim();
            if (_enviroment_data.takeFrom("", command))
            {
                write("Der Gegenstand wurde aufgenommen.");
            }
            else
            {
                write("Der Gegenstand konnte nicht aufgenommen werden.");
            }
        }
    }
}
