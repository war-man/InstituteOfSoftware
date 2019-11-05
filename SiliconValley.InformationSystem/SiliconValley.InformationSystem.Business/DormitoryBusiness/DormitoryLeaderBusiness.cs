using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    public  class DormitoryLeaderBusiness:BaseBusiness<DormitoryLeader>
    {
        /// <summary>
        /// 获取现在的寝室长
        /// </summary>
        /// <returns></returns>
        public List<DormitoryLeader> GetLeaders() {
            var aa= this.GetIQueryable().Where(a => a.IsDelete == false).ToList();
            return aa;
        }

        /// <summary>
        /// 根据房间号获取这个房间的寝室长
        /// </summary>
        /// <param name="DormInfoID"></param>
        /// <returns></returns>
        public DormitoryLeader GetLeader(int DormInfoID) {
            var dd= this.GetLeaders().Where(a => a.DormInfoID == DormInfoID).FirstOrDefault();
            return dd;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="leader"></param>
        /// <returns></returns>
        public bool AddLeader(DormitoryLeader leader) {
            bool result = false;
            try
            {
                this.Insert(leader);
                result = true;
              
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        /// <summary>
        /// 注销对象
        /// </summary>
        /// <param name="leader"></param>
        /// <returns></returns>
        public bool Cancellation(DormitoryLeader leader) {
            bool result = false;
            try
            {
                leader.IsDelete = true;
                this.Update(leader);
                result = true;

            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        /// <summary>
        /// 根据学生编号返回寝室长对象
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public DormitoryLeader GetLeaderByStudentNumber(string StudentNumber) {
          return  this.GetLeaders().Where(a => a.StudentNumber == StudentNumber).FirstOrDefault();
        }
    }
}
