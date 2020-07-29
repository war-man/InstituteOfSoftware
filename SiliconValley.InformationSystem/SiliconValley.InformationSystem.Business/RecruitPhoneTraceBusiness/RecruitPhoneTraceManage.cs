using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
namespace SiliconValley.InformationSystem.Business.RecruitPhoneTraceBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public  class RecruitPhoneTraceManage:BaseBusiness<RecruitPhoneTrace>
    {
        public RecruitPhoneTraceView GetRptView(int id) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            RecruitPhoneTraceView rptview = new RecruitPhoneTraceView();
            var item = this.GetEntity(id);
            var dept = empmanage.GetDeptByPid((int)item.Pid);
            rptview.Id = item.Id;
            rptview.Name = item.Name;
            rptview.PhoneNumber = item.PhoneNumber;
            rptview.TraceTime = item.TraceTime;
            rptview.Channel = item.Channel;
            rptview.ResumeType = item.ResumeType;
            rptview.PhoneCommunicateResult = item.PhoneCommunicateResult;
            rptview.IsEntry = item.IsEntry;
            rptview.Remark = item.Remark;
            rptview.IsDel = item.IsDel;
            rptview.Pid = item.Pid;
            rptview.Deptid = dept.DeptId;
            rptview.Pname = empmanage.GetPobjById((int)item.Pid).PositionName;
            rptview.Dname = dept.DeptName;
            rptview.ForwardDate = item.ForwardDate;
            return rptview;
        }

        /// <summary>
        /// 获取某条追踪数据的面试记录数据集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<RecruitPhoneTraceView> GetRptViewList(int id) {
            List<RecruitPhoneTraceView> rptviewlist = new List<RecruitPhoneTraceView>();
            var rpt = this.GetEntity(id);
            var rptdata = this.GetList().Where(r => r.SonId == rpt.SonId && r.IsDel == true).ToList();
            foreach (var item in rptdata)
            {
                var rptlist = GetRptView(item.Id);
                rptviewlist.Add(rptlist);
            }
            return rptviewlist;
        }
    }
}
