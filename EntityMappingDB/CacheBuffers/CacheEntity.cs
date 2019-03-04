#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：EntityMappingDBEmit.DBToEntity
* 项目描述 ：
* 类 名 称 ：CacheEntity
* 类 描 述 ：
* 命名空间 ：EntityMappingDBEmit.DBToEntity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019/3/1 11:22:10
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



using System;
using System.Threading;

namespace EntityMappingDBEmit.CacheBuffers
{
    /* ============================================================================== 
* 功能描述：CacheEntity 缓存实体
* 创 建 者：jinyu 
* 创建日期：2019
* 更新时间 ：2019
* ==============================================================================*/

    internal class CacheEntity<T>
    {
        public CacheEntity(T obj)
        {
            this.entity = obj;
        }

        /// <summary>
        /// 使用次数
        /// </summary>
        private int usenum = 0;

        /// <summary>
        /// 创建时间
        /// </summary>
        private readonly long CreateTicks = DateTime.Now.Ticks;

        /// <summary>
        /// Ticks与毫秒转换
        /// </summary>
        private const int TickMS = 10000;

        /// <summary>
        /// 缓存实体
        /// </summary>
        private T entity;

        /// <summary>
        /// 缓存对象
        /// </summary>
        public T Entity { get { Interlocked.Increment(ref usenum); return entity; }  set{ entity = value; } }

        /// <summary>
        /// 使用频率
        /// </summary>
        public float UseRate { get { return  (float)usenum / ((DateTime.Now.Ticks - CreateTicks)/TickMS); } }

        
    }
}
