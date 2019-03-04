//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

//namespace EntityMappingDBEmit.EntityToDB
//{
//    /// <summary>
//    /// 测试
//    /// </summary>
//   public class SaveEmit
//    {
//        public static Delegate ConvertTableList<T>(Dictionary<string, string> map = null, Dictionary<string, Type> mapType = null)
//        {

//            if (File.Exists("PersonT.dll"))
//            {
//                File.Delete("PersonT.dll");
//            }

//            AssemblyName assemblyName = new AssemblyName("assemblyName");

//            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
//            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("PersonModule", "PersonT.dll");
//            TypeBuilder typeBuilder = moduleBuilder.DefineType("PersonT", TypeAttributes.Public | TypeAttributes.Class);

//            //*【不同点1】

//            MethodBuilder sayHelloMethod = typeBuilder.DefineMethod("CDataT", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null, new Type[] { typeof(T), typeof(DataTable) });

//            ILGenerator generator = sayHelloMethod.GetILGenerator();
//            LocalBuilder reslut = generator.DeclareLocal(typeof(DataRow));
//            generator.Emit(OpCodes.Ldarg_1);
//            generator.Emit(OpCodes.Call, typeof(DataTable).GetMethod("NewRow"));
//            generator.Emit(OpCodes.Stloc, reslut);
//            if (map == null)
//            {
//                foreach (var p in typeof(T).GetProperties())
//                {
//                    generator.Emit(OpCodes.Ldloc, reslut);
//                    generator.Emit(OpCodes.Ldstr, p.Name);
//                    generator.Emit(OpCodes.Ldarg_1);
//                    generator.Emit(OpCodes.Call, p.GetGetMethod());//直接给属性赋值
//                    if (mapType == null || !mapType.ContainsKey(p.Name))
//                    {
//                        if (p.PropertyType.IsValueType)
//                            generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
//                        else
//                            generator.Emit(OpCodes.Castclass, p.PropertyType);
//                    }
//                    else
//                    {
//                        generator.Emit(OpCodes.Ldtoken, p.PropertyType);
//                        generator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
//                        generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) }));
//                    }
//                    generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
//                }
//                generator.Emit(OpCodes.Ldarg_0);
//                generator.Emit(OpCodes.Call, typeof(DataTable).GetMethod("get_Rows"));
//                generator.Emit(OpCodes.Ldloc, reslut);
//                generator.Emit(OpCodes.Call, typeof(DataRowCollection).GetMethod("Add", new Type[] { typeof(DataRow) }));
//                generator.Emit(OpCodes.Ret);
//            }
//            else
//            {
//                var properties = typeof(T).GetProperties();
//                List<PropertyInfo> lst = new List<PropertyInfo>(properties);
//                foreach (var kv in map)
//                {
//                    var p = lst.Find(x => x.Name == kv.Value);
//                    generator.Emit(OpCodes.Ldloc, reslut);
//                    generator.Emit(OpCodes.Ldstr, kv.Value);
//                    generator.Emit(OpCodes.Ldarg_1);
//                    generator.Emit(OpCodes.Call, p.GetGetMethod());//获取属性值
//                    //if (p.PropertyType.IsValueType)
//                    //    generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
//                    //else
//                    //    generator.Emit(OpCodes.Castclass, p.PropertyType);
//                    if (mapType == null || !mapType.ContainsKey(kv.Key))
//                    {
//                        if (p.PropertyType.IsValueType)
//                            generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
//                        else
//                            generator.Emit(OpCodes.Castclass, p.PropertyType);
//                    }
//                    else
//                    {
                        
//                        generator.Emit(OpCodes.Ldtoken, mapType[kv.Key]);
//                        generator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
//                        generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ChangeType",new Type[] { typeof(object), typeof(Type) }));
//                    }

//                    generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
//                }
//                generator.Emit(OpCodes.Ldarg_0);
//                generator.Emit(OpCodes.Call, typeof(DataTable).GetMethod("get_Rows"));
//                generator.Emit(OpCodes.Ldloc, reslut);
//                generator.Emit(OpCodes.Call, typeof(DataRowCollection).GetMethod("Add", new Type[] { typeof(DataRow) }));
//                generator.Emit(OpCodes.Ret);
//            }
//                Type personType = typeBuilder.CreateType();
//            assemblyBuilder.Save("PersonT.dll");

//            MethodInfo methodInfo = personType.GetMethod("CDataT");
//            //

//            var fun = Delegate.CreateDelegate(typeof(LoadDataRow<Person>), methodInfo);
//            return fun;
//            //【不同点3】
//            // methodInfo.Invoke(obj, new object[] { "蝈蝈" });

//            //  return method;
//            //【不同点3】
//            //   methodInfo.Invoke(obj, new object[] { "蝈蝈" });

//        }

//        public static Delegate ConvertRowList<T>(Dictionary<string, string> map = null, Dictionary<string, Type> mapType = null)
//        {

//            if (File.Exists("PersonT.dll"))
//            {
//                File.Delete("PersonT.dll");
//            }

//            AssemblyName assemblyName = new AssemblyName("assemblyName");

//            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
//            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("PersonModule", "PersonT.dll");
//            TypeBuilder typeBuilder = moduleBuilder.DefineType("PersonT", TypeAttributes.Public | TypeAttributes.Class);

//            //*【不同点1】

//            MethodBuilder sayHelloMethod = typeBuilder.DefineMethod("CDataRow", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, null, new Type[] {  typeof(DataRow), typeof(T) });

//            ILGenerator generator = sayHelloMethod.GetILGenerator();
//            if (map == null)
//            {
//                foreach (var p in typeof(T).GetProperties())
//                {
//                    generator.Emit(OpCodes.Ldarg_0);
//                    generator.Emit(OpCodes.Ldstr, p.Name);
//                    generator.Emit(OpCodes.Ldarg_1);
//                    generator.Emit(OpCodes.Call, p.GetGetMethod());//直接给属性赋值
//                    //if (p.PropertyType.IsValueType)
//                    //    generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
//                    //else
//                    //    generator.Emit(OpCodes.Castclass, p.PropertyType);
//                    if (mapType == null || !mapType.ContainsKey(p.Name))
//                    {
//                        if (p.PropertyType.IsValueType)
//                            generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
//                        else
//                            generator.Emit(OpCodes.Castclass, p.PropertyType);
//                    }
//                    else
//                    {
//                        generator.Emit(OpCodes.Ldtoken, mapType[p.Name]);
//                        generator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
//                        generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) }));
//                    }

//                    generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
//                }
//                //generator.Emit(OpCodes.Ldarg_0);
//                //generator.Emit(OpCodes.Call, typeof(DataTable).GetMethod("get_Rows"));

//                //generator.Emit(OpCodes.Call, typeof(DataRowCollection).GetMethod("Add", new Type[] { typeof(DataRow) }));
//                generator.Emit(OpCodes.Ret);
//            }
//            else
//            {

//                var properties = typeof(T).GetProperties();
//                List<PropertyInfo> lst = new List<PropertyInfo>(properties);
//                foreach (var kv in map)
//                {
//                    var p = lst.Find(x => x.Name == kv.Value);
//                    generator.Emit(OpCodes.Ldarg_0);
//                    generator.Emit(OpCodes.Ldstr, kv.Key);
//                    generator.Emit(OpCodes.Ldarg_1);
//                    generator.Emit(OpCodes.Call, p.GetGetMethod());//获取属性值
                  
//                    if (mapType == null || !mapType.ContainsKey(kv.Key))
//                    {
//                        if (p.PropertyType.IsValueType)
//                            generator.Emit(OpCodes.Box, p.PropertyType);//一直在折腾这个地方，哎
//                        else
//                            generator.Emit(OpCodes.Castclass, p.PropertyType);
//                    }
//                    else
//                    {
//                        generator.Emit(OpCodes.Ldtoken, mapType[kv.Key]);
//                        generator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
//                        generator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(Type) }));
//                    }

//                    generator.Emit(OpCodes.Call, typeof(DataRow).GetMethod("set_Item", new Type[] { typeof(string), typeof(object) }));
//                }
                
//                generator.Emit(OpCodes.Ret);
//            }
//            Type personType = typeBuilder.CreateType();
//            assemblyBuilder.Save("PersonT.dll");

//            MethodInfo methodInfo = personType.GetMethod("CDataT");
//            //

//            var fun = Delegate.CreateDelegate(typeof(LoadDataRow<Person>), methodInfo);
//            return fun;
//            //【不同点3】
//            // methodInfo.Invoke(obj, new object[] { "蝈蝈" });

//            //  return method;
//            //【不同点3】
//            //   methodInfo.Invoke(obj, new object[] { "蝈蝈" });

//        }

//    }
//}
