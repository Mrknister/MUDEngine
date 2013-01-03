using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;


namespace MUDServer
{
    class UserData
    {
        OdbcConnection DbConnection;//= new OdbcConnection("DRIVER={MySQL ODBC 5.2w Driver}; SERVER=localhost; DATABASE=MUDEngine; UID=mudenineer;PWD=1234;");
        public UserData()
        {
            try
            {
                DbConnection = new OdbcConnection("DRIVER={MySQL ODBC 5.2w Driver}; SERVER=localhost; DATABASE=MUDEngine; UID=mudenineer;PWD=1234;");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public bool login(string name, string password)
        {
            string query = "select * from User where UName=? and Password=?";

            OdbcCommand command = new OdbcCommand(query, DbConnection);
            OdbcParameter param;

            // add name to parameters
            param = new OdbcParameter();
            param.DbType = DbType.String;
            param.Value = name;
            command.Parameters.Add(param);

            //add password...
            param = new OdbcParameter();
            param.DbType = DbType.String;
            param.Value = password;
            command.Parameters.Add(param);

            

            return true;
        }
        public bool register(string name, string password)
        {
            name = name.Trim();
            if (!check_register(name, password))
            {
                return false;
            }
            string query = "";
            return true;
        }

        private bool check_register(string name, string password)
        {
            if (name.Length < 3 || name.Length > 12)
            {
                return false;
            }
            if (password.Length < 8)
            {
                return false;
            }
            return true;
        }
    }
}
