using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Business.StudentBusiness;

namespace SiliconValley.InformationSystem.Business.ClassSchedule_Business
{
   public class ClassScheduleBusiness:BaseBusiness<ClassSchedule>
    {
        //学生委员职位
        BaseBusiness<Members> MemBers = new BaseBusiness<Members>();

        //班级群号
        BaseBusiness<GroupManagement> GriupMan = new BaseBusiness<GroupManagement>();
        //学生委员
        BaseBusiness<ClassMembers> Business = new BaseBusiness<ClassMembers>();
        //根据班级号查询出所有学员
        ScheduleForTraineesBusiness ss = new ScheduleForTraineesBusiness();
        //班级群管理
        BaseBusiness<GroupManagement> GetBase = new BaseBusiness<GroupManagement>();

        //班会
        BaseBusiness<Assmeetings> myassmeetings = new BaseBusiness<Assmeetings>();
        //拆班记录
        BaseBusiness<RemovalRecords> Dismantle = new BaseBusiness<RemovalRecords>();
        //班级异动表
        BaseBusiness<ClassDynamics> CLassdynamic = new BaseBusiness<ClassDynamics>();
        /// <summary>
        /// 通过班级名称获取学号，姓名，职位
        /// </summary>
        /// <returns></returns>
        public List<ClassStudentView> ClassStudentneList(string classid)
        {
           
            List<ClassStudentView> listview = new List<ClassStudentView>();
       
            var list = ss.ClassStudent(classid);
            foreach (var item in list)
            {
                List<ClassStudentView> Nameofmember = new List<ClassStudentView>();
                ClassStudentView classStudentView = new ClassStudentView();
                if (Business.GetList().Where(a => a.Studentnumber == item.StudentNumber && a.IsDelete == false).ToList().Count>0)
                {
                   var mylist= Business.GetList().Where(a => a.Studentnumber == item.StudentNumber&&a.IsDelete==false).ToList();
                    foreach (var item1 in mylist)
                    {
                        ClassStudentView view = new ClassStudentView();
                        view.Nameofmembers = MemBers.GetList().Where(c => c.ID ==item1.Typeofposition&& c.IsDelete == false).FirstOrDefault().Nameofmembers;
                        Nameofmember.Add(view);
                    }
                    classStudentView.Nameofmembers = Nameofmember;
                }
               
                classStudentView.Name = item.Name;
            
                classStudentView.StuNameID = item.StudentNumber;
                if (classStudentView!=null)
                {
                    listview.Add(classStudentView);
                }
            }
            return listview;



        }

        public List<ClassStudentView> ClassStudentneViewList(string classid)
        {
            //学员班级
            ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();
            //学员信息表
            StudentInformationBusiness student = new StudentInformationBusiness();

            List<ClassStudentView> listview = new List<ClassStudentView>();
            var x = scheduleForTraineesBusiness.GetList().Where(a => a.ClassID == classid).ToList();
            foreach (var item in x)
            {
                ClassStudentView classStudentView = new ClassStudentView();
                classStudentView.Name = student.GetEntity(item.StudentID).Name;
                classStudentView.StuNameID = student.GetEntity(item.StudentID).StudentNumber;
                if (item.CurrentClass==false)
                {
                    var z = scheduleForTraineesBusiness.GetList().Where(a => a.StudentID == item.StudentID && a.CurrentClass == true).FirstOrDefault();
                    classStudentView.ClassNameView = z.ClassID;
                }
                listview.Add(classStudentView);
            }
            return listview;



        }

        /// <summary>
        /// 根据班级查询返回出班级人数,微信号,QQ号,班级名称
        /// </summary>
        /// <param name="claassid"></param>
        /// <returns></returns>
        public List< ClassdetailsView> Listdatails( string claassid)
        {
            int count = ss.ClassStudent(claassid).Count();
            List<ClassdetailsView> list = new List<ClassdetailsView>();
            ClassdetailsView classdetailsView = new ClassdetailsView();
          var x=  GetBase.GetList().Where(a => a.ClassNumber == claassid && a.IsDelete == false).FirstOrDefault();
            if (x != null)
            {
                classdetailsView.count = count;
                classdetailsView.QQ = x.QQGroupnumber;
                classdetailsView.WeChat = x.WechatGroupNumber;
                classdetailsView.ClassName = claassid;
            }
            else
            {
                classdetailsView.count = count;
                classdetailsView.QQ = "未记录";
                classdetailsView.WeChat = "未记录";
                classdetailsView.ClassName = claassid;
            }
            list.Add(classdetailsView);
            return list;
        }

        /// <summary>
        /// 根据班级号查询出班级的联系方式
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public GroupManagement Grouselect(string className)
        {
          return  GriupMan.GetList().Where(a => a.ClassNumber == className && a.IsDelete == false).FirstOrDefault();

        }
        /// <summary>
        /// 添加或者修改班级群号
        /// </summary>
        /// <returns></returns>
        public AjaxResult GroupAdd(GroupManagement groupManagement)
        {
            AjaxResult retus = null;
          
            try
            {
              
                if (groupManagement.ID > 0)
                {
                  GroupManagement x = GriupMan.GetEntity(groupManagement.ID);
                    groupManagement.Addtime = x.Addtime;
                    groupManagement.IsDelete = false;
                    GriupMan.Update(groupManagement);
                   
                   
                    BusHelper.WriteSysLog("数据修改", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                }
                else
                {


                    groupManagement.Addtime = DateTime.Now;
                    groupManagement.IsDelete = false;
                    GriupMan.Insert(groupManagement);
                    BusHelper.WriteSysLog("数据添加", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
                  
                retus = new SuccessResult();
                retus.Msg = "操作成功";
                retus.Success = true;
            }
            catch (Exception ex)
            {
                if (groupManagement.ID > 0)
                {
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                }
                else
                {
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
                retus = new ErrorResult();
                retus.Msg = "服务器错误";

                retus.Success = false;
                retus.ErrorCode = 500;
            }
            return retus;
        }
        /// <summary>
        /// 班委职位
        /// </summary>
        /// <returns></returns>
        public List<Members> MembersList()
        {
            return MemBers.GetList();
        }
        /// <summary>
        /// 班级委员操作
        /// </summary>
        /// <param name="Stuid">学员学号</param>
        /// <param name="Typeofposition">班委名称</param>
        /// <param name="ClassNumber">班级号</param>
        /// <param name="Entity">数据操作为空则添加</param>
        /// <returns></returns>
        public AjaxResult Entityembers(string Stuid,string Typeofposition, string ClassNumber,string Entity)
        {
            //获取班委id
         
            AjaxResult retus = null;

            try
            {
                    //如果不为空则进行删除委员
                    if (Entity!= "undefined")
                       {
                   var x= Business.GetList().Where(a => a.IsDelete == false && a.Studentnumber == Stuid).ToList();
                 
                        Business.Delete(x);

                    retus = new SuccessResult();
                    retus.Msg = "班委撤销成功";
                    retus.Success = true;
                    BusHelper.WriteSysLog("数据删除", EnumType.LogType.删除数据);
                }
                    else
                      {
                    int ID = MemBers.GetList().Where(a => a.Nameofmembers == Typeofposition && a.IsDelete == false).FirstOrDefault().ID;
                    //查询出同个职位有多少个
                    var mylist=   Business.GetList().Where(a => a.Typeofposition == ID && a.ClassNumber == ClassNumber && a.IsDelete == false).ToList();
                    if (mylist.Count()>0)
                    {
                        Business.Delete(mylist);
                    }
                    ClassMembers classMembers = new ClassMembers();
                        classMembers.Typeofposition = ID;
                        classMembers.ClassNumber = ClassNumber;
                        classMembers.Studentnumber = Stuid;
                        classMembers.IsDelete = false;
                        classMembers.Addtime = DateTime.Now;
                        Business.Insert(classMembers);
                        retus = new SuccessResult();
                        retus.Msg = "任命班委成功";
                        retus.Success = true;
                        BusHelper.WriteSysLog("数据添加", EnumType.LogType.添加数据);
                        }
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, EnumType.LogType.系统异常);
            }
            return retus;
        }

        /// <summary>
        /// 班会数据操作
        /// </summary>
        /// <param name="assmeetings">对象数据</param>
        /// <returns></returns>
        public AjaxResult EntityAssmeetings(Assmeetings assmeetings)
        {

            AjaxResult retus = null;

          
            try
            {
                retus = new SuccessResult();
                retus.Success = true;
                if (assmeetings.ID > 0)
                {
                    var x = myassmeetings.GetEntity(assmeetings.ID);
                    x.Title = assmeetings.Title;
                    x.Content = assmeetings.Content;
                    x.Classmeetingdate = assmeetings.Classmeetingdate;
                    x.Remarks = assmeetings.Remarks;
                    myassmeetings.Update(x);
                    BusHelper.WriteSysLog("数据修改", EnumType.LogType.编辑数据);
                    retus.Msg = "数据编辑成功";
                }
                else
                {
                    assmeetings.Addtime = DateTime.Now;
                    assmeetings.IsDelete = false;
                    myassmeetings.Insert(assmeetings);
                    BusHelper.WriteSysLog("添加数据", EnumType.LogType.添加数据);
                 
                    retus.Msg = "数据添加成功";
            
                }
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";

                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, EnumType.LogType.系统异常);
            }
            return retus;
        }
        /// <summary>
        /// 班会根据id查询出一个实体
        /// </summary>
        /// <param name="id">参数id</param>
        /// <returns></returns>
        public Assmeetings AssmeetingsSelect(int id)
        {
            return myassmeetings.GetEntity(id);
        }
       /// <summary>
       /// 通过班级名称拿到所有班会数据
       /// </summary>
       /// <param name="ClassName">班级名称</param>
       /// <returns></returns>
        public List<Assmeetings> AssmeetingsList(string ClassName)
        {
            return myassmeetings.GetList().Where(a=>a.IsDelete==false&&a.ClassNumber==ClassName).ToList();
        }

        /// <summary>
        /// 查询这个委员是否有重复的
        /// </summary>
        /// <param name="Typeofposition">委员名称</param>
        /// <param name="ClassNumber">班级号</param>
        /// /// <param name="Stuid">学号</param>
        /// <returns></returns>
        public AjaxResult AssmeetingsBool(string Typeofposition, string ClassNumber,string Stuid)
        {
            AjaxResult retus = null;
            retus = new SuccessResult();
            retus.Success = true;
            int ID = MemBers.GetList().Where(a => a.Nameofmembers == Typeofposition && a.IsDelete == false).FirstOrDefault().ID;
            var mylist = Business.GetList().Where(a => a.Typeofposition == ID && a.ClassNumber == ClassNumber && a.IsDelete == false&&a.Studentnumber!=Stuid).ToList().Count();
            if (mylist>0)
            {
              
                retus.Msg = "已有学员为"+ Typeofposition+"是否替换";
               
                retus.Data = "aaa";

            }
         
            //查询出同个职位有多少个
            var mylist1 = Business.GetList().Where(a => a.Typeofposition == ID && a.ClassNumber == ClassNumber && a.IsDelete == false && a.Studentnumber == Stuid).ToList().Count();
            if (mylist1>0)
            {
                retus.Msg = Typeofposition+"已任命该学员,请勿重复任命";
             
            }
         
            return retus;
        }
        /// <summary>
        /// 拆班数据操作
        /// </summary>
        /// <param name="Addtime">拆班日期</param>
        /// <param name="FormerClass">原班级</param>
        /// <param name="List">现班级param>
        /// <param name="Reasong">原因</param>
        /// <param name="Remarks">备注</param>
        /// <param name="StudentID">学号</param>
        /// <returns></returns>
       public AjaxResult Dismantleclasses(string Addtime, string FormerClass, string List, string Reasong, string Remarks, string StudentID)
        {
        var x=  Dismantle.GetList().Where(a => a.IsDelete == false && a.FormerClass == FormerClass).Count();
            AjaxResult retus = null;
            try
            {
                retus = new SuccessResult();
                retus.Success = true;
                if (x < 1)
                {

                    RemovalRecords removalRecords = new RemovalRecords();
                    removalRecords.Addtime = Convert.ToDateTime(Addtime);
                    removalRecords.FormerClass = FormerClass;
                    removalRecords.Reasong = Reasong;
                    removalRecords.Remarks = Remarks;
                    removalRecords.IsDelete = false;
                    Dismantle.Insert(removalRecords);
                }
             
                    string[] Student = StudentID.Split(',');
                    List<ClassDynamics> MyDynamise = new List<ClassDynamics>();

                List<ScheduleForTrainees> UpdateScheduleFor = new List<ScheduleForTrainees>();

                List<ScheduleForTrainees> AddScheduleFor = new List<ScheduleForTrainees>();
                foreach (var item in Student)
                    {
                        ClassDynamics classDynamics = new ClassDynamics();
                        classDynamics.Addtime = Convert.ToDateTime(Addtime);
                        classDynamics.FormerClass = FormerClass;
                        classDynamics.CurrentClass = List;
                        classDynamics.Studentnumber = item;
                        classDynamics.Remarks = Remarks;
                        classDynamics.Reason = Reasong;
                        classDynamics.IsDelete = false;
                        MyDynamise.Add(classDynamics);
                    var UpdateSche = ss.GetList().Where(a => a.StudentID == item && a.ClassID == FormerClass).FirstOrDefault();
                    if (UpdateSche!=null)
                    {
                        UpdateSche.CurrentClass = false;
                        UpdateScheduleFor.Add(UpdateSche);
                    }
                    ScheduleForTrainees scheduleForTrainees = new ScheduleForTrainees();
                    scheduleForTrainees.ClassID = List;
                    scheduleForTrainees.StudentID = item;
                    scheduleForTrainees.CurrentClass = true;
                    scheduleForTrainees.AddDate = Convert.ToDateTime(Addtime); 
                    AddScheduleFor.Add(scheduleForTrainees);
                    }
                     ss.Update(UpdateScheduleFor);
                 ss = new ScheduleForTraineesBusiness();
                ss.Insert(AddScheduleFor);
                    CLassdynamic.Insert(MyDynamise);
             
                BusHelper.WriteSysLog("拆班数据添加", EnumType.LogType.添加数据);
                retus.Msg = "拆班成功";
            }
            catch (Exception ex)
            {
                retus = new ErrorResult();
                retus.Msg = "服务器错误";

                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, EnumType.LogType.系统异常);
            }
            return retus;
        }
    }
}
