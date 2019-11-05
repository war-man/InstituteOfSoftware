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

        private RoomdeWithPageXmlHelp dbxml;
        private StaffAccdationBusiness dbstaffacc;
        /// <summary>
        /// 获取正在使用的房间
        /// </summary>
        /// <returns></returns>
        public List<DormInformation> GetDorms()
        {
            return this.GetIQueryable().ToList();
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
        /// 根据栋楼层id返回对应房间集合 用于信息详细 显示禁用的房间都行
        /// </summary>
        /// <param name="TungFloorID"></param>
        /// <returns></returns>
        public List<DormInformation> GetDormsByTungFloorID(int TungFloorID)
        {
            return this.GetDorms().Where(a => a.TungFloorId == TungFloorID).ToList();
        }

        /// <summary>
        /// 用于对房间的针对 对可用的房间进行的一个操作
        /// </summary>
        /// <param name="TungFloorID"></param>
        /// <returns></returns>
        public List<DormInformation> GetDormsByTungFloorIDing(int TungFloorID)
        {
            return this.GetDorms().Where(a => a.TungFloorId == TungFloorID&&a.IsDelete==false).ToList();
        }

        /// <summary>
        /// 根据房间id获取房间对象
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public DormInformation GetDormByDorminfoID(int DorminfoID) {
          return  this.GetDorms().Where(a => a.ID == DorminfoID).FirstOrDefault();
        }

        /// <summary>
        /// 获取员工已经居住的寝室
        /// </summary>
        /// <param name="tungfloorid"></param>
        /// <returns></returns>
        public List<DormInformation> GetStaffJuzhuing() {
            dbxml = new RoomdeWithPageXmlHelp();
            dbstaffacc = new StaffAccdationBusiness();
            int roomtype = dbxml.GetRoomType(Entity.ViewEntity.RoomTypeEnum.RoomType.StaffRoom);
            var list0 = this.GetIQueryable().Where(a=>a.IsDelete==false&& a.RoomStayTypeId == roomtype).ToList();

            for (int i = list0.Count-1; i >=0; i--)
            {
               var list1= dbstaffacc.GetStaffAccdationsByDorminfoID(list0[i].ID);
                if (list1.Count==0)
                {
                    list0.Remove(list0[i]);
                }
            }
            return list0;
        }
    }
}
