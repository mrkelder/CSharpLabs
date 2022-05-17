using System;

namespace ConsoleApp1
{

    class MyMath
    {
        public virtual void square(int num)
        {
            Console.WriteLine("From MyMath");
            Console.WriteLine(Math.Pow(num, 2));
            Console.WriteLine();

        }

        public void add20ToInitialNumber(out int num)
        {
            num = 20;
        }

        public void add20ToInitialNumber(ref int num, params int[] nums)
        {
            bool fromMinToMax = num % 2 == 0;
            int[] newArray = nums;

            for (var a = 0; a < newArray.Length - 1; a++)
            {
                for (var b = 0; b < newArray.Length - a; b++)
                {
                    if (fromMinToMax)
                    {
                        if (newArray[a + b] < newArray[a])
                        {
                            int temp = newArray[a + b];
                            newArray[a + b] = newArray[a];
                            newArray[a] = temp;
                        }
                    }

                    else
                    {
                        if (newArray[a + b] > newArray[a])
                        {
                            int temp = newArray[a + b];
                            newArray[a + b] = newArray[a];
                            newArray[a] = temp;
                        }
                    }
                }
            }

            Console.Write("[");

            foreach (var i in newArray)
            {
                Console.Write(" {0} ", i);
            }
            Console.WriteLine("]");
        }
    }

    class MyMath2 : MyMath
    {
        public override void square(int num)
        {
            Console.WriteLine("From MyMath2");
            Console.WriteLine(Math.Pow(num, 3));
            base.square(num);
        }
    }

    class MyMath3 : MyMath2
    {
        public override void square(int num)
        {
            Console.WriteLine("From MyMath3");
            Console.WriteLine(Math.Pow(num, 4));
            base.square(num);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            MyMath ex = new MyMath();
            MyMath2 ex2 = new MyMath2();
            MyMath3 ex3 = new MyMath3();

            int num;


            ex.square(2);
            ex.add20ToInitialNumber(out num);
            Console.WriteLine(num);
            ex.add20ToInitialNumber(ref num, 1, 2, 3, 1, 5, 8, 7, 1);
            ex2.square(2);
            ex3.square(2);
        }
    }
}