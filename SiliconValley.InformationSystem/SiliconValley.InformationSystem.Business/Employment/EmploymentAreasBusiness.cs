
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 区域业务类
    /// </summary>
    public class EmploymentAreasBusiness : BaseBusiness<EmploymentAreas>
    {
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<EmploymentAreas> GetAll()
        {
           var bb=this.GetIQueryable().Where(a => a.IsDel == false).ToList();
            return bb;
        }
        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public EmploymentAreas GetObjByID(int? ID) {
           var bb= this.GetAll().Where(a => a.ID == ID).FirstOrDefault();
            return bb;
        }
    }
}
