#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：EntityMappingDB.DBToEntity
* 项目描述 ：
* 类 名 称 ：AssembleInfo
* 类 描 述 ：
* 命名空间 ：EntityMappingDB.DBToEntity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.1.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



using System;
using System.Reflection;
using System.Data;

namespace EntityMappingDB.DBToEntity
{
    /* ============================================================================== 
* 功能描述：AssembleInfo  emit所需要的元数据信息
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/

    public class DynamicAssembleInfo
    {
        public string MethodName;
        public Type SourceType;
        public MethodInfo CanSettedMethod;
        public MethodInfo GetValueMethod;
        public MethodInfo EnumConvert;
      
        public DynamicAssembleInfo(Type type)
        {
            SourceType = type;
            MethodName = "Convert" + type.Name + "To";
            CanSettedMethod = this.GetType().GetMethod("CanSetted", new Type[] { type, typeof(string) });
            GetValueMethod = type.GetMethod("get_Item", new Type[] { typeof(string) });
            EnumConvert= this.GetType().GetMethod("ConvertEnum", new Type[] { typeof(object),typeof(Type)});
        }

       

        /// <summary>
        /// 判断datareader是否存在某字段并且值不为空
        /// 已经改为一次验证
        /// </summary>
        /// <param name="dr">当前的datareader</param>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public static bool CanSetted(IDataRecord dr, string name)
        {
            return !dr[name].Equals(DBNull.Value);
        }

        /// <summary>
        /// 判断datarow所在的datatable是否存在某列并且值不为空
        /// 已经修改成了一次性验证
        /// </summary>
        /// <param name="dr">当前datarow</param>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public static bool CanSetted(DataRow dr, string name)
        {
            return !dr.IsNull(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ConvertEnum(object obj,Type type)
        {
           return Enum.Parse(type, obj.ToString());
        }
     
    }
}
