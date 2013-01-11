using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;


namespace MUDServer
{
    public abstract class BasicSQLExecuter
    {
        public bool error = false;
        public string error_string = "";
        public string query = "";

        protected List<OdbcParameter> _parameters = new List<OdbcParameter>();



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
        public void add_parameter(bool value)
        {
            OdbcParameter param = new OdbcParameter();
            param.DbType = DbType.Boolean;
            param.Value = value;

            _parameters.Add(param);
        }
        public void add_parameter(OdbcParameter param)
        {
            _parameters.Add(param);
        }

        protected virtual OdbcConnection get_connection()
        {
            return new OdbcConnection("DRIVER={MySQL ODBC 5.2w Driver}; SERVER=mrknister.justdied.com; DATABASE=MUDEngine; UID=mudengineer;PWD=hAWFYe2YsNHXZrtF;");
        }
        public abstract bool execute_query();

    }
}
