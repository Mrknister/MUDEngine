using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CharacterBuildDialogue : UserDialogue
    {
        long U_Id;
        public CharacterBuildDialogue(UserDialogue.WriteMethod write,long U_Id)
        {
            this.U_Id = U_Id;
            this.write = write;
            writeStartMessage();
        }
        public override bool interpretMessage(string message)
        {
            if(createCharacter(message.Trim()))
            {
                return true;
            }
            write("Der Charactername ist bereits vergeben.\nDenke dir einen anderen aus\n");
            return false;

        }
        public override UserDialogue getNextDialogue()
        {
            return new CharacterDialogue(write,U_Id);
        }
        private void writeStartMessage()
        {
            write("Gebe den Namen des Characters ein.\n");
        }
        private bool createCharacter(string name)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select C_Id from `Character` where Name=?";
            sql.add_parameter(name);
            sql.execute_query();
            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return false;
            }
            if (sql.HasRows)
            {
                return false;
            }
            UnreadableSQLExecuter exec = new UnreadableSQLExecuter();
            exec.query = "insert into `Character` (U_Id,Name,Money,Health,Damage,PhRes,MaxHealth,R_Id) values (?,?,200,100,30,10,100,1)";

            exec.add_parameter(U_Id);
            exec.add_parameter(name);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            return true;
        }
    }
}
