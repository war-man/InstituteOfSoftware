using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Coursewaremaking_Business
{
    /// <summary>
    /// 课件制作业务类
    /// </summary>
   public class CoursewaremakingBusiness: BaseBusiness<Coursewaremaking>
    {
        //班主任业务类
        HeadmasterBusiness headmasterBusiness = new HeadmasterBusiness();
        //专业课老师
        TeacherBusiness teacherBusiness = new TeacherBusiness();
        //员工业务类
        EmployeesInfoManage employeesInfoManage = new EmployeesInfoManage();
        /// <summary>
        /// 课程制作上传
        /// </summary>
        /// <param name="coursewaremaking"></param>
        /// <returns></returns>
        public bool AddCoursewaremaking(Coursewaremaking coursewaremaking)
        {
            bool bit = false;
            try
            {
              int count=  this.GetList().Where(a => a.RampDpersonID == coursewaremaking.RampDpersonID && a.Filename == coursewaremaking.Filename).ToList().Count();
                if (count<1)
                {
                    coursewaremaking.Submissiontime = DateTime.Now;
                    this.Insert(coursewaremaking);
                    bit = true;
                    BusHelper.WriteSysLog("课程上传成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
                
            } 
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
              
            }
            return bit;
            
        }
        /// <summary>
        /// 删除课件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveCoursewaremaking(int id)
        {
            bool bit = false;
            try
            {
                this.Delete(this.GetEntity(id));
                bit = true;
                BusHelper.WriteSysLog("课程删除成功", Entity.Base_SysManage.EnumType.LogType.删除数据);
            }
            catch (Exception ex)
            {

                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.删除数据);
            }
            return bit;

           
        }
        /// <summary>
        /// 课件研发人数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="EmployeeId"员工编号></param>
        /// <param name="EmpName">员工姓名</param>
        /// <param name="department">部门</param>
        /// <returns></returns>
        public object TraineeDate(int page, int limit, string EmployeeId, string EmpName, string department)
        {
            //总共需要的员工数据
            List<TraineeDateVIew> listteainees = new List<TraineeDateVIew>();
            //专业老师数据
            var teaclist= teacherBusiness.GetList().Where(a => a.IsDel == false).ToList();
            foreach (var item in teaclist)
            {
                TraineeDateVIew traineeDateVIew = new TraineeDateVIew();
                traineeDateVIew.id = item.TeacherID;
                traineeDateVIew.EmployeeId = item.EmployeeId;
                //拿到员工对象
                var employee = employeesInfoManage.GetList().Where(a => a.EmployeeId == item.EmployeeId).FirstOrDefault();
                traineeDateVIew.EmpName = employee.EmpName;
                traineeDateVIew.sex = employee.Sex;
                traineeDateVIew.department = employeesInfoManage.GetDeptByEmpid(employee.EmployeeId).DeptName;
                traineeDateVIew.departmentID = employeesInfoManage.GetDeptByEmpid(employee.EmployeeId).DeptId;
                listteainees.Add(traineeDateVIew);
            }
            //班主任数据
            var headmasterLiat=headmasterBusiness.GetList().Where(a => a.IsDelete == false).ToList();
            foreach (var item in headmasterLiat)
            {
                TraineeDateVIew traineeDateVIew = new TraineeDateVIew();
                traineeDateVIew.id = item.ID;
                traineeDateVIew.EmployeeId = item.informatiees_Id;
                //拿到员工对象
                var employee = employeesInfoManage.GetList().Where(a => a.EmployeeId == item.informatiees_Id).FirstOrDefault();
                traineeDateVIew.EmpName = employee.EmpName;
                traineeDateVIew.sex = employee.Sex;
                traineeDateVIew.department = employeesInfoManage.GetDeptByEmpid(employee.EmployeeId).DeptName;
                traineeDateVIew.departmentID = employeesInfoManage.GetDeptByEmpid(employee.EmployeeId).DeptId;
                listteainees.Add(traineeDateVIew);
            }
            if (!string.IsNullOrEmpty(EmployeeId))
            {
                listteainees = listteainees.Where(a => a.EmployeeId.Contains(EmployeeId)).ToList();
            }
            if (!string.IsNullOrEmpty(EmpName))
            {
                listteainees = listteainees.Where(a => a.EmpName.Contains(EmpName)).ToList();
            }
            if (!string.IsNullOrEmpty(department))
            {
                int departmentID = int.Parse(department);
                listteainees = listteainees.Where(a => a.departmentID == departmentID).ToList();
            }
            var dataList = listteainees.OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit).ToList();
            var data = new
            {
                code = "",
                msg = "",
                count = listteainees.Count,
                data = dataList
            };
            return data;
        }

        /// <summary>
        /// 获取类型数据
        /// </summary>
        /// <returns></returns>
        public List<string> MakingType()
        {
            return new List<string>() { "PPT", "Word" };

        }

        //专业
        SpecialtyBusiness Techarcontext = new SpecialtyBusiness();
        //阶段
        GrandBusiness Grandcontext = new GrandBusiness();
        /// <summary>
        /// 根据文件名获取文件基础信息
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public object FineCoursewaremaking(string FileName)
        {
         var fine= this.GetList().Where(a => a.Filename == FileName).FirstOrDefault();
            var x = new
            {
               Emp= employeesInfoManage.GetEntity(fine.RampDpersonID),
               fine.MakingType,
               fine.Remarks,
              Grandcontext.GetEntity(fine.StageID).GrandName,
                SpecialtyName=fine.MajorID==null?"": Techarcontext.GetEntity( fine.MajorID).SpecialtyName,
                fine.Title,
                fine.Filepath,
                fine.Filename,
                fine.Submissiontime,
                department=employeesInfoManage.GetDeptByEmpid(fine.RampDpersonID).DeptName
            };
            return x;
        }
        /// <summary>
        /// 获取课件上传数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="EmpName">名字</param>
        /// <param name="DeptId">部门</param>
        /// <param name="MajorID">专业</param>
        /// <param name="StageID">阶段</param>
        /// <param name="MakingType">类型</param>
        /// <param name="Filename">文件名</param>
        /// <returns></returns>
        public object CourDate(int page, int limit,string EmpName,string DeptId, string MajorID,string StageID,string MakingType,string Filename)
        {
         
           
          var Couda =  this.GetList().Select(a => new
            {
                a.id,
                EmpName= employeesInfoManage.GetEntity(a.RampDpersonID).EmpName,
                employeesInfoManage.GetEntity(a.RampDpersonID).EmployeeId,
                DeptName = employeesInfoManage.GetDeptByEmpid(a.RampDpersonID).DeptName,
              employeesInfoManage.GetDeptByEmpid(a.RampDpersonID).DeptId,
                SpecialtyName = a.MajorID == null ? "暂无专业" : Techarcontext.GetEntity(a.MajorID).SpecialtyName,
                a.MajorID,
                a.StageID,
                Grandcontext.GetEntity(a.StageID).GrandName,
                a.MakingType,
                a.Title,
                a.Filename,
                a.Filepath,
                a.Submissiontime,
                a.Chaptersnumber
          });
            if (!string.IsNullOrEmpty(EmpName))
            {
                Couda= Couda.Where(a => a.EmpName.Contains(EmpName)).ToList();
            }
            if (!string.IsNullOrEmpty(DeptId))
            {
                int DeptIds = int.Parse(DeptId);
                Couda = Couda.Where(a => a.DeptId== DeptIds).ToList();
            }
            if (!string.IsNullOrEmpty(MajorID))
            {
                int MajorIDs = int.Parse(MajorID);
                Couda = Couda.Where(a => a.MajorID==MajorIDs).ToList();
            }
            if (!string.IsNullOrEmpty(StageID))
            {
                int StageIDs = int.Parse(StageID);
                Couda = Couda.Where(a => a.StageID== StageIDs).ToList();
            }
            if (!string.IsNullOrEmpty(MakingType))
            {
                Couda = Couda.Where(a => a.MakingType==MakingType).ToList();
            }
            if (!string.IsNullOrEmpty(MakingType))
            {
                Couda = Couda.Where(a => a.MakingType == MakingType).ToList();
            }
            if (!string.IsNullOrEmpty(Filename))
            {
                Couda = Couda.Where(a => a.Filename.Contains(Filename)).ToList();
            }
            var Myx = Couda.OrderBy(a => a.id).Skip((page - 1) * limit).Take(limit).ToList();
            var data = new
            {
                code = "",
                msg = "",
                count = Couda.Count(),
                data = Myx
            };
            return data;
        }
    }
}
