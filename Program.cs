using System;

namespace ConsoleApp1
{

    class MyMath
    {
        public virtual void square(int num)
        {
            Console.WriteLine("From MyMath");
            Console.WriteLine(num * num);
            Console.WriteLine();

        }

        public void add20ToInitialNumber(ref int num)
        {
            num += 20;
        }

        public void add20ToInitialNumber(ref int num, params int[] nums)
        {
            bool fromMinToMax = num % 2 == 0;
            int[] newArray = nums;

            for (var a = 0; a < newArray.Length - 1; a++)
            {
                for(var b = 0; b < newArray.Length - a; b++)
                {
                    if(fromMinToMax)
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

            foreach(var i in newArray)
            {
                Console.Write(" {0} ", i);
            }
            Console.WriteLine("]");
        }
    }

    class MyMath2: MyMath
    {
        public override void square(int num)
        {
            Console.WriteLine("From MyMath2");
            Console.WriteLine(num * num * num);
            base.square(num);
        }
    }

    class MyMath3 : MyMath2
    {
        public override void square(int num)
        {
            Console.WriteLine("From MyMath3");
            Console.WriteLine(num * num * num * num);
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

            int num = 2;


            ex.square(2);
            ex.add20ToInitialNumber(ref num);
            Console.WriteLine(num);
            ex.add20ToInitialNumber(ref num, 1, 2 ,3 ,1, 5 ,8 ,7, 1);
            ex2.square(3);
            ex3.square(5);
        }
    }
}