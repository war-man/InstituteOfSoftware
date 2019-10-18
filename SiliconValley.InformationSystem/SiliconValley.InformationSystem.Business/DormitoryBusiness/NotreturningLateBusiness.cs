using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 晚归登记
    /// </summary>
    public class NotreturningLateBusiness:BaseBusiness<NotreturningLate>
    {
        /// <summary>
        /// 返回可用的数据
        /// </summary>
        /// <returns></returns>
        public List<NotreturningLate> GetNotreturningLates() {
            return this.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        /// <summary>
        /// 根据学生编号返回的数据
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public List<NotreturningLate> GeNotreturningLatesByStudentNumber(string StudentNumber) {
           return this.GetNotreturningLates().Where(a => a.StudentNumber == StudentNumber).ToList();
        }

        /// <summary>
        /// 根据老师编号返回的数据
        /// </summary>
        /// <param name="HeadMasterID"></param>
        /// <returns></returns>
        public List<NotreturningLate> GetNotreturningLatesByHeaderMaster(int HeadMasterID) {
            return this.GetNotreturningLates().Where(a => a.HeadMasterID == HeadMasterID).ToList();
        }

        /// <summary>
        /// 根据ID返回对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public NotreturningLate GetNotreturningLateByID(int ID) {
            return this.GetNotreturningLates().Where(a => a.Id == ID).FirstOrDefault();
        }

        /// <summary>
        /// 添加晚归登记记录
        /// </summary>
        /// <param name="notreturningLate"></param>
        /// <returns></returns>
        public bool AddNotreturningLate(NotreturningLate notreturningLate) {
            bool result = false;
            try
            {
                this.Insert(notreturningLate);
                result = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
    }
}
