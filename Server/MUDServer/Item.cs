using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Item
    {
        public static Item getItemByName(ref DataContainer cont, string name)
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select `Item`.I_Id from `Item`,`BelongsTo` where `Item`.I_Id=`BelongsTo`.I_Id and `BelongsTo`.C_Id=? and `Item`.Name=?";
            exec.add_parameter(cont.c_data.C_Id);
            exec.add_parameter(name);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return null;
            }
            if (!exec.HasRows)
            {
                return null;
            }

            return new Item(ref cont, Convert.ToInt64(exec.result[0][0]));
        }
        public static List<Item> loadItems(ref DataContainer cont)
        {
            List<Item> items = new List<Item>();
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select I_Id from BelongsTo where C_Id=?";
            exec.add_parameter(cont.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return null;
            }
            if (!exec.HasRows)
            {
                return null;
            }
            foreach (object[] tmp in exec.result)
            {
                long id = Convert.ToInt64(tmp[0]);
                items.Add(new Item(ref cont, id));
            }
            return items;
        }
        public static bool giveUser(ref DataContainer cont, long I_Id)
        {
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "update `BelongsTo` set Amount=Amount+1 where I_Id=? and C_Id=?";
            exec.add_parameter(I_Id);
            exec.add_parameter(cont.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            if (exec.rows_affected == 0)
            {
                exec = new UnreadableSQLExecuter();
                exec.query = "insert into `BelongsTo` (I_Id,C_Id,Amount,Equipped) values (?,?,1,0)";
                exec.add_parameter(I_Id);
                exec.add_parameter(cont.c_data.C_Id);
                exec.execute_query();
                if (exec.error)
                {
                    Console.WriteLine(exec.error_string);
                    return false;
                }
            }
            cont.c_data.calculateItemStats();
            return true;
        }

        DataContainer container;
        public bool error;
        public string error_string;

        long I_Id;
        public string Name, Description;
        public string Category;
        public bool Equipped;

        public Item(ref DataContainer cont, long I_Id)
        {
            this.container = cont;
            this.I_Id = I_Id;
            loadAttributes();
        }
        
        
        public void loadAttributes()
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select `Item`.Name,`Item`.Description,`Item`.Category,`BelongsTo`.Equipped from `Item`,`BelongsTo` where `Item`.I_Id=`BelongsTo`.I_Id and `BelongsTo`.C_ID=? and `Item`.I_Id=?";
            exec.add_parameter(container.c_data.C_Id);
            exec.add_parameter(I_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                this.error = true;
                this.error_string = exec.error_string;
                return;
            }
            if (!exec.HasRows)
            {
                this.error = true;
                this.error_string = "No Item with this Id was found";
                return;
            }
            this.Name = Convert.ToString(exec.result[0][0]);
            this.Description = Convert.ToString(exec.result[0][1]);
            this.Category = Convert.ToString(exec.result[0][2]);
            this.Equipped = Convert.ToBoolean(exec.result[0][3]);
        }
        public long getOwnedAmount()
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Amount from `BelongsTo` where I_Id=? and C_Id=?";
            exec.add_parameter(I_Id);
            exec.add_parameter(container.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                this.error = true;
                this.error_string = exec.error_string;
                return 0;
            }
            if (!exec.HasRows)
            {
                return 0;
            }
            return Convert.ToInt64(exec.result[0][0]);
        }
        public void destroyItem(long amount)
        {
            UnreadableSQLExecuter u_exec = new UnreadableSQLExecuter();
            u_exec.query = "update `BelongsTo` set Amount=Amount-? where I_Id=? and C_Id=?";
            u_exec.add_parameter(amount);
            u_exec.add_parameter(I_Id);
            u_exec.add_parameter(container.c_data.C_Id);
            u_exec.execute_query();
            if (u_exec.error)
            {
                Console.WriteLine(u_exec.error_string);
                this.error = true;
                this.error_string = u_exec.error_string;
                return ;
            }
            u_exec = new UnreadableSQLExecuter();
            u_exec.query = "delete from `BelongsTo` where Amount<=0";
            u_exec.execute_query();
            if (u_exec.error)
            {
                Console.WriteLine(u_exec.error_string);
                this.error = true;
                this.error_string = u_exec.error_string;
            }
        }
        public bool equip()
        {
            if (Category == "Armor")
            {
                unequip_armor();
                if (error)
                {
                    return false;
                }
            }
            else if (Category == "Weapon")
            {
                unequip_weapon();
                if (error)
                {
                    return false;
                }
            }
            else//it is nothing you can equip
            {
                Console.WriteLine("Failed to equip object: " + Category);
                return false;
            }
            UnreadableSQLExecuter u_exec = new UnreadableSQLExecuter();
            u_exec.query = "Update `BelongsTo` set Equipped =1 where C_Id=? and I_Id=?";
            u_exec.add_parameter(container.c_data.C_Id);
            u_exec.add_parameter(I_Id);
            u_exec.execute_query();
            if (u_exec.error)
            {
                Console.WriteLine(u_exec.error_string);
                return false;
            }
            Equipped = true;
            container.c_data.calculateItemStats();
            return true;
        }
        public int consume(bool consumation_type)
        {
            if (Category != "Consumable")
            {
                return 2;
            }
            long amount = getOwnedAmount();
            if (error)
            {
                Console.WriteLine(error_string);
                return -1;
            }
            if (amount==0)
            {
                return 1;
            }

            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Type,Amount,ConsumptionType,NOW()+Duration from Consumable where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();

            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return -1;
            }
            if (!exec.HasRows)
            {
                Console.WriteLine("Consumable not found!");
                return -1;
            }
            string buff_type = Convert.ToString(exec.result[0][0]);
            long buff_amount = Convert.ToInt64(exec.result[0][1]);
            string runsoutat = Convert.ToString(exec.result[0][3]);

            if (Convert.ToBoolean(exec.result[0][2]) != consumation_type)
            {
                return 3;
            }
            if (buff_type == "Health")
            {
                consumeHealth(buff_amount);
                if (error)
                {
                    return -1;
                }
            }
            else if (buff_type == "PhRes")
            {
                container.c_data.addBuff(Name, runsoutat, buff_amount,2);
                
            }
            else if (buff_type == "Damage")
            {
                container.c_data.addBuff(Name, runsoutat, buff_amount, 1);
            }
            else
            {
                return 2;
            }
            destroyItem(1);
            return 0;
        }
        private void consumeHealth(long amount)
        {
            container.c_data.heal(amount);
        }
        private void consumeArmor(long amount, long I_Id)
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Name from `Item` where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();
            string Name = Convert.ToString(exec.result[0][0]);

            exec = new ReadableSQLExecuter();
            exec.query = "select now()+Duration from `Consumable` where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return ;
            }
            

            double Buffduration = Convert.ToDouble(exec.result[0][0]);

            exec = new ReadableSQLExecuter();
            exec.query = "insert into `Buff`(Name, Amount, Type, RunsOutAt, C_Id) values(?,?,2,?,?) ";
            exec.add_parameter(Name);
            exec.add_parameter(amount);
            exec.add_parameter(Convert.ToString(Buffduration));
            exec.add_parameter(container.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return;
            }
            
        }
        private bool consumeDamage(long amount, long I_Id)
        {

            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Name from `Item` where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }

            string Name = Convert.ToString(exec.result[0][0]);

            exec = new ReadableSQLExecuter();
            exec.query = "select now()+Duration from `Consumable` where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }

            double Buffduration = Convert.ToDouble(exec.result[0][0]);

            exec = new ReadableSQLExecuter();
            exec.query = "insert into `Buff`(Name, Amount, Type, RunsOutAt, C_Id) values(?,?,1,?,?) ";
            exec.add_parameter(Name);
            exec.add_parameter(amount);
            exec.add_parameter(Convert.ToString(Buffduration));
            exec.add_parameter(container.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool unequip_armor() // it is assumed that the user ownes this item
        {
            // find out if an armor of this type is allready equipped
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "update `BelongsTo`,`Armor` set `BelongsTo`.Equipped = 0 where `BelongsTo`.I_Id = `Armor`.I_Id and `BelongsTo`.Equipped=1 and `BelongsTo`.C_Id=? and `Armor`.Type in (select Type from Armor where I_Id=?)";
            exec.add_parameter(container.c_data.C_Id);
            exec.add_parameter(I_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }

            return true;
        }
        private bool unequip_weapon() // here as well
        {
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "update `BelongsTo`,`Item` set `BelongsTo`.Equipped=0 where `BelongsTo`.I_Id=`Item`.I_Id and `Item`.Category='Weapon' and `BelongsTo`.C_Id = ?";
            exec.add_parameter(container.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            return true;
        }
    }
}
