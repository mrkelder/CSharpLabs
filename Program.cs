using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

/*
Создать класс Element, содержащий следующие члены:
-свойство Name String типа (RW)
-свойство Index int типа(RW)
-метод PrintName() выводящий Name и Index в консоль
Создать пользовательскую коллекцию Elements (1), добавить в коллекцию более 10 элементов, проверить работу коллекции по следующему сценарию.
1. вывести коллекцию в консоль;
2.вставить новый элемент в коллекцию на любое место, кроме первого и последнего;
3.вывести коллекцию в консоль;
4.выполнить сериализацию коллекции(2);
5.удалить любой элемент коллекции;
6.скопировать коллекцию в массив;
7.вывести массив в консоль;
8.очистить коллекцию;
9.отобразить количество элементов коллекции и вывести коллекцию в консоль;
10.выполнить десериализацию коллекции из потока (файл);
11.вывести коллекцию в консоль.
(1) реализует интерфейсы IEnumerable, IEnumerator
(2) формат сериализации по варианту XML
*/

namespace System.Collections
{
    [Serializable]

    public class Element
    {
        public string Name { get; set; }
        public int Index { get; set; }

        public Element(){ }

        public Element (string name, int index)
        {
            Name = name;
            Index = index;
        }

        public void PrintName()
        {
            Console.WriteLine("Имя: {0,20:0}, индекс: {1}", Name, Index);
        }

        public new string ToString()
        {
            return Name + " " + Index;
        }
    }

    class Elements : IEnumerable, IEnumerator {
        int pos = -1;
        int currentPos = 0;
        Element[] elements = new Element[50];

        public Element this[int index]
        {
            get { return elements[index]; }
            set { elements[index] = value; }
        }

        public void PrintCollection()
        {
            bool flag = true;
            foreach (var i in elements)
            {
                if(i != null)
                {
                    flag = false;
                    i.PrintName();      
                }
            }
            if(flag)
            {
                Console.WriteLine("Коллекция пуста");
            }
            Console.WriteLine();
        }

        public void Push(Element element)
        {
            if (currentPos < elements.Length)
            {
                elements[currentPos] = element;
                currentPos++;
            }
            else Console.WriteLine("Array is full, please clear it");
        }

        public void Insert(int position, Element element)
        {
            if(position > 0 && position < elements.Length - 1)
            {
                Element[] copiedElements = new Element[50];

                for (int i = 0; i < elements.Length; i++)
                {
                    copiedElements[i] = elements[i];
                }

                for(int i = position; i < elements.Length - 1; i++)
                {
                    if(elements[i] != null)
                    {
                        elements[i + 1] = copiedElements[i];
                    }
                }
                elements[position] = element;
            }
        }

        public void PrintQuantityOfElements()
        {
            int counter = 0;
            foreach (Element i in elements)
            {
                if (i != null) counter++;
            }
            Console.WriteLine("Количество элементов в коллекции: {0}", counter);
        }

        public void RemoveAt(int index)
        {
            if(index > 0 && index < elements.Length)
            {
                elements[index] = null;
            }
        }

        public void ResetCollection()
        {
            elements = new Element[50];
        }

        bool IEnumerator.MoveNext()
        {
            if (pos < elements.Length - 1)
            {
                pos++;
                return true;
            }
        ((IEnumerator)this).Reset();
            return false;
        }

        void IEnumerator.Reset()
        {
            pos = -1;
        }
        object IEnumerator.Current
        {
            get { return elements[pos]; }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // 0. cоздание коллекции
            Elements elements = new Elements();
            elements.Push(new Element("Vlad", 1));
            elements.Push(new Element("Misha", 2));
            elements.Push(new Element("Teodor Rusvelt", 3));
            elements.Push(new Element("Grisha", 4));
            elements.Push(new Element("Jisha", 5));
            elements.Push(new Element("Ilya", 6));
            elements.Push(new Element("V", 7));
            elements.Push(new Element("Scout", 8));
            elements.Push(new Element("Artem", 9));
            elements.Push(new Element("George", 10));
            elements.Push( new Element("Funke", 11));
            elements.Push( new Element("Heavy weapons guy", 12));
            elements.Push( new Element("Ralsei", 13));
            elements.Push( new Element("Jeff", 14));
            elements.Push(new Element("MEDIC", 15));
            // 1. вывести коллекцию в консоль
            Console.WriteLine("================ Начальная коллекция ================");
            elements.PrintCollection();

            // 2. вставить новый элемент в коллекцию на любое место, кроме первого и последнего
            elements.Insert(5, new Element("LEEEEROOOOOOOY", 16));

            // 3. вывести коллекцию в консоль
            Console.WriteLine("================ Вставили один элемент ================");
            elements.PrintCollection();

            // 4. выполнить сериализацию коллекции
            List<Element> listElements = new List<Element>();
            foreach (Element i in elements)
            {
                if (i != null) listElements.Add(i);
            }
            XmlSerializer xml = new XmlSerializer(listElements.GetType());
            FileStream f = new FileStream("serialization.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            xml.Serialize(f, listElements);

            // 5. удалить любой элемент коллекции
            elements.RemoveAt(2);

            // 6. скопировать коллекцию в массив
            Element[] arrayElements = new Element[50];
            for(int i = 0; i < 50; i++)
            {
                arrayElements[i] = elements[i];
            }

            // 7. вывести массив в консоль
            Console.WriteLine("================ Массив ================");
            foreach(Element i in arrayElements)
            {
                if (i != null)
                    i.PrintName();
            }
            Console.WriteLine();

            // 8. очистить коллекцию
            elements.ResetCollection();
            Console.WriteLine("================ Колекция удалена ================");

            // 9. отобразить количество элементов коллекции и вывести коллекцию в консоль
            elements.PrintQuantityOfElements();
            elements.PrintCollection();

            // 10. выполнить десериализацию коллекции из потока(файл)
            f.Position = 0;
            f.Seek(0, SeekOrigin.Begin);
            listElements = (List<Element>)xml.Deserialize(f);
            foreach (Element i in listElements)
            {
                elements.Push(i);
            }
            Console.WriteLine("================ Колекция прочитана из файла ================");

            // 11. вывести коллекцию в консоль
            elements.PrintCollection();

        }
    }
}