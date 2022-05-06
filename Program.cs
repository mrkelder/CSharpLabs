using System;

namespace ConsoleApp1
{

    class Car
    {
        public readonly string vin = "82758";
        public readonly string numberPlate = "DC2412R";
        private int fuelLeft = 15;

        public string FuelLeft
        {
            get
            {
                return "Осталось " + this.fuelLeft + " литров топлива";
            }
            set
            {
                if (Convert.ToInt32(value) > 0 && Convert.ToInt32(value) < 39)
                {
                    this.fuelLeft = Convert.ToInt32(value);
                }
                else
                {
                    this.fuelLeft = 0;
                }
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Car bmw = new Car();
            Console.WriteLine(bmw.FuelLeft);
            bmw.FuelLeft = "40";
            Console.WriteLine(bmw.FuelLeft);

            // Console.ReadKey();
        }
    }
}