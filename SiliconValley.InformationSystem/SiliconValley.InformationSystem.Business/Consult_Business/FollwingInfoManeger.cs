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
    }
}
