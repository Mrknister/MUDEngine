using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;


namespace MUDServer
{
    class UserData
    {
        OdbcConnection DbConnection = new OdbcConnection("DRIVER={MySQL ODBC 5.2w Driver}; SERVER=localhost; DATABASE=MUDEngine; UID=mudenineer;PWD=1234;");
        public UserData(string name, string password)
        {
        }
    }
}
