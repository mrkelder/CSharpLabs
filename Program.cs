using System;

namespace ConsoleApp1
{

    public interface Descriminant
    {
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public float findDescrimenant();
    }

    public interface SquareRoots
    {
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public void printSquareRoots();
    }

    class DescrimenantFinder : Descriminant
    {
        private float a;
        private float b;
        private float c;

        public virtual float A
        {
            set { a = value; }
            get { return a; }
        }

        public virtual float B
        {
            set { b = value; }
            get { return b; }
        }

        public virtual float C
        {
            set { c = value; }
            get { return c; }
        }

        public float findDescrimenant()
        {
            return MathF.Pow(b, 2) - 4 * a * c;
        }
    }

    class SquareRootsFinder : DescrimenantFinder, SquareRoots
    {
        private float a;
        private float b;
        private float c;

        public override float A
        {
            set { base.A = value; a = value; }
            get { return a; }
        }

        public override float B
        {
            set { base.B = value; b = value; }
            get { return b; }
        }
        public override float C
        {
            set { base.C = value; c = value; }
            get { return c; }
        }

        public void printSquareRoots()
        {
            float D = this.findDescrimenant();

            if (D > 0)
            {
                Console.WriteLine("x1 = {0}", (-b + Math.Sqrt(D)) / (2 * a));
                Console.WriteLine("x2 = {0}", (-b - Math.Sqrt(D)) / (2 * a));
            }
            else if (D == 0)
                Console.WriteLine("x = {0}", (-b + Math.Sqrt(D)) / (2 * a));
            else
                Console.WriteLine("Корней нет");

        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // 3 2 -1
            SquareRootsFinder srf = new SquareRootsFinder();
            
            srf.A = Convert.ToInt32(Console.ReadLine());
            srf.B = Convert.ToInt32(Console.ReadLine());
            srf.C = Convert.ToInt32(Console.ReadLine());

            srf.printSquareRoots();
        }
    }
}