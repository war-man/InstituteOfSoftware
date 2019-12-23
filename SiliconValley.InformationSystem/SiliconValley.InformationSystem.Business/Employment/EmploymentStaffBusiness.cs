using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业专员业务类
    /// </summary>
    public class EmploymentStaffBusiness : BaseBusiness<EmploymentStaff>
    {

        private EmpClassBusiness dbempClass;
        private EmpStaffAndStuBusiness dbempStaffAndStu;
        private StaffAccdationBusiness dbstaffAccdation;


        /// <summary>
        /// 获取没离职的就业专员列表
        /// </summary>
        /// <returns></returns>
        public List<EmploymentStaff> GetALl()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据就业专员id返回就业专员对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmploymentStaff GetEmploymentByID(int id)
        {
            return this.GetALl().Where(a => a.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// 根据员工编号查找i员工对象
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmployeesInfoByID(string EmployeeId)
        {
            var NomyEmp = new EmployeesInfoManage();
            var cc = NomyEmp.GetIQueryable().Where(a => a.EmployeeId == EmployeeId && a.IsDel == false).FirstOrDefault();
            return cc;
        }
        /// <summary>
        /// 根据员工编号查找i员工对象
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public EmploymentStaff GetEmploymentStaffByempid(string EmployeeId)
        {
            return this.GetALl().Where(a => a.EmployeesInfo_Id == EmployeeId).FirstOrDefault();
        }
        /// <summary>
        /// 用于详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployStaffDetailView GetStaffDetailView(int id)
        {
            //区域业务类
            EmploymentAreasBusiness areasBusiness = new EmploymentAreasBusiness();
            //就业专员业务类
            EmployStaffDetailView staffDetailView = new EmployStaffDetailView();
            //专员带班业务类
            EmpClassBusiness EmpClassDB = new EmpClassBusiness();
            //就业专员对象
            var empstaffinfo = this.GetEmploymentByID(id);
            //区域id
            var AreasID = empstaffinfo.AreaID;
            EmploymentAreas areas = new EmploymentAreas();
            //区域对象
            var seAreas = areasBusiness.GetObjByID(AreasID);
            areas.AreaName = seAreas.AreaName;
            //员工id
            var EmployeesInfo_Id = empstaffinfo.EmployeesInfo_Id;
            //拿员工对象
            EmployeesInfo employeesInfo = this.GetEmployeesInfoByID(EmployeesInfo_Id);
            //岗位id
            var posiid = employeesInfo.PositionId;
            //拿岗位对象 
            var PositionInfo = this.GetPositionByID(posiid);
            //部门id
            var DepID = PositionInfo.DeptId;
            //拿部门对象
            var DepInfo = this.GetDepartmentByID(DepID);
            //获取该专员带班所有记录
            var ClassList = EmpClassDB.GetEmpsByEmpID(id);
            //获取带班毕业班级
            var ClassedList = EmpClassDB.GetClassedList(ClassList);
            //获取带班没毕业班级
            var ClassingList = EmpClassDB.GetClassingList(ClassList);

            staffDetailView.emp = employeesInfo;
            staffDetailView.Position = PositionInfo;
            staffDetailView.Department = DepInfo;
            staffDetailView.Areas = areas;
            staffDetailView.ClassSchedulesed = ClassedList;
            staffDetailView.ClassSchedulesing = ClassingList;
            staffDetailView.AttendClassStyle = empstaffinfo.AttendClassStyle;
            staffDetailView.EmployExperience = empstaffinfo.EmployExperience;
            staffDetailView.EmployStaffID = empstaffinfo.ID;
            staffDetailView.WorkExperience = empstaffinfo.WorkExperience;
            staffDetailView.Remark = empstaffinfo.Remark;
            return staffDetailView;
        }


        /// <summary>
        /// 获取岗位全部数据
        /// </summary>
        /// <returns></returns>
        public List<Position> GetPositions()
        {
            BaseBusiness<Position> PositionDb = new BaseBusiness<Position>();
            return PositionDb.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据岗位自己的id获取岗位对象
        /// </summary>
        /// <returns></returns>
        public Position GetPositionByID(int posiid)
        {
            return this.GetPositions().Where(a => a.Pid == posiid).FirstOrDefault();
        }
        /// <summary>
        /// 获取所有的部门数据
        /// </summary>
        /// <returns></returns>
        public List<Department> GetDepartments()
        {
            BaseBusiness<Department> baseBusiness = new BaseBusiness<Department>();
            return baseBusiness.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据部门id获取部门对象
        /// </summary>
        /// <param name="DepID"></param>
        /// <returns></returns>
        public Department GetDepartmentByID(int DepID)
        {
            return this.GetDepartments().Where(a => a.DeptId == DepID).FirstOrDefault();
        }

        /// <summary>
        /// 根据区域id返回专员对对象
        /// </summary>
        /// <param name="AreasID"></param>
        /// <returns></returns>
        public EmploymentStaff GetEmploymentByAreasID(int AreasID)
        {
            return this.GetALl().Where(a => a.AreaID == AreasID).FirstOrDefault();
        }
        /// <summary>
        /// 根据就业专员id返回员工对象
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public EmployeesInfo GetEmpInfoByEmpID(int EmpID)
        {
            var empdata = this.GetEmploymentByID(EmpID);
            return this.GetEmployeesInfoByID(empdata.EmployeesInfo_Id);
        }
        /// <summary>
        /// 添加就业专员
        /// </summary>
        /// <param name="EmployNO"></param>
        /// <returns></returns>
        public bool AddEmploystaff(string EmployNO)
        {
            EmploymentStaff staff = new EmploymentStaff();
            staff.EmployeesInfo_Id = EmployNO;
            staff.Date = DateTime.Now;
            staff.IsDel = false;
            bool result = false;
            try
            {
                this.Insert(staff);
                result = true;
                BusHelper.WriteSysLog("当添加就业员工的时候，位于Employment文件夹中EmploymentStaffBusiness业务类中AddEmploystaff方法，添加成功。", EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                result = false;
                BusHelper.WriteSysLog("当添加就业员工的时候，位于Employment文件夹中EmploymentStaffBusiness业务类中AddEmploystaff方法，添加成功。", EnumType.LogType.添加数据);
            }
            return result;
        }
        /// <summary>
        /// 根据员工id获取就业专员对象
        /// </summary>
        /// <param name="EmpInfoID"></param>
        /// <returns></returns>
        public EmploymentStaff GetEmploymentByEmpInfoID(string EmpInfoID)
        {
            return this.GetALl().Where(a => a.EmployeesInfo_Id == EmpInfoID).FirstOrDefault();
        }
        /// <summary>
        /// 删除就业专员
        /// </summary>
        /// <param name="EmpInfoID"></param>
        /// <returns></returns>
        public bool DelEmploystaff(string EmpInfoID)
        {
            bool result = false;
            try
            {
                ///删除员工表
                dbempStaffAndStu = new EmpStaffAndStuBusiness();
                EmploymentStaff data = this.GetEmploymentByEmpInfoID(EmpInfoID);
                if (data == null)
                {
                    result = true;
                }
                else
                {
                    data.IsDel = true;
                    this.Update(data);
                    ///删除宿舍记住信息 调方法
                    dbstaffAccdation = new StaffAccdationBusiness();
                    dbstaffAccdation.DelStaffacc(EmpInfoID);
                    ///删除他现在在带的班级
                    dbempClass = new EmpClassBusiness();
                    dbempClass.delempforclass(data.ID);
                    ///删除现在学生记录
                    dbempStaffAndStu = new EmpStaffAndStuBusiness();
                    dbempStaffAndStu.delempstaffandstuByempid(data.ID);
                    result = true;
                    BusHelper.WriteSysLog("当就业员工离职的时候，对就业专员的isdel进行修改，位于Employment文件夹中EmploymentStaffBusiness业务类中DelEmploystaff方法，编辑成功。", EnumType.LogType.编辑数据);
                }

            }
            catch (Exception ex)
            {

                result = false;
                BusHelper.WriteSysLog("当就业员工离职的时候，对就业专员的isdel进行修改，位于Employment文件夹中EmploymentStaffBusiness业务类中DelEmploystaff方法，编辑失败。", EnumType.LogType.编辑数据);
            }
            return result;
        }
        /// <summary>
        /// 获取当前登陆的就业员工
        /// </summary>
        /// <returns></returns>
        public EmploymentStaff Getloginuser()
        {
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            return this.GetEmploymentByEmpInfoID(user.EmpNumber);
        }
        /// <summary>
        /// 根据班级id返回带班老师的员工编号
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public EmploymentStaff GetStaffByclassid(int classid)
        {
            dbempClass = new EmpClassBusiness();
            EmpClass empClass = dbempClass.GetEmpClassByclassid(classid);
            if (empClass == null)
            {
                return null;
            }
            else
            {
                return this.GetEntity(empClass.EmpStaffID);
            }
        }


        /// <summary>
        ///根据传入过来得值获取这年或者是这季度得参与过得员工
        /// </summary>
        /// <param name="isyear"></param>
        /// <param name="param0"></param>
        /// <returns></returns>
        public List<EmploymentStaff> GetStaffsByQuarter(bool isyear, int param0)
        {
            return null;
        }
    }
}
