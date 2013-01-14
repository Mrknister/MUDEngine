using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Angreifen : BasicCommand
    {
        public Angreifen(UserDialogue.WriteMethod write, DataContainer cont)
        {
            this.write = write;
            this.container = cont;
        }
        public override bool interpret(string userinput)
        {
            if (userinput.ToLower().StartsWith("a "))
            {
                angriff(userinput);
            }
            return false;
        }
        private void angriff(string userinput)
        {
            userinput = userinput.Remove(0, 2).Trim();
            Monster monst = Monster.getMonsterByName(ref container, userinput);
            if (monst == null)
            {
                write("Ich kann diese Monster hier nicht finden.\n");
                return;
            }
            if (monst.attack())
            {
                write("Du hast das Ungetuem bezwungen!\n");
            }
            else
            {
                write(monst.Name + ": " + monst.Health + " Lebenspunkte\n" + "Du:" + container.c_data.Health+"\n");
            }
            if (container.c_data.Health <= 0)
            {
                write("Du wurdes vernichtend geschlagen ( du noob )\n");
                container.c_data.died();
            }
        }
    }
}
