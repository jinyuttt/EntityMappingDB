#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：EntityMappingDBCore.DBToEntity
* 项目描述 ：
* 类 名 称 ：DataField
* 类 描 述 ：
* 命名空间 ：EntityMappingDBCore.DBToEntity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



using System;

namespace EntityMappingDBCore
{
    /* ============================================================================== 
* 功能描述：DataField  列名称映射
* 创 建 者：jinyu 
* 创建日期：2019
* 更新时间 ：2019
* ==============================================================================*/

      [AttributeUsage(AttributeTargets.Property,AllowMultiple =false)]
    public class DataFieldAttribute : Attribute
    {
        public string ColumnName { get; set; }

        public DataFieldAttribute(string name)
        {
            this.ColumnName = name;
        }
    }
}
