using System;

namespace ConsoleApp1
{

    class Car
    {
        public readonly string numberPlate = "DC2412R";
        private int gear = 1;
        private int fuelLeft = 15;

        public void changeGear (char gear)
        {
            if (gear == 'r' || gear == 'R') this.gear = -1;
        }

        public void changeGear(int gear)
        {
            if (gear >= 0 && gear <= 5) this.gear = Convert.ToChar(gear);
        }

        public void driveOneMile()
        {
            if (this.fuelLeft > 0) this.fuelLeft--;
            else Console.WriteLine("Бак пуст");
        }

        public void printStats()
        {
            this.printCurrentGear();
            this.printFuelStatus();
            this.printNumberPlate();
            Console.WriteLine();
        }  

        private void printNumberPlate()
        {
            Console.WriteLine("Номер авто: " + this.numberPlate);
        }

        private void printFuelStatus()
        {
            if (this.fuelLeft > 0) Console.WriteLine("Осталось " + this.fuelLeft +" единиц топлива");
            else Console.WriteLine("Бак пуст");
        }
        private void printCurrentGear()
        {
            if (this.gear == -1) Console.WriteLine("Задняя передача");
            else Console.WriteLine("Данная передача: " + this.gear);
        }

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
            bmw.printStats();

            bmw.FuelLeft = "2";
            bmw.changeGear(5);
            bmw.printStats();

            bmw.driveOneMile();
            bmw.changeGear('R');
            bmw.printStats();


            bmw.driveOneMile();
            bmw.changeGear(1);
            bmw.printStats();
        }
    }
}