using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace MUDServer
{
    class Server
    {
        List<Connection> connections = new List<Connection>();
        List<Thread> thread_list = new List<Thread>();

        TcpListener listener;
        
        public Server(int port = 666)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
        public void startListen()
        {
            try
            {

                listener.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Connection conn = new Connection(client);
                    connections.Add(conn);
                    Thread th = new Thread(conn.Work);
                    thread_list.Add(th);
                    th.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
        }
    }
}
