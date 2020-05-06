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
using SiliconValley.InformationSystem.Business.DepartmentBusiness; //获取岗位信息实体 
using SiliconValley.InformationSystem.Business.PositionBusiness;//获取岗位实体
using SiliconValley.InformationSystem.Entity.ViewEntity;//获取员工岗位部门实体
using SiliconValley.InformationSystem.Entity.Entity;
using System.Text;
using System.IO;
using System.Data;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Util;
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
using SiliconValley.InformationSystem.Business.RegionManage;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    //zheshi
    [CheckLogin]
    public class StudentDataKeepController : BaseMvcController
    {
        // GET: /Market/StudentDataKeep/FielsPath
        #region 创建实体
        //创建一个用于操作数据的备案实体
        private StudentDataKeepAndRecordBusiness s_Entity =new StudentDataKeepAndRecordBusiness();
        //创建一个用于查询数据的上门学生状态实体
        StuStateManeger Stustate_Entity;
        //创建一个用于查询数据的上门学生信息来源实体
        StuInfomationTypeManeger StuInfomationType_Entity; 
        //创建一个用于查询数据的部门信息实体
        DepartmentManage Department_Entity;
        //创建一个用于查询岗位信息实体
        PositionManage Position_Entity;
        //创建区域业务类
        //创建一个用于查询区域的实体
        RegionManeges region_Entity;

        ExcelHelper Excel_Entity;

        Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
        
        #endregion

        //这是一个数据备案的主页面
        public ActionResult StudentDataKeepIndex()
        {
            StuInfomationType_Entity = new StuInfomationTypeManeger();
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
            var r_list= s_Entity.GetEffectiveRegionAll(true).Select(r => new SelectListItem { Text = r.RegionName, Value = r.ID.ToString() }).ToList();
            r_list.Add(newselectitem);
            ViewBag.are = r_list;
            return View();
        }

        //往数据库中获取数据备案的信息
        public ActionResult GetStudentPutOnRecordData(int limit,int page)
        { 
            region_Entity = new RegionManeges();
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            Stustate_Entity = new StuStateManeger();
            EmployeesInfoManage Enplo_Entity = new EmployeesInfoManage();
            Department department= Enplo_Entity.GetDeptByEmpid(UserName.EmpNumber);
            #region 取值
            string findNamevalue = Request.QueryString["findNamevalue"];
            string findPhonevalue = Request.QueryString["findPhonevalue"];
            string findInformationvalue = Request.QueryString["findInformationvalue"];
            string findStartvalue = Request.QueryString["findStartvalue"];
            string findEndvalue = Request.QueryString["findEndvalue"];
            string findBeanManvalue = Request.QueryString["findBeanManvalue"];
            string findAreavalue = Request.QueryString["findAreavalue"];
            #endregion
            List<StudentPutOnRecord> stu_IQueryable;
            #region 判断是网路部人员登录还是咨询部人员登录

            StuInfomationType find_type = StuInfomationType_Entity.GetNameSearchId("网络招生");
            if (department.DeptName=="咨询部")
            {
                //获取网络招生的数据
                stu_IQueryable = s_Entity.GetAllStudentKeepData().OrderByDescending(s => s.Id).Where(s=>s.StuInfomationType_Id!= find_type.Id).ToList();
            }
            else if(department.DeptName == "网络部")
            {
                //获取除了网络招生以外的数据
                stu_IQueryable = s_Entity.GetAllStudentKeepData().OrderByDescending(s => s.Id).Where(s => s.StuInfomationType_Id == find_type.Id).ToList();
            }
            else
            {
                stu_IQueryable = s_Entity.GetAllStudentKeepData().OrderByDescending(s => s.Id).ToList();
            }
            #endregion
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
                string empyeid =s_Entity.GetNameSreachEmploId(Beanname)==null?null: s_Entity.GetNameSreachEmploId(Beanname).EmployeeId;
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
                    StuInfomationType_Id = StuInfomationType_Entity.GetEntity(s.StuInfomationType_Id)==null?null: StuInfomationType_Entity.GetEntity(s.StuInfomationType_Id).Name,
                    StuStatus_Id = Stustate_Entity.GetEntity(s.StuStatus_Id)==null?null: Stustate_Entity.GetEntity(s.StuStatus_Id).StatusName,
                    StuIsGoto = s.StuIsGoto,
                    StuVisit = s.StuVisit,
                    EmployeesInfo_Id =s_Entity.GetEmployeeValue(s.EmployeesInfo_Id, false),
                    StuDateTime = s.StuDateTime,
                    StuEntering = s.StuEntering,
                    Reak = s.Reak,
                    Regin_id = s.Region_id,
                    ReginName = region_Entity.GetEntity(s.Region_id) == null?"区域外":region_Entity.GetEntity(s.Region_id).RegionName
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
       
        //这是一个添加数据的页面
        public ActionResult AddorEdit(string id)
        {
            Stustate_Entity = new StuStateManeger();
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            //获取信息来源的所有数据
            ViewBag.infomation = StuInfomationType_Entity.GetList().Where(s=>s.IsDelete==false).Select(s=>new SelectListItem { Text=s.Name, Value=s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
            List<SelectListItem> ss = Stustate_Entity.GetList().Where(s => s.IsDelete == false && !s.StatusName.Contains("已报名") && !s.StatusName.Contains("未报名")).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();
            SelectListItem s1 = new SelectListItem() { Value = "-1", Text = "请选择", Selected = true };
            ss.Add(s1);
            ViewBag.state = ss;
            //获取所有区域
            SelectListItem s2 = new SelectListItem() { Text = "区域外", Value = "区域外" };
             var r_list = s_Entity.GetEffectiveRegionAll(true).Select(r=>new SelectListItem { Text=r.RegionName,Value=r.ID.ToString()}).ToList();
             r_list.Add(s2);
            ViewBag.area = r_list;


            return View();
        }

        //将所有员工显示给用户选择
        public ActionResult ShowEmployeInfomation()
        {
            Department_Entity = new DepartmentManage();
            Position_Entity = new PositionManage();
            List<EmployeesInfo> list_Enploy = s_Entity.GetEffectiveEmpAll(true);//获取所有在职员工
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
            EmployeesInfo finde= s_Entity.GetEffectiveEmpAll(true).Where(s => s.EmployeeId==id).FirstOrDefault();
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
            Stustate_Entity = new StuStateManeger();
            AjaxResult a ;
            try
            {
                 //判断是否有姓名相同的备案数据
                 StudentPutOnRecord er= s_Entity.GetAllStudentKeepData().Where(s => s.StuName == news.StuName && s.StuPhone == news.StuPhone).FirstOrDefault();
                if (er==null)
                {
                    news.StuDateTime = DateTime.Now;
                    news.IsDelete = false;
                    news.StuEntering = UserName.EmpNumber;
                    if (news.StuStatus_Id==-1 || news.StuStatus_Id==null)
                    {
                       AjaxResult a1  = Stustate_Entity.GetStu("未报名");
                        if (a1.Success==true)
                        {
                            StuStatus find_status = a1.Data as StuStatus;
                            news.StuStatus_Id = find_status.Id;
                        }                         
                    }
                       a= s_Entity.Add_data(news);
                    if (a.Success==true)
                    {
                        //通知备案人备案成功
                        //string number = "13204961361";//根据备案人查询电话号码
                        //string smsText = "备案提示:已备案成功";
                        //string t = PhoneMsgHelper.SendMsg(number, smsText);
                    }                                                                                                             
                }
                else
                {
                    a = new AjaxResult();
                    a.Success = false;
                    a.Msg = "该学生已备案";
                }              
                return Json(a);    
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
            AjaxResult a = new AjaxResult();
            List<StudentPutOnRecord> fins= s_Entity.GetAllStudentKeepData().Where(s=>s.StuName==id).ToList();
            if (fins.Count>0)
            {
                a.Data = fins;
                a.Success = false;              
            }
            else
            {
                a.Success = true;               
            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        //创建一个编辑页面
        public ActionResult EditView(string id)
        {
            Stustate_Entity = new StuStateManeger();
            StuInfomationType_Entity = new StuInfomationTypeManeger();
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
            var r_list = s_Entity.GetEffectiveRegionAll(true).Select(r => new SelectListItem { Text = r.RegionName, Value = r.ID.ToString() }).ToList();
            r_list.Add(s2);
            ViewBag.area = r_list;
            return View();
        }

        //创建一个用于编辑的处理方法
        public ActionResult EditFunction(StudentPutOnRecord olds)
        {
            //需要判断是咨询部人员修改还是网络部人员修改  SessionHelper.Session["UserId"]=""
            AjaxResult a= s_Entity.Update_data(olds);
            return Json(a,JsonRequestBehavior.AllowGet);
        }
 
        //将Excel中的数据加载到数据库中
        public bool AddExcelToServer(List<MyExcelClass> list)
        {
            region_Entity = new RegionManeges();
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            Stustate_Entity = new StuStateManeger();
            bool Is = false;
            try
            {
                foreach (MyExcelClass item1 in list)
                {
                    StudentPutOnRecord s = new StudentPutOnRecord();
                    s.StuName = item1.StuName;
                    s.StuSex = item1.StuSex == "男" ? true : false;
                    s.EmployeesInfo_Id =s_Entity.GetNameSreachEmploId(item1.EmployeesInfo_Id)==null?null: s_Entity.GetNameSreachEmploId(item1.EmployeesInfo_Id).EmployeeId;
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
                    s.StuEntering = UserName.EmpNumber;
                    StuInfomationType find_sinfomation= StuInfomationType_Entity.SerchSingleData(item1.StuInfomationType_Id, false);
                    if (find_sinfomation!=null)
                    {
                        s.StuInfomationType_Id = find_sinfomation.Id;
                    }                   
                    s.StuIsGoto = false;
                    s.StuPhone = item1.StuPhone;
                    s.StuSchoolName = item1.StuSchoolName;
                    s.StuStatus_Id = Stustate_Entity.GetIdGiveName("未报名", false).Success==true? (Stustate_Entity.GetIdGiveName("未报名", false).Data as StuStatus).Id:-1;
                    AjaxResult add_result= s_Entity.Add_data(s);                     
                }
                Is = true;
            }
            catch (Exception)
            {
                //BusHelper.WriteSysLog(UserName + "Excel大批量导入数据时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return Is;
        }
        //根据ID找到学生信息并赋值
        public ActionResult FindStudentInfomation(string id)
        {
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            Stustate_Entity = new StuStateManeger();
            region_Entity = new RegionManeges();
            if (!string.IsNullOrEmpty(id) && id!="undifind")
            {
               StudentPutOnRecord finds = s_Entity.GetAllStudentKeepData().Where(s=>s.Id==Convert.ToInt32(id)).FirstOrDefault();
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
                    e_Name = s_Entity.GetEmployeeValue(finds.EmployeesInfo_Id, false),
                    StuEntering_1 = s_Entity.GetEmployeeValue(finds.StuEntering, false),
                    InfomationTypeName = StuInfomationType_Entity.GetEntity(finds.StuInfomationType_Id) == null ? "未定义" : StuInfomationType_Entity.GetEntity(finds.StuInfomationType_Id).Name,
                    StatusName = Stustate_Entity.GetEntity(finds.StuStatus_Id) == null ? "未填写" : Stustate_Entity.GetEntity(finds.StuStatus_Id).StatusName,
                    Region_id = finds.Region_id == null ? "区域外" : region_Entity.GetEntity(finds.Region_id).ID.ToString()
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
            Stustate_Entity = new StuStateManeger();
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            int s_id = Convert.ToInt32(id);
            StudentPutOnRecord find= s_Entity.GetEntity(s_id);
            region_Entity = new RegionManeges();
            ViewBag.id = id;
            //获取信息来源的所有数据
            ViewBag.infomation = StuInfomationType_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
            ViewBag.state = Stustate_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();

            //获取区域
            ViewBag.regin = region_Entity.GetEntity(find.Region_id)==null?"区域外": region_Entity.GetEntity(find.Region_id).RegionName;
            return View();
        }

        //显示有疑似的数据
        public ActionResult StudentSomeData(string id)
        {
            StuInfomationType_Entity = new StuInfomationTypeManeger();
            region_Entity = new RegionManeges();
            Stustate_Entity = new StuStateManeger();
            List<StudentPutOnRecord> list = s_Entity.GetAllStudentKeepData().Where(s=>s.StuName==id).ToList();
            var data = list.Select(s => new StudentData
            {
                stuSex = s.StuSex == true ? "男" : "女",
                StuName = s.StuName,
                StuPhone = s.StuPhone,
                StuSchoolName = s.StuSchoolName,
                StuAddress = s.StuAddress,
                StuInfomationType_Id = StuInfomationType_Entity.GetEntity(s.StuInfomationType_Id).Name,
                StuStatus_Id =Stustate_Entity.GetEntity(s.StuStatus_Id).StatusName,
                StuIsGoto = s.StuIsGoto==false?"否":"是",
                StuVisit=s.StuVisit,
                EmployeesInfo_Id=s_Entity.GetEmployeeValue(s.EmployeesInfo_Id,true),
                StuDateTime=s.StuDateTime,
                StuEntering =s_Entity.GetEmployeeValue(s.StuEntering,true),
                AreName= region_Entity.GetAreValue(s.Region_id.ToString(),true).RegionName
            }).ToList();
            ViewBag.Student =data;
            return View(data);
        }
       
        #region Excle文件导入
        //获取已备案中的相似数据
        public List<MyExcelClass> SercherStudent(List<MyExcelClass> ex)
        {
            region_Entity = new RegionManeges();
            List<StudentPutOnRecord> s_list = s_Entity.GetAllStudentKeepData();
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
                        m.StuInfomationType_Id = StuInfomationType_Entity.GetEntity(item2.StuInfomationType_Id)==null?null: StuInfomationType_Entity.GetEntity(item2.StuInfomationType_Id).Name;
                        m.Region_id =region_Entity.SerchRegionName(item2.Region_id.ToString(),true).RegionName;
                        m.StuEducational = item2.StuEducational;
                        m.StuAddress = item2.StuAddress;
                        m.Reak = item2.Reak;
                        m.EmployeesInfo_Id =s_Entity.GetEmployeeValue(item2.EmployeesInfo_Id,false);
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
           
        }
        //获取Excle文件中的值
        public List<StudentPutOnRecord> GetExcelFunction()
        {
            region_Entity = new RegionManeges();
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
                        Region find_region = region_Entity.GetAreValue(t.Rows[i][5].ToString(), false);
                        if (find_region!=null)
                        {
                        create_s.Region_id = find_region.ID;
                        }                       
                        create_s.StuInfomationType_Id = StuInfomationType_Entity.GetNameSearchId(t.Rows[i][6].ToString())==null?0: StuInfomationType_Entity.GetNameSearchId(t.Rows[i][6].ToString()).Id;//信息来源
                        create_s.StuEducational = t.Rows[i][7].ToString() == null ? "初中" : t.Rows[i][7].ToString();
                        create_s.EmployeesInfo_Id =s_Entity.GetNameSreachEmploId(t.Rows[i][8].ToString())==null?null: s_Entity.GetNameSreachEmploId(t.Rows[i][8].ToString()).EmployeeId;//备案人
                        create_s.Reak = t.Rows[i][9].ToString();//备注
                        create_s.StuIsGoto = false;
                        create_s.StuStatus_Id = 1;
                        new_listStudent.Add(create_s);
                    }
                DeleteFile();
                return new_listStudent;
            }
            else
            {
                DeleteFile();
                return new_listStudent;
            }
        }
        //转型为Excle集合(目的:有些是区域外的，会被处理)
        public bool GetExcel()
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
                DeleteFile();
                SessionHelper.Session["ExcelData"] = new_listStudent;
                return true;
            }
            else
            {
                DeleteFile();
                return false;
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
                bool studentlist = GetExcel();
                if (studentlist)//说明文件格式是可以读取的
                {                     
                    var jsondata = new
                    {
                        code = "",
                        msg = "ok",
                        data = SessionHelper.Session["ExcelData"] as List<MyExcelClass>,
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
            List<MyExcelClass> ExcelList = SessionHelper.Session["ExcelData"] as List<MyExcelClass>;//获取Excle文件数据     
            List<StudentPutOnRecord> All_list = s_Entity.GetAllStudentKeepData();//获取备案数据
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
            Excel_Entity = new ExcelHelper();
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
                        string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + equally_list[1].EmployeesInfo_Id + ".xls";
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
                            //string number = "13204961361";
                            //string smsText = "备案提示:已备案成功，但是有重复数据";
                            //string t = PhoneMsgHelper.SendMsg(number, smsText);
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
                    List<MyExcelClass> nochongfu_list = SessionHelper.Session["ExcelData"] as List<MyExcelClass>;
                    if(AddExcelToServer(nochongfu_list))
                    {
                        var datajson = new
                        {
                            resut = "ok",
                        };
                        //通知备案人备案成功
                        //string number = "13204961361";
                        //string smsText = "备案提示:已备案成功";
                        //string t = PhoneMsgHelper.SendMsg(number, smsText);

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
               // BusHelper.WriteSysLog(s_Entity.Enplo_Entity.GetEntity(UserName).EmpName +"Excel大批量导入数据时出现:"+ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
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


        #region 疑似数据业务
        public ActionResult SuspectedView()
        {
            return View();
        }
        
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        public void LoadFile()
        {

            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id) && id!= "undefined")
            {                 
                        string path = Server.MapPath("~/uploadXLSXfile/ConsultUploadfile/ConflictExcel/" + id);

                        FileStream fs = new FileStream(path, FileMode.Open);
                        byte[] bytes = new byte[(int)fs.Length];
                        fs.Read(bytes, 0, bytes.Length);
                        fs.Close();
                        //二进制流数据（如常见的文件下载）
                        Response.ContentType = "application/octet-stream";
                        //通知浏览器下载文件而不是打开 
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(id, System.Text.Encoding.UTF8));
                        Response.BinaryWrite(bytes);
                        Response.Flush();
                        Response.End();
                    
            }             
        }
       
        public ActionResult PageData()
        {
            int page = Convert.ToInt32(Request.Form["page"]);
            int limit = Convert.ToInt32(Request.Form["limit"]);
            string filename = Request.Form["filename"];
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Server.MapPath("~/uploadXLSXfile/ConsultUploadfile/ConflictExcel"));
            FileInfo[] files = di.GetFiles();
            List<SelectListItem> FilesName = new List<SelectListItem>();
            foreach (FileInfo f in files)
            {
                SelectListItem s = new SelectListItem();
                int lenth = f.Name.Length;
                s.Text = f.Name.Substring(0, 4) + "年" + f.Name.Substring(4, 2) + "月" + f.Name.Substring(6, 2) + "日" + f.Name.Substring(14, lenth - 14);
                s.Value = f.Name;
                FilesName.Add(s);
            }

            if (!string.IsNullOrEmpty(filename))
            {
                FilesName = FilesName.Where(f => f.Value.Contains(filename)).ToList();
            }
            int count= Convert.ToInt32(Math.Ceiling(FilesName.Count / 12.0));
            List<SelectListItem> selectdata  = FilesName.Skip(((Convert.ToInt32(page) - 1) * Convert.ToInt32(limit))).Take(Convert.ToInt32(limit)).ToList();

            var data = new { count= count ,data= selectdata ,page=page};

            return Json(data,JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult FielsPath()
        {
            string name = Request.Form["id"];
            List<string> list = new List<string>();
            string[] names = name.Split(',');
            foreach (string id in names)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    list.Add(Server.MapPath("~/uploadXLSXfile/ConsultUploadfile/ConflictExcel/" + id));
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}