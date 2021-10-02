# EntityMappingDB

1.采用emit方法DataTable,IDataReader与实体相互转换。

2.DataTable,IDataReader转换实体：
ToEntityList扩展方法

3.实体转换DataTable：
FromEntityToTable扩展方法

4.如果实体转换DataTable带有特性：
FromEntityToTableAttribute扩展方法

设计了三类特性ColumnType（列类型映射）,DataField(列名称映射），NoColumn（没有对应的列，忽略该属性）  

没有使用高级语法糖，应该.net 4.0及以上都可。
已经是.net standard版本


说明：  
1.如果程序报错：没有找到列，则说明你使用了不同的数据转model,但是这些数据不一样。这时你可以使用重载方法，定一个唯一key  

2.不出意外，该项目没有升级的必要，除非emit技术有更新  
程序中默认使用了缓存，datatable需要不同的名称来区分并且使用缓存  

