using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class MessageInterpreter
    {
        public delegate void writemethod(string message);
        writemethod write;
        uint status = 0;
        uint substatus = 0; // Niemals vergessen, diesen status wieder auf 0 zu setzen!
        long user_id = 42;

        public MessageInterpreter(writemethod w)
        {
            write = w;
            write("Wenn du ein neuer Nutzer bist tippe 'neu' ein. \nWenn du schon mal hier warst, tippe 'login' ein.");
        }
        public void interpretMessage(String message)
        {
            switch (status)
            {
                case 0:
                    interpretStart(message);
                    break;
                case 1:
                    interpretLogin(message);
                    break;
                case 2:
                    interpretRegister(message);
                    break;
                case 3:
                    interpretGameLoop(message);
                    break;
                default:
                    status = 0;
                    break;
            }
        }
        private void interpretStart(string Message)
        {

        }
        private void interpretLogin(string Message)
        {
        }
        private void interpretRegister(string Message)
        {
        }
        private void interpretGameLoop(string Message)
        {
        }
    }
}
