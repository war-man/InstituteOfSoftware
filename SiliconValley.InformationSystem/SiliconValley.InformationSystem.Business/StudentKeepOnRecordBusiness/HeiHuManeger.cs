using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness
{
   public class HeiHuManeger:BaseBusiness<HeiHu>
    {
        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="hu"></param>
        /// <returns></returns>
        public bool Add_SingData(HeiHu hu)
        {
            try
            {
                this.Insert(hu);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            string sql = " select * from HeiHu";

            int count= this.GetListBySql<HeiHu>(sql).Count;

            return count;
        }
    }
}
