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

        public CharacterData(ref DataContainer cont)
        {
            this.container = cont;
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

            long damage = this.Damage + this.ItemDamage + this.BuffDamage;
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

    }

}
