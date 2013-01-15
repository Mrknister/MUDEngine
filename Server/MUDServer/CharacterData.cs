using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CharacterData
    {
        DataContainer container;

        
        public long C_Id = 0;
        public string Name;
        public bool error;
        public string error_string;
        public long Money, Health, Damage, PhRes, MaxHealth;
        public long BuffDamage, BuffPhRes;
        public long ItemDamage, ItemPhRes;

        public CharacterData(ref DataContainer cont,long CharacterId)
        {
            this.container = cont;
            this.C_Id = CharacterId;
            loadAttributes();
            calculateBuffs();
            calculateItemStats();
            Console.WriteLine(C_Id);
        }

        public void loadAttributes()
        {
            if (C_Id == 0)
                return;

            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Money,Health,Damage,PhRes,MaxHealth,Name from `Character` where C_Id = ?";
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return ;
            }
            if (!exec.HasRows) // this should not happen
            {
                error = true;
                error_string = "No Character with this Id";
                return;
            }
            object[] attributes = exec.result[0];
            Money = Convert.ToInt64(attributes[0]);
            Health = Convert.ToInt64(attributes[1]);
            Damage = Convert.ToInt64(attributes[2]);
            PhRes = Convert.ToInt64(attributes[3]);
            MaxHealth = Convert.ToInt64(attributes[4]);
            Name = Convert.ToString(attributes[5]);

            return;
        }
        public void calculateBuffs()
        {
            //load buff damage
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.add_parameter(C_Id);
            exec.query = "select sum(Amount) from `Buff` where  Type=1 and RunsOutAt > now() and C_Id = ?";
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return;
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
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return;
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
        }
        public void calculateItemStats()
        {
            // load weapon damage
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.add_parameter(C_Id);
            exec.query = "select sum(`Weapon`.Damage)  from `Weapon`,`BelongsTo` where `Weapon`.I_Id=`BelongsTo`.I_Id and `BelongsTo`.Equipped=1 and `BelongsTo`.C_Id = ?";
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return;
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
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return;
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
        }
        public void takeDamage(long amount)
        {
            calculateBuffs();
            calculateItemStats();

            amount -= PhRes;
            if (amount < 1)
                amount = 1;

            changeHealth(-amount);
        }
        public void heal(long amount)
        {
            changeHealth(amount);
        }
        private void changeHealth(long amount)
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "update `Character` set Health = case when Health+? <MaxHealth then Health+? else MaxHealth end where `Character`.C_Id=? ";
            exec.add_parameter(amount);
            exec.add_parameter(amount);
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
            }
            loadAttributes();
        }
        public void addBuff(string name,string runsoutat,long amount,long type)
        {
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "insert into `Buff`(Name, Amount, Type, RunsOutAt, C_Id) values(?,?,?,?,?) ";
            exec.add_parameter(name);
            exec.add_parameter(amount);
            exec.add_parameter(type);
            exec.add_parameter(runsoutat);
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return;
            }
            calculateBuffs();
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
            loadAttributes();
            return true;
        }

    }

}
