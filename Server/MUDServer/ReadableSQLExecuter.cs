using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;

namespace MUDServer
{
    class ReadableSQLExecuter : BasicSQLExecuter
    {
        public List<object[]> result;
        public bool HasRows = false;



        public ReadableSQLExecuter()
        {

        }


        public override bool execute_query()
        {
            using (OdbcConnection DbConnection = get_connection() )
            {
                
                OdbcCommand _command = new OdbcCommand(query, DbConnection);
                if (_parameters.Count != 0)
                {
                    _command.Parameters.AddRange(_parameters.ToArray());
                }

                try
                {
                    DbConnection.Open();

                    OdbcDataReader reader = _command.ExecuteReader();
                    HasRows = reader.HasRows;

                    if (reader.HasRows && reader.FieldCount > 0)
                    {
                        result = new List<object[]>();
                        while (reader.Read())
                        {
                            object[] tmp = new object[reader.FieldCount];
                            reader.GetValues(tmp);
                            result.Add(tmp);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Empty set");
                    }
                    reader.Close();

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
