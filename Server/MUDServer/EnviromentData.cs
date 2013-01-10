using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class EnviromentData
    {
        long R_Id;
        long C_Id;
        string R_Name;
        string R_Discription;
        public EnviromentData(long C_Id)
        {
            this.C_Id = C_Id;
            loadRoom();
        }
        public bool loadRoom()
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select `Room`.R_Id,`Room`.Name,`Room`.Description from `Room`,`Character` where `Room`.R_Id = `Character`.R_Id and `Character`.C_Id=?";
            sql.add_parameter(C_Id);
            sql.execute_query();
            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }
            if(!sql.HasRows)
            {
                Console.WriteLine("Character Id not found");
                return false;
            }
            R_Id = Convert.ToInt64(sql.result[0][0]);
            R_Name = Convert.ToString(sql.result[0][1]);
            R_Discription = Convert.ToString(sql.result[0][2]);

            return true;
        }
        public List<string> loadMonster()
        {
            List<string> monster_list = new List<string>();
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select Name from `Monster`,`MonsterIsIn` where R_Id=? and RespawnAtTime<NOW()";
            sql.add_parameter(R_Id);
            sql.execute_query();

            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return monster_list;
            }
            foreach (object[] tmp in sql.result)
            {
                string m_name = "";
                m_name += Convert.ToString(tmp[1]);
                monster_list.Add(m_name);
            }
            return monster_list;
        }
        public bool changeRoom (string direction)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select R_IdT from `Gate` where R_IdF=? and Direction=?";
            sql.add_parameter(R_Id);
            sql.add_parameter(direction);
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
            R_Id = Convert.ToInt64(sql.result[0][0]);

            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "update `Character` set R_Id=? where C_Id=?";
            exec.add_parameter(R_Id);
            exec.add_parameter(C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }
            loadRoom();
            if (loadRoom() == false)
            {
                return false;
            }
            return true;
        }
        public bool takeFrom(string TakeFrom,string ObjectName)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select I_Id from `Takeable`,`Objekt`,`ObjInRoom` where `Takeable`.O_Id=`Objekt`.O_Id and `Objekt`.O_Id=`ObjInRoom`.O_Id and `Takeable`.TakeFrom=? and `Objekt`.Name=? and `ObjInRoom`.R_Id=?";
            sql.add_parameter(TakeFrom);
            sql.add_parameter(ObjectName);
            sql.add_parameter(R_Id);
            sql.execute_query();
            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }
            long I_Id = Convert.ToInt64(sql.result[0][0]);
            if (!sql.HasRows)
            {
                return false;
            }

            sql.query = "select * from `BelongsTo` where I_Id=? and C_Id=?";
            sql.add_parameter(I_Id);
            sql.add_parameter(C_Id);
            sql.execute_query();
            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }
            if (sql.HasRows)
            {
                UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
                exec.query = "update `BelongsTo` set Amount=Amount+1 where I_Id=? and C_Id=?";
                exec.add_parameter(I_Id);
                exec.add_parameter(C_Id);
                exec.execute_query();
                if (exec.error)
                {
                    Console.WriteLine(sql.error_string);
                    return false;
                }
            }
            else
            {
                UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
                exec.query = "insert into `BelongsTo` (I_Id,C_Id,Amount) values (?,?,1)";
                exec.add_parameter(I_Id);
                exec.add_parameter(C_Id);
                exec.execute_query();
                if (exec.error)
                {
                    Console.WriteLine(sql.error_string);
                    return false;
                }
            }
            UnreadableSQLExecuter exec2 = new UnreadableSQLExecuter();
            exec2.query = "update `Takeable` set RespawnAtTime=RespawnAtTime+RespawnTime";
            exec2.execute_query();
            return true;
        }
    }
}
