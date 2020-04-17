using EntityMappingDB.DBToEntity;
using System;
using System.Data;

namespace MyTest
{
    public class Study
    {
        public void  Test(DataRow row)
        {
            Person person = new Person();
            string str = Convert.ToString(row["ss"]);
            if (DynamicAssembleInfo.ScientificNotation(str))
            {
                person.Age=Convert.ToDecimal( Convert.ToDouble(str));
            }
            else
            {
                person.Age = Convert.ToDecimal(str);
            }
        }
    }

    public class Person
    {
        public decimal Age { get; set; }
    }
}