using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class BookingController : BasedController
    {
        public BookingController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, date, term from booking";

            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Date: {0}", rdr.GetValue(1));
                    Console.WriteLine("Term: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into booking(date, term) VALUES(@date, @term)";

            string date = null;
            int term = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter Booking properties:");
                Console.WriteLine("date:");
                if (date.Length > 40)
                {
                    correct = false;
                    Console.WriteLine("Length of date shouldn't be bigger than 40.");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("Term:");
                correct = Int32.TryParse(Console.ReadLine(), out term);
                if (correct == false)
                {
                    Console.WriteLine("Term must be a number!");
                    Console.ReadLine();
                }
                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("date", date);
            cmd.Parameters.AddWithValue("term", term);
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
            base.Delete("delete from booking where id = ");
        }
        public override void Update()
        {
            base.Update("Update booking ");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;

            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);

            string sqlGenerate = "insert into booking(date, term) (select "
                + base.sqlRandomString
                + ", "
                + base.sqlRandomInteger
                + " bookingid from generate_series(1, 1000000)  limit(" + recordsAmount + "))";
            base.Generate(sqlGenerate);
        }
    }
}
