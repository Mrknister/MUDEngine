using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUDServer
{
    class Item
    {

        DataContainer container;





        public int consume(string item_name, bool consumation_type)
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select `Item`.I_Id,`Item`.Category,`BelongsTo`.Amount from `Item`,`BelongsTo` where `BelongsTo`.I_Id=`Item`.I_Id and `BelongsTo`.C_Id=? and `Item`.Name=?";
            exec.add_parameter(container.c_data.C_Id);
            exec.add_parameter(item_name);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return -1;
            }
            if (!exec.HasRows)
            {
                return 1;
            }
            long I_Id = Convert.ToInt64(exec.result[0][0]);
            string category = Convert.ToString(exec.result[0][1]);

            if (category != "Consumable")
            {
                return 2;
            }
            exec = new ReadableSQLExecuter();
            exec.query = "select Type,Amount,ConsumptionType from Consumable where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();

            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return -1;
            }
            if (!exec.HasRows)
            {
                Console.WriteLine("Consumable not found!");
                return -1;
            }
            string cons_type = Convert.ToString(exec.result[0][0]);
            long amount = Convert.ToInt64(exec.result[0][1]);
            if (Convert.ToBoolean(exec.result[0][2]) != consumation_type)
            {
                return 3;
            }
            if (cons_type == "Health")
            {
                if (!consumeHealth(amount))
                {
                    return -1;
                }
            }
            else if (cons_type == "PhRes")
            {
                if (!consumeArmor(amount, I_Id))
                {
                    return -1;
                }
            }
            else if (cons_type == "Damage")
            {
                if (!consumeDamage(amount, I_Id))
                {
                    return -1;
                }
            }


            return 0;
        }
        private bool consumeHealth(long amount)
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "update `Character` set Health = case when Health+=? <MaxHealth then Health+? else MaxHealth where `Character`.C_Id=? ";
            exec.add_parameter(amount);
            exec.add_parameter(amount);
            exec.add_parameter(container.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool consumeArmor(long amount, long I_Id)
        {
            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Name from `Item` where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();
            string Name = Convert.ToString(exec.result[0][0]);

            exec = new ReadableSQLExecuter();
            exec.query = "select now()+Duration from `Consumable` where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }

            double Buffduration = Convert.ToDouble(exec.result[0][0]);

            exec = new ReadableSQLExecuter();
            exec.query = "insert into `Buff`(Name, Amount, Type, RunsOutAt, C_Id) values(?,?,2,?,?) ";
            exec.add_parameter(Name);
            exec.add_parameter(amount);
            exec.add_parameter(Convert.ToString(Buffduration));
            exec.add_parameter(container.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool consumeDamage(long amount, long I_Id)
        {

            ReadableSQLExecuter exec = new ReadableSQLExecuter();
            exec.query = "select Name from `Item` where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }

            string Name = Convert.ToString(exec.result[0][0]);

            exec = new ReadableSQLExecuter();
            exec.query = "select now()+Duration from `Consumable` where I_Id=?";
            exec.add_parameter(I_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }

            double Buffduration = Convert.ToDouble(exec.result[0][0]);

            exec = new ReadableSQLExecuter();
            exec.query = "insert into `Buff`(Name, Amount, Type, RunsOutAt, C_Id) values(?,?,1,?,?) ";
            exec.add_parameter(Name);
            exec.add_parameter(amount);
            exec.add_parameter(Convert.ToString(Buffduration));
            exec.add_parameter(container.c_data.C_Id);
            exec.execute_query();
            if (exec.error)
            {
                Console.WriteLine(exec.error_string);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
