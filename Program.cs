﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

// Данная абстарктная система предсатвляет собой атоматизировнное такси-автопилот

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
// 11. Повторить все вышеперечисленное (i Сохраняем название точки назначения - 1) раз

namespace CourseWork
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

    // интерфейс системы управления электродвигателем
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

        // запись пунтка назначения в навигацию
        public void setDestination(string destination);
        // поиск ближайшей заправочной станции
        public void lookForPowerStation();
        // поиск ближайшего пути к пунтку назначения
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
        // метод запуска движения к пунтку назначения
        public void drive();
    }


    class Program
    {
        // глобальный объект класса Random
        public static Random rnd = new Random();

        // класс иммитации прогресса
        public static class LoadingImitator
        {
            // данные константы представляют собой минимальный
            // и максимальный пороги для иммитации загрузки
            private const int MIN_PROCESS_AWAIT_CYCLES = 3; // минимальное количество итераций
            private const int MAX_PROCESS_AWAIT_CYCLES = 8; // максимальное количество итераций
        
            public static void printDotsWithMessage(string message)
            {
                // метод, выводящий сообщение с точками, которые
                // иммитируют какой-либо процесс (например, подсчет)
                Console.Write(message);
                for (int i = rnd.Next(MIN_PROCESS_AWAIT_CYCLES, MAX_PROCESS_AWAIT_CYCLES); i > 0; i--)
                {
                    Thread.Sleep(500);
                    Console.Write(".");
                }
                Console.WriteLine();
            }
        }

        // класс пунтков назначения
        static public class Destinations {
            // коллекция пунктов назначения
            private static List<string> destinations = new List<string>(); 

            // метод инициализации всех пунктов назначения
            public static void initDestinations()
            {
                destinations.Clear();
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
            }

            // метод получения произольного пунтка назначения
            public static string getRandomDestination()
            {
                int destinationIndex = rnd.Next(0, destinations.Count - 1); // в произвольном порядке выбираем индекс пункта назначения
                string destination = destinations[destinationIndex];        // получаем сам пункт назначения
                destinations.RemoveAt(destinationIndex);                    // удаляем данный пункт назначения из списка (он уже посещен)
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
                Console.WriteLine("A \tПользователь запрашивает запуск двигателя");
                EventFromAToC();
            }

            public void requestRideTo(string destination) {
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
            public int price = 0;

            public void setPrice(int totalPrice)
            {
                LoadingImitator.printDotsWithMessage("D-B \tПроисходит вычисление цены");
                price = totalPrice;
            }

            public void requestPayment()
            {
                Console.WriteLine("B \tСтоимость поездки {0}$", price);
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
                    LoadingImitator.printDotsWithMessage("C \tДвигатель запускается");

                    isIgnitionRequested = false;
                    EventFromCToA();
                }
            }

            public void setRidePaid()
            {
                Console.WriteLine("B-C \tФлаг оплаченной поездки установлен");
                isRidePaid = true;
            }

            public void stopEngine()
            {
                if(isRidePaid)
                {
                    Console.WriteLine("C \tДвигатель заглушен");
                    isRidePaid = false;
                }
            }
        }

        public class D: ID
        {            
            public event CarDelegateWithIntAndString EventFromDToE;
            public event CarDelegateWithInt EventFromDToB;
            private string userDestination = "";
            private int priceToStation = 0;
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
                userDestination = destination;
            }

            public void lookForPowerStation()
            {
                string processMessage = "D \tНачат поиск станции заправки";
                lookForLocation(processMessage, "Заправочная станция");
            }

            public void navigateToClientDestination()
            {
                string processMessage = "D \tНачат поиск точки назначения \"" + userDestination + "\"";
                lookForLocation(processMessage, userDestination);
            }    
            
            private void lookForLocation(string processMessage, string destination)
            {
                LoadingImitator.printDotsWithMessage(processMessage);

                int estimatedTime = rnd.Next(MIN_ESTIMATED_TIME, MAX_ESTIMATED_TIME);
                priceToStation = estimatedTime * PRICE_PER_TIME;
                Console.WriteLine("\tПункт назначения \"{0}\" найден. Время до прибытия: {1}", destination, estimatedTime);
                EventFromDToE(estimatedTime, destination);
            }

            public void resetNavigation()
            {
                int totalPrice = priceToStation + priceToDestination;
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
            private int currentEstimatedTime = 0;
            private string currentDestination = "";

            public void setEstimatedTimeAndDestination(int estimatedTime, string destination)
            {
                Console.WriteLine("D-E \tНазвание пунтка назначения и продолжительность поездки сохранены");
                currentEstimatedTime = estimatedTime;
                currentDestination = destination;
            }

            public void drive()
            {
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
            Destinations.initDestinations();
            A passanger = new A();
            B payment = new B();
            C engine = new C();
            D navigation = new D();
            E rideController = new E();

            // количество клиентков
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
                string destination = Destinations.getRandomDestination();

                Console.WriteLine("\nКлиент №{0}", i);
                passanger.requestIgnition();                                // A запрашивает C запустить двигатель
                engine.igniteEngine();                                      // C запускает двигатель
                passanger.requestRideTo(destination);                       // A запрашивает поездку
                navigation.lookForPowerStation();                           // D прокладывает маршрут к ближайшей заправочной станции
                rideController.drive();                                     // E ведет машину к заправке
                navigation.navigateToClientDestination();                   // D прокладывает маршрут к пунтку назначения
                rideController.drive();                                     // E ведет машину к пункту назначения
                navigation.resetNavigation();                               // D сбрасывает навигацию
                payment.requestPayment();                                   // B запрашивает оплату
                engine.stopEngine();                                        // C заглушает двигатель                    
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы закончить");
            Console.ReadKey();  
        }
    }
}