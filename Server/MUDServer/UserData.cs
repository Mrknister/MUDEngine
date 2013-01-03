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
            try
            {
                

                if (!sql.HasRows)
                {

                    return false;
                }
                U_Id = (long) sql.result[0][0];



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
            command.Parameters.Clear();

            name = name.Trim();
            if (!check_register(name, password)) // check register adds the parameters to the command.
            {
                return false;
            }

            string query = "insert into User (Name,Password,LastLogin) values (?,?,NOW())";
            command.CommandText = query;
            command.ExecuteNonQuery();
            return true;
        }

        private bool check_register(string name, string password)
        {
            command.Parameters.Clear();

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
            command.CommandText = query;
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
                    return false;
                }

                reader.Close();

            }
            catch (Exception e)
            {

                Console.WriteLine("Fehler!");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}
