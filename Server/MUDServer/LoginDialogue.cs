using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class LoginDialogue : UserDialogue
    {
        int status = 0;
        string Name;
        string Password;
        long U_Id;

        public LoginDialogue(UserDialogue.WriteMethod write)
        {
            this.write = write;
            writeStartMessage();
        }
        public override bool interpretMessage(string message)
        {
            if (status == 0)
            {
                Name = message.Trim();
                write("Gebe nun ein Passwort ein.\n");
                status++;
            }
            else if (status == 1)
            {
                Password = message;
                if (login(Name, Password))
                {
                    write("Login erfolgreich.\n");
                    return true;
                }
                else
                {
                    write("Login fehlgeschlagen.\n");
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
            return new CharacterDialogue(write, U_Id);
        }
        private void writeStartMessage()
        {
            write("Gebe bitte deinen Nutzernamen an\n");
        }
        public bool login(string name, string password)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select U_Id from User where Name=? and Password=?";

            sql.add_parameter(name);
            sql.add_parameter(password);
            sql.execute_query();

            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }

            if (!sql.HasRows)
            {
                return false;
            }
            U_Id = Convert.ToInt64(sql.result[0][0]);

            return true;
        }
    }
}
