using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class GameObject
    {

        public static GameObject getObjectByName(ref DataContainer cont, string objectName)
        {

            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select `Objekt`.O_Id from `Objekt`,`ObjInRoom` where `Objekt`.O_Id=`ObjInRoom`.O_Id and `ObjInRoom`.R_Id=? and `Objekt`.O_Id not in (select O_Id from `Takeable` where R_Id=? and not Takefrom='') and `Objekt`.Name=?";
            sql.add_parameter(cont.r_data.R_Id);
            sql.add_parameter(cont.r_data.R_Id);
            sql.add_parameter(objectName);
            sql.execute_query();
            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return null;
            }
            if (!sql.HasRows)
            {
                return null;
            }
            return new GameObject(ref cont,Convert.ToInt64(sql.result[0][0]));

        }
        DataContainer container;

        long O_Id;
        public string Name, Description;

        public bool error;
        public string error_string;

        public GameObject(ref DataContainer cont,long objectId)
        {
            this.O_Id = objectId;
            this.container = cont;
            loadAttributes();
        }
        public bool isValid()
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select `Object`.O_Id from `Object`,`ObjectIn` where `Object`.O_Id=`ObjectIn`.O_Id and `Object`.O_Id=? and `ObjectIn`.R_Id=? ";
            exec.add_parameter(O_Id);
            exec.add_parameter(container.r_data.R_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                this.error = true;
                this.error_string = exec.error_string;
                return false;
            }
            if (!exec.HasRows)
            {
                return false;
            }
            return true;
        }
        private void loadAttributes()
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Name,Description from `Object` where O_Id=?";
            exec.add_parameter(O_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                this.error = true;
                this.error_string = exec.error_string;
            }
            if (!exec.HasRows)
            {
                this.error = true;
                this.error_string = "Object Id not found.";
                return;
            }
            this.Name = Convert.ToString(exec.result[0][0]);
            this.Description = Convert.ToString(exec.result[0][1]);
        }
    }
}
