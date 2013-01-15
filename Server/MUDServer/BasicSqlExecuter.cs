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
        static Config conf;

        public bool error = false;
        public string error_string = "";
        public string query = "";

        protected List<OdbcParameter> _parameters = new List<OdbcParameter>();

        static BasicSQLExecuter()
        {
            conf = new Config();
            if (!conf.ReadFile(@"..\..\..\..\doc\config.txt"))
            {
                Console.WriteLine("Failed to read the Config file.");
            }
            Console.WriteLine(conf.DatabaseHost);
            Console.WriteLine(conf.DatabasePassword);
            Console.WriteLine(conf.DatabaseUser);
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
        public void add_parameter(bool value)
        {
            OdbcParameter param = new OdbcParameter();
            param.DbType = DbType.Boolean;
            param.Value = value;

            _parameters.Add(param);
        }
        public void add_parameter(double value)
        {
            OdbcParameter param = new OdbcParameter();
            param.DbType = DbType.Double;
            param.Value = value;

            _parameters.Add(param);
        }
        public void add_parameter(OdbcParameter param)
        {
            _parameters.Add(param);
        }

        protected virtual OdbcConnection get_connection()
        {
            string connection_string="DRIVER={MySQL ODBC 5.2w Driver}; DATABASE=MUDEngine;";
            
            connection_string += "SERVER=" + conf.DatabaseHost+"; UID="+conf.DatabaseUser+"; PWD="+conf.DatabasePassword+";";
            OdbcConnection conn = new OdbcConnection(connection_string);

            
            return conn;
        }
        public abstract bool execute_query();

    }
}
