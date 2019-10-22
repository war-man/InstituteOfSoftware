using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 宿舍房间信息业务类
    /// </summary>
    public class DormInformationBusiness : BaseBusiness<DormInformation>
    {
        /// <summary>
        /// 获取正在使用的房间
        /// </summary>
        /// <returns></returns>
        public List<DormInformation> GetDorms()
        {
            return this.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        /// <summary>
        /// 获取学生寝室
        /// </summary>
        /// <returns></returns>
        public List<DormInformation> GetStudentDorms()
        {
            return this.GetDorms().Where(a => a.RoomStayTypeId == 1).ToList();
        }

      
        /// <summary>
        /// 根据栋楼层id返回对应房间集合
        /// </summary>
        /// <param name="TungFloorID"></param>
        /// <returns></returns>
        public List<DormInformation> GetDormsByTungFloorID(int TungFloorID)
        {
            return this.GetDorms().Where(a => a.TungFloorId == TungFloorID).ToList();
        }

        /// <summary>
        /// 根据房间id获取房间对象
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public DormInformation GetDormByDorminfoID(int DorminfoID) {
          return  this.GetDorms().Where(a => a.ID == DorminfoID).FirstOrDefault();
        }
       

    }
}
