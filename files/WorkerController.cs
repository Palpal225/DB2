using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class WorkerController : BasedController
    {
        public WorkerController(string connectionString) : base(connectionString) { }

        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, name, surname, hotelId from worker";


            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Name: {0}", rdr.GetValue(1));
                    Console.WriteLine("Surname: {0}", rdr.GetValue(2));
                    Console.WriteLine("Hotel Id: {0}", rdr.GetValue(3));
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
            string sqlInsert = "Insert into worker (name,surname, hotelid) VALUES(@name,@surname,@hotelid)";

            string name = null;
            string surname = null;
            int hotel_id = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter worker properties:");
                Console.WriteLine("Name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name shouldn't be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Surname:");
                surname = Console.ReadLine();
                if (surname.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of surname shouldn't be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }


                Console.WriteLine("Hotel id:");
                correct = Int32.TryParse(Console.ReadLine(), out hotel_id);
                if (correct == false)
                {
                    Console.WriteLine("Hotel id must be a number!");
                    Console.ReadLine();
                }


                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("surname", surname);
            cmd.Parameters.AddWithValue("hotelid", hotel_id);
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
            base.Delete("delete from worker where id = ");
        }
        public override void Update()
        {
            base.Update("Update worker ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into worker(name, surname, hotelid) (select "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomString
                + ",hotel.id from generate_series(1, 1000000), hotel limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
