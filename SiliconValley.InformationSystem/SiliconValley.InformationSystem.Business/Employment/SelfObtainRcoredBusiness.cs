using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 自主就业业务类
    /// </summary>
   public class SelfObtainRcoredBusiness:BaseBusiness<SelfObtainRcored>
    {

        /// <summary>
        /// 获取全部可用的深数据
        /// </summary>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainRcoreds() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据学生编号获取这个自主就业的对象
        /// </summary>
        /// <param name="Studentno"></param>
        /// <returns></returns>
        public SelfObtainRcored GetSelfObtainByStudentno(string Studentno)
        {
           return this.GetSelfObtainRcoreds().Where(a => a.StudentNO == Studentno).FirstOrDefault();
        }

        /// <summary>
        ///根据这个计划id 获取这个计划中自主就业的对象
        /// </summary>
        /// <param name="QuarterID"></param>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainsByQuarterID(int QuarterID) {
            return this.GetSelfObtainRcoreds().Where(a => a.QuarterID == QuarterID).ToList();
        }
    }
}
