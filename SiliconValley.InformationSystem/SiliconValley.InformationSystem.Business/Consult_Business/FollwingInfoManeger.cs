using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Business.Consult_Business
{
    public class FollwingInfoManeger : BaseBusiness<FollwingInfo>
    {
        /// <summary>
        /// 添加咨询跟踪数据
        /// </summary>
        /// <param name="flist"></param>
        /// <returns></returns>
        public bool Addlist(List<FollwingInfo> flist)
        {
            try
            {
                this.Insert(flist);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据分量获取所有的咨询跟踪数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<FollwingInfo> GetFoll_ConsltId(int id)
        {
           return GetListBySql<FollwingInfo>(" select * from FollwingInfo where Consult_Id=" + id);
        }

      
    }
}
