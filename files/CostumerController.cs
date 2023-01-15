using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class CostumerController : BasedController
    {
        public CostumerController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, email, name, surname from customer";


            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("email: {0}", rdr.GetValue(1));
                    Console.WriteLine("Name: {0}", rdr.GetValue(2));
                    Console.WriteLine("Surname: {0}", rdr.GetValue(3));
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
            string sqlInsert = "Insert into customer(email,name,surname) VALUES(@email, @name, @surname)";

            string email = null;
            string name = null;
            string surname = null;
            

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Customer properties:");
                Console.WriteLine("Email:");
                email = Console.ReadLine();
                if (email.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of email shouldn't be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }

                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name shouldn't be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }


                surname = Console.ReadLine();
                if (surname.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of surname shouldn't be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }
                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("surname", surname);
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
            base.Delete("delete from customer where id = ");
        }
        public override void Update()
        {
            base.Update("Update customer ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into customer(email,name,surname) (select "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomString
                + "from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
