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
            else if (command.StartsWith("rueste "))
            {
                ausruesten(command);
            }
            else if (command.StartsWith("nimm "))
            {
                
                
                    nimm(command);
                
                
            }
            else
            {
                write("Ich habe dich nicht verstanden.");
            }
        }
        private void inventar(string command)
        {
            List<string> items = _user_data.loadItems();
            if (items.Count == 0)
            {
                write("Du besitzt momentan nichts\n");
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
                write("Du bist tod!\n");
            }
            if (monster_lost == -1)
            {
                write("Ich kann das Monster " + command + " nicht finden.\n");
            }
            else if (monster_lost == 0)
                write("Du hast das Monster bezwungen.\n");
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
        public void ausruesten(string command)
        {
            if(command.StartsWith("rueste "))
            {
                command=command.Remove(0,7);
            }
            if (command.EndsWith(" aus"))
            {
                command = command.Remove(command.Length - 4);

            }
            command = command.Trim();
            _user_data.equip(command);
        }
        
        private void nimm(string command)
        {
            string object_name, from_name;
            
            if (command.Contains(" aus "))
            {
                if (command.StartsWith("nimm ")) // in case some idiot removed the nehme somewhere before
                {
                    command = command.Remove(0, 5);
                }

                command = command.Trim();
                object_name = command.Remove(command.IndexOf(" aus "));
                object_name = object_name.Trim();

                from_name = command.Remove(0, command.IndexOf(" aus ")+6); // plus 6 because index of returns index and no the number of characters
                from_name = from_name.Trim();

                Console.WriteLine(object_name+"|"+from_name);

                if (_enviroment_data.takeFrom(from_name, object_name))
                {
                    write("Der Gegenstand wurde aufgenommen.\n");
                }
                else
                {
                    write("Der Gegenstand konnte nicht aufgenommen werden.\n");
                }
            }
            else
            {
                if (command.StartsWith("nimm "))
                {
                    command = command.Remove(0, 5);
                }
                command = command.Trim();

                if (_enviroment_data.takeFrom("", command))
                {
                    write("Der Gegenstand wurde aufgenommen.\n");
                }
                else
                {
                    write("Der Gegenstand konnte nicht aufgenommen werden.\n");
                }
            }
            


        }
        private void executeTakeFrom(string command)
        {
            string command2 = command.Trim();
            if (_enviroment_data.takeFrom(command2, command))
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
