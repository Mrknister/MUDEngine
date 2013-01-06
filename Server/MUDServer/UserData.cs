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
        long C_Id = 0;
        string Name;
        long Money, Health, Mana, Damage, PhRes, MaRes, MaxHealth, MaxMana;
        public List<string> Items;

        public UserData()
        {

        }
        public bool loadAttributes()
        {
            if (C_Id == 0)
                return false;

            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Name,Money,Health,Mana,Damage,PhRes,MaRes,MaxHealth,MaxMana where C_Id = ?";
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
            Name = Convert.ToString(attributes[0]);
            Money = Convert.ToInt64(attributes[1]);
            Health = Convert.ToInt64(attributes[2]);
            Damage = Convert.ToInt64(attributes[3]);
            PhRes = Convert.ToInt64(attributes[4]);
            MaRes = Convert.ToInt64(attributes[5]);
            MaxHealth = Convert.ToInt64(attributes[6]);
            MaxMana = Convert.ToInt64(attributes[7]);
            return true;
        }
        public bool loadItems()
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();

            return true;
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
                Console.WriteLine("Empty Set");
                return false;
            }
            U_Id = Convert.ToInt64(sql.result[0][0]);

            return true;
        }
        public List<string> getCharacters()
        {
            List<string> to_return = new List<string>();

            return to_return;
        }
        public bool createCharacter(string name)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select C_Id from `Character` where Name=?";
            sql.add_parameter(name);
            sql.execute_query();
            
            if (sql.HasRows)
            {
                return false;
            }
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "insert into `Character` (U_Id,Name,Money,Health,Mana,Damage,PhRes,MaRes,MaxHealth,MaxMana) values (?,?,200,100,100,10,10,10,100,100)";

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
    }
}
