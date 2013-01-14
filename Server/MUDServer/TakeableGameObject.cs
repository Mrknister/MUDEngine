using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class TakeableGameObject
    {
        public static TakeableGameObject getObjectByName(ref DataContainer cont, string objectName)
        {

            return getObjectByName(ref cont, objectName, "");

        }
        public static TakeableGameObject getObjectByName(ref DataContainer cont, string objectName,string object_in)
        {

            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select O_Id from `Objekt` where `Objekt`.O_Id in (select O_Id from `Takeable` where R_Id=? and Takefrom=? and RespawnAtTime<NOW()) and `Objekt`.Name=?";
            sql.add_parameter(cont.r_data.R_Id);
            sql.add_parameter(object_in);
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
            return new TakeableGameObject(ref cont, Convert.ToInt64(sql.result[0][0]));

        }
        DataContainer container;

        long O_Id;
        public string Name, Description;
        long I_Id;

        public bool error;
        public string error_string;

        public TakeableGameObject(ref DataContainer cont, long objectId)
        {
            this.O_Id = objectId;
            this.container = cont;
            loadAttributes();
        }
        public bool isValid()
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select `Objekt`.O_Id from `Objekt`,`Takeable` where `Objekt`.O_Id=`Takeable`.O_Id and `Objekt`.O_Id=? and `Takeable`.RespawnAtTime<NOW() and `Takeable`.R_Id=? ";
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
            exec.query = "select `Objekt`.Name,`Objekt`.Description,`Takeable`.I_Id from `Objekt`,`Takeable` where `Objekt`.O_Id=`Takeable`.I_Id and `Objekt`.O_Id=?";
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
            this.I_Id = Convert.ToInt64(exec.result[0][2]);
        }
        public bool take()
        {
            if (!isValid())
            {
                return false;
            }
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "update Takeable set RespawnAtTime=NOW()+Respawntime where I_Id=? and O_Id=?";
            exec.add_parameter(I_Id);
            exec.add_parameter(O_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }

            return Item.giveUser(ref container, I_Id); ;
        }
    }
}
