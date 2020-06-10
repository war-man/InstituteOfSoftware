using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.ViewEntity;
namespace SiliconValley.InformationSystem.Business.NetClientRecordBusiness
{
    using SiliconValley.InformationSystem.Business.Channel;
    using SiliconValley.InformationSystem.Business.Common;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public  class NetClientRecordManage:BaseBusiness<NetClientRecord>
    {
        /// <summary>
        /// 通过备案id添加网咨跟踪信息
        /// </summary>
        /// <returns></returns>
        public bool AddNCRData(int id) {
            NetClientRecord ncr = new NetClientRecord();
            StudentDataKeepAndRecordBusiness sdkmanage = new StudentDataKeepAndRecordBusiness();
            bool result = false;
            try
            {
                ncr.SPRId = id;
                ncr.EmpId = sdkmanage.GetEntity(id).EmployeesInfo_Id;//跟踪回访人首先默认为备案人，有不同则跟踪信息表修改
                ncr.IsDel = false;
                this.Insert(ncr);
                result = true;
                BusHelper.WriteSysLog("网咨学生回访信息添加成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;
        }
        /// <summary>
        /// 通过备案数据集添加网咨跟踪信息
        /// </summary>
        /// <param name="spridlist"></param>
        /// <returns></returns>
        public bool AddNCRData(List<StudentPutOnRecord> spridlist) {
            bool result = false;
            try
            {
                foreach (var item in spridlist)
                {
                    NetClientRecord ncr = new NetClientRecord();
                    ncr.SPRId = item.Id;
                    ncr.EmpId = item.EmployeesInfo_Id;//跟踪回访人首先默认为备案人，有不同则跟踪信息表修改
                    ncr.IsDel = false;
                    this.Insert(ncr);
                    result = true;
                    BusHelper.WriteSysLog("网咨学生回访信息添加成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;
        }
        /// <summary>
        /// 通过网咨回访编号返回回访视图对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NetClientRecordView GetNcrviewById(int id) {
            StudentDataKeepAndRecordBusiness sdkrmanage = new StudentDataKeepAndRecordBusiness();
            EmployeesInfoManage empmanege = new EmployeesInfoManage();
            ChannelStaffBusiness channel = new ChannelStaffBusiness();
            NetClientRecordView ncr = new NetClientRecordView();
            var item = this.GetEntity(id);
            #region 赋值
            var sdk = sdkrmanage.findId(Convert.ToString(item.SPRId));//获取对应的备案数据对象
            var channelemp = item.MarketTeaId==null?null: empmanege.GetInfoByEmpID(channel.GetChannelByID(item.MarketTeaId).EmployeesInfomation_Id);//获取渠道员工信息
            var emp = empmanege.GetInfoByEmpID(item.EmpId);//获取跟踪回访员工
            ncr.Id = item.Id;
            ncr.SPRId = item.SPRId;
            ncr.EmpId = item.EmpId;
            ncr.Channelemp = channelemp==null?null: channelemp.EmpName;
            ncr.Empname = emp.EmpName;
            ncr.StuName = sdk.StuName;
            ncr.StuSex = sdk.StuSex;
            ncr.StuPhone = sdk.Stuphone;
            ncr.StuQQ = sdk.StuQQ;
            ncr.StuWeiXin = sdk.StuWeiXin;
            ncr.StuEducational = sdk.StuEducational;
            ncr.IsFaceConsult = sdk.StuisGoto == false ? "否" : "是";
            ncr.StuStatus = sdk.StatusName;
            ncr.RegionName = sdk.RegionName;
            ncr.consultemp = sdk.ConsultTeacher;
            ncr.SprEmp = sdk.empName;
            ncr.NetClientDate = item.NetClientDate;
            ncr.CallBackCase = item.CallBackCase;
            ncr.IsDel = item.IsDel;
            ncr.Grade = item.Grade;
            #endregion
            return ncr;
        }
    }
}
