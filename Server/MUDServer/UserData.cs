using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;



namespace MUDServer
{
    class UserData
    {

        public long U_Id = 0;
        public long C_Id = 0;
        public long R_Id;
        public string Name;
        public long Money, Health,  Damage, PhRes,  MaxHealth;
        public long BuffDamage, BuffPhRes;
        public long ItemDamage, ItemPhRes;

        public UserData()
        {

        }
        public bool loadAttributes()
        {
            if (C_Id == 0)
                return false;

            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Money,Health,Damage,PhRes,R_Id,MaxHealth from `Character` where C_Id = ?";
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            if (!exec.HasRows)
            {
                return false;
            }
            object[] attributes = exec.result[0];
            Money = Convert.ToInt64(attributes[0]);
            Health = Convert.ToInt64(attributes[1]);
            Damage = Convert.ToInt64(attributes[2]);
            PhRes = Convert.ToInt64(attributes[3]);
            R_Id = Convert.ToInt64(attributes[4]);
            MaxHealth = Convert.ToInt64(attributes[5]);
            return true;
        }
        public bool calculateBuffs()
        {
            //load buff damage
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.add_parameter(C_Id);
            exec.query = "select sum(Amount) from `Buff` where  Type=1 and RunsOutAt > now() and C_Id = ?";
            exec.execute_query();
            if (exec.error) // an error occured somewhere during the loading of the player attributes
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            if (exec.HasRows)
            {
                if (exec.result[0][0] != DBNull.Value)
                {
                    BuffDamage = Convert.ToInt64(exec.result[0][0]);
                }
                else
                {
                    BuffDamage = 0;
                }
            }
            //load buff res
            exec = new ReadableSQLExecuter();
            exec.add_parameter(C_Id);
            exec.query = "select sum(Amount) from `Buff` where  Type=2 and RunsOutAt > now() and C_Id = ? ";
            exec.execute_query();
            if (exec.error) // an error occured somewhere during the loading of the player attributes
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            if (exec.HasRows)
            {
                if (exec.result[0][0] != DBNull.Value)
                {
                    BuffPhRes = Convert.ToInt64(exec.result[0][0]);
                }
                else
                {
                    BuffPhRes = 0;
                }
            }
            return true;
        }
        public bool calculateItemStats()
        {
            // load weapon damage
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.add_parameter(C_Id);
            exec.query = "select sum(`Weapon`.Damage)  from `Weapon`,`BelongsTo` where `Weapon`.I_Id=`BelongsTo`.I_Id and `BelongsTo`.Equipped=1 and `BelongsTo`.C_Id = ?";
            exec.execute_query();
            if (exec.error) // an error occured somewhere during the loading of the player attributes
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            if (exec.HasRows)
            {
                if (exec.result[0][0] != DBNull.Value)
                {
                    ItemDamage = Convert.ToInt64(exec.result[0][0]);
                }
                else
                {
                    ItemDamage = 0;
                }
            }
            else
            {
                ItemDamage = 0;
            }


            // load armor res
            exec = new ReadableSQLExecuter();
            exec.add_parameter(C_Id);
            exec.query = "select sum(`Armor`.PhyRes)  from `Armor`,`BelongsTo` where `Armor`.I_Id=`BelongsTo`.I_Id and `BelongsTo`.Equipped=1 and `BelongsTo`.C_Id = ?";
            exec.execute_query();
            if (exec.error) // an error occured somewhere during the loading of the player attributes
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            if (exec.HasRows)
            {
                if (exec.result[0][0] != DBNull.Value)
                {
                    ItemPhRes = Convert.ToInt64(exec.result[0][0]);
                }
                else
                {
                    ItemPhRes = 0;
                }
            }
            else
            {
                ItemDamage = 0;
            }
            return true;
        }
        public bool saveAttributes()
        {
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "update `Character` set Name=?,Money=?,Health=?,Damage=?,PhRes=?,R_Id=? where C_Id=?";
            exec.add_parameter(Name);
            exec.add_parameter(Money);
            exec.add_parameter(Health);
            exec.add_parameter(Damage);
            exec.add_parameter(PhRes);
            exec.add_parameter(R_Id);
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            return true;

        }
        public long attackMonster(string monstername)
        {
            loadAttributes();
            calculateBuffs();
            calculateItemStats();

            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            // load monster attributes
            exec.query = "select `Monster`.M_Id, `Monster`.Damage, MIIn.Health, MIIn.Mana from Monster, MonsterIsIn as MIIn where `Monster`.M_Id=MIIn.M_Id and MIIn.RespawnAtTime < NOW() and MIIn.R_Id=? and `Monster`.Name=?";
            exec.add_parameter(R_Id);
            exec.add_parameter(monstername);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return -1;
            }
            if (!exec.HasRows) // no monster with this name
            {
                Console.WriteLine("No monster " + monstername);
                return -1;
            }

            long M_Id = Convert.ToInt64(exec.result[0][0]);
            long M_Damage = Convert.ToInt64(exec.result[0][1]);
            long M_Health = Convert.ToInt64(exec.result[0][2]);
            long M_Mana = Convert.ToInt64(exec.result[0][3]);

            //load and calculate player attributes

            long damage = this.Damage+this.ItemDamage+this.BuffDamage;
            long phres = this.PhRes + this.ItemPhRes + this.BuffPhRes;

            long m_caused_dmg = M_Damage - phres;
            long p_caused_dmg = damage;

            if (m_caused_dmg < 1)
                m_caused_dmg = 1;

            if (p_caused_dmg < 1)
                p_caused_dmg = 1;

            //this.Health -= m_caused_dmg

            M_Health -= p_caused_dmg;

            UnreadableSQLExecuter u_exec = new UnreadableSQLExecuter();

            if (M_Health < 0) //congrats... you killed it
            {
                u_exec.query = "update `MonsterIsIn`,`Monster` set `MonsterIsIn`.RespawnAtTime=`MonsterIsIn`.RespawnTime+now(),`MonsterIsIn`.Health =`Monster`.MaxHealth  where `MonsterIsIn`.M_Id=? and `MonsterIsIn`.R_Id=?";
                u_exec.add_parameter(M_Id);
                u_exec.add_parameter(R_Id);
                u_exec.execute_query();
                if (u_exec.error) // an error occured somewhere during the loading of the player attributes
                {
                    Console.WriteLine(u_exec.error_string);
                }
                return 0;

            }


            u_exec.query = "update `MonsterIsIn` set Health=? where M_Id=? and R_Id =?";
            u_exec.add_parameter(M_Health);
            u_exec.add_parameter(M_Id);
            u_exec.add_parameter(R_Id);
            u_exec.execute_query();
            if (u_exec.error) // an error occured somewhere during the loading of the player attributes
            {
                Console.WriteLine(u_exec.error_string);
                return -1;
            }
            this.Health -= m_caused_dmg;
            saveAttributes();
            return M_Health;
        }

        public List<string> loadItems()
        {
            List<string> to_return = new List<string>();
            if (C_Id == 0)
                return to_return;

            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "Select BelongsTo.I_Id,BelongsTo.Amount,BelongsTo.Equipped,Item.Name from BelongsTo,Item where BelongsTo.C_Id=? and BelongsTo.I_Id = Item.I_Id";
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return to_return;
            }
            if (!exec.HasRows)
                Console.WriteLine("Leer");
            foreach (object[] tmp in exec.result)
            {
                string message = "";
                if (Convert.ToBoolean(tmp[2]))
                {
                    message = "* ";
                }
                message += Convert.ToString(tmp[3]);
                message += "(" + Convert.ToString(tmp[1]) + ")";
                to_return.Add(message);
            }
            return to_return;
        }

        public bool selectCharacter(string name)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select C_Id from `Character` where Name=? and U_Id=?";
            sql.add_parameter(name);
            sql.add_parameter(U_Id);
            sql.execute_query();

            if (!sql.HasRows)
            {
                return false;
            }
            C_Id = Convert.ToInt64(sql.result[0][0]);
            this.Name = name;

            return true;
        }
        public bool login(string name, string password)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select U_Id from User where Name=? and Password=?";

            sql.add_parameter(name);
            sql.add_parameter(password);
            sql.execute_query();

            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }

            if (!sql.HasRows)
            {
                return false;
            }
            U_Id = Convert.ToInt64(sql.result[0][0]);

            return true;
        }
        
        public bool createCharacter(string name)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select C_Id from `Character` where Name=?";
            sql.add_parameter(name);
            sql.execute_query();
            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }
            if (sql.HasRows)
            {
                return false;
            }
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "insert into `Character` (U_Id,Name,Money,Health,Damage,PhRes,MaxHealth,R_Id) values (?,?,200,100,30,10,100,1)";

            exec.add_parameter(U_Id);
            exec.add_parameter(name);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            return true;
        }

        public bool register(string name, string password)
        {

            name = name.Trim();
            if (!check_register(name, password)) // check register adds the parameters to the command.
            {
                return false;
            }

            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "insert into User (Name,Password,LastLogin) values (?,?,NOW())";
            exec.add_parameter(name);
            exec.add_parameter(password);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            return true;
        }

        private bool check_register(string name, string password)
        {

            //check if lenght of name is between 3 and 12 and password lenght > 8
            if (name.Length < 3 || name.Length > 12)
            {
                return false;
            }
            if (password.Length < 8)
            {
                return false;
            }


            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select U_Id from User where Name=?";
            //check if name already exist
            exec.add_parameter(name);
            exec.execute_query();
            if (exec.HasRows)
            {
                return false;
            }
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            return true;
        }
        public int equip(string to_equip)
        {
            Console.WriteLine("\"" + to_equip + "\"");
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Item.I_Id,Item.Category,BelongsTo.Equipped from BelongsTo,Item where Item.I_Id=BelongsTo.I_Id and Item.Name=? and BelongsTo.C_Id=?";
            exec.add_parameter(to_equip);
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return -1;
            }
            if (!exec.HasRows) // item not found/owned
            {
                return -2;
            }
            if (Convert.ToBoolean(exec.result[0][2]))//allready equipped
            {
                return -3;
            }
            Console.WriteLine(exec.result[0][1].GetType());
            string category = Convert.ToString(exec.result[0][1]);
            long I_Id = Convert.ToInt64(exec.result[0][0]);
            if (category == "Armor")
            {
                
                if (!unequip_armor(I_Id))
                {
                    return -1;
                }
            }
            else if (category == "Weapon")
            {
                if (!unequip_weapon())
                {
                    return -1;
                }
            }
            else//it is nothing you can equip
            {
                Console.WriteLine("Failed to equip object: " + category);
                return -4;
            }
            UnreadableSQLExecuter u_exec = new UnreadableSQLExecuter();
            u_exec.query = "Update `BelongsTo` set Equipped =1 where C_Id=? and I_Id=?";
            u_exec.add_parameter(C_Id);
            u_exec.add_parameter(I_Id);
            u_exec.execute_query();
            if (u_exec.error)
            {
                Console.WriteLine(u_exec.error_string);
                return -1;
            }
            return 0;
        }
        private bool unequip_armor(long I_Id) // it is assumed that the user ownes this item
        {
            // find out if an armor of this type is allready equipped
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "update `BelongsTo`,`Armor` set `BelongsTo`.Equipped = 0 where `BelongsTo`.I_Id = `Armor`.I_Id and `BelongsTo`.Equipped=1 and `BelongsTo`.C_Id=? and `Armor`.Type in (select Type from Armor where I_Id=?)";
            exec.add_parameter(C_Id);
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
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            return true;
        }
        public bool died()
        {
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "update `Character` set Health=MaxHealth, R_Id=1 where C_Id=?";
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            return true;
        }
        public int consume(string item_name,bool consumation_type)
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter(); 
            exec.query = "select `Item`.I_Id,`Item`.Category,`BelongsTo`.Amount from `Item`,`BelongsTo` where `BelongsTo`.I_Id=`Item`.I_Id and `BelongsTo`.C_Id=? and `Item`.Name=?";
            exec.add_parameter(C_Id);
            exec.add_parameter(item_name);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return -1;
            }
            if (!exec.HasRows)
            {
                return 1;
            }
            long I_Id = Convert.ToInt64(exec.result[0][0]);
            string category = Convert.ToString(exec.result[0][1]);

            if (category != "Consumable")
            {
                return 2;
            }
            exec = new ReadableSQLExecuter();
            exec.query = "select Type,Amount,ConsumptionType from Consumable where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();

            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return -1;
            }
            if (!exec.HasRows)
            {
                Console.WriteLine("Consumable not found!");
                return -1;
            }
            string cons_type = Convert.ToString(exec.result[0][0]);
            long amount = Convert.ToInt64(exec.result[0][1]);
            if (Convert.ToBoolean(exec.result[0][2]) != consumation_type)
            {
                return 3;
            }

            return 0;
        }
        private bool consumeHealth(long amount, long I_Id)
        {
            ReadableSQLExecuter exec =new ReadableSQLExecuter();
            exec.query = "update `Character` set Health = case when Health+=? <MaxHealth then Health+? else MaxHealth where `Character`.C_Id=? ";
            exec.add_parameter(amount);
            exec.add_parameter(amount);
            exec.add_parameter(C_Id);
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

        private bool consumeArmor(long amount, long I_Id, DateTime duration)
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "update `Character` set Health = case when Health+=? <MaxHealth then Health+? else MaxHealth where `Character`.C_Id=? ";
            exec.add_parameter(amount);
            exec.add_parameter(amount);
            exec.add_parameter(C_Id);
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
        private bool usePoison(long amount, long I_Id, DateTime duration)
        {
            return true;
        }
    }
}
