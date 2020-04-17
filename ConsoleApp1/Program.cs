using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityMappingDBCore;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Value", typeof(string));
            dt.Rows.Add(1, "3e-6");
            dt.Rows.Add(2, "3e+6");
          var lst=  dt.ToEntityList<Person>();
        }

    }

    public class Person
    {
        public int Id { get; set; }

        public decimal Value { get; set; }
    }
         
}
