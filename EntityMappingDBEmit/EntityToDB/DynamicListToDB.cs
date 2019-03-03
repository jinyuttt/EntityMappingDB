using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using System.Reflection;
namespace EntityMappingDBEmit
{
   

    /// <summary>
    /// List转DataTable扩展
    /// </summary>
    public static partial class DynamicEntityMappingDBEmit
    {
        public delegate void EntityDataTable<T>(DataTable dr, T obj);
        public delegate void EntityDataRow<T>(DataRow row, T obj);
        /// <summary>
        /// 缓存动态方法委托
        /// 实体转换DataTable是唯一的，因此这里单独缓存
        /// 不还有设计的缓存ConvertCache
        /// </summary>
        private static Dictionary<string, object> cache = new Dictionary<string, object>();

        /// <summary>
        /// 缓存对应的DataTable结构
        /// </summary>
        private static Dictionary<string, DataTable> cacheDataTable = new Dictionary<string, DataTable>();

        /// <summary>
        /// 直接转换整个DataTable
        /// </summary>
        /// <typeparam name="T">model</typeparam>
        /// <param name="map">列名称映射:Key列名称，Value属性名称</param>
        /// <param name="mapType">列类型映射：key列类型，value属性类型</param>
        /// <returns></returns>
        public static DynamicMethod EntityToDataTableEmit<T>(Dictionary<string,string> map=null,Dictionary<string,Type>mapType=null)
        {
            DynamicMethod method = new DynamicMethod(typeof(T).Name + "ToDataTable", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null,
                new Type[] {typeof(DataTable), typeof(T) }, typeof(EntityContext).Module, true);
            ILGenerator generator = method.GetILGenerator();
            //创建行 实现DataRow row=dt.NewRow();
            LocalBuilder reslut = generator.DeclareLocal(typeof(DataRow));
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Call, typeof(DataTable).GetMethod("NewRow"));
            generator.Emit(OpCodes.Stloc, reslut);//结果存储
            var properties = typeof(T).GetProperties();
            //
            Dictionary<string, Type> dic = new Dictionary<string, Type>();
            Dictionary<string, LocalBuilder> dicLocalBuilder = new Dictionary<string, LocalBuilder>();
            List<PropertyInfo> lstNull = new List<PropertyInfo>();

            //获取空类型属性
            foreach(var item in properties)
            {
               if (Nullable.GetUnderlyingType(item.PropertyType) != null)
                {
                    lstNull.Add(item);
                }
            }
            int cout = lstNull.Count;
            List<LocalBuilder> lstLocal = new List<LocalBuilder>(lstNull.Count);
            foreach (var item in lstNull)
            {
                //获取所有空类型属性的类型
                dic[item.Name] = item.PropertyType;
            }

            for (int i = 0; i < cout; i++)
            {
                //定义足够的bool
                lstLocal.Add(generator.DeclareLocal(typeof(bool)));
            }
            foreach (var kv in dic)
            {
                //定义包含的空类型
                if (!dicLocalBuilder.ContainsKey(kv.Value.FullName))
                {
                    dicLocalBuilder[kv.Value.FullName] = generator.DeclareLocal(kv.Value);
                }
            }
            //没有列名称映射
            int index = -1;//必须-1合适
            if (map == null)
            {
                //遍历属性
                foreach (var p in properties)
                {
                    if (dic.ContainsKey(p.Name))
                    {
                        var endIfLabel = generator.DefineLabel();
                        //判断
                        //
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//

                        generator.Emit(OpCodes.Stloc_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Ldloca_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Call, dic[p.Name].GetMethod("get_HasValue"));

                        generator.Emit(OpCodes.Stloc, lstLocal[++index]);
                        generator.Emit(OpCodes.Ldloc, lstLocal[index]);
                        generator.Emit(OpCodes.Brfalse_S, endIfLabel);
                        //赋值
                        generator.Emit(OpCodes.Ldloc, reslut);//取出变量
                        generator.Emit(OpCodes.Ldstr, p.Name);//row["Name"]
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//
                                                                       //
                        generator.Emit(OpCodes.Stloc_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Ldloca_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Call, dic[p.Name].GetMethod("get_Value"));
                        //
                        generator.Emit(OpCodes.Box, Nullable.GetUnderlyingType(p.PropertyType));//一直在折腾这个地方，哎
                                                                                                //
                        generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
                        generator.MarkLabel(endIfLabel);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldloc, reslut);
                        generator.Emit(OpCodes.Ldstr, p.Name);
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//获取属性
                        if (mapType == null || !mapType.ContainsKey(p.Name))
                        {
                            if (p.PropertyType.IsValueType)
                                generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
                            else
                                generator.Emit(OpCodes.Castclass, p.PropertyType);
                        }
                        else
                        {
                            generator.Emit(OpCodes.Ldtoken, mapType[p.Name]);
                            generator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                            generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) }));
                        }
                        generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
                    }
                }
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Call, typeof(DataTable).GetMethod("get_Rows"));
                    generator.Emit(OpCodes.Ldloc, reslut);
                    generator.Emit(OpCodes.Call, typeof(DataRowCollection).GetMethod("Add", new Type[] { typeof(DataRow) }));
                    generator.Emit(OpCodes.Ret);
                
            }
            else
            {
              
                List<PropertyInfo> lst = new List<PropertyInfo>(properties);
                foreach (var kv in map)
                {
                    var p = lst.Find(x => x.Name == kv.Value);//找到属性
                    if (dic.ContainsKey(p.Name))
                    {
                        var endIfLabel = generator.DefineLabel();
                        //判断
                        //
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//

                        generator.Emit(OpCodes.Stloc_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Ldloca_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Call, dic[p.Name].GetMethod("get_HasValue"));

                        generator.Emit(OpCodes.Stloc, lstLocal[++index]);
                        generator.Emit(OpCodes.Ldloc, lstLocal[index]);
                        generator.Emit(OpCodes.Brfalse_S, endIfLabel);
                        //赋值
                        generator.Emit(OpCodes.Ldloc, reslut);
                        generator.Emit(OpCodes.Ldstr, kv.Value);//属性名称
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//
                                                                       //
                        generator.Emit(OpCodes.Stloc_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Ldloca_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Call, dic[p.Name].GetMethod("get_Value"));
                        //
                        generator.Emit(OpCodes.Box, Nullable.GetUnderlyingType(p.PropertyType));//一直在折腾这个地方，哎
                                                                                                //
                        generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
                        generator.MarkLabel(endIfLabel);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldloc, reslut);
                        generator.Emit(OpCodes.Ldstr, kv.Value);//属性名称
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//获取属性值
                        if (mapType == null || !mapType.ContainsKey(kv.Key))
                        {
                            if (p.PropertyType.IsValueType)
                                generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
                            else
                                generator.Emit(OpCodes.Castclass, p.PropertyType);
                        }
                        else
                        {
                            generator.Emit(OpCodes.Ldtoken, mapType[kv.Key]);
                            generator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                            generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) }));
                        }

                        generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
                    }

                }
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Call, typeof(DataTable).GetMethod("get_Rows"));
                generator.Emit(OpCodes.Ldloc, reslut);
                generator.Emit(OpCodes.Call, typeof(DataRowCollection).GetMethod("Add", new Type[] { typeof(DataRow) }));
                generator.Emit(OpCodes.Ret);
            }
            return method;

        }

        /// <summary>
        /// 转换DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DynamicMethod EntityToDataRowEmit<T>(Dictionary<string, string> map = null, Dictionary<string, Type> mapType = null)
        {
            DynamicMethod method = new DynamicMethod(typeof(T).Name + "ToDataRow", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null,
                new Type[] { typeof(DataRow), typeof(T)}, typeof(EntityContext).Module, true);
            ILGenerator generator = method.GetILGenerator();
            //
            Dictionary<string, Type> dic = new Dictionary<string, Type>();
            Dictionary<string, LocalBuilder> dicLocalBuilder = new Dictionary<string, LocalBuilder>();
            List<PropertyInfo> lstNull = new List<PropertyInfo>();
            var properties = typeof(T).GetProperties();
            //获取空类型属性
            foreach (var item in properties)
            {
                if (Nullable.GetUnderlyingType(item.PropertyType) != null)
                {
                    lstNull.Add(item);
                }
            }
            int cout = lstNull.Count;
            List<LocalBuilder> lstLocal = new List<LocalBuilder>(lstNull.Count);
            foreach (var item in lstNull)
            {
                //获取所有空类型属性的类型
                dic[item.Name] = item.PropertyType;
            }

            for (int i = 0; i < cout; i++)
            {
                //定义足够的bool
                lstLocal.Add(generator.DeclareLocal(typeof(bool)));
            }
            foreach (var kv in dic)
            {
                //定义包含的空类型
                if (!dicLocalBuilder.ContainsKey(kv.Value.FullName))
                {
                    dicLocalBuilder[kv.Value.FullName] = generator.DeclareLocal(kv.Value);
                }
            }
            //没有列名称映射
            int index = -1;//必须-1合适

            if (map == null)
            {
                foreach (var p in properties)
                {
                    if (dic.ContainsKey(p.Name))
                    {
                        var endIfLabel = generator.DefineLabel();
                        //判断
                        //
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//

                        generator.Emit(OpCodes.Stloc_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Ldloca_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Call, dic[p.Name].GetMethod("get_HasValue"));

                        generator.Emit(OpCodes.Stloc, lstLocal[++index]);
                        generator.Emit(OpCodes.Ldloc, lstLocal[index]);
                        generator.Emit(OpCodes.Brfalse_S, endIfLabel);
                        //赋值
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldstr, p.Name);
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//
                                                                       //
                        generator.Emit(OpCodes.Stloc_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Ldloca_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Call, dic[p.Name].GetMethod("get_Value"));
                        //
                        generator.Emit(OpCodes.Box, Nullable.GetUnderlyingType(p.PropertyType));//一直在折腾这个地方，哎
                                                                                                //
                        generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
                        generator.MarkLabel(endIfLabel);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldstr, p.Name);
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//直接给属性赋值
                        if (mapType == null || !mapType.ContainsKey(p.Name))
                        {
                            if (p.PropertyType.IsValueType)
                                generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
                            else
                                generator.Emit(OpCodes.Castclass, p.PropertyType);
                        }
                        else
                        {
                            generator.Emit(OpCodes.Ldtoken, mapType[p.Name]);
                            generator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                            generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) }));
                        }
                        generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
                    }
                }
                generator.Emit(OpCodes.Ret);
            }
            else
            {
                List<PropertyInfo> lst = new List<PropertyInfo>(properties);
                foreach (var kv in map)
                {
                    var p = lst.Find(x => x.Name == kv.Value);
                    if (dic.ContainsKey(p.Name))
                    {
                        var endIfLabel = generator.DefineLabel();
                        //判断
                        //
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//

                        generator.Emit(OpCodes.Stloc_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Ldloca_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Call, dic[p.Name].GetMethod("get_HasValue"));

                        generator.Emit(OpCodes.Stloc, lstLocal[++index]);
                        generator.Emit(OpCodes.Ldloc, lstLocal[index]);
                        generator.Emit(OpCodes.Brfalse_S, endIfLabel);
                        //赋值
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldstr, kv.Key);//row["Name"]
                        //
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//
                        generator.Emit(OpCodes.Stloc_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Ldloca_S, dicLocalBuilder[p.PropertyType.FullName]);
                        generator.Emit(OpCodes.Call, dic[p.Name].GetMethod("get_Value"));
                        //
                        generator.Emit(OpCodes.Box, Nullable.GetUnderlyingType(p.PropertyType));//一直在折腾这个地方，哎
                                                                                                //
                        generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
                        generator.MarkLabel(endIfLabel);
                    }

                    else
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldstr, kv.Key);//row["Name"]
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Call, p.GetGetMethod());//获取属性值
                        if (mapType == null || !mapType.ContainsKey(kv.Key))
                        {
                            if (p.PropertyType.IsValueType)
                                generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
                            else
                                generator.Emit(OpCodes.Castclass, p.PropertyType);
                        }
                        else
                        {
                            generator.Emit(OpCodes.Ldtoken, mapType[kv.Key]);
                            generator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                            generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) }));
                        }

                        generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
                    }
                }
                generator.Emit(OpCodes.Ret);
            }

            return method;

        }


        /// <summary>
        /// 直接属性转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static DataTable FromEntityToTable<T>(this IList<T> lst)
        {
            DataTable dt = new DataTable();
            if (!cacheDataTable.ContainsKey(typeof(T).FullName))
            {
               
                //
                var properties = typeof(T).GetProperties();
               
                foreach (var p in properties)
                {
                   var cur= Nullable.GetUnderlyingType(p.PropertyType);
                    dt.Columns.Add(p.Name,cur==null? p.PropertyType:cur);
                }
            }
            else
            {
                dt = cacheDataTable[typeof(T).FullName].Clone();
            }
            //1.如果调用table转换
            //LoadDataTable<T> load = (LoadDataTable<T>)PersonToDataTable<T>().CreateDelegate(typeof(LoadDataTable<T>));
            EntityDataTable<T> load = Find<T>();
            foreach (var item in lst)
            {
                load(dt, item);
            }
            ////2.如果调用行转换(控制度大些)
            //LoadDataRow<T> loadrow = (LoadDataRow<T>)PersonToDataRow<T>().CreateDelegate(typeof(LoadDataRow<T>));
            //foreach (var item in lst)
            //{
            //    var row = dt.NewRow();
            //    loadrow(row, item);
            //    dt.Rows.Add(row);
            //}
            return dt;
        }

        /// <summary>
        /// 带有特性的转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static DataTable FromEntityToTableMap<T>(this IList<T> lst)
        {
            EntityDataRow<T> loadrow = FindMap<T>();
            DataTable dt = new DataTable();
            if (loadrow == null)
            {
                var properties = typeof(T).GetProperties();
               
                Dictionary<string, string> map = new Dictionary<string, string>();
                Dictionary<string, Type> mapType = new Dictionary<string, Type>();
                foreach (var p in properties)
                {

                    if (p.GetCustomAttribute(typeof(NoColumnAttribute)) != null)
                    {
                        //没有该列映射
                        continue;
                    }
                    else if (p.GetCustomAttribute(typeof(DataFieldAttribute)) != null)
                    {
                        DataFieldAttribute ttr = p.GetCustomAttribute<DataFieldAttribute>();
                        var type = p.GetCustomAttribute<ColumnTypeAttribute>();
                        map.Add(ttr.ColumnName, p.Name);
                        if (type != null && !type.ColumnType.Equals(p.PropertyType))
                        {
                            dt.Columns.Add(ttr.ColumnName, type.ColumnType);
                            mapType[ttr.ColumnName] = type.ColumnType;
                        }
                        else
                        {
                            var cur = Nullable.GetUnderlyingType(p.PropertyType);
                            dt.Columns.Add(ttr.ColumnName, cur == null ? p.PropertyType : cur);
                            //dt.Columns.Add(ttr.ColumnName, p.PropertyType);
                        }
                    }
                    else if (p.GetCustomAttribute(typeof(ColumnTypeAttribute)) != null)
                    {
                        var type = p.GetCustomAttribute<ColumnTypeAttribute>();
                        dt.Columns.Add(p.Name, type.ColumnType);
                        map.Add(p.Name, p.Name);
                        if (!type.ColumnType.Equals(p.PropertyType))
                        {
                            mapType[p.Name] = type.ColumnType;
                        }
                    }
                    else
                    {
                        var cur = Nullable.GetUnderlyingType(p.PropertyType);
                        dt.Columns.Add(p.Name, cur == null ? p.PropertyType : cur);
                        map.Add(p.Name, p.Name);
                    }
                }
                if (map.Count == 0)
                {
                    map = null;
                }
                if (mapType.Count == 0)
                {
                    mapType = null;
                }
                 loadrow = CreateMap<T>(map, mapType);
                cacheDataTable[typeof(T).FullName + "_map"]= dt;
            }
            else
            {
                dt = cacheDataTable[typeof(T).FullName + "_map"].Clone();
            }
            
            ////1.如果调用table转换
            //LoadDataTable<T> load = (LoadDataTable<T>)PersonToDataTable<T>(map,mapType).CreateDelegate(typeof(LoadDataTable<T>));
            //foreach (var item in lst)
            //{
            //    load(dt, item);
            //}
            //2.如果调用行转换(控制度大些)

           
            foreach (var item in lst)
            {
                var row = dt.NewRow();
                loadrow(row, item);
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>
        /// 忽略特性的查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static EntityDataTable<T> Find<T>()
        {
            EntityDataTable<T> load = null;
            object v = null;
            string name = typeof(T).FullName;
            if (cache.TryGetValue(name, out v))
            {
                load = v as EntityDataTable<T>;
            }
            else
            {
                load =(EntityDataTable< T >) EntityToDataTableEmit<T>().CreateDelegate(typeof(EntityDataTable<T>));
                cache[name] = load;
            }
            return load;
        }

        /// <summary>
        /// 带有特性的查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static EntityDataRow<T> FindMap<T>()
        {
            EntityDataRow<T> loadrow = null;
            object v = null;
            if(cache.TryGetValue(typeof(T).FullName+"_map", out v))
            {
                loadrow = v as EntityDataRow<T>;
            }
            return loadrow;
        }

        /// <summary>
        /// 带有特性的创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="mapType"></param>
        /// <returns></returns>
        private static EntityDataRow<T> CreateMap<T>(Dictionary<string,string>map,Dictionary<string,Type>mapType)
        { 
            var loadRow= (EntityDataRow<T>)EntityToDataRowEmit<T>(map, mapType).CreateDelegate(typeof(EntityDataRow<T>));
            cache[typeof(T).FullName+"_map"] = loadRow;
            return loadRow;
        }
    }
}
