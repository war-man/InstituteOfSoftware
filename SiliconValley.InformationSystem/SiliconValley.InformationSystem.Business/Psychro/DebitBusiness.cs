using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 员工借资页面
    /// </summary>
    public class DebitBusiness : BaseBusiness<Debit>
    {
        /// <summary>
        /// 学校年计划业务类
        /// </summary>
        private SchoolYearPlanBusiness dbschoolYearPlan;
        /// <summary>
        /// 获取所有的成功借资数据
        /// </summary>
        /// <returns></returns>
        public List<Debit> GetAll()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据员工编号获取这个员工的借资记录
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public List<Debit> GetDebitsByEmpID(string EmpID)
        {
            return this.GetAll().Where(a => a.EmpNumber == EmpID).ToList();
        }

        /// <summary>
        ///获取这个员工这年的借资次数
        /// </summary>
        /// <param name="EmpID">员工id</param>
        /// <param name="nowplan">年计划对象</param>
        /// <returns></returns>
        public List<Debit> GetDebitsByYear(string EmpID, SchoolYearPlan nowplan)
        {
            var data = this.GetDebitsByEmpID(EmpID);
            dbschoolYearPlan = new SchoolYearPlanBusiness();
            var nextplan= dbschoolYearPlan.GetNextPlan(nowplan);
            List<Debit> result = new List<Debit>();
            foreach (var item in data)
            {
                if (item.date>=nowplan.PlanDate)
                {
                    if (nextplan.ID==0)
                    {
                        result.Add(item);
                    }
                    else
                    {
                        if (item.date<nextplan.PlanDate)
                        {
                            result.Add(item);
                        }
                    }
                }
            }
            return result;
        }
    }
}
