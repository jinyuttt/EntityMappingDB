# EntityMappingDB
DB与实体转换(已经上传neuget,名称：EntityMappingDB)

采用emit方法DataTable,IDataReader与实体相互转换。

DataTable,IDataReader转换实体：
ToEntityList扩展方法

实体转换DataTable：
FromEntityToTable扩展方法

如果实体转换DataTable带有特性：
FromEntityToTableAttribute扩展方法

我设计了三类特性ColumnType（列类型映射）,DataField(列名称映射），NoColumn（没有对应的列，忽略该属性）  

没有使用高级语法糖，应该.net 4.0及以上都可。
已经是.net standard版本

#升级
2020-1-12  
1.升级Emit微软官方库  
2.增加底层转换，支持type对象传入  

2020-3-11  
1.增加枚举类型转换,DB的int类型或者字符串类型可以直接转换成枚举类型但是字符串区分大小写，必须与枚举名称一致  

2020-3-20  
1.修改bug,去除只读属性的转换  
2.增加默认参数，忽视属性与列名称的大小写映射   

  
2020-3-21  
1.修正大小写忽略  
2.默认缓存，DataTable使用tableName作为缓存key,如果没有设置则使用列数+转换实体类型名称做key  
2.datareader默认使用列数+转换实体类型名称做key,如果不能区分需要传入缓存key
 

2010-4-19  
1.增加字符串转decimal类型是科学计数法格式


说明：
不出意外，该项目没有升级的必要，除非emit技术有更新  
程序中默认使用了缓存，datatable需要不同的名称来区分并且使用缓存

