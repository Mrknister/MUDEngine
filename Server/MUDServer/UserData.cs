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

        long U_Id;


        public UserData()
        {

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
            Console.WriteLine(U_Id);




            return true;
        }
        public List<string> getCharacters()
        {
            List<string> to_return = new List<string>();

            return to_return;
        }
        public bool createCharacter()
        {
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
            Console.WriteLine(exec.error_string);
            return exec.error;
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
