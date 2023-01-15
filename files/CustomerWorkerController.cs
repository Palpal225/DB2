using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class CustomerWorkerController : BasedController
    {
        public CustomerWorkerController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, customerid, workerid from customerworker";


            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Customer Id: {0}", rdr.GetValue(1));
                    Console.WriteLine("Worker Id: {0}", rdr.GetValue(2));
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                sqlConnection.Close();
            }


            Console.ReadLine();
        }
        public override void Create()
        {
            string sqlInsert = "Insert into customerworker (customer, worker) VALUES(@customer, @worker)";

            int customer_id = 0;
            int worker_id = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter customerworker properties:");
                Console.WriteLine("customer id:");
                correct = Int32.TryParse(Console.ReadLine(), out customer_id);
                if (correct == false)
                {
                    Console.WriteLine("Customer id must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Worker id:");
                correct = Int32.TryParse(Console.ReadLine(), out worker_id);
                if (correct == false)
                {
                    Console.WriteLine("Worker id must be a number!");
                    Console.ReadLine();
                }


                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("customerid", customer_id);
            cmd.Parameters.AddWithValue("workerid", worker_id);
            cmd.Prepare();

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public override void Delete()
        {
            base.Delete("delete from customerworker where id = ");
        }
        public override void Update()
        {
            base.Update("Update customerworker ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into customerworker(customerid, workerid) (select customer.id, worker.id"
                + " from customer, worker limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
