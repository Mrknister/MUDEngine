using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;


namespace MUDServer
{
    class UserData
    {
        OdbcConnection DbConnection ;//= new OdbcConnection("DRIVER={MySQL ODBC 5.2w Driver}; SERVER=localhost; DATABASE=MUDEngine; UID=mudenineer;PWD=1234;");
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

            return true;
        }
        public bool register(string name, string password)
        {

            return true;
        }
    }
}
