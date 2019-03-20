# EntityMappingDB
DB与实体转换(已经上传neuget,名称：EntityMappingDB)

采用emit方法DataTable,IDataReader与实体相互转换。

DataTable,IDataReader转换实体：
ToEntityList扩展方法

实体转换DataTable：
FromEntityToTable扩展方法

如果实体转换DataTable带有特性：
FromEntityToTableMap扩展方法

我设计了三类特性ColumnType（列类型映射）,DataField(列名称映射），NoColumn（没有对应的列，忽略该属性）  

没有使用高级语法糖，应该.net 4.0及以上都可。
已经是.net standard版本

说明：
不出意外，该项目没有升级的必要，除非emit技术有更新

