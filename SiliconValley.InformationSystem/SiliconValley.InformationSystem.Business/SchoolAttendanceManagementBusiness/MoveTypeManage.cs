using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness
{
   public  class MoveTypeManage:BaseBusiness<MoveType>
    {
        /// <summary>
        /// 根据异动类型编号获取异动类型对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MoveType GetmtById(int id) {
            return this.GetEntity(id);
        }
        /// <summary>
        /// 根据类型名称获取类型编号
        /// </summary>
        /// <param name="tname"></param>
        /// <returns></returns>
        public int GetmtTypeidByTname(string tname) {
           return this.GetList().Where(s => s.MoveTypeName == tname).FirstOrDefault().ID;
        }
    }
}
