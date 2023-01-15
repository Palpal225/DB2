using BD2.Controllers;
using System;

namespace BD2
{
    class Program
    {
        static void Main(string[] args)
        {
            String connectionString = "Host=localhost;Username=postgres;Password=Pavelstep117;Database=hoteln";

            int table = 0;
            int action = 0;
            do
            {
                table = FirstMenu();
                if (table == 0)
                {
                    return;
                }

                BasedController controller = null;

                switch (table)
                {
                    case 1:
                        action = SecondMenu("Hotel");
                        controller = new HotelController(connectionString);
                        break;
                    case 2:
                        action = SecondMenu("Booking");
                        controller = new BookingController(connectionString);
                        break;
                    case 3:
                        action = SecondMenu("CustomerWorker");
                        controller = new CustomerWorkerController(connectionString);
                        break;
                    case 4:
                        action = SecondMenu("Customer");
                        controller = new CostumerController(connectionString);
                        break;
                    case 5:
                        action = SecondMenu("CustomerBooking");
                        controller = new CustomerBookingController(connectionString);
                        break;
                    case 6:
                        action = SecondMenu("Worker");
                        controller = new WorkerController(connectionString);
                        break;
                }


                switch (action)
                {
                    case 1:
                        controller.Create();
                        break;
                    case 2:
                        controller.Read();
                        break;
                    case 3:
                        controller.Update();
                        break;
                    case 4:
                        controller.Delete();
                        break;
                    case 5:
                        controller.Find();
                        break;
                    case 6:
                        controller.Generate();
                        break;
                }



            } while (true);
        }

        public static int FirstMenu()
        {
            var choice = 0;
            var correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Choose table to operate with:");
                Console.WriteLine("Press the number to choose a table");
                Console.WriteLine("1.Hotel");
                Console.WriteLine("2.Booking");
                Console.WriteLine("3.CustomerWorker");
                Console.WriteLine("4.Customer");
                Console.WriteLine("5.CustomerBooking");
                Console.WriteLine("6.Worker");
                Console.WriteLine("0.Exit");
                correct = Int32.TryParse(Console.ReadLine(), out choice);
            } while (choice < 0 || choice > 6 || correct == false);


            return choice;
        }

        public static int SecondMenu(string tableToChange)
        {
            var choice = 0;
            var correct = false;
            do
            {
                Console.Clear();
                Console.WriteLine("What do you want to do with '" + tableToChange + "' table:");
                Console.WriteLine("Press the number to choose an action");
                Console.WriteLine("1.Create");
                Console.WriteLine("2.Read");
                Console.WriteLine("3.Update");
                Console.WriteLine("4.Delete");
                Console.WriteLine("5.Find");
                Console.WriteLine("6.Generate");
                correct = Int32.TryParse(Console.ReadLine(), out choice);
            } while (choice < 0 || choice > 6 || correct == false);


            return choice;
        }
    }
}
