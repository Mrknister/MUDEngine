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
                command = command.Remove(0, 1);
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
                write("Ich kann das Monster "+command+"nicht finden.");
            }
            else if (monster_lost == 0)
                write("Du hast das Monster bezwungen.");
            else
                write("Das Monster hat noch " + Convert.ToString(monster_lost) + " Lebenspunkte\n");
        }
    }
}
