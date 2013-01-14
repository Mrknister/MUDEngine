using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Betrachten : BasicCommand
    {
        public Betrachten(UserDialogue.WriteMethod write, DataContainer cont)
        {
            this.write = write;
            this.container = cont;
        }
        public override bool interpret(string userinput)
        {
            if (userinput == "b")
            {
                write(container.r_data.Name + "\n\n" + container.r_data.Description + "\n");
                List<Monster> tmp = Monster.loadMonsters(ref container);
                if (tmp != null)
                {
                    write("Monster:\n");
                    foreach (Monster m in tmp)
                    {
                        write(" "+m.Name+"\n");
                    }
                }
                return true;
            }
            else if(userinput.ToLower().StartsWith("b "))
            {
                userinput = userinput.Remove(0, 2).Trim();
                if (userinput.Contains(" in "))
                {
                    string object_to_inspect, object_in;
                    object_to_inspect = userinput.Remove(userinput.IndexOf(" in "));
                    object_in = userinput.Remove(0, userinput.IndexOf(" in ") + 5);
                    if (!viewTakeableObject(object_to_inspect, object_in))
                    {
                        write("Ich kann das Object nicht finden\n");
                    }
                }
                else
                {
                    Console.WriteLine(userinput);
                    view(userinput);
                }
            }
            

            return false;
        }
        private void view(string userinput)
        {
            if (viewObject(userinput))
            {
                return;
            }
            else if (viewItem(userinput))
            {
                return;
            }
            else if (viewMonster(userinput))
            {
                return;
            }
            else
            {
                write("Ich finde das nicht.\n");
            }
        }
        private bool viewObject(string userinput)
        {
            GameObject tmp = GameObject.getObjectByName(ref container, userinput);
            if(tmp!=null)
            {
                write(tmp.Description + "\n");
                return true;
            }

            return viewTakeableObject(userinput,"");
        }
        private bool viewTakeableObject(string object_name,string object_in)
        {
            TakeableGameObject tmp = TakeableGameObject.getObjectByName(ref container, object_name,object_in);
            if (tmp != null)
            {
                write(tmp.Description + "\n");
                return true;
            }
            return false;
        }
        private bool viewItem(string object_name)
        {
            Item tmp = Item.getItemByName(ref container, object_name);
            if (tmp != null)
            {
                write(tmp.Description + "\n");
                return true;
            }
            return false;
        }
        private bool viewMonster(string monster_name)
        {
            Monster tmp = Monster.getMonsterByName(ref container, monster_name);
            if (tmp != null)
            {
                write(tmp.Description + "\n");
                return true;
            }
            return false;
        }
        
    }
}
