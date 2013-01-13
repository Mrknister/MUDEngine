using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class CharacterSelectionDialogue : UserDialogue
    {
        long U_Id;
        long C_Id;
        bool dialogue_type=false; 


        public CharacterSelectionDialogue(UserDialogue.WriteMethod write,long U_Id)
        {
            this.U_Id = U_Id;
            this.write = write;
            writeStartMessage();
        }
        public override bool interpretMessage(string message)
        {
            if (dialogue_type)
            {
                if (message.ToLower().Trim() == "neu")
                {
                    return true;
                }
                else
                {
                    write("Du sollst neu eingeben du Honk!");
                    return false;
                }
            }

            if (selectCharacter(message.Trim()))
            {
                write("Character gewaehlt.\n");
                return true;
            }
            else
            {
                write("Ich habe den Character nicht gefunden.");
                writeStartMessage();
                return false;
            }
            
        }
        public override UserDialogue getNextDialogue()
        {
            if (dialogue_type)
            {
                return new CharacterDialogue(write, U_Id);
            }
            else
            {
                return new GameLoopDialogue(write,C_Id);
            }
        }

        private void writeStartMessage()
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select Name from `Character` where U_Id=?";
            sql.add_parameter(U_Id);
            sql.execute_query();

            if (sql.error)
            {
                Console.WriteLine(sql.error_string);
                return;
            }
            string name;
            if (!sql.HasRows)
            {
                write("Du hast noch keinen Charakter!\nGebe 'neu' ein um einen Character zu erstellen.");
                dialogue_type = true;
                return;
            }
            write("Waehle aus der Liste den Charakter mit dem du spielen willst\n");
            foreach (object[] names in sql.result)
            {
                name = Convert.ToString(names[0]);
                write(name + "\n");
            }
        }
        private bool selectCharacter(string name)
        {
            ReadableSQLExecuter sql = new ReadableSQLExecuter();
            sql.query = "select C_Id from `Character` where Name=? and U_Id=?";
            sql.add_parameter(name);
            sql.add_parameter(U_Id);
            sql.execute_query();

            if (!sql.HasRows)
            {
                return false;
            }
            C_Id = Convert.ToInt64(sql.result[0][0]);

            return true;
        }
    }
}
