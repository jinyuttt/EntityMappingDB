using EntityMappingDB.DBToEntity;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace MyTest
{
    public class Study
    {
        public void  Test(DataRow row)
        {

            Person person = new Person();
            string str = Convert.ToString(row["DTO"]);
            object obj = str;
            if (string.IsNullOrEmpty(str))
            {
                 obj = Convert.ToDouble(str);
            }
            person.DTO = Convert.ToDecimal(obj);
          
            
         
        }
    }

    public class Person
    {
        public float Age { get; set; }

        public decimal DTO { get; set; }
    }
}