using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MUDServer
{
    class Config
    {
        public string DatabaseHost = "mrknister.justdied.com";
        public string DatabaseUser = "mudengineer";
        public string DatabasePassword = "hAWFYe2YsNHXZrtF";

        public bool ReadFile(String ConfigFile)
        {
            if (!File.Exists(ConfigFile))
            {
                return false;
            }
            StreamReader ConfigFiles = new StreamReader(ConfigFile);
            while (!ConfigFiles.EndOfStream)                            // While Loop for setting global variables
            {
                string FileLine;
                FileLine = ConfigFiles.ReadLine();
                FileLine = FileLine.Trim();

                if (FileLine.ToLower().StartsWith("databaseuser"))
                {
                    DatabaseUser = GetValue(FileLine);
                }
                if (FileLine.ToLower().StartsWith("databasepassword") )
                {
                    DatabasePassword = GetValue(FileLine);
                }
                if (FileLine.ToLower().StartsWith("databasehost"))
                {
                    DatabaseHost = GetValue(FileLine);
                }
                else
                {
                }
            }
            return true;

        }

        private string GetValue(string line)                            // Method to get the values for the Database out of the configfile.
        {
            int Found;
            
            Found = line.IndexOf("=");
            line = line.Remove(0, Found + 1);
            line = line.Trim();
            Console.WriteLine(line);

            return line;

        }
    }
}
