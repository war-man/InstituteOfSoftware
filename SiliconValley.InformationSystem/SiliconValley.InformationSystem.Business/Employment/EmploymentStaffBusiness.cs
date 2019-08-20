using SiliconValley.InformationSystem.Business.EmployeesBusiness;
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


        /// <summary>
        /// 获取没离职的就业专员列表
        /// </summary>
        /// <returns></returns>
        public List<EmploymentStaff> GetALl()
        {
            var data = this.GetIQueryable().ToList();
            var newdata = new List<EmploymentStaff>();
            foreach (var item in data)
            {
                if (!IsDel(item.EmployeesInfo_Id))
                {
                    newdata.Add(item);
                }
            }
            return newdata;
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
        /// 判断是否员工是否离职
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public bool IsDel(string EmployeeId)
        {

            var EmpInfo = GetEmployeesInfoByID(EmployeeId);
            if (EmpInfo.IsDel == false)
            {
                return false;
            }
            else
            {
                return true;
            }

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
        /// 返回属于就业岗位的员工
        /// </summary>
        public List<EmployeesInfo> EmployeesInfos()
        {
            BaseBusiness<Department> DepbaseBusiness = new BaseBusiness<Department>();
            BaseBusiness<Position> PositionBusiness = new BaseBusiness<Position>();
            BaseBusiness<EmployeesInfo> EmployeesInfoBusiness = new BaseBusiness<EmployeesInfo>();
            var DepList = DepbaseBusiness.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var PositionList = PositionBusiness.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var EmployStaffList = this.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var EmployInfoList = EmployeesInfoBusiness.GetIQueryable().Where(a => a.IsDel == false).ToList();

            //得到就业岗位的员工
            var ResultList = new List<EmployeesInfo>();
            var NewResultList = new List<EmployeesInfo>();


            foreach (var item in EmployInfoList)
            {
                var PositionObj = PositionList.Where(a => a.Pid == item.PositionId).First();
                if (PositionObj.DeptId == 4)
                {
                    ResultList.Add(item);
                    NewResultList.Add(item);
                }
            }

            foreach (var item in EmployStaffList)
            {
                foreach (var result in ResultList)
                {
                    if (item.EmployeesInfo_Id == result.EmployeeId)
                    {
                        NewResultList.Remove(result);
                    }
                }
            }
            return NewResultList;
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
        /// 添加就业专员
        /// </summary>
        /// <param name="EmployNO">员工编号</param>
        /// <returns></returns>
        public bool AddEmploystaff(string EmployNO) {
            EmploymentStaff staff = new EmploymentStaff();
            staff.EmployeesInfo_Id = EmployNO;
            try
            {
                this.Insert(staff);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
