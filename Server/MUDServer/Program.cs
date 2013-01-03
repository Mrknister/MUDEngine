using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Program
    {
        static void test()
        {
            ReadableSQLExecuter test = new ReadableSQLExecuter();

            test.query = "select * from User";
            //test.add_parameter(1);
            test.execute_query();
            Console.WriteLine(test.error_string);
            foreach (object[] res in test.result)
            {
                Console.WriteLine(res[1]);
            }


        }
        static void Main(string[] args)
        {
            //Server s = new Server();
            //s.startListen();
            test();
            Console.ReadLine();

        }
    }
}
