using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MUDServer
{
    class Config
    {
        public string  FileLine, DatabaseHost, DatabaseUser, DatabasePassword;
        
        public void ReadFile(String ConfigFile)
        {          
            {
            StreamReader ConfigFiles = new StreamReader(@ConfigFile);
            while (!ConfigFiles.EndOfStream)                            // While Loop for setting global variables
              {
                FileLine = ConfigFiles.ReadLine();
                FileLine = FileLine.Replace(" ", "");
                if (FileLine.StartsWith("databaseuser=") == true)
                {
                    DatabaseUser = GetValue(FileLine);
                }
                if (FileLine.StartsWith("databasepassword=") == true)
                {
                    DatabasePassword = GetValue(FileLine);
                }
                if (FileLine.StartsWith("databasehost=") == true)
                {
                    DatabaseHost = GetValue(FileLine);
                }
              } 
            }
        }

        private string GetValue(string line)                            // Method to get the values for the Database out of the configfile.
        {
            int Found;
            string value=line;

            Found = line.IndexOf("=");
            value = value.Remove(0, Found);

            return value;

        }
    }
}
