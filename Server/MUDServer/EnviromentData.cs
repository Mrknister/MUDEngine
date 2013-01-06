using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class EnviromentData
    {
        Int64 R_Id;
        string R_Name;
        string R_Discription;
        public EnviromentData(long C_Id)
        {
        }
        public void RoomData()
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select `Room`.R_Id,`Room`.Name,`Room`.Description from `Room`,`Character` where `Room`.R_Id = `Character`.R_Id";
            R_Id = Convert.ToInt64(sql.result[0][0]);
            R_Name = Convert.ToString(sql.result[0][1]);
            R_Name = Convert.ToString(sql.result[0][2]);
        }
    }
}
