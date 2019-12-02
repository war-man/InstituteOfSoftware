using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 班主任带班业务类
    /// </summary>
    public class ProHeadClass : BaseBusiness<HeadClass>
    {
        /// <summary>
        /// 获取正在班主任带班记录
        /// </summary>
        /// <returns></returns>
        public List<HeadClass> GetHeadClasses() {
            return this.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        /// <summary>
        /// 根据班级编号获取带班记录
        /// </summary>
        /// <param name="ClassNO"></param>
        /// <returns></returns>
        public HeadClass GetClassByClassid(int classid) {
            //学员班级
            //ClassScheduleBusiness classScheduleBusiness = new ClassScheduleBusiness();
            return  this.GetHeadClasses().Where(a => a.ClassID == classid).FirstOrDefault();
        }

        /// <summary>
        /// 根据班主任id获取带班记录
        /// </summary>
        /// <param name="MasterID"></param>
        /// <returns></returns>
        public HeadClass GetClassByMasterID(int MasterID) {
            return this.GetHeadClasses().Where(a => a.LeaderID == MasterID).FirstOrDefault();
        }
    }
}
