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
            return rptview;
        }
    }
}
