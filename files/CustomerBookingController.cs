using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD2.Controllers
{
    public class CustomerBookingController : BasedController
    {


        public CustomerBookingController(string connectionString) : base(connectionString) { }
        public override void Read(string whereCondition)
        {
            Console.Clear();

            sqlConnection.Open();

            string sqlSelect = "select id, customerid, bookingid from customerbooking";


            using var cmd = new NpgsqlCommand(sqlSelect + whereCondition, sqlConnection);
            try
            {
                using NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine("Id: {0}", rdr.GetValue(0));
                    Console.WriteLine("Customer Id: {0}", rdr.GetValue(1));
                    Console.WriteLine("Booking Id: {0}", rdr.GetValue(2));
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
            string sqlInsert = "Insert into customerbooking (customerid, bookingid) VALUES(@customerid, @bookingid)";

            int customer_id = 0;
            int booking_id = 0;

            bool correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter customerbooking properties:");
                Console.WriteLine("Customer id:");
                correct = Int32.TryParse(Console.ReadLine(), out customer_id);
                if (correct == false)
                {
                    Console.WriteLine("Customer id must be a number!");
                    Console.ReadLine();
                }

                Console.WriteLine("Booking id:");
                correct = Int32.TryParse(Console.ReadLine(), out booking_id);
                if (correct == false)
                {
                    Console.WriteLine("Booking id must be a number!");
                    Console.ReadLine();
                }
                correct = true;
            } while (correct == false);


            sqlConnection.Open();

            using var cmd = new NpgsqlCommand(sqlInsert, sqlConnection);
            cmd.Parameters.AddWithValue("customerid", customer_id);
            cmd.Parameters.AddWithValue("bookingid", booking_id);
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
            base.Delete("delete from customerbooking where id = ");
        }
        public override void Update()
        {
            base.Update("Update customerbooking");
        }

        public override void Generate()
        {
            Console.WriteLine("What amount of records do you want?");
            bool correct = false;
            int recordsAmount;
            correct = Int32.TryParse(Console.ReadLine(), out recordsAmount);
            string subquery = "with pa as (    select customer.id    from customer    where customer.id not in (        select customer.id        from customerbooking         join customer on customerbooking.customerid = customer.id 	) 	limit(1)), "
                + "va as (    select booking.id     from booking    where booking.id not in (         select booking.id         from customerbooking join booking on customerbooking.bookingid = bookingid 	)	limit(1))";
            string sqlGenerate = subquery + " insert into customerbooking(customerid, bookingid) (select pa.id, va.id from pa, va limit(1))";

            for (int i = 0; i < recordsAmount; i++)
            {
                base.Generate(sqlGenerate);
            }
        }
    }
}

