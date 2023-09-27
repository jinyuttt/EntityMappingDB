#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：EntityMappingDB.DBToEntity
* 项目描述 ：
* 类 名 称 ：ColumnType
* 类 描 述 ：
* 命名空间 ：EntityMappingDB.DBToEntity
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

namespace EntityMappingDB
{
    /* ============================================================================== 
* 功能描述：ColumnType  列类型映射
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/

    public class ColumnTypeAttribute : Attribute
    {
        /// <summary>
        /// 列类型
        /// </summary>
        public Type ColumnType { get; set; }

        /// <summary>
        /// 特性
        /// </summary>
        /// <param name="type"></param>
        public ColumnTypeAttribute(Type type)
        {
            this.ColumnType = type;
        }
    }
}
