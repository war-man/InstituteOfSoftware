using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    ///学生房间入住信息业务类
    /// </summary>
    public class AccdationinformationBusiness:BaseBusiness<Accdationinformation>
    {
        private DormInformationBusiness dbdorm;
        private StaffAccdationBusiness dbstaffacc;
        private RoomdeWithPageXmlHelp dbroomxml;
        /// <summary>
        /// 返回正在居住的入住信息
        /// </summary>
        /// <returns></returns>
        public List<Accdationinformation> GetAccdationinformations() {
           return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据房间号码返回正在居住信息
        /// </summary>
        /// <param name="DormId">房间号码</param>
        /// <returns></returns>
        public List<Accdationinformation> GetAccdationinformationByDormId(int DormId) {
           return this.GetAccdationinformations().Where(a => a.DormId == DormId).ToList();
        }

        /// <summary>
        /// 添加入住信息
        /// </summary>
        /// <returns></returns>
        public bool AddAcc(Accdationinformation accdationinformation) {
            bool result = false;
            try
            {
                this.Insert(accdationinformation);
                result = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        /// <summary>
        /// 根据学生编号返回居住信息
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public Accdationinformation GetAccdationByStudentNumber(string StudentNumber) {
           return this.GetAccdationinformations().Where(a => a.Studentnumber == StudentNumber).FirstOrDefault();
        }

        /// <summary>
        /// 根据栋楼层id获取这一层的学生居住信息
        /// </summary>
        /// <param name="tungfloorid"></param>
        /// <returns></returns>
        public List<Accdationinformation> GetAccdationinformationsByTungFloorID(int tungfloorid) {
            dbdorm = new DormInformationBusiness();
           var list0=  dbdorm.GetDormsByTungFloorID(tungfloorid);
            List<Accdationinformation> result0 = new List<Accdationinformation>();
            foreach (var item in list0)
            {
                result0.AddRange(this.GetAccdationinformationByDormId(item.ID));
            }
            return result0;
        }

        /// <summary>
        /// 是否i有人
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public bool HasSomeone(int param0) {
           
            dbdorm = new DormInformationBusiness();
            dbroomxml = new RoomdeWithPageXmlHelp();
            var fpxnb = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StaffRoom);
            var fpxnb1 = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StudentRoom);

            var entity0 = dbdorm.GetEntity(param0);
            bool Candelete = true;
            if (entity0.RoomStayTypeId == fpxnb)
            {
                dbstaffacc = new StaffAccdationBusiness();
                var fpxnb3 = dbstaffacc.GetStaffAccdationsByDorminfoID(param0);
                if (fpxnb3.Count != 0)
                {
                    Candelete = false;
                }
            }
            if (entity0.RoomStayTypeId == fpxnb1)
            {
                var fpx4 = this.GetAccdationinformationByDormId(param0);
                if (fpx4.Count != 0)
                {
                    Candelete = false;
                }
            }
            return Candelete;
        }
    }
}
