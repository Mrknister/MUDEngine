using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Monster
    {
        public static Monster getMonsterByName(ref DataContainer cont, string monstername)
        {
            Monster to_return;
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            // load monster attributes
            exec.query = "select `Monster`.M_Id from Monster, MonsterIsIn as MIIn where `Monster`.M_Id=MIIn.M_Id and MIIn.RespawnAtTime < NOW() and MIIn.R_Id=? and `Monster`.Name=?";
            exec.add_parameter(cont.r_data.R_Id);
            exec.add_parameter(monstername);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return null;
            }
            if (!exec.HasRows) // no monster with this name
            {
                Console.WriteLine("No monster " + monstername);
                return null;
            }

            long M_Id = Convert.ToInt64(exec.result[0][0]);
            to_return = new Monster(ref cont, M_Id);

            return to_return;
        }
        public static List<Monster> loadMonsters(ref DataContainer cont)
        {
            List<Monster> monsters = new List<Monster>();
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select M_Id from MonsterIsIn where RespawnAtTime<NOW() and  R_Id=?";
            exec.add_parameter(cont.r_data.R_Id);
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
                monsters.Add(new Monster(ref cont, id));
            }
            return monsters;
        }


        DataContainer container;
        long M_Id;
        public string Name, Description,error_string;
        public long Health, Damage, PhRes, MaxHealth;
        public bool Hostile,error;

        public Monster(ref DataContainer cont, long M_Id)
        {
            this.container = cont;
            this.M_Id = M_Id;
            loadAttributes();
        }
        public bool isValid()
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            // load monster attributes
            exec.query = "select R_ID from  MonsterIsIn  where RespawnAtTime < NOW() and M_Id=? and R_Id=?";
            exec.add_parameter(M_Id);
            exec.add_parameter(container.r_data.R_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return false;
            }
            if (!exec.HasRows) // this should not happen
            {
                return false;
            }
            return true;
        }
        public bool attack()
        {
            container.c_data.calculateItemStats();
            container.c_data.calculateBuffs();

            Health -= container.c_data.Damage + container.c_data.BuffDamage+container.c_data.ItemDamage - PhRes;

            UnreadableSQLExecuter u_exec = new UnreadableSQLExecuter();

            if (Health <= 0) //congrats... you killed it
            {
                u_exec.query = "update `MonsterIsIn`,`Monster` set `MonsterIsIn`.RespawnAtTime=`MonsterIsIn`.RespawnTime+now(),`MonsterIsIn`.Health =`Monster`.MaxHealth  where `MonsterIsIn`.M_Id=? and `MonsterIsIn`.R_Id=?";
                u_exec.add_parameter(M_Id);
                u_exec.add_parameter(container.r_data.R_Id);
                u_exec.execute_query();
                if (u_exec.error) // an error occured somewhere during the loading of the monster attributes
                {
                    Console.WriteLine(u_exec.error_string);
                    error = true;
                    error_string = u_exec.error_string;
                    return false;
                }
                return true;

            }


            u_exec.query = "update `MonsterIsIn` set Health=? where M_Id=? and R_Id =?";
            u_exec.add_parameter(Health);
            u_exec.add_parameter(M_Id);
            u_exec.add_parameter(container.r_data.R_Id);
            u_exec.execute_query();
            if (u_exec.error) // an error occured somewhere during the loading of the player attributes
            {
                Console.WriteLine(u_exec.error_string);
                error = true;
                error_string = u_exec.error_string;
                return false;
            }
            
            container.c_data.takeDamage(Damage);
            return false;
        }
        private void loadAttributes()
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            // load monster attributes
            exec.query = "select `Monster`.Name,`Monster`.Description,`Monster`.Damage,`Monster`.PhRes,`Monster`.MaxHealth,`Monster`.Hostile,MIIn.Health from Monster, MonsterIsIn as MIIn where `Monster`.M_Id=MIIn.M_Id and `Monster`.M_Id=? and MIIn.R_Id=?";
            exec.add_parameter(M_Id);
            exec.add_parameter(container.r_data.R_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                error = true;
                error_string = exec.error_string;
                return;
            }
            if (!exec.HasRows) // this should not happen
            {
                error = true;
                error_string = "No Monster with this Id";
                return;
            }
            Name = Convert.ToString(exec.result[0][0]);
            Description = Convert.ToString(exec.result[0][1]);
            Damage = Convert.ToInt64(exec.result[0][2]);
            PhRes = Convert.ToInt64(exec.result[0][3]);
            MaxHealth = Convert.ToInt64(exec.result[0][4]);
            Hostile = Convert.ToBoolean(exec.result[0][5]);
            Health = Convert.ToInt64(exec.result[0][6]);
        }
        
    }
}
