using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Room
    {
        DataContainer container;
        public long R_Id;
        public string Name, Description;

        public Room(ref DataContainer cont)
        {
            this.container = cont;
            loadAttributes();
            
        }
        public void loadAttributes()
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select `Room`.R_Id,`Room`.Name,`Room`.Description from `Room`,`Character` where `Room`.R_Id = `Character`.R_Id and `Character`.C_Id=?";
            sql.add_parameter(container.c_data.C_Id);
            sql.execute_query();
            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return;
            }
            if (!sql.HasRows) // very unlikely
            {
                Console.WriteLine("Character Id not found");
                return;
            }
            R_Id = Convert.ToInt64(sql.result[0][0]);
            Name = Convert.ToString(sql.result[0][1]);
            Description = Convert.ToString(sql.result[0][2]);
        }
        public bool changeRoom(string direction)
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
            exec.add_parameter(container.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }
            loadAttributes();
            return true;
        }
    }
}
