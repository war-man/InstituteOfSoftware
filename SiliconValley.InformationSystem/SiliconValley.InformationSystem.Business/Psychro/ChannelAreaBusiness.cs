using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 渠道专员区域分布
    /// </summary>
    public class ChannelAreaBusiness : BaseBusiness<ChannelArea>
    {

        /// <summary>
        ///年度学校计划
        /// </summary>
        private SchoolYearPlanBusiness dbpaln;
        /// <summary>
        /// 根据区域id获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChannelArea GetAreaByID(int id)
        {
            return this.GetIQueryable().Where(a => a.ID == id && a.IsDel == false).FirstOrDefault();
        }
        /// <summary>
        /// 根据渠道员工id获取员工区域
        /// </summary>
        /// <param name="ChannelStaffID"></param>
        /// <returns></returns>
        public List<ChannelArea> GetAreaByChannelID(int ChannelStaffID)
        {
            return this.GetIQueryable().Where(a => a.ChannelStaffID == ChannelStaffID && a.IsDel == false).ToList();
        }
        /// <summary>
        /// 获取全部的没有伪删除的数据
        /// </summary>
        /// <returns></returns>
        public List<ChannelArea> GetChannelAreas()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据渠道员工id跟要查询的年度计划，获取该员工最后负责的区域
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="nowplan"></param>
        /// <returns></returns>
        public ChannelArea GetAreaByPaln(int ChannelID, SchoolYearPlan nowplan)
        {
            var channelarealist = this.GetAreaByChannelID(ChannelID);
            dbpaln = new SchoolYearPlanBusiness();
            var nextplan = dbpaln.GetNextPlan(nowplan);
            List<ChannelArea> fuzeArea = new List<ChannelArea>();
            dbpaln.GetNextPlan(nowplan);
            foreach (var item in channelarealist)
            {
                //老员工从以前就一直负责下来的。
                if (item.StaffAreaDate <= nowplan.PlanDate)
                {
                    fuzeArea.Add(item);
                }
                //老员工换区域了或者是新员工刚刚分配
                else
                {
                    //没有下一个i计划，这就是正在时
                    if (nextplan.ID == 0)
                    {
                        fuzeArea.Add(item);
                    }
                    else
                    {
                        if (item.StaffAreaDate <= nextplan.PlanDate)
                        {
                            fuzeArea.Add(item);
                        }
                    }
                }
            }

            //上面的循环找到了这个员工在一计划年内的区域异动列表

            //现在我们找最接近下一个计划的最近的一次记录
            var areaing = fuzeArea.OrderByDescending(a => a.StaffAreaDate).FirstOrDefault();
            return areaing;
        }
    }
}
