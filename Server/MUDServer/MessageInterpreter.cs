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

        string user_name = "";
        string password = "";
        string user_character = "";
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
                    interpretRegister(message);
                    break;
                case 2:
                    interpretLogin(message);
                    break;
                case 3:
                    interpretCharacterSelection(message);
                    break;
                case 4:
                    interpretCharacterBuild(message);
                    break;
                case 5:
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

        private void interpretStart(string Message)//case 0
        {
            Message = Message.ToLower();
            if (Message.StartsWith("login"))
            {
                write("Gebe nun deinen Nutzernamen ein");
                changeStatus(2);
            }
            else if (Message.StartsWith("neu"))
            {
                write("Gebe einen Nutzernamen ein");
                changeStatus(1);
            }
        }

        private void interpretRegister(string Message)//case 1
        {
            if (substatus == 0)
            {
                user_name = Message.Trim();
                write("Gebe nun das Passwort ein");
                substatus++;
            }
            
            else if (substatus == 1)
            {
                password = Message;
                write("Gebe das Passwort erneut ein");
                substatus++;
            }
            else if (substatus == 2)
            {
                if (password != Message)
                {
                    write("Die Passwörter stimmen nicht überein\n Gebe dein Passwort ein");
                    substatus = 1;
                    return;
                }



                this._user_data = new UserData();
                if (this._user_data.register(user_name, password))
                {
                    write("Registrierung erfolgreich");
                }
                else
                {
                    write("Registrierung fehlgeschlagen! Der Benutzer existiert bereits, der Benutzername enthält ungültige Zeichen oder dein Passwort ist nicht lang genug");
                }
                changeStatus(0);
            }

            else
            {
                changeStatus(0);
            }
        }

        private void interpretLogin(string Message)//case 2
        {
            if (substatus == 0)
            {
                user_name = Message.Trim();
                write("Gebe nun das Passwort ein");
                substatus++;
            }
            else if (substatus == 1)
            {
                password = Message;
                
                this._user_data = new UserData();
                if (this._user_data.login(user_name, password))
                {
                    write("login erfolgreich \nUm einen Charakter zu waehlen tippe: select \nUm einen Charakter zu erstellen tippe: build");
                    changeStatus(3);
                }
                else
                {
                    write("login fehlgeschlagen!");
                    changeStatus(0);
                }
            }
            
            else
            {
                changeStatus(0);
            }
        }

        public void interpretCharacterSelection(string Message)//case 3
        {

            Message = Message.ToLower();
            if (Message.StartsWith("select"))
            {
                ReadableSQLExecuter sql = new ReadableSQLExecuter();
                sql.query = "select Name from `Character` where U_Id=?";
                sql.add_parameter(_user_data.U_Id);
                sql.execute_query();
                if (sql.error)
                {
                    Console.WriteLine(sql.error_string);
                }
                string name;
                
                foreach (object[] names in sql.result)
                {
                   name = Convert.ToString(names[0]);
                   write(name);
                }
                
            }
            else if (Message.StartsWith("build"))
            {
                write("Gebe den Namen deines Charakters ein");
                changeStatus(4);
            }
        }
        
        private void interpretCharacterBuild(string Message)//case 4
        {
            if (_user_data.U_Id != 0)
            {
                user_character = Message.Trim();
                UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
                exec.query = "insert into `Character` (U_Id,Name,Money,Health,Mana,Damage,PhRes,MaRes,MaxHealth,MaxMana) values (?,?,200,100,100,10,10,10,100,100)";

                exec.add_parameter(_user_data.U_Id);
                exec.add_parameter(user_character);
                exec.execute_query();
                if (exec.error)
                {
                    Console.WriteLine(exec.error_string);
                }
                changeStatus(3);
            }
            else
            {
                write("Du bist nicht eingeloggt !");
                changeStatus(0);
            }
        }
        
        private void interpretGameLoop(string Message)//case 5
        {
        }

    }
}
