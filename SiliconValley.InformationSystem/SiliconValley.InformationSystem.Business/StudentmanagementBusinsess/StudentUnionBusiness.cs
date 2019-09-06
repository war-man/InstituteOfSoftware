using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.StudentmanagementBusinsess
{
    //学生会
  public class StudentUnionBusiness:BaseBusiness<StudentUnionMembers>
    {
        //学员信息
        BaseBusiness<StudentInformation> Student = new BaseBusiness<StudentInformation>();
        //班级学员
        BaseBusiness<ScheduleForTrainees> ClassSche = new BaseBusiness<ScheduleForTrainees>();
        //学生会部门
        BaseBusiness<StudentUnionDepartment> UnionDepart = new BaseBusiness<StudentUnionDepartment>();
        //学生会职位
        BaseBusiness<StudentUnionPosition> SUnionPosition = new BaseBusiness<StudentUnionPosition>();
        //班级学员表 
        ScheduleForTraineesBusiness ClassStudent = new ScheduleForTraineesBusiness();
        /// <summary>
        /// 获取学生会所有成员
        /// </summary>
        /// <param name="Name">学生会部门名称</param>
        /// <returns></returns>
        public object UnionMembersList(string Name,int page,int limit)
        {
          int UnId=  UnionDepart.GetList().Where(a => a.Dateofregistration == false && a.Departmentname == Name).FirstOrDefault().ID;
         var list=this.GetList().Where(a => a.Dateofregistration == false && a.department == UnId).OrderByDescending(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();
         var dataList =  list.Select(
                aa => new
                {
                    aa.Studentnumber,//学号
                    StidentName = Student.GetList().Where(a => a.IsDelete == false && a.State == null && a.StudentNumber == aa.Studentnumber).First().Name,
                    StudentSex = Student.GetList().Where(a => a.IsDelete == false && a.State == null && a.StudentNumber == aa.Studentnumber).First().Sex,
                    ClassName = ClassSche.GetList().Where(a => a.CurrentClass == true && a.StudentID == aa.Studentnumber).First().ClassID,
                    Department= UnionDepart.GetList().Where(a=>a.Dateofregistration==false&&a.ID==aa.department).First().Departmentname,
                    Position= SUnionPosition.GetList().Where(a=>a.Dateofregistration==false&&a.ID==aa.position).First().Jobtitle,
                    aa.Inrtiationtime
                });
            var data = new
            {
                code = "",
                msg = "",
                count = list.Count,
                data = dataList
            };
            return data;
        }
        /// <summary>
        /// 添加或者编辑 当有ID值大于零是就编辑
        /// </summary>
        /// <param name="studentUnionMembers">学生会成员对象</param>
        /// <returns></returns>
        public AjaxResult UnionMembersEntity(StudentUnionMembers studentUnionMembers,string Stuid)
        {
            AjaxResult retus = null;
            
            try
            {
                retus = new SuccessResult();

                retus.Success = true;
                if (studentUnionMembers.ID > 0)
                {
                    retus.Msg = "修改成功";
                    BusHelper.WriteSysLog("编辑数据", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                }
                else
                {
                    Stuid= Stuid.Substring(0, Stuid.Length - 1);
                    string[] Studentid = Stuid.Split(',');
                    List<StudentUnionMembers> list = new List<StudentUnionMembers>();
                    foreach (var item in Studentid)
                    {
                        StudentUnionMembers student = new StudentUnionMembers();
                        student.Inrtiationtime = DateTime.Now;
                        student.Dateofregistration = false;
                        student.department = studentUnionMembers.department;
                        student.Remarks = studentUnionMembers.Remarks;
                        student.position = studentUnionMembers.position;
                        student.Studentnumber = item;
                        list.Add(student);
                    }
                    retus.Msg = "添加成功";
                    
                    this.Insert(list);
                    BusHelper.WriteSysLog("数据添加", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";

                retus.Success = false;
                retus.ErrorCode = 500;
                if (studentUnionMembers.ID > 0)
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                else
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                throw;
            }
            return retus;


        }
        /// <summary>
        /// 根据班级号查询是否有未进学生会的人员
        /// </summary>
        /// <param name="ClassName">班级号</param>
        /// <returns></returns>
        public List<StudentInformation> StudentList(string ClassName)
        {
         
            //查询班级所有学生
            StudentInformationBusiness student = new StudentInformationBusiness();
            var mylist= this.GetList().Where(a => a.Dateofregistration == false && a.Departuretime == null).ToList();
            List<StudentInformation> list = new List<StudentInformation>();
         
            if (!string.IsNullOrEmpty(ClassName))
            {
                //CurrentClass是否为正常状态 查询使用以参数为班级的学员学号
                var stuid = ClassStudent.GetList().Where(a => a.ClassID == ClassName && a.CurrentClass == true).ToList();
                foreach (var item in stuid)
                {
                    var studentobj = student.GetList().Where(d => d.IsDelete == false && d.StudentNumber == item.StudentID).FirstOrDefault();

                    if (studentobj != null)
                        list.Add(studentobj);
                }
                foreach (var item in mylist)
                {
                    list = list.Where(d => d.StudentNumber != item.Studentnumber).ToList();

                }
            }
            return list;

        }
        /// <summary>
        /// 查询部门名称是否重复。重复则大于0
        /// </summary>
        /// <param name="Name">部门名称</param>
        /// <returns></returns>
        public int SelectDepa(string Name)
        {
           return UnionDepart.GetList().Where(a => a.Dateofregistration == false && a.Departmentname == Name).ToList().Count();
        }
        /// <summary>
        /// 添加学生会部门
        /// </summary>
        /// <param name="studentUnionDepartment">数据对象</param>
        /// <returns></returns>
        public bool AddDepa(StudentUnionDepartment studentUnionDepartment)
        {
            bool str = false;
            try
            {
                str=true;
                UnionDepart.Insert(studentUnionDepartment);
             BusHelper.WriteSysLog("添加学生会部门数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return str;
        }
        /// <summary>
        /// 修改部门状态
        /// </summary>
        /// <param name="Name">部门名称</param>
        /// <returns></returns>
        public bool UodateDepa(string Name)
        {
           var x= UnionDepart.GetList().Where(a => a.Dateofregistration == false && a.Departmentname == Name).FirstOrDefault();
            x.Dateofregistration = true;
            bool str = true;
            try
            {
                UnionDepart.Update(x);
                BusHelper.WriteSysLog("修改学生会部门状态", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                str = false;
            }
            return str;
        }

        /// <summary>
        ///学生会职位所有数据
        /// </summary>
        /// <returns></returns>
        public List<StudentUnionPosition> UnionPositionList()
        {
          return  SUnionPosition.GetList().Where(a => a.Dateofregistration == false).ToList();
        }
    }
}
