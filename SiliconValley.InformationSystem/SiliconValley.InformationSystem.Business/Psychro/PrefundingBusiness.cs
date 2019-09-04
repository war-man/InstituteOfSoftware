using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    public class PrefundingBusiness : BaseBusiness<Prefunding>
    {
        /// <summary>
        /// 学校年度计划
        /// </summary>
        private SchoolYearPlanBusiness dbschoolplan;
        /// <summary>
        /// 全部预资数据
        /// </summary>
        /// <returns></returns>
        public List<Prefunding> GetAll()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据预资id返回预资单对象
        /// </summary>
        /// <param name="PerfundingID"></param>
        /// <returns></returns>
        public Prefunding GetPrefundingByID(int PerfundingID)
        {
            return this.GetAll().Where(a => a.ID == PerfundingID).FirstOrDefault();
        }
        /// <summary>
        /// 根据员工编号返回他的所有借资单集合
        /// </summary>
        /// <param name="ChannelStaffID"></param>
        /// <returns></returns>
        public List<Prefunding> GetPrefundingByEmpNumber(string EmpNumber)
        {
            return this.GetAll().Where(a => a.EmpNumber == EmpNumber).ToList();
        }
        /// <summary>
        /// 查询该员工在该计划借资次数
        /// </summary>
        /// <param name="EmpNumber"></param>
        /// <param name="nowplan"></param>
        /// <returns></returns>
        public List<Prefunding> GetPrefundingsByYear(string EmpNumber, SchoolYearPlan nowplan)
        {
            var data = this.GetPrefundingByEmpNumber(EmpNumber);
            dbschoolplan = new SchoolYearPlanBusiness();
           var nextplan=  dbschoolplan.GetNextPlan(nowplan);
            List<Prefunding> result = new List<Prefunding>();
            foreach (var item in data)
            {
                if (item.PreDate>=nowplan.PlanDate)
                {
                    if (nextplan.ID==0)
                    {
                        result.Add(item);
                    }
                    else
                    {
                        if (item.PreDate<nextplan.PlanDate)
                        {
                            result.Add(item);
                        }
                    }
                    
                }
            }
            return result;


        }
        /// <summary>
        /// 添加预资单
        /// </summary>
        /// <param name="Prefunding"></param>
        public void AddPerfunding(Prefunding Prefunding)
        {
            this.Insert(Prefunding);
        }
    }
}
