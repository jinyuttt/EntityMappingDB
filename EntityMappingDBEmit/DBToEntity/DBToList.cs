#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：EntityMappingDBEmit.DBToEntity
* 项目描述 ：
* 类 名 称 ：DBToList
* 类 描 述 ：
* 命名空间 ：EntityMappingDBEmit.DBToEntity
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019/3/3 15:20:05
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EntityMappingDBEmit.DBToEntity
{
    /* ============================================================================== 
* 功能描述：DBToList 
* 创 建 者：jinyu 
* 创建日期：2019
* 更新时间 ：2019
* ==============================================================================*/

   public  class DBToList
    {
       
        public static Person ConvertToPerson(DataRow row)
        {
            Person person = new Person();
            if (DynamicAssembleInfo.CanSetted(row, "Age"))
            {
                person.Age = Convert.ToInt32(row["Age"]);
            }
            return person;
        }
        public static void ConvertToNull()
        {
            Person person = new Person();
            person.Age = 3;
        }
    }
}
