using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;


namespace MUDServer
{
    class UnreadableSQLExecuter
    {
        public bool error = false;
        public string error_string = "";
        public string query;
        public int rows_affected = 0;
        
        List<OdbcParameter> _parameters = new List<OdbcParameter>();


        public UnreadableSQLExecuter()
        {
        }
        
        
        public void add_parameter(long value)
        {
            
            OdbcParameter param = new OdbcParameter();
            param.DbType = DbType.Int64;
            param.Value = value;
            _parameters.Add(param);
        }
        public void add_parameter(string value)
        {
            
            OdbcParameter param = new OdbcParameter();
            param.DbType = DbType.String;
            param.Value = value;

            _parameters.Add(param);
        }
        public void add_parameter(OdbcParameter param)
        {
            _parameters.Add(param);
        }
        
        
        public bool execute_query()
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
