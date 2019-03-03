# EntityMappingDBEmit
DB与实体转换(已经上传neuget,名称：EntityMappingDBEmitCore)

采用emit方法DataTable,IDataReader与实体相互转换。

DataTable,IDataReader转换实体：
ToEntityList扩展方法

实体转换DataTable：
FromEntityToTable扩展方法

如果实体转换DataTable带有特性：
FromEntityToTableMap扩展方法

我设计了三类特性ColumnType（列类型映射）,DataField(列名称映射），NoColumn（没有对应的列，忽略该属性）  

没有使用高级语法糖，应该.net 4.0及以上都可以，如果使用了低版本的.net,直接复制源码文件即可；
该项目是支持.net core库的，我没有弄.net framework的，如果需要直接建立一个.net framework库，把文件添加进去即可，我已经测试但是没有提供。

说明：
不出意外，该项目没有升级的必要，除非emit技术有更新；  
最后因为.net standard不支持emit实现，如果发布的版本支持以后将实现.net standard版本；该项目会直接替换库类型，名称不变。

