using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;//获取树实体
using SiliconValley.InformationSystem.Business.Common;//获取日志实体
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;//获取学生状态实体
using SiliconValley.InformationSystem.Business.StuInfomationType_Maneger;//获取学生信息来源实体
using SiliconValley.InformationSystem.Business.EmployeesBusiness;//获取员工信息实体
using SiliconValley.InformationSystem.Business.DepartmentBusiness; //获取岗位信息实体 
using SiliconValley.InformationSystem.Business.PositionBusiness;//获取岗位实体
using SiliconValley.InformationSystem.Entity.ViewEntity;//获取员工岗位部门实体
using SiliconValley.InformationSystem.Entity.Entity;
using System.Text;
using System.IO;
using System.Data;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.RegionManage;//获取区域实体
using SiliconValley.InformationSystem.Business.Base_SysManage;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using SiliconValley.InformationSystem.Business.NewExcel;
using DataTable = System.Data.DataTable;
using SiliconValley.InformationSystem.Depository.CellPhoneSMS;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class StudentDataKeepController : BaseMvcController
    {
        // GET: /Market/StudentDataKeep/DelteInfomationType
        #region 创建实体
        //创建一个用于操作数据的备案实体
        StudentDataKeepAndRecordBusiness s_Entity = new StudentDataKeepAndRecordBusiness();
        //创建一个用于查询数据的上门学生状态实体
        StuStateManeger Stustate_Entity = new StuStateManeger();
        //创建一个用于查询数据的上门学生信息来源实体
        StuInfomationTypeManeger StuInfomationType_Entity = new StuInfomationTypeManeger();
        //创建一个用于查询数据的员工信息实体
        EmployeesInfoManage Enplo_Entity = new EmployeesInfoManage();
        //创建一个用于查询数据的部门信息实体
        DepartmentManage Department_Entity = new DepartmentManage();
        //创建一个用于查询岗位信息实体
        PositionManage Position_Entity = new PositionManage();
        //创建一个用于查询区域的实体
        RegionManeges region_Entity = new RegionManeges();
        //创建一个用于缓存的实体
        RedisCache my_redis = new RedisCache();

        ExcelHelper Excel_Entity = new ExcelHelper();

        string UserName = Base_UserBusiness.GetCurrentUser().UserName;//获取当前登录人
        #endregion
        #region 获取各种外键的值
        //获取学生状态名称
        public string GetStuStatuValue(int? id)
        {
            StuStatus Get_List_stustate = Stustate_Entity.GetEntity(id);//获取上门学生状态所有数据
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                return Get_List_stustate.StatusName;
            }
            else
            {
                return "未填写";
            }
        }
        //获取信息来源名称
        public string GetStuInfomationTypeValue(int? id)
        {
            StuInfomationType Get_List_stuInfomationtype = StuInfomationType_Entity.GetEntity(id);

            if (!string.IsNullOrEmpty(id.ToString()))
            {
                return Get_List_stuInfomationtype.Name;
            }
            return "未填写";
        }
        //查询员工
        public string GetEmployeeValue(string id,bool isglu)
        {
            if (isglu)
            {
                EmployeesInfo finde = Enplo_Entity.GetList().Where(s => s.EmployeeId == id && s.IsDel == false).FirstOrDefault();
                if (finde != null)
                {
                    return finde.EmpName;
                }
                else
                {
                    return "无";
                }
            }
            else
            {
                EmployeesInfo finde = Enplo_Entity.GetEntity(id);
                if (finde != null)
                {
                    return finde.EmpName;
                }
                else
                {
                    return "无";
                }
            }            
        }       
        //这个方法是用于通过名字来查询信息来源Id的
        public int GetNameSearchId(string name)
        {
            StuInfomationType liststuinfomation = StuInfomationType_Entity.GetList().Where(i=>i.Name==name).FirstOrDefault();
            if (liststuinfomation != null)
            {
                return liststuinfomation.Id;
            }
            else
            {
                return 1;
            }

        }
        //这个方法是用于通过员工姓名来查询员工的员工编号
        public string GetNameSreachEmploId(string name)
        {
            EmployeesInfo e = Enplo_Entity.GetList().Where(es => es.EmpName == name).FirstOrDefault();
            if (e == null)
            {
                return "无";
            }
            else
            {
                return e.EmployeeId;
            }

        }
        //缓存
        public List<StudentPutOnRecord> GetList()
        {            
            var resutllist = new List<StudentPutOnRecord>();
            resutllist = my_redis.GetCache<List<StudentPutOnRecord>>("ListStudentPutOnRecord");             
            //my_redis.RemoveCache("ListStudentPutOnRecord"); //每当数据改变的时候就需要清空一次             
                if (resutllist == null)
                {
                    resutllist = s_Entity.GetList();
                    my_redis.SetCache("ListStudentPutOnRecord", resutllist);
                }
            return resutllist;
        }

         
        #endregion

        //这是一个数据备案的主页面
        public ActionResult StudentDataKeepIndex()
        {         
            //获取信息来源的所有数据
            List<SelectListItem> se=  StuInfomationType_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();
            SelectListItem e = new SelectListItem();
            e.Selected = true;
            e.Text = "请选择";
            e.Value = "请选择";
            se.Add(e);
            ViewBag.infomation = se;
            //获取区域所有信息
            SelectListItem newselectitem = new SelectListItem() { Text="请选择",Value="请选择",Selected=true};
            var r_list= region_Entity.GetList().Where(r => r.IsDel == false).Select(r => new SelectListItem { Text = r.RegionName, Value = r.ID.ToString() }).ToList();
            r_list.Add(newselectitem);
            ViewBag.are = r_list;
            return View();
        }

        //往数据库中获取数据备案的信息
        public ActionResult GetStudentPutOnRecordData(int limit,int page)
        {
            #region 取值
            string findNamevalue = Request.QueryString["findNamevalue"];
            string findPhonevalue = Request.QueryString["findPhonevalue"];
            string findInformationvalue = Request.QueryString["findInformationvalue"];
            string findStartvalue = Request.QueryString["findStartvalue"];
            string findEndvalue = Request.QueryString["findEndvalue"];
            string findBeanManvalue = Request.QueryString["findBeanManvalue"];
            string findAreavalue = Request.QueryString["findAreavalue"];
            #endregion
            List<StudentPutOnRecord> stu_IQueryable = GetList().OrderByDescending(s=>s.Id).ToList();
            #region 模糊查询
            if (!string.IsNullOrEmpty(findNamevalue))
            {
                stu_IQueryable = stu_IQueryable.Where(s => s.StuName.Contains(findNamevalue)).ToList();
            }
            if (!string.IsNullOrEmpty(findPhonevalue))
            {
                stu_IQueryable = stu_IQueryable.Where(s => s.StuPhone.Contains(findPhonevalue)).ToList();
            }
            if (!string.IsNullOrEmpty(findInformationvalue) && findInformationvalue!="请选择")
            {
                int type_id = Convert.ToInt32(findInformationvalue);
                stu_IQueryable = stu_IQueryable.Where(s => s.StuInfomationType_Id== type_id).ToList();
            }
            if (!string.IsNullOrEmpty(findStartvalue))
            {
                DateTime t1 = Convert.ToDateTime(findStartvalue);
                
                stu_IQueryable = stu_IQueryable.Where(s => s.StuDateTime >=  t1).ToList();
            }
            if (!string.IsNullOrEmpty(findEndvalue))
            {
                DateTime t2 = Convert.ToDateTime(findEndvalue);
                DateTime dd = new DateTime(t2.Year, t2.Month, t2.Day, 23, 59, 59);
                stu_IQueryable = stu_IQueryable.Where(s => s.StuDateTime <= dd ).ToList();
            }
            if (!string.IsNullOrEmpty(findBeanManvalue))
            {
                string Beanname = findBeanManvalue;
                string empyeid = GetNameSreachEmploId(Beanname);
                stu_IQueryable = stu_IQueryable.Where(s => s.EmployeesInfo_Id == empyeid).ToList();
            }
            if (!string.IsNullOrEmpty(findAreavalue)&& findAreavalue!= "请选择")
            {
                int Areeid =Convert.ToInt32( findAreavalue);                
                stu_IQueryable = stu_IQueryable.Where(s => s.Region_id == Areeid).ToList();
            }
            #endregion            
            try
            {
                #region 分页转前端数据格式类型
                int SunLimit = stu_IQueryable.Count();//总行数
                int SunPage = Convert.ToInt32(Math.Ceiling((double)SunLimit / limit));//总页数
                stu_IQueryable = stu_IQueryable.Skip((page - 1) * limit).Take(limit).ToList();
                var Get_List_studentPutOnRecord = stu_IQueryable.Select(s => new {
                    Id = s.Id,
                    StuName = s.StuName,
                    StuSex = s.StuSex,
                    StuBirthy = s.StuBirthy,
                    StuPhone = s.StuPhone,
                    StuSchoolName = s.StuSchoolName,
                    StuEducational = s.StuEducational,
                    StuAddress = s.StuAddress,
                    StuWeiXin = s.StuWeiXin,
                    StuQQ = s.StuQQ,
                    StuInfomationType_Id = GetStuInfomationTypeValue(s.StuInfomationType_Id),
                    StuStatus_Id = GetStuStatuValue(s.StuStatus_Id),
                    StuIsGoto = s.StuIsGoto,
                    StuVisit = s.StuVisit,
                    EmployeesInfo_Id = GetEmployeeValue(s.EmployeesInfo_Id, false),
                    StuDateTime = s.StuDateTime,
                    StuEntering = s.StuEntering,
                    Reak = s.Reak,
                    Regin_id = s.Region_id,
                    ReginName = region_Entity.GetAreValue(s.Region_id.ToString(), true)
                }).ToList();//获取了数据库中所有数据备案信息;                                                                                                         
                var JsonData = new {     
                    code=0, //解析接口状态,
                    msg="", //解析提示文本,
                    count= SunLimit, //解析数据长度
                    data= Get_List_studentPutOnRecord //解析数据列表
                };
                #endregion
                return Json(JsonData,JsonRequestBehavior.AllowGet);
            }                            
            catch (Exception ex)         
            {                            
                //将错误填写到日志中     
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);
                return Json(Error("加载数据有误"),JsonRequestBehavior.AllowGet);
            }                            
        }       
        #region
        //这是一个添加数据的页面
        public ActionResult AddorEdit(string id)
        {
            //获取信息来源的所有数据
            ViewBag.infomation = StuInfomationType_Entity.GetList().Where(s=>s.IsDelete==false).Select(s=>new SelectListItem { Text=s.Name, Value=s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
            List<SelectListItem> ss = Stustate_Entity.GetList().Where(s => s.IsDelete == false && !s.StatusName.Contains("已报名") && !s.StatusName.Contains("未报名")).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();
            SelectListItem s1 = new SelectListItem() { Value = "-1", Text = "请选择", Selected = true };
            ss.Add(s1);
            ViewBag.state = ss;
            //获取所有区域
            SelectListItem s2 = new SelectListItem() { Text = "区域外", Value = "区域外" };
             var r_list = region_Entity.GetList().Where(r=>r.IsDel==false).Select(r=>new SelectListItem { Text=r.RegionName,Value=r.ID.ToString()}).ToList();
             r_list.Add(s2);
            ViewBag.area = r_list;


            return View();
        }

        //将所有员工显示给用户选择
        public ActionResult ShowEmployeInfomation()
        {
            List<EmployeesInfo> list_Enploy = Enplo_Entity.GetList().Where(s=>s.IsDel==false).ToList();//获取所有在职员工
            List<TreeClass> list_Tree = Department_Entity.GetList().Select(d=>new TreeClass() {id=d.DeptId.ToString(),title=d.DeptName, children=new List<TreeClass>(), disable=false, @checked=false, spread=false }).ToList();
            List<Position> list_Position = Position_Entity.GetList().Where(s=>s.IsDel==false).ToList();//获取所有岗位有用的数据
            foreach (TreeClass item1 in list_Tree)
            {
                List<TreeClass> bigTree = new List<TreeClass>();
                foreach (Position item2 in list_Position)
                {
                    if (item1.id==item2.DeptId.ToString())
                    {                        
                        foreach (EmployeesInfo item3 in list_Enploy)
                        {                                                         
                            if (item3.PositionId==item2.Pid)
                            {
                                TreeClass tcc2 = new TreeClass();
                                tcc2.id = item3.EmployeeId;
                                tcc2.title = item3.EmpName;
                                bigTree.Add(tcc2);
                            }
                        }
                        item1.children = bigTree;
                    }                     
                }
            }
            return Json(list_Tree,JsonRequestBehavior.AllowGet);
        }

        //树形图
        public ActionResult ShowTree()
        {
            return View();
        }

        //查看是否是选中员工
        public ActionResult FindEmply(string id)
        {
            EmployeesInfo finde= Enplo_Entity.GetList().Where(s => s.EmployeeId==id && s.IsDel==false).FirstOrDefault();
            if (finde!=null)
            {                 
                return Json(finde, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("no", JsonRequestBehavior.AllowGet);
            }
             
        }

        //添加备案数据
        public ActionResult StudentDataKeepAdd(StudentPutOnRecord news)
        {            
            try
            {
               StudentPutOnRecord er= s_Entity.GetList().Where(s => s.StuName == news.StuName && s.StuPhone == news.StuPhone).FirstOrDefault();
                if (er==null)
                {
                    news.StuDateTime = DateTime.Now;
                    news.IsDelete = false;
                    news.StuEntering = "201908150001";
                    if (news.StuStatus_Id==-1 || news.StuStatus_Id==null)
                    {
                        StuStatus find_status = Stustate_Entity.GetId("未报名");
                        if (find_status != null)
                        {
                            news.StuStatus_Id = find_status.Id;
                        }
                         
                    }                  
                    s_Entity.Insert(news);
                    my_redis.RemoveCache("ListStudentPutOnRecord");
                    //通知备案人备案成功
                    string number = "13204961361";//根据备案人查询电话号码
                    string smsText = "备案提示:已备案成功";
                    string t = PhoneMsgHelper.SendMsg(number, smsText);
                    return Json("ok", JsonRequestBehavior.AllowGet);                   
                }
                else
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                //将错误填写到日志中     
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                return Json(Error("数据添加有误"), JsonRequestBehavior.AllowGet);
            }
            
            
        }

        //查看是否有重复的学员信息名称
        public ActionResult FindStudent(string id)
        {
           List<StudentPutOnRecord> fins= s_Entity.GetList().Where(s=>s.StuName==id).ToList();
            if (fins.Count>0)
            {
                return Json(fins,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("ok");
            }          
        }

        //创建一个编辑页面
        public ActionResult EditView(string id)
        {
            ViewBag.id = id;
            //获取信息来源的所有数据
            ViewBag.infomation = StuInfomationType_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
             List<SelectListItem>  ss  = Stustate_Entity.GetList().Where(s => s.IsDelete == false && !s.StatusName.Contains("已报名") && !s.StatusName.Contains("未报名")).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();
            SelectListItem s1 = new SelectListItem() { Value="-1",Text="请选择",Selected=true};
            ss.Add(s1);
            ViewBag.state = ss;
            //获取所有区域
            SelectListItem s2 = new SelectListItem() { Text = "区域外", Value = "区域外" };
            var r_list = region_Entity.GetList().Where(r => r.IsDel == false).Select(r => new SelectListItem { Text = r.RegionName, Value = r.ID.ToString() }).ToList();
            r_list.Add(s2);
            ViewBag.area = r_list;
            return View();
        }

        //创建一个用于编辑的处理方法
        public ActionResult EditFunction(StudentPutOnRecord olds)
        {
            //需要判断是咨询部人员修改还是网络部人员修改  SessionHelper.Session["UserId"]=""
            try
            {
                StudentPutOnRecord fins = s_Entity.GetList().Where(s => s.Id == olds.Id).FirstOrDefault();//找到要修改的实体
                fins.StuSex = olds.StuSex;
                fins.StuBirthy = olds.StuBirthy;
                fins.StuSchoolName = olds.StuSchoolName;
                fins.StuEducational = olds.StuEducational;
                fins.StuAddress = olds.StuAddress;
                fins.StuWeiXin = olds.StuWeiXin;
                fins.StuQQ = olds.StuQQ;
                fins.StuIsGoto = olds.StuIsGoto;
                fins.StuVisit = olds.StuVisit;
                fins.StuInfomationType_Id = olds.StuInfomationType_Id;
                if (olds.StuStatus_Id==-1 || olds.StuStatus_Id==null)
                {
                    StuStatus find_status = Stustate_Entity.GetId("未报名");
                    if (find_status != null)
                    {
                        fins.StuStatus_Id = find_status.Id;
                    }                   
                }
                else
                {
                    fins.StuStatus_Id = olds.StuStatus_Id;
                }
                fins.Region_id = olds.Region_id;
                s_Entity.Update(fins);
                my_redis.RemoveCache("ListStudentPutOnRecord");
                return Json("ok", JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                //将错误填写到日志中     
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                return Json(Error("数据编辑有误"), JsonRequestBehavior.AllowGet);
            }            
        }
        #endregion
        //将Excel中的数据加载到数据库中
        public bool AddExcelToServer(List<MyExcelClass> list)
        {
            bool Is = false;
            try
            {
                foreach (MyExcelClass item1 in list)
                {
                    StudentPutOnRecord s = new StudentPutOnRecord();
                    s.StuName = item1.StuName;
                    s.StuSex = item1.StuSex == "男" ? true : false;
                    s.EmployeesInfo_Id = GetNameSreachEmploId(item1.EmployeesInfo_Id);
                    s.IsDelete = false;
                    s.Reak = item1.Reak;
                    Region find_region = region_Entity.SerchRegionName(item1.Region_id, false);
                    if (find_region!=null)
                    {
                        s.Region_id = find_region.ID;
                    }                     
                    s.StatusTime = null;
                    s.StuAddress = item1.StuAddress;
                    s.StuBirthy = null;
                    s.StuDateTime = DateTime.Now;
                    s.StuEducational = item1.StuEducational;
                    s.StuEntering = "201909020020";//需要判断登录人
                    StuInfomationType find_sinfomation= StuInfomationType_Entity.SerchSingleData(item1.StuInfomationType_Id, false);
                    if (find_sinfomation!=null)
                    {
                        s.StuInfomationType_Id = find_sinfomation.Id;
                    }                   
                    s.StuIsGoto = false;
                    s.StuPhone = item1.StuPhone;
                    s.StuSchoolName = item1.StuSchoolName;
                    s.StuStatus_Id = Stustate_Entity.GetIdGiveName("未报名", false).Id;
                    s_Entity.Insert(s);
                }
                my_redis.RemoveCache("ListStudentPutOnRecord");//清除缓存
                Is = true;
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(UserName + "Excel大批量导入数据时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return Is;
        }
        //根据ID找到学生信息并赋值
        public ActionResult FindStudentInfomation(string id)
        {
            if (!string.IsNullOrEmpty(id) && id!="undifind")
            {
               StudentPutOnRecord finds = s_Entity.GetList().Where(s=>s.Id==Convert.ToInt32(id)).FirstOrDefault();
                var newdata = new {
                    EmployeesInfo_Id = finds.EmployeesInfo_Id,
                    Id = finds.Id,
                    IsDelete = finds.IsDelete,
                    Reak = finds.Reak,
                    StuAddress = finds.StuAddress,
                    StuBirthy = finds.StuBirthy,
                    StuDateTime = finds.StuDateTime,
                    StuEducational = finds.StuEducational,
                    StuEntering = finds.StuEntering,
                    StuInfomationType_Id = finds.StuInfomationType_Id,
                    StuIsGoto = finds.StuIsGoto,
                    StuName = finds.StuName,
                    StuPhone = finds.StuPhone,
                    StuQQ = finds.StuQQ,
                    StuSchoolName = finds.StuSchoolName,
                    StuSex = finds.StuSex,
                    StuStatus_Id = finds.StuStatus_Id,
                    StuVisit = finds.StuVisit,
                    StuWeiXin = finds.StuWeiXin,
                    e_Name = GetEmployeeValue(finds.EmployeesInfo_Id, false),
                    StuEntering_1 = GetEmployeeValue(finds.StuEntering, false),
                    InfomationTypeName = GetStuInfomationTypeValue(finds.StuInfomationType_Id),
                    StatusName = GetStuStatuValue(finds.StuStatus_Id)
                };
                return Json(newdata, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("学生ID未拿到", JsonRequestBehavior.AllowGet);
            }
            
        }
        //数据详情查看页面
        public ActionResult LookDetailsView(string id)
        {
            ViewBag.id = id;
            //获取信息来源的所有数据
            ViewBag.infomation = StuInfomationType_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
            ViewBag.state = Stustate_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();
            return View();
        }

        //显示有疑似的数据
        public ActionResult StudentSomeData(string id)
        {
            string[] id_list = id.Split(',');
            List<StudentPutOnRecord> Student_list = new List<StudentPutOnRecord>();
            List<StudentPutOnRecord> list = s_Entity.GetList();
            foreach (StudentPutOnRecord item1 in list)
            {
                foreach (string item2 in id_list)
                {
                    if (!string.IsNullOrEmpty(item2))
                    {
                        if (item1.Id == Convert.ToInt32(item2))
                        {
                            Student_list.Add(item1);
                        }
                    }
                     
                }
            }
            var data = Student_list.Select(s=>new StudentData
            {
                stuSex=s.StuSex==true?"男":"女",
                StuName=s.StuName,
                StuPhone=s.StuPhone,
                StuSchoolName = s.StuSchoolName,
                StuAddress=s.StuAddress,
                StuInfomationType_Id = GetStuInfomationTypeValue(s.StuInfomationType_Id),
                StuStatus_Id = GetStuStatuValue(s.StuStatus_Id),
                StuIsGoto = s.StuIsGoto==false?"否":"是",
                StuVisit=s.StuVisit,
                EmployeesInfo_Id= GetEmployeeValue(s.EmployeesInfo_Id,true),
                StuDateTime=s.StuDateTime,
                StuEntering = GetEmployeeValue(s.StuEntering,true),
                AreName= region_Entity.GetAreValue(s.Region_id.ToString(),true)
            }).ToList();
            ViewBag.Student =data;
            return View(data);
        }
        #region Excle文件导入
        //获取已备案中的相似数据
        public List<MyExcelClass> SercherStudent(List<MyExcelClass> ex)
        {           
            List<StudentPutOnRecord> s_list = s_Entity.GetList();
            List<MyExcelClass> list = new List<MyExcelClass>();
            foreach (MyExcelClass item1 in ex)
            {
                foreach (StudentPutOnRecord item2 in s_list)
                {
                    if (item2.StuName==item1.StuName && item2.StuPhone==item1.StuPhone)
                    {
                        MyExcelClass m = new MyExcelClass();
                        m.StuSex = item2.StuSex == true ? "男" : "女";
                        m.StuName = item2.StuName;
                        m.StuPhone = item2.StuPhone;
                        m.StuSchoolName = item2.StuSchoolName;
                        m.StuInfomationType_Id = GetStuInfomationTypeValue(item2.StuInfomationType_Id);
                        m.Region_id =region_Entity.SerchRegionName(item2.Region_id.ToString(),true).RegionName;
                        m.StuEducational = item2.StuEducational;
                        m.StuAddress = item2.StuAddress;
                        m.Reak = item2.Reak;
                        m.EmployeesInfo_Id = GetEmployeeValue(item2.EmployeesInfo_Id,false);
                        if( list.Where(s => s.StuName == m.StuName && s.StuPhone==m.StuPhone).FirstOrDefault() == null)
                        {
                            list.Add(m);
                        }
                                                 
                    }
                }                
            }
             
            return list;
        }
        ///一个删除文件的方法
        public void DeleteFile()
        {
            var namef = SessionHelper.Session["filename"];
            var namef2 = SessionHelper.Session["filename2"];
            if (namef!=null)
            {
                FileInfo fi = new FileInfo(namef.ToString());
                bool ishave = fi.Exists;
                if (ishave)
                {
                    fi.Delete();
                }
            }
            if (namef2!=null)
            {
                FileInfo fi = new FileInfo(namef2.ToString());
                bool ishave = fi.Exists;
                if (ishave)
                {
                    fi.Delete();
                }
            }
            
           
        }
        //获取Excle文件中的值
        public List<StudentPutOnRecord> GetExcelFunction()
        {
            string namef = SessionHelper.Session["filename"].ToString();//获取要读取的Excel文件名称
            System.Data.DataTable t = AsposeOfficeHelper.ReadExcel(namef, false);//从Excel文件拿值
            List<StudentPutOnRecord> new_listStudent = new List<StudentPutOnRecord>();
                if (t.Rows[0][0].ToString() == "姓名" && t.Rows[0][1].ToString() == "性别" && t.Rows[0][2].ToString() == "电话" && t.Rows[0][3].ToString() == "学校" && t.Rows[0][4].ToString() == "家庭住址" && t.Rows[0][5].ToString() == "区域" && t.Rows[0][6].ToString() == "信息来源" && t.Rows[0][7].ToString() == "学历" && t.Rows[0][8].ToString() == "备案人" && t.Rows[0][9].ToString() == "备注")
            {                
                    //需要转型
                    for (int i = 1; i < (t.Rows.Count); i++)
                    {
                        StudentPutOnRecord create_s = new StudentPutOnRecord();
                        create_s.StuName = t.Rows[i][0].ToString();
                        create_s.StuSex = t.Rows[i][1].ToString() == "女" ? false : true;
                        create_s.StuPhone = t.Rows[i][2].ToString();
                        create_s.StuSchoolName = t.Rows[i][3].ToString();//学校
                        create_s.StuAddress = t.Rows[i][4].ToString();//家庭住址
                        string find_region = region_Entity.GetAreValue(t.Rows[i][5].ToString(), false);
                        if (find_region!=null)
                        {
                        create_s.Region_id = Convert.ToInt32(find_region);
                        }                       
                        create_s.StuInfomationType_Id = GetNameSearchId(t.Rows[i][6].ToString());//信息来源
                        create_s.StuEducational = t.Rows[i][7].ToString() == null ? "初中" : t.Rows[i][7].ToString();
                        create_s.EmployeesInfo_Id = GetNameSreachEmploId(t.Rows[i][8].ToString());//备案人
                        create_s.Reak = t.Rows[i][9].ToString();//备注
                        create_s.StuIsGoto = false;
                        create_s.StuStatus_Id = 1;
                        new_listStudent.Add(create_s);
                    }
                return new_listStudent;
            }
            else
            {
                return new_listStudent;
            }
        }
        //转型为Excle集合(目的:有些是区域外的，会被处理)
        public List<MyExcelClass> GetExcel()
        {
            string namef = SessionHelper.Session["filename"].ToString();//获取要读取的Excel文件名称
            System.Data.DataTable t = AsposeOfficeHelper.ReadExcel(namef, false);//从Excel文件拿值
            List<MyExcelClass> new_listStudent = new List<MyExcelClass>();
            if (t.Rows[0][0].ToString() == "姓名" && t.Rows[0][1].ToString() == "性别" && t.Rows[0][2].ToString() == "电话" && t.Rows[0][3].ToString() == "学校" && t.Rows[0][4].ToString() == "家庭住址" && t.Rows[0][5].ToString() == "区域" && t.Rows[0][6].ToString() == "信息来源" && t.Rows[0][7].ToString() == "学历" && t.Rows[0][8].ToString() == "备案人" && t.Rows[0][9].ToString() == "备注")
            {
                //直接设为Excel实体
                for (int i = 1; i < (t.Rows.Count); i++)
                {
                    MyExcelClass create_s = new MyExcelClass();
                    create_s.StuName = t.Rows[i][0].ToString();
                    create_s.StuSex = t.Rows[i][1].ToString();
                    create_s.StuPhone = t.Rows[i][2].ToString();
                    create_s.StuSchoolName = t.Rows[i][3].ToString();//学校
                    create_s.StuAddress = t.Rows[i][4].ToString();//家庭住址
                    create_s.Region_id = t.Rows[i][5].ToString();//区域
                    create_s.StuInfomationType_Id = t.Rows[i][6].ToString();//信息来源
                    create_s.StuEducational = t.Rows[i][7].ToString();
                    create_s.EmployeesInfo_Id = t.Rows[i][8].ToString();//备案人
                    create_s.Reak = t.Rows[i][9].ToString();//备注
                    new_listStudent.Add(create_s);
                }
                return new_listStudent;
            }
            else
            {
                return new_listStudent;
            }
        }
        //文件上传页面
        public ActionResult ExcleIntoView()
        {
            return View();
        }
        //处理文件上传的方法
        public ActionResult IntoFunction()
        {
            StringBuilder ProName = new StringBuilder();
            try
            {
                HttpPostedFileBase file = Request.Files["file"];
                string fname = Request.Files["file"].FileName; //获取上传文件名称（包含扩展名）
                string f = Path.GetFileNameWithoutExtension(fname);//获取文件名称
                string name = Path.GetExtension(fname);//获取扩展名
                string pfilename = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/ConsultUploadfile/";//获取当前程序集下面的uploads文件夹中的excel文件夹目录
                //获取当前上传的操作人
                string UserName= Base_UserBusiness.GetCurrentUser().UserName;
                string completefilePath = f + DateTime.Now.ToString("yyyyMMddhhmmss")+UserName + name;//将上传的文件名称转变为当前项目名称
                ProName.Append(Path.Combine(pfilename, completefilePath));//合并成一个完整的路径;
                file.SaveAs(ProName.ToString());//上传文件   
                SessionHelper.Session["filename"] = ProName.ToString();
                List<MyExcelClass> studentlist = GetExcel();              
                if (studentlist.Count>0)//如果拿到值说明文件格式是可以读取的
                {                     
                    var jsondata = new
                    {
                        code = "",
                        msg = "ok",
                        data = studentlist,
                    };
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
                else //该文件格式不正确
                {
                    var jsondata = new
                    {
                        code = "",
                        msg = "文件格式错误",
                        data = "",
                    };
                    DeleteFile();//如果格式不符合规范则删除上传的文件
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
                 
            }
            catch (Exception ee)
            {
                BusHelper.WriteSysLog(ee.Message, EnumType.LogType.上传文件);
                return Json("no", JsonRequestBehavior.AllowGet);
            }

        }                 
        //判断是否有重复的值
        public List<MyExcelClass> Repeatedly(bool IsRepea)
        {
            List<MyExcelClass> ExcelList = GetExcel();//获取Excle文件数据     
            List<StudentPutOnRecord> All_list = s_Entity.GetList();//获取备案数据
            List<MyExcelClass> result = new List<MyExcelClass>();                           
                foreach (StudentPutOnRecord a1 in All_list)
                {
                    foreach (MyExcelClass a2 in ExcelList)
                    {
                        if (a1.StuName == a2.StuName && a1.StuPhone == a2.StuPhone)
                        {
                            result.Add(a2);
                        }
                    }
                }
            if (IsRepea)
            {
                //获取冲突的数据
                return result;
            }
            else
            {
                //获取没有冲突的数据                
                for (int i = 0; i < result.Count; i++)
                {
                    ExcelList.Remove(result[i]);
                }
                return ExcelList;
            }             
            
        }
        //将文件中的内容写入到数据库中
        public ActionResult IntoServer()
        {
            try
            {
                List<MyExcelClass> equally_list = Repeatedly(true); //两个数据集合之间比较取交集(挑出相同的)
                if (equally_list.Count > 0)
                {
                    //将未重复的数据添加到数据库中
                    List<MyExcelClass> equally_list2 = Repeatedly(false);
                    //添加数据
                    bool mis = AddExcelToServer(equally_list2);
                    if (mis)
                    {
                        //有重复的值                                
                        //获取当前年月日
                        string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + equally_list[1].EmployeesInfo_Id + "ErrorExcel.xls";
                        string path = "~/uploadXLSXfile/ConsultUploadfile/ConflictExcel/" + filename;
                        SessionHelper.Session["filename2"] = path;
                        //获取表头数据
                        string jsonfile = Server.MapPath("/Config/MyExcelClass.json");
                        System.IO.StreamReader file = System.IO.File.OpenText(jsonfile);
                        JsonTextReader reader = new JsonTextReader(file);
                        //转化为JObject
                        JObject ojb = (JObject)JToken.ReadFrom(reader);

                        var jj = ojb["MyExcelClass"].ToString();

                        JObject jo = (JObject)JsonConvert.DeserializeObject(jj);
                        DataTable user = equally_list.ToDataTable<MyExcelClass>();
                        //生成字段名称 
                        List<string> Head = new List<string>();
                        foreach (DataColumn col in user.Columns)
                        {
                            Head.Add(jo[col.ColumnName].ToString());
                        }
                        bool s = Excel_Entity.DaoruExcel(equally_list, Server.MapPath(path), Head);//将有重复的数据写入Excel表格中
                        if (s)
                        {
                            //成功写入,两个数据进行对比
                            //获取已备案的数据集合
                            var datajson = new
                            {
                                resut="okk",
                                msg = "其他数据已导入,以下是有冲突的数据",
                                old = SercherStudent(equally_list),
                                news = equally_list
                            };
                            string number = "13204961361";
                            string smsText = "备案提示:已备案成功，但是有重复数据";
                            string t = PhoneMsgHelper.SendMsg(number, smsText);
                            return Json(datajson, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //写入出错
                            DeleteFile();
                            var datajson = new
                            {
                                resut = "no",
                                msg = "系统错误,请重试！！！",
                                
                            };
                            return Json(datajson, JsonRequestBehavior.AllowGet);                           
                        }
                    }
                    else
                    {
                        var datajson = new
                        {
                            resut = "no",
                            msg = "系统错误,请重试！！！",

                        };
                        return Json(datajson, JsonRequestBehavior.AllowGet);
                    }
               
                }
                else
                {
                    //没有重复的值
                    List<MyExcelClass> nochongfu_list = GetExcel();
                    if(AddExcelToServer(nochongfu_list))
                    {
                        var datajson = new
                        {
                            resut = "ok",
                        };
                        //通知备案人备案成功
                        string number = "13204961361";
                        string smsText = "备案提示:已备案成功";
                        string t = PhoneMsgHelper.SendMsg(number, smsText);

                        return Json(datajson, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var datajson = new
                        {
                            resut = "no",
                            msg = "系统错误,请重试！！！",

                        };
                        return Json(datajson, JsonRequestBehavior.AllowGet);
                    }
                     
                }

            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(UserName+"Excel大批量导入数据时出现:"+ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                var datajson = new
                {
                    resut = "no",
                    msg = "系统错误,请重试！！！",

                };
                return Json(datajson, JsonRequestBehavior.AllowGet);
            }                       
        }
        
        /// <summary>
        /// 模板下载 
        /// </summary>
        /// <returns></returns>        
        public FileStreamResult DownFile()
        {           
            string rr = Server.MapPath("/uploadXLSXfile/Template/Excle模板.xls");  //获取下载文件的路径         
            FileStream stream = new FileStream(rr, FileMode.Open);
             return File(stream, "application/octet-stream", Server.UrlEncode("ExcleTemplate.xls"));
        }
         
        #endregion

        #region 基础数据的添加（信息类型、学生状态）
        //这是一个显示所有信息类型与学生状态的主页面
        public ActionResult InfomationAndStatuView()
        {
            return View();
        }
        //获取信息类型所有的数据
        public ActionResult GetInfomationData(int page,int limit)
        {
            List<StuInfomationType> infomation_List = StuInfomationType_Entity.GetList().OrderByDescending(i=>i.Id).ToList();
            List<StuInfomationType> infomation_List2 = infomation_List.Skip((page - 1) * limit).Take(limit).ToList();
            var Jsondata = new {
                code = 0, //解析接口状态,
                msg = "", //解析提示文本,
                count = infomation_List.Count, //解析数据长度
                data = infomation_List2 //解析数据列表
            };
            return Json(Jsondata,JsonRequestBehavior.AllowGet);
        }
        //修改信息类型
        public ActionResult EditInfomationType(string id,string name,string state)
        {
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            int Id = Convert.ToInt32(id);
            
            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    StuInfomationType findinfomation = StuInfomationType_Entity.GetList().Where(i => i.Id == Id).FirstOrDefault();
                    StuInfomationType Isrepeat = StuInfomationType_Entity.GetList().Where(i => i.Name==name).FirstOrDefault();
                    if (findinfomation != null && Isrepeat==null)
                    {
                        findinfomation.Name = name;
                        StuInfomationType_Entity.Update(findinfomation);
                    }else if (findinfomation==null)
                    {
                        BusHelper.WriteSysLog("操作人:" + UserName + "操作时出现:数据未能查找到该值", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                        return Json("数据有误，请重试!", JsonRequestBehavior.AllowGet);
                    }else if (Isrepeat != null)
                    {
                        return Json("数据重复", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {  
                    //将错误填写到日志中     
                    BusHelper.WriteSysLog("操作人:" + UserName + "操作时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                    return Json("no", JsonRequestBehavior.AllowGet);
                }
                
            }
            if (!string.IsNullOrEmpty(state))
            {
                StuInfomationType findinfomation = StuInfomationType_Entity.GetList().Where(i => i.Id == Id).FirstOrDefault();
                if (findinfomation != null)
                {
                    try
                    {
                        bool Isdel = Convert.ToBoolean(state)==true?false:true;
                        findinfomation.IsDelete = Isdel; 
                        StuInfomationType_Entity.Update(findinfomation);
                    }
                    catch (Exception ex)
                    {
                        //将错误填写到日志中     
                        BusHelper.WriteSysLog("操作人:"+UserName+"操作时出现:"+ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                        return Json("修改错误，请重试", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            BusHelper.WriteSysLog("操作人:" + UserName + "编辑成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        //添加信息类型
        public ActionResult AddInfomationType()
        {
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            StuInfomationType findIsNull = StuInfomationType_Entity.GetList().Where(i => i.Name == null).FirstOrDefault();
            try
            {
                if (findIsNull==null)
                {
                    StuInfomationType_Entity.Insert(new StuInfomationType() { IsDelete = false });
                    BusHelper.WriteSysLog("操作人:" + UserName + "触发了添加学生信息类型来源的按钮", Entity.Base_SysManage.EnumType.LogType.加载数据);
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("有信息未填写，请填写之后在添加", JsonRequestBehavior.AllowGet);
                }                 
            }
            catch (Exception ex)
            {
                //将错误填写到日志中     
                BusHelper.WriteSysLog("操作人:" + UserName + "操作时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);
                return Json("修改错误，请重试", JsonRequestBehavior.AllowGet);
            }             
        }
        //删除没有名称的数据
        public void DelteInfomationType()
        {
            //删除信息来源
            List<StuInfomationType> findIsNull = StuInfomationType_Entity.GetList().Where(i => i.Name == null).ToList();
            if (findIsNull.Count>0)
            {
                foreach (StuInfomationType item in findIsNull)
                {
                    StuInfomationType_Entity.Delete(item);
                }
            }
            //删除学生状态
            List<StuStatus> findStateNull = Stustate_Entity.GetList().Where(i=>i.StatusName==null).ToList();
            if (findStateNull.Count > 0)
            {
                foreach (StuStatus item2 in findStateNull)
                {
                    Stustate_Entity.Delete(item2);
                }
            }
        }
        //获取学生状态的所有数据
        public ActionResult GetStatusData(int page,int limit)
        {
            List<StuStatus> state_list = Stustate_Entity.GetList();
            List<StuStatus> newstate = state_list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var Jsondata = new
            {
                code = 0, //解析接口状态,
                msg = "", //解析提示文本,
                count = state_list.Count, //解析数据长度
                data = newstate //解析数据列表
            };
            return Json(Jsondata, JsonRequestBehavior.AllowGet);
        }
        //修改状态
        public ActionResult EditStates(string id,string name,string state)
        {
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            try
            {
                int Id = Convert.ToInt32(id);
                StuStatus findstate= Stustate_Entity.GetEntity(Id);
                StuStatus IsRepart = Stustate_Entity.GetList().Where(s => s.StatusName == name).FirstOrDefault();
                if (!string.IsNullOrEmpty(name) && findstate!=null && IsRepart==null)
                {
                    findstate.StatusName = name;
                    Stustate_Entity.Update(findstate);                     
                }
                else if(IsRepart != null)
                {
                    return Json("数据重复!!!", JsonRequestBehavior.AllowGet);
                }
                else if (findstate==null)
                {
                    return Json("数据错误，请重试!", JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(state))
                {
                    bool states = Convert.ToBoolean(state);
                    findstate.IsDelete = states==true?false:true;
                    Stustate_Entity.Update(findstate);
                }
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("操作人:" + UserName + "出现:"+ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                return Json("系统异常，请联系管理员", JsonRequestBehavior.AllowGet);
            }
            BusHelper.WriteSysLog("操作人:" + UserName + "编辑成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        //添加
        public ActionResult AddStates()
        {
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            List<StuStatus> Isrepert = Stustate_Entity.GetList().Where(s=>s.StatusName==null).ToList();
            try
            {
                if (Isrepert.Count>0)
                {
                    return Json("有信息未填写，请填写之后在添加", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Stustate_Entity.Insert(new StuStatus() { IsDelete = false });
                }             
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("操作人:" + UserName + "出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                return Json("系统异常，请联系管理员", JsonRequestBehavior.AllowGet);
            }
            BusHelper.WriteSysLog("操作人:" + UserName + "触发了添加按钮", Entity.Base_SysManage.EnumType.LogType.添加数据);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        #endregion
                
    }
}