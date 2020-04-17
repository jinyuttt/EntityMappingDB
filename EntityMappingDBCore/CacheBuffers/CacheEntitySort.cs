#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：EntityMappingDBCore.DBToEntity
* 项目描述 ：
* 类 名 称 ：CacheEntitySort
* 类 描 述 ：
* 命名空间 ：EntityMappingDBCore.DBToEntity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019/3/1 16:00:55
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMappingDBCore.CacheBuffers
{
    /* ============================================================================== 
* 功能描述：CacheEntitySort 
* 创 建 者：jinyu 
* 创建日期：2019
* 更新时间 ：2019
* ==============================================================================*/

  internal class CacheEntitySort<TKey>
    {
        public TKey key { get; set; }
        public float rate { get; set; }
    }
}
