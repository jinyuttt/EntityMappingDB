using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EntityMappingDBEmit.EntityToDB
{

    /// <summary>
    /// 样例
    /// </summary>
   public class ListToDataTable
    {
        public static void PersonToDataRow(DataRow row,Person person)
        {
           
            row["KK"] = Convert.ChangeType(person.KK, typeof(string));
            row["PersonName"] = person.Name;
            row["Age"] = person.Age;
            row["Score"] = person.Score;
            
        }
        public static void PersonToDataTable(DataTable dt, Person person)
        {

            DataRow row = dt.NewRow();
            row["Name"] = person.Name;
            row["Age"] = person.Age;
            row["Score"] = person.Score;
            dt.Rows.Add(row);

        }

       

        //public static void PersonToDataTableCheck(DataRow row, Person person)
        //{
        //    DataTable dt = row.Table;
        //    if (dt.Columns.Contains("Name"))
        //    {
        //        row["Name"] = person.Name;
        //    }
        //    if (dt.Columns.Contains("Age"))
        //    {
        //        row["Age"] = person.Age;
        //    }
        //    if (dt.Columns.Contains("Score"))
        //    {
        //        row["Score"] = person.Score;
        //    }

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="row"></param>
        ///// <param name="person"></param>
        ///// <param name="map">列对应的字段</param>
        //public static void PersonToDataTableCheck(DataRow row, Person person,Dictionary<string,string> map=null)
        //{

        //    DataTable dt = row.Table;
        //    if (map == null)
        //    {
        //        if (dt.Columns.Contains("Name"))
        //        {
        //            row["Name"] = person.Name;
        //        }
        //        if (dt.Columns.Contains("Age"))
        //        {
        //            row["Age"] = person.Age;
        //        }
        //        if (dt.Columns.Contains("Score"))
        //        {
        //            row["Score"] = person.Score;
        //        }
        //    }
        //    else
        //    {
        //        row["K1"] = person.Name;
        //        foreach(var kv in map)
        //        {
        //            object v = null;
        //            switch (kv.Value)
        //            {
        //                case "Name":
        //                    v = person.Name;
        //                    break;
        //                case "Age":
        //                    v = person.Age;
        //                    break;
        //                case "Score":
        //                    v = person.Score;
        //                    break;
        //                default:
        //                    break;
        //            }
        //            row[kv.Key] = v;
        //        }
        //    }

        //}



    }
}
