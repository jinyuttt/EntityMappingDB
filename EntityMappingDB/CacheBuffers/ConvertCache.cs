#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：EntityMappingDB.DBToEntity
* 项目描述 ：
* 类 名 称 ：ConvertCache
* 类 描 述 ：
* 命名空间 ：EntityMappingDB.DBToEntity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019/3/1 11:04:22
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EntityMappingDB.CacheBuffers
{
    /* ============================================================================== 
* 功能描述：ConvertCache  缓存，可以根据使用频率和最新使用维护
* 创 建 者：jinyu 
* 创建日期：2019
* 更新时间 ：2019
* ==============================================================================*/

    public  class ConvertCache<TKey,TValue>
    {
        public static int MaxSize = 30000;//3万
        public static float Scale = 0.25f;//移除比例
        
        /// <summary>
        /// 单例
        /// </summary>
        private static readonly Lazy<ConvertCache<TKey, TValue>> instance = new Lazy<ConvertCache<TKey, TValue>>();

        /// <summary>
        /// 缓存池
        /// </summary>
        private readonly Dictionary<TKey, CacheEntity<TValue>> _cache = null;

        /// <summary>
        /// 标记正在清理
        /// </summary>
        private volatile bool isRun = false;

      

        /// <summary>
        /// 单例
        /// </summary>
        public static ConvertCache<TKey, TValue> Singleton
        {
            get { return instance.Value; }
        }

        public ConvertCache()
        {
           
            _cache = new Dictionary<TKey, CacheEntity<TValue>>();
           
        }

        /// <summary>
        /// 保持缓存,不会精确控制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Set(TKey key, TValue obj)
        {
            _cache[key] = new CacheEntity<TValue>(obj);
            if (IsRemove())
            {
                isRun = true;
                TrimeSize();
            }
        }

        /// <summary>
        /// 获取缓存实体
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">缓存对象</param>
        /// <returns></returns>
        public bool TryGet(TKey key, out TValue obj)
        {
            obj = default(TValue);
            CacheEntity<TValue> v;
            if (_cache.TryGetValue(key, out v))
            {
                obj = v.Entity;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
           
        }


        /// <summary>
        /// 触发移除
        /// </summary>
        /// <returns></returns>
        private bool IsRemove()
        {
            if((_cache.Count>MaxSize)&&!isRun)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清理缓存，更新数据;最多清理一半
        /// </summary>
        private void TrimeSize()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var keys = _cache.Keys.ToList();
                    List<CacheEntitySort<TKey>> lst = new List<CacheEntitySort<TKey>>(_cache.Count);
                    foreach (var item in keys)
                    {
                        CacheEntity<TValue> entity;
                        if (_cache.TryGetValue(item, out entity))
                        {
                            CacheEntitySort<TKey> sort = new CacheEntitySort<TKey>() { Key = item, Rate = entity.UseRate };
                            lst.Add(sort);
                        }
                    }
                    //
                    lst.Sort((x, y) => { return x.Rate.CompareTo(y.Rate); });
                    int num = (int)(MaxSize * Scale);
                    if(num>_cache.Count/2)
                    {
                        num = _cache.Count / 2;
                    }
                    for (int i = 0; i < num; i++)
                    {
                        _cache.Remove(lst[i].Key);
                    }
                    isRun = false;
                }
                catch
                {
                    TrimeSize();//递归执行
                }
            });
        }
    }
}
