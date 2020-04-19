
using System;
using System.Data;
using System.Text.RegularExpressions;
using EntityMappingDB;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "3e-6";
          var ss=  Convert.ChangeType(str, typeof(float));
            Convert.ToDecimal(ss);
            DataTable dt = new DataTable();
           dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("DTO", typeof(string));
            dt.Rows.Add(1, "3e-6");
            dt.Rows.Add(2, "5000");
             var lst = dt.ToEntityList<Person>();
            //foreach (DataRow row in dt.Rows)
            //{
            //    MyMethod(row);
            //}
        }
        public static Person MyMethod(DataRow P_0)
        {
         
                 Person person = new Person();
            if (CanSetted(P_0, "Id"))
            {
                person.Id=Convert.ToInt32(P_0["Id"]);
             }
            if (CanSetted(P_0, "DTO"))
            {

                object obj = Convert.ToString(P_0["DTO"]);
                if (ScientificNotation(obj))
                {
                    obj = Convert.ToDouble(obj);
                }
                person.DTO = (Convert.ToDecimal(obj));
            }
            return person;
        }
        public static bool CanSetted(DataRow dr, string name)
        {
            return !dr.IsNull(name);
        }
        public static bool ScientificNotation(object obj)
        {

            string str = (string)obj;
            Regex reg = new Regex("^[+-]?((\\d+\\.?\\d*)|(\\.\\d+))[Ee][+-]?\\d+$", RegexOptions.IgnoreCase);
            return reg.IsMatch(str);

        }
    }

    public class Person
    {
        public int Id { get; set; }

        public decimal? DTO { get; set; }
    }
         
}
