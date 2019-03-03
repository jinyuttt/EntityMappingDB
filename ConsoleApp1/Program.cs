using EntityMappingDBEmit;
using EntityMappingDBEmit.DBToEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            TestToDB();
            TestToEntity();
            Console.ReadKey();
        }
        static void TestToDB()
        {
            List<Person> lst = new List<Person>();
            lst.Add(new Person() { Age = 1, Name = "ji", Score = 23, KK = "100" });
            lst.Add(new Person() { Name = "ty", Score = 26, KK = "101" });
            lst.Add(new Person() { Age = 7, Name = "er", Score = 29, KK = "120" });
            IList<Person> tmp = lst;
            var dt = lst.FromEntityToTableMap();
            var dd = tmp.FromEntityToTable();
            Console.WriteLine(dt.Rows.Count);
        }

        static void TestToEntity()
        {
            Person person = new Person();
            person.Age = 3;
            person.ID = 1;
            person.Note = "";
            int ss = Convert.ToInt32(person.Age);

            DataTable dt = new DataTable();
            Random random = new Random();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("PersonName", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            for (int i = 1; i < 1000000; i++)
            {
                var row = dt.NewRow();
                row[0] = i;
                row[1] = "jy" + random.Next();
                row[2] = random.Next(10, 50);
                dt.Rows.Add(row);
            }
            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            List<Person> lst = dt.ToEntityList<Person>();
            watch.Stop();
            Console.WriteLine(lst.Count + "," + watch.ElapsedMilliseconds);
            while (true)
            {
                watch.Restart();
                lst = dt.ToEntityList<Person>();
                //Stopwatch watchDD = new Stopwatch();
                //watchDD.Start();
                watch.Stop();
                Console.WriteLine(lst.Count + "," + watch.ElapsedMilliseconds);
            }
          
        }
    }
}
