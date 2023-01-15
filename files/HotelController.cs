using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class HotelController : BasedController
    {
        public HotelController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, location, name from Hotel";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("location: {0}", rdr.GetValue(1));
                    Console.WriteLine("name: {0}", rdr.GetValue(2));
                    Console.WriteLine();
                }
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
            string sqlInsert = "Insert into hotel(location, name) VALUES(@location, @name)";

            string location = null;
            string name = null;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Provide the Hotel properties:");
                Console.WriteLine("Locatoin:");
                location = Console.ReadLine();
                if (location.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of location shouldn't be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("name:");
                name = Console.ReadLine();
                if (name.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of name shouldn't be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }


                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("location", location);
            cmd.Parameters.AddWithValue("name", name);
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
            base.Delete("delete from hotel where id = ");
        }
        public override void Update()
        {
            base.Update("Update hotel ");
        }
        public override void Find()
        {
            base.Find();
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into hotel(location, name) (select "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomString
                + "from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }


    }
}

