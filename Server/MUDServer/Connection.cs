using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace MUDServer
{
    class Connection
    {
        TcpClient _client;
        NetworkStream _stream;
        String client_message = "";
        byte[] bytemessage = new byte[256];
        MessageInterpreter _interpreter;
        
        public Connection(TcpClient client)
        {
            Console.WriteLine("Connected");
            _client = client;
            _stream = client.GetStream();
            MessageInterpreter.writemethod wm = new MessageInterpreter.writemethod(write_message);
            _interpreter = new MessageInterpreter(wm);

            

        }
        public void Work()
        {
            while (_stream.CanRead)
            {
                try
                {
                    int bytes_read = _stream.Read(bytemessage, 0, bytemessage.Length);
                    client_message += Encoding.UTF8.GetString(bytemessage, 0, bytes_read);

                    if (client_message.Contains('\n'))
                    {
                        string tmp = client_message.Substring(0, client_message.IndexOf('\n'));
                        client_message = client_message.Remove(0, client_message.IndexOf('\n') + 1);
                        Console.WriteLine("tmp:" + tmp);
                        _interpreter.interpretMessage(tmp);
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception in Work:"+e.Message);
                    return;
                }
                
            }
        }
        private void write_message(String message)
        {
            byte[] b_array = Encoding.UTF8.GetBytes(message);
            try
            {
                _stream.BeginWrite(b_array, 0, b_array.Length, new AsyncCallback(write_callback), null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in write_message:" + e.Message);
            }
        }
        private void write_callback(IAsyncResult r)
        {
            _stream.EndWrite(r);
        }
    }
}
