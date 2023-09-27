# EntityMappingDB

1.采用emit方法DataTable,IDataReader与实体相互转换。

2.DataTable,IDataReader转换实体：
ToEntityList扩展方法

3.实体转换DataTable：
FromEntityToTable扩展方法

4.如果实体转换DataTable带有特性：
FromEntityToTableAttribute扩展方法

设计了三类特性ColumnType（列类型映射）,DataField(列名称映射），NoColumn（没有对应的列，忽略该属性）  
5.nuget  
EntityMappingDB

------------------------------------------------------------
使用  
1.datatable  
            DataTable dt = new DataTable();    
            dt.Columns.Add("Id", typeof(int));  
            dt.Columns.Add("DTO", typeof(string));  
            dt.Rows.Add(1, "3e-6");  
            dt.Rows.Add(2, "5000");  

          List<Person> lst=  dt.ToEntityList<Person>(); //转model  

          DataTable dd=  lst.FromEntityToTable();//转回DataTable  
2.IDataReader  
            IDataReader idr = null;    
            List<Person> lstm=  idr.ToEntityList<Person>();  //转model   
            DataTable dds=   lstm.FromEntityToTableAttribute<Person>();  //转回DataTable
定义：  
 public class Person  
    {  
        public int Id { get; set; }  

        [ColumnType(typeof(double))]  
        public decimal? DTO { get; set; }  

        [DataField("user")]  
        public string Name { get; set; }  
    }  

------------------------------------------------------------------------------
说明：  
1.如果程序报错：没有找到列，则说明你使用了不同的数据转model,但是这些数据不一样。这时你可以使用重载方法，定一个唯一key  

2.不出意外，该项目没有升级的必要，除非emit技术有更新  


