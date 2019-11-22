using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.ClassesBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;

    /// <summary>
    /// 项目聚集地
    /// </summary>
   public class ProjectBusiness:BaseBusiness<ProjectTasks>
    {
        BaseBusiness<ProjectType> db_projecttype = new BaseBusiness<ProjectType>();
        BaseBusiness<StudentInformation> db_student = new BaseBusiness<StudentInformation>();
        BaseBusiness<ProjectGroupMember> db_TeamItem = new BaseBusiness<ProjectGroupMember>();
        TeacherBusiness db_teacher = new TeacherBusiness();

        /// <summary>
        /// 获取所有的项目
        /// </summary>
        /// <returns></returns>
        public List<ProjectTasks> GetProjectTasks()
        {

            //是否需要判读角色


            return this.GetList().Where(d => d.IsDel == false).ToList();

        }


        /// <summary>
        /// 将EF模型转换为视图模型
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public ProjectDetailView ConvertToDetailView(ProjectTasks project)
        {

            ProjectDetailView projectview = new ProjectDetailView();

            projectview.BeginDate = project.BeginDate;
            projectview.EndDate = project.EndDate;

            if (project.StudentNO == null)
            
                projectview.GroupLeader=null;
                
            else
            {
                //查询学员信息
                BaseBusiness<StudentInformation> db_student = new BaseBusiness<StudentInformation>();
                projectview.GroupLeader = db_student.GetList().Where(d => d.StudentNumber == project.StudentNO).FirstOrDefault();
            }

            projectview.IsDel = project.IsDel;
            projectview.IsStop = project.IsStop;
            projectview.ProjectID = project.ProjectID;
            projectview.ProjectIntroduce = project.ProjectIntroduce;
            projectview.ProjectName = project.ProjectName;
            projectview.ProjectType = db_projecttype.GetList().Where(d => d.Id == project.ProjectType).FirstOrDefault();
            projectview.Remark = project.Remark;

            //获取项目开发人员
            var templist = db_TeamItem.GetList().Where(d => d.ProjectID == project.ProjectID).ToList();

            List<StudentInformation> studetnlist = new List<StudentInformation>();

            foreach (var item in templist)
            {
                if (!IsContainsStudent(studetnlist, item.StudentID))
                {
                    studetnlist.Add(db_student.GetList().Where(d => d.StudentNumber == item.StudentID).FirstOrDefault());

                }
            }
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            projectview.TeamImte = studetnlist;
            projectview.Tutor = teacher;

            projectview.TutorEmp = db_teacher.GetEmpByEmpNo(teacher.EmployeeId);

            projectview.ShowImages = project.ShowImages;

            return projectview;




        }

        public bool IsContainsStudent(List<StudentInformation> source, string studentnumber)
        {
            foreach (var item in source)
            {
                if (item.StudentNumber == studentnumber)
                    return true;
            }

            return false;
        }



        public bool IsContainsStudent(List<ProjectGroupMember> source, string studentnumber)
        {
            foreach (var item in source)
            {
                if (item.StudentID == studentnumber)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取组员的项目模块
        /// </summary>
        /// <param name="studentnumber"></param>
        /// <returns></returns>
        public List<ProjectGroupMember> GetProjectModularByStudent(string studentnumber, int projectid)
        {


           return  db_TeamItem.GetList().Where(d=>d.ProjectID==projectid && d.StudentID==studentnumber).ToList();


        }

        public void AddGroupItem(List<string> list, int projectid)
        {

            foreach (var item in list)
            {


                if (!this.IsContainItem(projectid, item))
                {
                    ProjectGroupMember projectGroup = new ProjectGroupMember();

                    projectGroup.IsAccomplish = false;
                    projectGroup.StudentID = item;
                    projectGroup.ProjectID = projectid;

                    db_TeamItem.Insert(projectGroup);
                }

               

            }

        }

        public bool IsContainItem(int projectid , string studentnumber)
        {

           return db_TeamItem.GetList().Where(d => d.ProjectID == projectid && d.StudentID == studentnumber).FirstOrDefault()!=null;

        }

        public List<StudentInformation> UngetProjectItem(int projectid ,int classnumber)
        {
            ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();

            var studentlist = scheduleForTraineesBusiness.ClassStudent(classnumber);

            List<StudentInformation> result = new List<StudentInformation>();

           var templist = this.GetProjectTasks().Where(d=>d.IsStop==false);

           var grouplist = new List<ProjectGroupMember>();

            foreach (var item in templist)
            {

              var temp2list =  db_TeamItem.GetList().Where(d => d.ProjectID == item.ProjectID).ToList();

                foreach (var item1 in studentlist)
                {
                  var s=  temp2list.Where(d => d.StudentID == item1.StudentNumber).FirstOrDefault();


                    if (s == null)
                    {
                        if (!this.IsContainsStudent(result, item1.StudentNumber))
                        {
                            var student = db_student.GetList().Where(d => d.StudentNumber == item1.StudentNumber).FirstOrDefault();

                            result.Add(student);
                        }
                    }

                   
                }
            }


         
            return result;

        }


        /// <summary>
        /// 给组员添加模块
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="studentnumber"></param>
        /// <param name="ModularName"></param>
        public void AddModular(string projectid, string studentnumber, string ModularName)
        {
           var temp = db_TeamItem.GetList().Where(d => d.ProjectID == int.Parse(projectid) && d.StudentID == studentnumber).ToList();

            bool isNull = false;

            

            foreach (var item in temp)
            {

                if (string.IsNullOrEmpty(item.Task))
                {
                    isNull = true;

                    //做修改操作
                    item.Task = ModularName;

                    db_TeamItem.Update(item);


                }

            }


            if (isNull == false)
            {
                //添加操作

                ProjectGroupMember projectGroup = new ProjectGroupMember();
                projectGroup.IsAccomplish = false;
                projectGroup.ProjectID =int.Parse( projectid);
                projectGroup.Task = ModularName;
                projectGroup.StudentID = studentnumber;

                db_TeamItem.Insert(projectGroup);

            }


        }


        /// <summary>
        /// 删除项目组成员
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="studentnumber"></param>
        public void RemoveTeamItem(int projectid, string studentnumber)
        {
            var templist =db_TeamItem.GetList().Where(d=>d.ProjectID==projectid && d.StudentID==studentnumber).ToList();


            foreach (var item in templist)
            {
                db_TeamItem.Delete(item);
            }
        }


        /// <summary>
        /// 修改项目组长
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="newstudentnumber"></param>
        /// <returns></returns>
        public void updateGroupLearder(int projectid, string newstudentnumber)
        {
          var project = this.GetProjectTasks().Where(d=>d.ProjectID==projectid).FirstOrDefault();

           var list = db_TeamItem.GetList().Where(d => d.ProjectID == projectid && d.StudentID == project.StudentNO).ToList();


            foreach (var item in list)
            {

                item.StudentID = newstudentnumber;
                db_TeamItem.Update(item);
            }

            project.StudentNO = newstudentnumber;

            this.Update(project);



        }


        /// <summary>
        /// 获取项目组员详细信息
        /// </summary>
        /// <param name="groupMember"></param>
        /// <returns></returns>
        public ProjectTeamDetailView GetTeamDetailView(ProjectGroupMember groupMember)
        {

            ProjectTeamDetailView projectTeam = new ProjectTeamDetailView();
            projectTeam.Id = groupMember.Id;
            projectTeam.IsAccomplish = groupMember.IsAccomplish ? "完成" : "开发中";

           var project = this.GetProjectTasks().Where(d=>d.ProjectID==groupMember.ProjectID).FirstOrDefault();

            projectTeam.Project = this.ConvertToDetailView(project);

            projectTeam.Student = db_student.GetList().Where(d => d.StudentNumber == groupMember.StudentID).FirstOrDefault();

            projectTeam.Task = groupMember.Task;
            return projectTeam;



        }


        /// <summary>
        /// 获取项目组员
        /// </summary>
        /// <returns></returns>
        public List<ProjectGroupMember> ProjectTeamInfo(int projectid)
        {
          return  db_TeamItem.GetList().Where(d=>d.ProjectID==projectid).ToList();

        }


        /// <summary>
        /// 修改项目模块完成状态
        /// </summary>
        /// <param name="modelid"></param>
        /// <param name="status"></param>
        public void EditModelStatus(int modelid, bool status)
        {
          var obj =  db_TeamItem.GetList().Where(d => d.Id == modelid).FirstOrDefault();

            obj.IsAccomplish = status;

            db_TeamItem.Update(obj);

        }

        /// <summary>
        /// 获取项目没有完成的模块
        /// </summary>
        /// <param name="projectid">项目ID</param>
        /// <returns></returns>
        public List<ProjectGroupMember> GetNoFinshModel(int projectid)
        {

          return  db_TeamItem.GetList().Where(d=>d.ProjectID==projectid && d.IsAccomplish==false).ToList();

        }

        /// <summary>
        /// 停止项目
        /// </summary>
        /// <param name="projectid"></param>
        public void StopProject(int projectid)
        {

           var obj = this.GetProjectTasks().Where(d => d.ProjectID == projectid).FirstOrDefault();

            obj.IsStop = true;

            this.Update(obj);
        }

        public List<ProjectType> ProjectTypes()
        {
            return db_projecttype.GetList();

        }

     
    }
}
