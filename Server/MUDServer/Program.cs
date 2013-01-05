using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Program
    {


        
        static void Main(string[] args)
        {
         /*   Config test = new Config();
            test.ReadFile(@"..\..\..\..\doc\configtest.txt");
            Console.WriteLine(test.DatabaseHost + " " +test.DatabasePassword +" "+ test.DatabaseUser);
            Console.ReadLine();
            */



           Server s = new Server();
            s.startListen();
            

        }
    }
}
