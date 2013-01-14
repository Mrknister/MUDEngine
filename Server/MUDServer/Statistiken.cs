using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Statistiken : BasicCommand
    {
        public Statistiken(UserDialogue.WriteMethod write, DataContainer cont)
        {
            this.write = write;
            this.container = cont;
        }
        public override bool interpret(string userinput)
        {
            userinput = userinput.ToLower().Trim();
            if (userinput == "stats")
            {
                showStats();
                return true;
            }
            else if (userinput == "inventar")
            {
                showInventory();
                return true;
            }
            return false;
        }
        private void showStats()
        {
            container.c_data.calculateBuffs();
            write("Euer Name ist: " + container.c_data.Name +
                "\n" + "Euere maximale Lebensenergie ist: " + Convert.ToString(container.c_data.MaxHealth) +
                "\n" + "Euere momentane Lebensenergie ist: " + Convert.ToString(container.c_data.Health) +
                "\n" + "Euer momentaner Schaden ist: " + Convert.ToString(container.c_data.Damage + container.c_data.ItemDamage + container.c_data.BuffDamage) +
                "\n" + "Euer momentaner Wiederstand ist: " + Convert.ToString(container.c_data.PhRes + container.c_data.BuffPhRes + container.c_data.ItemPhRes) +
                "\n");
        }
        private void showInventory()
        {
            List<Item> items = Item.loadItems(ref container);
            if (items == null)
            {
                write("Du besitzt momentan nichts.\n");
                return;
            }
            
            foreach (Item item in items)
            {
                string tmp = " ";
                if(item.Equipped)
                    tmp="*";
                tmp+=item.Name+" ("+Convert.ToString(item.getOwnedAmount())+")\n";
                write(tmp);
            }
        }
    }
}
