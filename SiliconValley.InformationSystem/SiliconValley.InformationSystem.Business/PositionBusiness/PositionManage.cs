using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.PositionBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    
   public class PositionManage:BaseBusiness<Position>
    {

        /// <summary>
        /// 获取没有被禁用的岗位集合
        /// </summary>
        /// <returns></returns>
        public List<Position> GetPositions() {
          return  this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据部门id 返回属于该部门的岗位
        /// </summary>
        /// <param name="param">部门id</param>
        /// <returns></returns>
        public List<Position> GetPositionByDepeID(int param) {
          return  this.GetPositions().Where(a => a.DeptId == param).ToList();
        }

    }
}
