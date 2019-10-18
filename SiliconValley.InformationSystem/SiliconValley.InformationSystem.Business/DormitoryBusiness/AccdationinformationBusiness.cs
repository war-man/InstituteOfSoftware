using SiliconValley.InformationSystem.Entity.MyEntity;
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
    }
}
