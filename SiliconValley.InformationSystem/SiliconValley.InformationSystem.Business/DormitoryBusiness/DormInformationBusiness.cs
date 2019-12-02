using SiliconValley.InformationSystem.Entity.Entity;
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
        private AccdationinformationBusiness dbacc;
        /// <summary>
        /// 获取正在使用的房间
        /// </summary>
        /// <returns></returns>
        public List<DormInformation> GetDorms()
        {
            var aa= this.GetIQueryable().Where(a=>a.IsDelete==false).ToList();
            return aa;
        }

        /// <summary>
        /// 获取学生寝室
        /// </summary>
        /// <returns></returns>
        public List<DormInformation> GetStudentDorms()
        {
            dbxml = new RoomdeWithPageXmlHelp();
            int roomtype = dbxml.GetRoomType(Entity.ViewEntity.RoomTypeEnum.RoomType.StudentRoom);
            return this.GetDorms().Where(a => a.RoomStayTypeId == roomtype).ToList();
        }
        /// <summary>
        /// 获取学生寝室
        /// </summary>
        /// <returns></returns>
        public List<DormInformation> GetStudentDormsByTungfloorid(int Tungfloorid)
        {
           return this.GetStudentDorms().Where(a => a.TungFloorId == Tungfloorid).ToList();
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

        /// <summary>
        /// 名字是否重复 true 就是存在 不能使用
        /// </summary>
        /// <param name="param0">栋楼层id</param>
        /// <param name="param1">房间名字</param>
        /// <returns></returns>
        public bool DuplicateName(int param0,string param1)
        {
            //根据这个楼层来说房间号是不能重复的。
            List<DormInformation> querydormlist = this.GetDormsByTungFloorID(param0).ToList();
            DormInformation querydorm = querydormlist.Where(a => a.DormInfoName == param1).FirstOrDefault();
            bool result = false;
            if (querydorm!=null)
            {
                return true;
            }
            return result;
        }

        /// <summary>
        /// 排除没有居住信息的房间
        /// </summary>
        /// <param name="querydorm"></param>
        /// <returns></returns>
        public List<DormInformation> Exclude(List<DormInformation> querydorm)
        {
            dbstaffacc = new StaffAccdationBusiness();
            dbacc = new AccdationinformationBusiness();
            //排除存在居住信息的房间
            for (int i = querydorm.Count - 1; i >= 0; i--)
            {
                List<StaffAccdation> querystaffacc = dbstaffacc.GetStaffAccdationsByDorminfoID(querydorm[i].ID);
                List<Accdationinformation> queryacc = dbacc.GetAccdationinformationByDormId(querydorm[i].ID);
                if (querystaffacc.Count == 0 && queryacc.Count == 0)
                {
                    querydorm.Remove(querydorm[i]);
                }
            }
            return querydorm;
        }

        /// <summary>
        /// 获取全部的员工宿舍
        /// </summary>
        /// <returns></returns>
        public List<DormInformation> GetDormsForStaff() {
            dbxml = new RoomdeWithPageXmlHelp();
            int roomtype = dbxml.GetRoomType(Entity.ViewEntity.RoomTypeEnum.RoomType.StaffRoom);
            return this.GetDorms().Where(a => a.RoomStayTypeId == roomtype).ToList();
        }

        /// <summary>
        /// 根据 栋-楼层id 获取这个层的员工房间
        /// </summary>
        /// <param name="Tungfloorid"></param>
        /// <returns></returns>
        public List<DormInformation> GetDormsForStaffByTungfloorid(int Tungfloorid) {
           return this.GetDormsForStaff().Where(a => a.TungFloorId == Tungfloorid).ToList();
        }
    }
}
