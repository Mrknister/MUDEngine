using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class RegisterDialogue : UserDialogue
    {
        int status=0;
        string Name;
        string Password;

        public RegisterDialogue(UserDialogue.WriteMethod write)
        {
            this.write = write;
            writeStartMessage();
        }
        public override bool interpretMessage(string message)
        {
            if (status == 0)
            {
                Name = message.Trim();
                write("Gebe nun ein Passwort ein.");
                status++;
            }
            else if (status == 1)
            {
                Password = message;
                if (Password.Length < 8)
                {
                    write("Dein Passwort muss mindestens 8 Buchstaben enthalten.\nDenke dir ein neues aus.\n");
                    return false;
                }
                write("Gebe es bitte erneut ein");
                status++;
            }
            else if (status == 2)
            {
                string Password2 = message;//.Trim();
                if (Password != Password2)
                {
                    status = 1;
                    write("Die Passwoerter stimmen nicht ueberein.\nGebe dein Passwort ein\n");
                    return false;
                }

                if (register())
                {
                    write("Registrierung erfolgreich!\n");
                    return true;
                }
                else
                {
                    write("Der Nutzername existiert bereits\nDenke dir einen neuen aus.\n");
                    writeStartMessage();
                    status = 0;
                }
            }
            else
            {
                writeStartMessage();
                status = 0;
            }
            return false;
        }
        public override UserDialogue getNextDialogue()
        {
            return new LoginDialogue(write);
        }
        private void writeStartMessage()
        {
            write("Gebe nun den gewuenschten Namen ein:\n");
        }
        private bool register()
        {
            Name = Name.Trim();
            if (!check_register(Name, Password)) // check register adds the parameters to the command.
            {
                return false;
            }

            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "insert into User (Name,Password,LastLogin) values (?,?,NOW())";
            exec.add_parameter(Name);
            exec.add_parameter(Password);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }

            return true;
        }

        private bool check_register(string name, string password)
        {

            //check if lenght of name is between 3 and 12 and password lenght > 8
            if (name.Length < 3 || name.Length > 12)
            {
                return false;
            }
            if (password.Length < 8)
            {
                return false;
            }


            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select U_Id from User where Name=?";
            //check if name already exist
            exec.add_parameter(name);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            if (exec.HasRows)
            {
                return false;
            }
            
            return true;
        }
    }
}
