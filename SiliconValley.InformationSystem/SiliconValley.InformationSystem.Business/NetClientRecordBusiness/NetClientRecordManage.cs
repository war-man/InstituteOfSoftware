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
               /* ncr.EmpId = sdkmanage.GetEntity(id).EmployeesInfo_Id;*///跟踪回访人首先默认为备案人，有不同则跟踪信息表修改
                ncr.MarketTeaId = null;
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
                   /* ncr.EmpId = item.EmployeesInfo_Id;*///跟踪回访人首先默认为备案人，有不同则跟踪信息表修改
                    ncr.MarketTeaId = null;
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
            ncr.Channelemp = channelemp==null ? null: channelemp.EmpName;
            ncr.Empname = emp==null?null: emp.EmpName;
            ncr.StuName = sdk.StuName;//姓名
            ncr.StuSex = sdk.StuSex;//性别
            ncr.StuPhone = sdk.Stuphone;//电话号码
            ncr.StuBirthy = sdk.StuBirthy;//出生日期
            ncr.StuSchoolName = sdk.StuSchoolName;//毕业学校
            ncr.StuEducational = sdk.StuEducational;//学历
            ncr.StuAddress = sdk.StuAddress;
            ncr.StuQQ = sdk.StuQQ;//QQ
            ncr.StuWeiXin = sdk.StuWeiXin;//微信
            ncr.IsFaceConsult = sdk.StuisGoto == false ? "否" : "是";//是否面咨
            ncr.StuVisit = sdk.StuVisit;//面咨时间
            ncr.StuStatus = sdk.StatusName;//是否报名
            ncr.SprEmp = sdk.empName;//备案人
            ncr.BeanDate = sdk.BeanDate;//备案时间
            ncr.StatusTime = sdk.StatusTime;//报名时间
            ncr.RegionName = sdk.RegionName;//所在区域
            ncr.consultemp = sdk.ConsultTeacher;//咨询师（咨询部的）
            ncr.Reak = sdk.Reak;//其他说明

            ncr.NetClientDate = item.NetClientDate;//网咨回访时间
            ncr.CallBackCase = item.CallBackCase;//回访情况
            ncr.IsDel = item.IsDel;
            ncr.Grade = item.Grade;//学生等级
            #endregion
            return ncr;
        }

        /// <summary>
        /// 通过编号获取该备案学生的回访记录集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<NetClientRecordView> GetNcrviewlist(int id) {
            NetClientRecordManage ncr = new NetClientRecordManage();
            List<NetClientRecordView> ncrviewlist = new List<NetClientRecordView>();
            var n = ncr.GetEntity(id);
            var trackdata = ncr.GetList().Where(s => s.SPRId == n.SPRId && s.NetClientDate != null).ToList();//获取跟踪的数据（没有跟踪时间的是初始数据即不属于跟踪数据）
            foreach (var item in trackdata)
            {
                var ncrview = ncr.GetNcrviewById(item.Id);
                ncrviewlist.Add(ncrview);
            }
            return ncrviewlist;
        }

       /// <summary>
       /// 判断某备案学生是否是网络部备案备过的，且有跟踪数据
       /// </summary>
       /// <param name="sprid"></param>
       /// <returns></returns>
        public bool IsExsitSprStu(int sprid) {
            bool result = false;
            var ncrlist = this.GetList().Where(s => s.SPRId == sprid).ToList();
            if (ncrlist.Count > 0)
            {
                result = true;//表示该学生已被网络部备案过
            }
            else {
                result = false;
            }
            return result;
        }
    }
}
