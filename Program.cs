using System;
using System.Collections.Generic;
using System.Threading;

// Данная абстрактная система представляет собой ароматизированное такси-автопилот

// 1. Пользователь запрашивает активацию двигателя
// 2. Автомобиль активируется
// 3. Пользователь указывает пункт назначения
// 4. Система навигации задает маршрут до ближайшей заправочной станции
// 5. Система автопилота ведет машину на заправочную станцию, завершает поездку
// 6. Система навигации выбирает маршрут до пункта назначения пользователя
// 7. Система автопилота ведет машину до пункта назначения, завершает поездку
// 8. Система навигации сбрасывает информацию о поездке
// 9. Происходит запрос оплаты за поездку
// 10. Автомобиль заглушает двигатель
// 11. Происходит повторение алгоритма до тех, пока не будут обслужены все клиенты

namespace Coursework
{
    // делегаты с разными параметрами
    public delegate void CarDelegate();
    public delegate void CarDelegateWithString(string destination);
    public delegate void CarDelegateWithIntAndString(int estimatedTime, string destination);
    public delegate void CarDelegateWithInt(int price);

    // интерфейс запросов пассажира
    interface IA
    {
        // список делегатов для реализации событийной модели
        public event CarDelegate EventFromAToC;
        public event CarDelegateWithString EventFromAToD;
        public event CarDelegateWithInt EventFromDToB;
        public event CarDelegateWithIntAndString EventFromDToE;
        public event CarDelegateWithString EventFromEToD;    

        // метод для запроса активации двигателя
        public void requestIgnition();
        // метод для запроса поездки в пункт назначения
        public void requestRideTo(string destination);
        // метод, сообщающий пользователю
        // о том, что двигатель запущен
        public void engineIgnitionResponse();
        // переходной метод между классами D и B
        public void proxyFromDToB(int price);
        // переходной метод между классами D и E
        public void proxyFromDToE(int estimatedTime, string destination);
        // переходной метод между классами E и D
        public void proxyFromEToD(string destination);
    }

    // интерфейс системы оплаты
    interface IB
    {
        // список делегатов для реализации событийной модели
        public event CarDelegate EventFromBToC;
        public event CarDelegate EventFromAToC;
        public event CarDelegate EventFromCToA;

        // сохранение цены для оплаты
        public void setPrice(int totalPrice);
        // запрос оплаты
        public void requestPayment();
        // переходной метод между классами A и C
        public void proxyFromAToC();
        // переходной метод между классами C и A
        public void proxyFromCToA();
    }

    // интерфейс системы управления двигателем
    interface IC
    {
        // список делегатов для реализации событийной модели
        public event CarDelegate EventFromCToA;

        // установка флага того, что был отправлен
        // запрос об активации двигателя
        public void setIgnitionRequestPending();
        // запуск двигателя
        public void igniteEngine();
        // установка флага того, поездка оплачена
        public void setRidePaid();
        // остановка двигателя
        public void stopEngine();
    }

    // интерфейс системы навигации
    interface ID
    {
        // список делегатов для реализации событийной модели
        public event CarDelegateWithIntAndString EventFromDToE;
        public event CarDelegateWithInt EventFromDToB;

        // запись пункта назначения в навигацию
        public void setDestination(string destination);
        // поиск ближайшей заправочной станции
        public void lookForPowerStation();
        // поиск ближайшего пути к пункту назначения
        public void navigateToClientDestination();
        // сброс навигации
        public void resetNavigation();
        // выводит оповещение о том, что поездка завершена
        public void finishRide(string destination);
    }

    // интерфейс системы движения автомобиля
    interface IE
    {
        // список делегатов для реализации событийной модели
        public event CarDelegateWithString EventFromEToD;

        // запись времени до прибытия и названия пункта назначения в контроллер автопилота
        public void setEstimatedTimeAndDestination(int estimatedTime, string destination);
        // метод запуска движения к пункту назначения
        public void drive();
    }


    class Program
    {
        // глобальный объект класса Random
        public static Random rnd = new Random();

        // класс имитации прогресса
        public static class LoadingImitator
        {
            // данные константы представляют собой минимальный
            // и максимальный пороги для имитации загрузки
            private const int MIN_PROCESS_AWAIT_CYCLES = 3; // минимальное количество итераций
            private const int MAX_PROCESS_AWAIT_CYCLES = 8; // максимальное количество итераций
        
            public static void printDotsWithMessage(string message)
            {
                // метод, выводящий сообщение с точками, которые
                // имитируют какой-либо процесс (например, подсчет)
                Console.Write(message);
                for (int i = rnd.Next(MIN_PROCESS_AWAIT_CYCLES, MAX_PROCESS_AWAIT_CYCLES); i > 0; i--)
                {
                    // вывод точек прогресса
                    Thread.Sleep(500);
                    Console.Write(".");
                }
                Console.WriteLine();
            }
        }

        // класс пунктов назначения
        static public class Destinations {
            // коллекция пунктов назначения
            private static List<string> destinations = new List<string>();
            // массив всех возможных пунктов назначения
            private static string[] denstinationsArray = { "Торговый центр", "Рынок", "Обзерватория", "Филормония", "Университет", "Кафе", "Школа английского языка" };

            // метод инициализации всех пунктов назначения
            public static void init()
            {
                // очищаем коллекцию
                destinations.Clear();
                // добавляем пункты назначения в коллекцию
                foreach (string i in denstinationsArray)
                    destinations.Add(i);
            }

            // метод получения произвольного пункта назначения
            public static string getRandomDestination()
            {
                // в произвольном порядке выбираем индекс пункта назначения
                int destinationIndex = rnd.Next(0, destinations.Count - 1);
                // получаем сам пункт назначения
                string destination = destinations[destinationIndex];
                // удаляем данный пункт назначения из списка (он уже посещен)
                destinations.RemoveAt(destinationIndex);   
                
                return destination;
            }
        }

        public class A : IA
        {            
            public event CarDelegate EventFromAToC;
            public event CarDelegateWithString EventFromAToD;
            public event CarDelegateWithInt EventFromDToB;                       
            public event CarDelegateWithIntAndString EventFromDToE;                       
            public event CarDelegateWithString EventFromEToD;

            public void requestIgnition() {
                // 1. Пользователь запрашивает активацию двигателя
                Console.WriteLine("A \tПользователь запрашивает запуск двигателя");
                EventFromAToC();
            }

            public void requestRideTo(string destination) {
                // 3. Пользователь указывает пункт назначения
                Console.WriteLine("A \tПользователь запрашивает поездку до пунтка назначения \"{0}\"", destination);
                EventFromAToD(destination);
            }

            public void engineIgnitionResponse()
            {
                Console.WriteLine("C-A \tДвигатель запущен");
            }

            public void proxyFromDToB(int price)
            {       
                EventFromDToB(price);
            }

            public void proxyFromDToE(int estimatedTime, string destination)
            {
                EventFromDToE(estimatedTime, destination);
            }

            public void proxyFromEToD(string destination)
            {
                EventFromEToD(destination);
            }
        }

        public class B: IB
        {            
            public event CarDelegate EventFromBToC;
            public event CarDelegate EventFromAToC;
            public event CarDelegate EventFromCToA;
            // внутренняя цена в классе системы оплаты
            private int price = 0;

            public void setPrice(int totalPrice)
            {
                LoadingImitator.printDotsWithMessage("D-B \tПроисходит вычисление цены");
                price = totalPrice;
            }

            public void requestPayment()
            {
                // 9. Происходит запрос оплаты за поездку
                Console.WriteLine("B \tСтоимость поездки {0}$", price);
                // после оплаты цена сбрасывается
                price = 0;
                EventFromBToC();
            }

            public void proxyFromAToC()
            {
                EventFromAToC();
            }

            public void proxyFromCToA()
            {
                EventFromCToA();
            }
        }

        public class C: IC
        {            
            public event CarDelegate EventFromCToA;
            // флаг того, что запуск двигателя был запрошен со стороны пользователя
            private bool isIgnitionRequested = false;
            // флаг того, что поездка оплачена
            private bool isRidePaid = false;

            public void setIgnitionRequestPending()
            {
                Console.WriteLine("A-C \tЗапрос запуска двигателя сохранен");
                // запуск двигателя запрошен
                isIgnitionRequested = true;
            }

            public void igniteEngine()
            {
                // 2. Автомобиль активируется
                if (isIgnitionRequested)
                {
                    LoadingImitator.printDotsWithMessage("C \tДвигатель запускается");

                    // сброс флага запроса запуска двигателя
                    isIgnitionRequested = false;
                    EventFromCToA();
                }
            }

            public void setRidePaid()
            {
                Console.WriteLine("B-C \tФлаг оплаченной поездки установлен");
                // поездка оплачена
                isRidePaid = true;
            }

            public void stopEngine()
            {
                // 10. Автомобиль заглушает двигатель
                if (isRidePaid)
                {
                    Console.WriteLine("C \tДвигатель заглушен");
                    // сброс флага оплаченной поездки
                    isRidePaid = false;
                }
            }
        }

        public class D: ID
        {            
            public event CarDelegateWithIntAndString EventFromDToE;
            public event CarDelegateWithInt EventFromDToB;
            // пользовательский пункт назначения внутри навигации
            private string userDestination = "";
            // цена до заправочной станции
            private int priceToStation = 0;
            // цена до пользовательского пункта назначения
            private int priceToDestination = 0;

            // минимальное количество времени на поездку
            private const int MIN_ESTIMATED_TIME = 2; 
            // максимальное количество времени на поездку
            private const int MAX_ESTIMATED_TIME = 5;
            // цена за одну единицу времени
            private const int PRICE_PER_TIME = 15;

            public void setDestination(string destination)
            {
                Console.WriteLine("A-D \tПункт назначения пользователя записан в навигатор");
                // установка пользовательского пункта назначения
                userDestination = destination;
            }

            public void lookForPowerStation()
            {
                // 4. Система навигации задает маршрут до ближайшей заправочной станции
                string processMessage = "D \tНачат поиск станции заправки";
                lookForLocation(processMessage, "Заправочная станция", ref priceToStation);
            }

            public void navigateToClientDestination()
            {
                // 6. Система навигации выбирает маршрут до пункта назначения пользователя
                string processMessage = "D \tНачат поиск точки назначения \"" + userDestination + "\"";
                lookForLocation(processMessage, userDestination, ref priceToDestination);
            }    
            
            private void lookForLocation(string processMessage, string destination, ref int price)
            {
                LoadingImitator.printDotsWithMessage(processMessage);

                // получение времени на поездку
                int estimatedTime = rnd.Next(MIN_ESTIMATED_TIME, MAX_ESTIMATED_TIME);
                // вычисление цены до пункта назначения
                price = estimatedTime * PRICE_PER_TIME;
                Console.WriteLine("\tПункт назначения \"{0}\" найден. Время до прибытия: {1}", destination, estimatedTime);
                EventFromDToE(estimatedTime, destination);
            }

            public void resetNavigation()
            {
                // 8. Система навигации сбрасывает информацию о поездке
                int totalPrice = priceToStation + priceToDestination;
                // сброс значений по умолчанию
                userDestination = "";
                priceToStation = 0;
                priceToDestination = 0;
                Console.WriteLine("D \tНавигация сброшена");

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
            public event CarDelegateWithString EventFromEToD;
            // внутреннее значение времени на поездку
            private int currentEstimatedTime = 0;
            // внутреннее значение пункта назначения
            private string currentDestination = "";

            public void setEstimatedTimeAndDestination(int estimatedTime, string destination)
            {
                Console.WriteLine("D-E \tНазвание пунтка назначения и продолжительность поездки сохранены");
                // запись внутренних значений
                currentEstimatedTime = estimatedTime;
                currentDestination = destination;
            }

            public void drive()
            {
                // 5. Система автопилота ведет машину на заправочную станцию, завершает поездку
                // 7. Система автопилота ведет машину до пункта назначения, завершает поездку
                Console.WriteLine("E \tПоездка в пункт назначения \"{0}\" начата", currentDestination);
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
            Destinations.init();
            A passanger = new A();
            B payment = new B();
            C engine = new C();
            D navigation = new D();
            E rideController = new E();

            // количество клиентов (от 1 до 3)
            // максимальное количество поездок не должно превышать
            // количества пунктов назначения           
            int quantityOfClients = rnd.Next(1, 4);         
 
            // подписание методов классов на события классов

            // A
            passanger.EventFromAToC += payment.proxyFromAToC;
            passanger.EventFromAToD += navigation.setDestination;
            passanger.EventFromDToB += payment.setPrice;
            passanger.EventFromDToE += rideController.setEstimatedTimeAndDestination;
            passanger.EventFromEToD += navigation.finishRide;

            // B
            payment.EventFromBToC += engine.setRidePaid;
            payment.EventFromAToC += engine.setIgnitionRequestPending;
            payment.EventFromCToA += passanger.engineIgnitionResponse;

            // C
            engine.EventFromCToA += payment.proxyFromCToA;

            // D
            navigation.EventFromDToE += passanger.proxyFromDToE;
            navigation.EventFromDToB += passanger.proxyFromDToB;

            // E
            rideController.EventFromEToD += passanger.proxyFromEToD;

            Console.WriteLine("Количество клиентов: {0}", quantityOfClients);

            for (int i = 1; i <= quantityOfClients; i++)
            {
                // пункт назначения на данной итерации
                string destination = Destinations.getRandomDestination();

                Console.WriteLine("\nКлиент №{0}", i);
                passanger.requestIgnition();                                // A
                engine.igniteEngine();                                      // C
                passanger.requestRideTo(destination);                       // A
                navigation.lookForPowerStation();                           // D
                rideController.drive();                                     // E
                navigation.navigateToClientDestination();                   // D
                rideController.drive();                                     // E
                navigation.resetNavigation();                               // D
                payment.requestPayment();                                   // B
                engine.stopEngine();                                        // C
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы закончить");
            Console.ReadKey();  
        }
    }
}