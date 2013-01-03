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
                DbConnection = new OdbcConnection("DRIVER={MySQL ODBC 5.2w Driver}; SERVER=localhost; DATABASE=MUDEngine; UID=mudengineer;PWD=1234;");
                Console.WriteLine(check_register("Jan", "12345678"));
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
            try
            {
                DbConnection.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

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
                reader.Close();
                DbConnection.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler!");
                Console.WriteLine(e.Message);
                DbConnection.Close();
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
            //check if lenght of name is between 3 and 12 and password lenght > 8
            if (name.Length < 3 || name.Length > 12)
            {
                return false;
            }
            if (password.Length < 8)
            {
                return false;
            }
            
            string query = "select U_Id from User where Name=?";
            //check if name already exist
            OdbcCommand command = new OdbcCommand(query, DbConnection);
            try
            {
                DbConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DbConnection.Close();
                return false;
            }

            OdbcParameter param;

            // add name to parameters
            param = new OdbcParameter();
            param.DbType = DbType.String;
            param.Value = name;
            command.Parameters.Add(param);
            try
            {
                OdbcDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return false;
                }
                DbConnection.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler!");
                Console.WriteLine(e.Message);
                DbConnection.Close();
                return false;
            }
<<<<<<< HEAD
            
            string query = "select U_Id from User where Name=?";
            //check if name already exist
            OdbcCommand command = new OdbcCommand(query, DbConnection);
            try
            {
                DbConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DbConnection.Close();
                return false;
            }

            OdbcParameter param;

            // add name to parameters
            param = new OdbcParameter();
            param.DbType = DbType.String;
            param.Value = name;
            command.Parameters.Add(param);
            try
            {
                OdbcDataReader reader = command.ExecuteReader();

                
                if (reader.HasRows)
                {
                    reader.Close();
                    DbConnection.Close();
                    return false;
                }
                reader.Close();
                DbConnection.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler!");
                Console.WriteLine(e.Message);
                DbConnection.Close();
                return false;
            }
            return true;
        }
    }
}
