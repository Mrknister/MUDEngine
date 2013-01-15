using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class DataContainer
    {
        public DataContainer()
        {
            Console.WriteLine("Datacontainer erstellt");
            //c_data = null;
            //r_data = null;
        }
        public CharacterData c_data;
        public Room r_data;
    }
}
