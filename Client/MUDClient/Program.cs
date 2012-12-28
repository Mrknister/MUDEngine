using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace MUDClient
{
    class Program
    {
        static NetworkStream stream;
        static byte[] myReadBuffer = new byte[1024];


        static void Main(string[] args)
        {
            Console.Write("Bitte geben sie den Hostnamen ein: ");
            String host_name = Console.ReadLine();
            TcpClient client;


            try
            {
                client = new TcpClient(host_name, 666);
                stream = client.GetStream();
                stream.BeginRead(myReadBuffer, 0, myReadBuffer.Length, new AsyncCallback(read_callback), stream);

                while (client.Connected && stream.CanWrite)
                {
                    String message = Console.ReadLine() + "\n";
                    byte[] outputArray = Encoding.UTF8.GetBytes(message);
                    try
                    {
                        stream.Write(outputArray, 0, outputArray.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Fehler! Der Hostname darf nicht null sein.");
                Console.ReadLine();
                return;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Ungültiger Port.");
                Console.ReadLine();
                return;
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                Environment.Exit(0);
            }




        }


        public static void read_callback(IAsyncResult ar)
        {

            try
            {
                String myCompleteMessage = "";
                int numberOfBytesRead;

                numberOfBytesRead = stream.EndRead(ar);
                myCompleteMessage =
                    String.Concat(myCompleteMessage, Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                // message received may be larger than buffer size so loop through until you have it all.

                if(stream.CanRead)
                    stream.BeginRead(myReadBuffer, 0, myReadBuffer.Length, new AsyncCallback(read_callback), stream);
                // Print out the received message to the console.
                Console.WriteLine( myCompleteMessage);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            

            
        }

    }
}
