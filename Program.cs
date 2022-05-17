using System;
using System.Diagnostics;

namespace ConsoleApp1
{

    class Car
    {
        private string numberPlate;
        private int gear;
        private int fuelLeft;

        ~Car()
        {
            Debug.WriteLine("Автомобиль с номером {0} уничтожен", this.numberPlate);
        }

        public Car()
        {
            this.numberPlate = "DF12342";
            this.gear = 1;
            this.fuelLeft = 15;
            Console.WriteLine("Car()");
            Console.WriteLine();
        }

        public Car(int fuelLeft) : this()
        {
            this.fuelLeft = fuelLeft;
            Console.WriteLine("Car(int fuelLeft) : this()");
            Console.WriteLine();
        }

        public Car(string numberPlate, int gear)
        {
            this.numberPlate = numberPlate;
            this.gear = gear;
            this.fuelLeft = 15;
            Console.WriteLine("Car(string numberPlate, int gear)");
            Console.WriteLine();
        }

        public Car(int gear, int fuelLeft)
        {
            this.numberPlate = "DF12342";
            this.gear = gear;
            this.fuelLeft = fuelLeft;
            Console.WriteLine("Car(int gear, int fuelLeft)");
            Console.WriteLine();
        }

        public void changeGear(char gear)
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
            if (this.fuelLeft > 0) Console.WriteLine("Осталось " + this.fuelLeft + " единиц топлива");
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
            Car mercedes = new Car(10);
            Car audi = new Car(2, 20);
            Car skoda = new Car("ASAProcky", 3);

            Car[] cars = { bmw, mercedes, audi, skoda };

            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].printStats();
            }

        }
    }
}