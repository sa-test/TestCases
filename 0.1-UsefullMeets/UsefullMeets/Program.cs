using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullMeets
{
    class Program
    {

        private static Dictionary<string, List<string>> OptimizeContacts(List<string> contacts)
        {
            var dictionary = new Dictionary<string, List<string>>();
            
            foreach(string row in contacts)
            {
                //Если запись = NULL или пустая - то пропускаем эту запись
                if (String.IsNullOrEmpty(row))
                {
                    //Console.WriteLine("Skip NULL or Empty");
                    continue;
                }
                // Разделяем запись контакта на Имя и емейл
                string[] rowParts = row.Split(':').ToArray();
                // Если длинна имени > 2 тогда shortName = 2 символа от имени, иначе: shortName = имя целиком
                string shortName = rowParts[0].Length > 2 ? rowParts[0].Substring(0, 2) : rowParts[0];

                //Проверяем: в словаре dictonary уже есть ключ shortName?
                if (dictionary.ContainsKey(shortName))
                {
                    //если ключ shortName есть, то добавлем к этому ключу новые Имя:емейл
                    dictionary[shortName].Add(rowParts[0]+":"+rowParts[1]);
                }
                else
                {
                    //если ключа нет, то создаем новую запись в словаре с ключом shortName и новой парой Имя:емейл
                    dictionary.Add(shortName, new List<string> { rowParts[0] + ":" + rowParts[1] } );
                }
            }


            return dictionary;
        }

        static void Main(string[] args)
        {
            List<string> Contacts = new List<string>();
            Contacts.Add("Sasha:sasha1995@sasha.ru");
            Contacts.Add("Sasha:alex99@mail.ru");
            Contacts.Add("Sasha:shuric2020@google.com");
            Contacts.Add("Vasya:vasyan@google.com");
            Contacts.Add("Petya:petr123@google.com");
            Contacts.Add("Vanya:vano2020@google.com");
            Contacts.Add("sasha:sanyo@google.com");
            Contacts.Add("Z:Zorro@google.com");
            Contacts.Add(":nobody@google.com");
            Contacts.Add(":nobody2@google.com");
            Contacts.Add("");

            Console.WriteLine("Original contact list:\n");
            foreach (string st in Contacts)
            {
                if (string.IsNullOrEmpty(st))
                {
                    Console.WriteLine("NULL");
                } 
                else
                {
                    Console.WriteLine(st);
                }
            }
            Console.WriteLine("");

            var OptimizedContacts = OptimizeContacts(Contacts);

            Console.WriteLine("Optimized contact list:");
            foreach (KeyValuePair<string, List<string>> row in OptimizedContacts)
            {
                Console.Write("\n" + row.Key);
                
                foreach(string st in row.Value)
                {
                    Console.Write(" " + st);
                }
            }
            Console.WriteLine("");


            Console.ReadKey();
        }
    }
}
