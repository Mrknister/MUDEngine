using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MUDServer
{
    class Config
    {
        public string DataTransfer, FileLine, DatabaseHost, DatabaseUser, DatabasePassword;
        

        public string ReadFile(String ConfigFile)
        {
            StreamReader ConfigFiles = new StreamReader(@ConfigFile);
            while ((FileLine = ConfigFiles.ReadLine()) != "#")
            {

                if( FileLine.StartsWith("databaseuser =")==true)
                {
                    DataTransfer = SetDatabaseUser(FileLine);
                }
                if( FileLine.StartsWith("databasepassword =")==true)
                {
                    DataTransfer = SetDatabasePassword(FileLine);
                }
                if( FileLine.StartsWith("databasehost =")==true)
                {
                   DataTransfer=SetDatabaseHost(FileLine);
                }
                else
                {
                    DataTransfer = "Invalid or End of Configfile";
                }

                return DataTransfer;
             }
            
           


        }

        private string SetDatabaseUser(string line)
        {
            

            return DataTransfer;

        }

        private string SetDatabasePassword(string line)
        {
            

            return DataTransfer;
        }

        private string SetDatabaseHost(string line)
        {
            

            return DataTransfer;
        }
       
      

    }
}
