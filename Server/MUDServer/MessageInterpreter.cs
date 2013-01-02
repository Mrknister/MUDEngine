﻿using System;
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
        string user_name = "";
        string password = "";
        UserData _user_data;

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

        private void changeStatus(uint status)
        {
            this.status = status;
            this.substatus = 0;
        }
        private void interpretStart(string Message)
        {
            Message = Message.ToLower();
            if (Message.StartsWith("login"))
            {
                write("Gebe nun deinen Nutzernamen ein");
                changeStatus(1);
            }
        }
        private void interpretLogin(string Message)
        {
            if (substatus == 0)
            {
                user_name = Message.Trim();
            }
            else if (substatus == 1)
            {
                password = Message;
            }
            else if (substatus == 2)
            {
                _user_data = new UserData(user_name, password);
            }
            else
            {
                changeStatus(0);
            }
        }
        private void interpretRegister(string Message)
        {
        }
        private void interpretGameLoop(string Message)
        {
        }
        
    }
}
