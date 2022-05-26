using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CourseWork
{
    public delegate void CarDelegate();
    public delegate void CarDelegateWithString(string destination);
    public delegate void CarDelegateWithIntAndString(int estimatedTime, string destination);
    public delegate void CarDelegateWithInt(int price);

    //public delegate void MyDelegate(DateTime date, byte WorkTime);

    interface IA
    {
        public event CarDelegate EventFromAToC;
        public event CarDelegateWithString EventFromAToD;
        public event CarDelegateWithInt EventFromDToB;
        public event CarDelegate EventFromBToC;

        public void requestIgnition();
        public void requestRideTo(string destination);
        public void engineIgnitionResponse();
        public void proxyFromDToB(int price);
        public void proxyFromBToC();
    }

    interface IB
    {
        public event CarDelegate EventFromBToC;

        public void setPrice(int totalPrice);
        public void requestPayment();
    }

    interface IC
    {
        public event CarDelegate EventFromCToA;

        public void setIgnitionRequestPending();
        public void igniteEngine();
        public void setRidePaid();
        public void stopEngine();
    }

    interface ID
    {
        public event CarDelegateWithIntAndString EventFromDToE;
        public event CarDelegateWithInt EventFromDToB;
  
        public void setDestination(string destination);
        public void lookForPowerStation();
        public void navigateToClientDestination();
        public void resetNavigation();
        public void finishRide(string destination);
    }

    interface IE
    {
        // система движения автомобиля
        public event CarDelegateWithString EventFromEToD;

        public void setEstimatedTime(int estimatedTime, string destination);
        public void drive();
    }


    class Program
    {
        public class A : IA
        {
            // запросы пассажира
            public event CarDelegate EventFromAToC;
            public event CarDelegateWithString EventFromAToD;
            public event CarDelegateWithInt EventFromDToB;
            public event CarDelegate EventFromBToC;

            public void requestIgnition() {
                Console.WriteLine("A \tПользователь запрашивает запуск двигателя");
                EventFromAToC();
            }

            public void requestRideTo(string destination) {
                Console.WriteLine("A \tПользователь запрашивает поездку до пунтка назначения \"{0}\"", destination);
                EventFromAToD(destination);
            }

            public void engineIgnitionResponse()
            {
                Console.WriteLine("\nC-A \tДвигатель запущен");
            }

            public void proxyFromDToB(int price)
            {
                EventFromDToB(price);
            }

            public void proxyFromBToC()
            {
                EventFromBToC();
            }
        }

        public class B: IB
        {
            // система оплаты
            public event CarDelegate EventFromBToC;
            public int price = 0;

            public void setPrice(int totalPrice)
            {
                Console.WriteLine("D-B \tУстанавливаем цену...");
                Thread.Sleep(1000);
                price = totalPrice;
            }

            public void requestPayment()
            {
                Console.WriteLine("B \tСтоимость поездки {0}$", price);
                price = 0;
                EventFromBToC();
            }
        }

        public class C: IC
        {
            // система управления электродвигателем
            public event CarDelegate EventFromCToA;
            private bool isIgnitionRequested = false;
            private bool isRidePaid = false;

            public void setIgnitionRequestPending()
            {
                Console.WriteLine("A-C \tЗапрос запуска двигателя сохранен");
                isIgnitionRequested = true;
            }

            public void igniteEngine()
            {
                if(isIgnitionRequested)
                {
                    Random rnd = new Random();
                    Console.Write("C \tДвигатель запускается");

                    for(int i = rnd.Next(3, 8); i > 0; i--)
                    {
                        Thread.Sleep(500);
                        Console.Write(".");
                    }

                    isIgnitionRequested = false;
                    EventFromCToA();
                }
            }

            public void setRidePaid()
            {
                Console.WriteLine("B-C \tУстонавливаем флаг того, что поездка оплачена");
                isRidePaid = true;
            }

            public void stopEngine()
            {
                if(isRidePaid)
                {
                    Console.WriteLine("C \tЗаглушаем двигатель");
                    isRidePaid = false;
                }
            }
        }

        public class D: ID
        {
            // система навигации для автопилота
            public event CarDelegateWithIntAndString EventFromDToE;
            public event CarDelegateWithInt EventFromDToB;
            private string userDestination = "";
            private int priceToStation = 0;
            private int priceToDestination = 0;

            private const int PRICE_PER_TIME = 15;

            public void setDestination(string destination)
            {
                Console.WriteLine("A-D \tПункт назначения пользователя записан в навигатор");
                userDestination = destination;
            }

            public void lookForPowerStation()
            {
                Random rnd = new Random();
                Console.Write("D \tНачат поиск станции заправки");

                for (int i = rnd.Next(3, 8); i > 0; i--)
                {
                    Thread.Sleep(500);
                    Console.Write(".");
                }

                int estimatedTime = rnd.Next(2, 5);
                priceToStation = estimatedTime * PRICE_PER_TIME;
                Console.WriteLine("\n\tЗаправка найдена. Время до прибытия: {0} условных единиц", estimatedTime);
                EventFromDToE(estimatedTime, "Заправка");
            }

            public void navigateToClientDestination()
            {
                Random rnd = new Random();
                Console.Write("D \tНачат поиск точки назначения \"{0}\"", userDestination);

                for (int i = rnd.Next(3, 8); i > 0; i--)
                {
                    Thread.Sleep(500);
                    Console.Write(".");
                }

                Console.WriteLine();

                int estimatedTime = rnd.Next(2, 5);
                priceToDestination = estimatedTime * PRICE_PER_TIME;
                Console.WriteLine("\tНачинаем движение в точку назначения \"{0}\". Время: {1} условных единиц", userDestination, estimatedTime);
                EventFromDToE(estimatedTime, userDestination);
            }          

            public void resetNavigation()
            {
                Console.WriteLine("D \tСбрасываем навигацию");
                int totalPrice = priceToStation + priceToDestination;
                userDestination = "";
                priceToStation = 0;
                priceToDestination = 0;

                EventFromDToB(totalPrice);
            }

            public void finishRide(string destination)
            {
                Thread.Sleep(500);
                Console.WriteLine("\nE-D \tПоездка до \"{0}\" завершена", destination);
            }
        }

        public class E: IE
        {
            // система движения автомобиля
            public event CarDelegateWithString EventFromEToD;
            private int currentEstimatedTime = 0;
            private string currentDestination = "";

            public void setEstimatedTime(int estimatedTime, string destination)
            {
                Console.WriteLine("D-E \tСохраняем название точки назначения и кол-во времени на дорогу");
                currentEstimatedTime = estimatedTime;
                currentDestination = destination;
            }

            public void drive()
            {
                Random rnd = new Random();
                Console.WriteLine("E \tНачинаем поездку в точку назначения \"{0}\"", currentDestination);
                Console.Write("\tДо конца поездки осталось:");
                for (int i = 0; i < currentEstimatedTime; i++)
                {
                    Thread.Sleep(1000);
                    Console.Write(" {0}", currentEstimatedTime - i);
                }

                EventFromEToD(currentDestination);
            }
        }

        static void Main(string[] args)
        {
            // данная абстарктная система предсатвляет собой атоматизировнное такси-автопилот

            // 1. Пользователь запрашивает активацию двигателя
            // 2. Автомобиль активируется
            // 3. Пользователь указывает пункт назначения
            // 4. Система навигации задает маршрут до ближайшей заправки для дозаправки
            // 5. Система автопилота ведет машину на заправку, завершает поездку
            // 6. Система навигации выбирает маршрут до пунтка назначения пользователя
            // 7. Система автопилота ведет машину на заправку, завершает поездку
            // 8. Система навигации сообщает о конце поездки
            // 9. Происходит запрос оплаты за поездку
            // 10. Автомобиль переходит в режим гибернации
            // 11. Повторить все вышеперечисленное (i - 1) раз
            
            Random rnd = new Random();
            A passanger = new A();
            B payment = new B();
            C engine = new C();
            D navigation = new D();
            E rideController = new E();

            int quantityOfClients = rnd.Next(1, 4);         // количество клиентков
            List<string> destinations = new List<string>(); // коллекция пунктов назначения

            destinations.Add("Торговый центр");
            destinations.Add("Рынок");
            destinations.Add("Обзерватория");
            destinations.Add("Филормония");
            destinations.Add("Университет");
            destinations.Add("Кафе");
            destinations.Add("Галлерея");
            destinations.Add("Пляж");
            destinations.Add("Школа английского языка");
            destinations.Add("Концертный зал");


            passanger.EventFromAToC += engine.setIgnitionRequestPending;
            passanger.EventFromAToD += navigation.setDestination;
            passanger.EventFromDToB += payment.setPrice;
            passanger.EventFromBToC += engine.setRidePaid;

            payment.EventFromBToC += passanger.proxyFromBToC;

            engine.EventFromCToA += passanger.engineIgnitionResponse;

            navigation.EventFromDToE += rideController.setEstimatedTime;
            navigation.EventFromDToB += passanger.proxyFromDToB;

            rideController.EventFromEToD += navigation.finishRide;

            Console.WriteLine("Количество клиентов: {0}", quantityOfClients);

            for (int i = 1; i <= quantityOfClients; i++)
            {
                Console.WriteLine("\nКлиент №{0}", i);
                passanger.requestIgnition();                                                // A запрашивает C запустить двигатель
                engine.igniteEngine();                                                      // C запускает двигатель
                passanger.requestRideTo(destinations[rnd.Next(0, destinations.Count - 1)]); // A запрашивает поездку
                navigation.lookForPowerStation();                                           // D прокладывает маршрут к ближайшей заправочной станции
                rideController.drive();                                                     // E ведет машину к заправке
                navigation.navigateToClientDestination();                                   // D прокладывает маршрут к пунтку назначения
                rideController.drive();                                                     // E ведет машину к пункту назначения
                navigation.resetNavigation();                                               // D сбрасывает навигацию
                payment.requestPayment();                                                   // B запрашивает оплату
                engine.stopEngine();                                                        // C заглушает двигатель                    
            }

            Console.ReadKey();  
        }
    }
}