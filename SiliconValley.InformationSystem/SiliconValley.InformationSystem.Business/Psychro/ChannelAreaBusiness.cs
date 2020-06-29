using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
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
        /// 员工
        /// </summary>
        private EmployeesInfoManage dbemp;

        /// <summary>
        /// 渠道员工
        /// </summary>
        private ChannelStaffBusiness dbchannel;

        /// <summary>
        ///年度学校计划
        /// </summary>
        private SchoolYearPlanBusiness dbpaln;

        private SchoolYearPlanBusiness dbplan;

        /// <summary>
        /// 获取全部的没有伪删除的数据
        /// </summary>
        /// <returns></returns>
        public List<ChannelArea> GetChannelAreas()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据年度计划来找到这个该年度的员工管理区域
        /// </summary>
        /// <param name="nowschoolplan"></param>
        /// <param name="dbschoolpaln"></param>
        /// <returns></returns>
        public List<ChannelArea> GetAreasByPlan(SchoolYearPlan nowschoolplan, SchoolYearPlanBusiness dbschoolpaln)
        {
            var nextdata = dbschoolpaln.GetNextPlan(nowschoolplan);
            List<ChannelArea> resultlist = new List<ChannelArea>();
            List<ChannelArea> all = this.GetIQueryable().ToList();
            foreach (var item in all)
            {
                //员工区域停止使用
                if (item.IsDel)
                {
                    if (item.StaffAreaDate<nowschoolplan.PlanDate)
                    {
                        if (item.StopDate > nowschoolplan.PlanDate)
                        {
                            resultlist.Add(item);
                        }
                    }
                    else
                    {
                        if (nextdata.ID!=0)
                        {
                            if (item.StaffAreaDate < nextdata.PlanDate)
                            {
                                resultlist.Add(item);
                            }
                        }
                        else
                        {
                            resultlist.Add(item);
                        }
                    }
                }
                //员工区域仍在使用
                else
                {
                    if (item.StaffAreaDate < nowschoolplan.PlanDate)
                    {
                        resultlist.Add(item);

                    }
                    else
                    {
                        if (nextdata.ID != 0)
                        {
                            if (item.StaffAreaDate < nextdata.PlanDate)
                            {
                                resultlist.Add(item);
                            }
                        }
                        else
                        {
                            resultlist.Add(item);
                        }
                    }
                }
            }
            return resultlist;
        }
        /// <summary>
        /// 根据渠道员工id获取员工现在负责区域
        /// </summary>
        /// <param name="ChannelStaffID"></param>
        /// <returns></returns>
        public List<ChannelArea> GetAreasingByChannelID(int ChannelStaffID)
        {
            return this.GetChannelAreas().Where(a => a.ChannelStaffID == ChannelStaffID).ToList();
        }

        /// <summary>
        /// 根据渠道员工id获取员工负责区域的全部
        /// </summary>
        /// <param name="ChannelStaffID"></param>
        /// <returns></returns>
        public List<ChannelArea> GetAreasedByChannelID(int ChannelStaffID)
        {
            return this.GetIQueryable().Where(a => a.ChannelStaffID == ChannelStaffID).ToList();
        }
        /// <summary>
        /// 根据一个员工id返回当前所在的区域
        /// </summary>
        /// <param name="channelStaffID"></param>
        /// <returns></returns>
        public List<ChannelArea> GetAreaByChannelID(int channelStaffID)         
        {
            return this.GetChannelAreas().Where(a => a.ChannelStaffID == channelStaffID).ToList();
        }

        /// <summary>
        /// 根据渠道员工id跟要查询的年度计划，如果这个员工区域已经没在负责了，就判断他的停止时间是什么时候，如果是下个计划的开始时间停用的就添加到现在的这个年度中
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="nowplan"></param>
        /// <returns></returns>
        public List<ChannelArea> GetAreaByPaln(int ChannelID, SchoolYearPlan nowschoolplan)
        {
            //以前跟现在的记录
            var channelarealist = this.GetAreasedByChannelID(ChannelID);
            dbpaln = new SchoolYearPlanBusiness();
            var nextdata = dbpaln.GetNextPlan(nowschoolplan);
            List<ChannelArea> resultlist = new List<ChannelArea>();
            foreach (var item in channelarealist)
            {
                //还在持续用
                if (!item.IsDel)
                {
                    //区域分配时间在制定计划之后
                    if (nowschoolplan.PlanDate < item.StaffAreaDate)
                    {
                        //有下一个计划
                        if (nextdata.ID != 0)
                        {
                            if (item.StaffAreaDate < nextdata.PlanDate)
                            {
                                resultlist.Add(item);
                            }
                        }
                        //没有计划
                        else
                        {
                            resultlist.Add(item);
                        }
                    }
                    //区域分配时间在制定计划之前
                    else
                    {
                        resultlist.Add(item);
                    }
                }
                //停用
                else
                {
                    if (nextdata.ID != 0)
                    {
                        if (item.StopDate > nextdata.PlanDate)
                        {
                            resultlist.Add(item);
                        }
                    }
                }
            }
            return resultlist;
        }

        /// <summary>
        /// 根据主任的员工编号获取他当年带的团队
        /// </summary>
        /// <returns></returns>
        public List<ChannelStaff> GetTeamByEmpID(string EmpID, SchoolYearPlan nowschoolplan, List<ChannelStaff> data)
        {
            dbchannel = new ChannelStaffBusiness();
            var channel = dbchannel.GetChannelByEmpID(EmpID);
            var listarea = new List<ChannelArea>();
            var resultlist = new List<ChannelStaff>();

            var fuzhuren = new List<ChannelArea>();
            var channelarealist = new List<ChannelArea>();
            resultlist.Add(channel);

            foreach (var item in data)
            {
                var mydata = this.GetAreaByPaln(item.ID, nowschoolplan);
                listarea.AddRange(mydata);
            }
            //副主任
            for (int i = 0; i < listarea.Count; i++)
            {
                if (listarea[i].RegionalDirectorID == channel.ID)
                {
                    fuzhuren.Add(listarea[i]);
                }
            }
            //员工
            for (int i = 0; i < listarea.Count; i++)
            {
                for (int k = 0; k < fuzhuren.Count; k++)
                {
                    if (listarea[i].RegionalDirectorID == fuzhuren[k].ChannelStaffID)
                    {
                        channelarealist.Add(listarea[i]);
                    }
                }
            }

            //添加到集合中
            for (int i = 0; i < fuzhuren.Count; i++)
            {
                var setfuzhuren = dbchannel.GetChannelByID(fuzhuren[i].ChannelStaffID);
                resultlist.Add(setfuzhuren);
                for (int k = 0; k < channelarealist.Count; k++)
                {
                    if (fuzhuren[i].ChannelStaffID == channelarealist[k].RegionalDirectorID)
                    {
                        var forchannelstaff = dbchannel.GetChannelByID(channelarealist[k].ChannelStaffID);
                        resultlist.Add(forchannelstaff);
                    }
                }
            }

            //去除重复的渠道员工
            for (int i = 0; i < resultlist.Count; i++) 
            {
                for (int j = resultlist.Count - 1; j > i; j--)
                {

                    if (resultlist[i].EmployeesInfomation_Id == resultlist[j].EmployeesInfomation_Id)
                    {
                        resultlist.RemoveAt(j);
                    }

                }
            }
            return resultlist;


        }


        /// <summary>
        /// 获取上级
        /// </summary>
        /// <param name="channelAreas"></param>
        /// <param name="channelArea"></param>
        /// <returns></returns>
        public ChannelArea forquerychannelarea(List<ChannelArea> channelAreas, ChannelArea channelArea)
        {
            ChannelArea result = new ChannelArea();
            foreach (var items in channelAreas)
            {
                if (items.ChannelStaffID == channelArea.RegionalDirectorID)
                {
                    result= items;
                }
            }
            return result;

        }

        /// <summary>
        /// 获取区域人员
        /// </summary>
        /// <param name="areaId">区域编号</param>
        /// <returns></returns>
        public List<EmployeesInfo> GetAreaEmplist(int areaId)
        {
            var list =  this.GetChannelAreas().Where(d => d.RegionID == areaId).ToList();

            List<EmployeesInfo> resultlist = new List<EmployeesInfo>();

            //list.ForEach(d=>
            //{
            //    var chanelObj = dbchannel.GetChannelByID(d.ChannelStaffID);

            //    if (chanelObj != null)
            //    {
            //        var emp = dbemp.GetInfoByEmpID(chanelObj.EmployeesInfomation_Id);

            //        if (emp != null)
            //        {
            //            resultlist.Add(emp);
            //        }
            //    }
            //});

            foreach (var item in list)
            {
                var chanelObj = dbchannel.GetChannelByID(item.ChannelStaffID);
                if (chanelObj != null)
                {
                    var emp = dbemp.GetInfoByEmpID(chanelObj.EmployeesInfomation_Id);

                    if (emp != null)
                    {
                        resultlist.Add(emp);
                    }
                }
            }

            return resultlist;
        }
    }
}
