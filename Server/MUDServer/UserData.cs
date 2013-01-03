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
        OdbcConnection DbConnection;
        long U_Id;

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
            string query = "select U_Id from User where Name=? and Password=?";

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

            try
            {
                OdbcDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    
                    return false;
                }
                reader.Read();
                U_Id = reader.GetInt64(0);
                Console.WriteLine(U_Id);

            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler!");
                Console.WriteLine(e.Message);
                return false;
            }


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
