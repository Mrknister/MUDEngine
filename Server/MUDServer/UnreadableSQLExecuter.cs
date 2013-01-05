using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;


namespace MUDServer
{
    class UnreadableSQLExecuter : BasicSQLExecuter
    {
        public int rows_affected = 0;
        

        public UnreadableSQLExecuter()
        {
        }
        
        
        
        public override bool execute_query()
        {
            using (OdbcConnection DbConnection = new OdbcConnection("DRIVER={MySQL ODBC 5.2w Driver}; SERVER=mrknister.justdied.com; DATABASE=MUDEngine; UID=mudengineer;PWD=hAWFYe2YsNHXZrtF;"))
            {
                DbConnection.Open();
                OdbcCommand _command = new OdbcCommand(query, DbConnection);
                if (_parameters.Count != 0)
                {
                    _command.Parameters.AddRange(_parameters.ToArray());
                }

                try
                {
                    rows_affected = _command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    error = true;
                    error_string = e.Message;
                    return false;
                }
            }
            return true;
        }
    }
}
