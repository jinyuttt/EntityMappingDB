#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：EntityMappingDBEmit.DBToEntity
* 项目描述 ：
* 类 名 称 ：Person
* 类 描 述 ：
* 命名空间 ：EntityMappingDBEmit.DBToEntity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019/3/1 2:39:12
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion




namespace EntityMappingDBEmit
{
    /* ============================================================================== 
* 功能描述：Person 
* 创 建 者：jinyu 
* 创建日期：2019
* 更新时间 ：2019
* ==============================================================================*/

  public  class Person
    {
        [DataField("PersonName")]
        public string Name { get; set; }

        public int ID { get; set; }

        public  int? Age { get; set; }

        [NoColumn]
        public  int Score { get; set; }

        public string Note { get; set; }

        public string KK { get; set; }
    }
}
