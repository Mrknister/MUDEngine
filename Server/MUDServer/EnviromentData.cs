using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class EnviromentData
    {
        Int64 R_Id;
        string R_Name;
        string R_Discription;
        public EnviromentData(long C_Id)
        {
        }
        public void loadRoom()
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select `Room`.R_Id,`Room`.Name,`Room`.Description from `Room`,`Character` where `Room`.R_Id = `Character`.R_Id";
            R_Id = Convert.ToInt64(sql.result[0][0]);
            R_Name = Convert.ToString(sql.result[0][1]);
            R_Discription = Convert.ToString(sql.result[0][2]);
            sql.execute_query();
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
    }
}
