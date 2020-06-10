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
//using SiliconValley.InformationSystem.Business.NetClientRecordBusiness;
using System.Threading;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using System.Text.RegularExpressions;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    //zheshi
    [CheckLogin]
    public class StudentDataKeepController : BaseMvcController
    {
        // GET: /Market/StudentDataKeep/ZhipaiConsustTacher
        #region 创建实体
        //创建一个用于操作数据的备案实体
        private StudentDataKeepAndRecordBusiness s_Entity = new StudentDataKeepAndRecordBusiness();

        ExcelHelper Excel_Entity;

        Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
       
        
        #endregion


        #region 数据操作
        //这是一个数据备案的主页面
        public ActionResult StudentDataKeepIndex()
        {
            //获取信息来源的所有数据
            List<SelectListItem> se = s_Entity.StuInfomationType_Entity.GetList().Select(s => new SelectListItem { Text = s.Name, Value = s.Name }).ToList();
            se.Add(new SelectListItem() { Text = "请选择", Selected = true, Value = "Value" });
            ViewBag.infomation = se;
            //获取区域所有信息
            SelectListItem newselectitem = new SelectListItem() { Text = "请选择", Value = "请选择", Selected = true };
            var r_list = s_Entity.GetEffectiveRegionAll(true).Select(r => new SelectListItem { Text = r.RegionName, Value = r.RegionName }).ToList();
            r_list.Add(newselectitem);
            ViewBag.are = r_list;
            //获取咨询师的所有数据
            List<SelectListItem> list_cteacher = new List<SelectListItem>();
            List<SelectListItem> list_one = new List<SelectListItem>();
            list_cteacher.Add(new SelectListItem() { Text = "请选择", Value = "0", Selected = true });
            list_one.Add(new SelectListItem() { Text = "请选择", Value = "0", Selected = true });
            list_cteacher.AddRange(EmployandCounTeacherCoom.getallCountTeacher(true).Select(c => new SelectListItem() { Text = c.empname, Value = c.empname }).ToList());
            list_one.AddRange(EmployandCounTeacherCoom.GetTeacher().Select(c=>new SelectListItem() { Text=c.Employees_Id,Value=c.Id.ToString()}).ToList());
            ViewBag.teacherlist = list_cteacher;
            ViewBag.Teacher = list_one;
            //获取学生状态所有数据
            List<SelectListItem> ss = new List<SelectListItem>();
            ss.Add(new SelectListItem() { Value = "0", Text = "请选择", Selected = true });
            ss.AddRange(s_Entity.Stustate_Entity.GetList().Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusName }).ToList());

            ViewBag.slist = ss;

            ViewBag.Pers = s_Entity.GetPostion(UserName.EmpNumber);
            return View();
        }
 
        public ActionResult TableData(int limit, int page)
        {
            List<ExportStudentBeanData> list = s_Entity.GetSudentDataAll().OrderByDescending(s => s.Id).ToList();

            var data = list.Skip((page - 1) * limit).Take(limit).ToList();

            var josndata = new { code = 0, count = list.Count, data = data };

            return Json(josndata, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetTableData(int limit, int page)
        {
            string str1 = "select * from StudentBeanView where 1=1 ";
            string str2 = "select * from Sch_MarketView where 1=1 ";
            #region 模糊查询
            string findNamevalue = Request.QueryString["findNamevalue"];//姓名
            string findPhonevalue = Request.QueryString["findPhonevalue"];//电话
            string findInformationvalue = Request.QueryString["findInformationvalue"];//信息来源
            string findStartvalue = Request.QueryString["findStartvalue"];//录入开始时间
            string findEndvalue = Request.QueryString["findEndvalue"];//录入结束时间
            string findBeanManvalue = Request.QueryString["findBeanManvalue"];//备案人
            string findAreavalue = Request.QueryString["findAreavalue"];//区域
            string findTeacher = Request.QueryString["S_consultTeacher"];//咨询师
            string findStatus = Request.QueryString["S_status"];//备案状态
            string findPary = Request.QueryString["S_party"];//关系人
            string findCreateMan = Request.QueryString["S_intosysMan"];//录入人

            if (!string.IsNullOrEmpty(findNamevalue))
            {
                str1 = str1 + " and StuName like  '%"+ findNamevalue + "%'";
                str2 = str2 + " and StudentName like  '%" + findNamevalue + "%'";
            }
            if (!string.IsNullOrEmpty(findPhonevalue))
            {
                str1 = str1 + " and Stuphone = '" + findPhonevalue + "'";
                str2 = str2 + " and Phone = '" + findPhonevalue + "'";
            }
            if (findInformationvalue != "Value" && !string.IsNullOrEmpty(findInformationvalue))
            {
                str1 = str1 + " and stuinfomation = '" + findInformationvalue + "'";
                str2 = str2 + " and source = '" + findInformationvalue + "'";
            }
            if (!string.IsNullOrEmpty(findBeanManvalue))
            {
                str1 = str1 + " and empName = '" + findBeanManvalue + "'";
                str2 = str2 + " and SalePerson = '" + findBeanManvalue + "'";
            }
            if (findAreavalue != "请选择" && !string.IsNullOrEmpty(findAreavalue))
            {
                str1 = str1 + " and RegionName = '" + findAreavalue + "'";
                str2 = str2 + " and Area = '" + findAreavalue + "'";
            }
            if (findStatus != "0" && !string.IsNullOrEmpty(findStatus))
            {
                str1 = str1 + " and StatusName = '" + findStatus + "'";
            }
            if (!string.IsNullOrEmpty(findPary))
            {
                str1 = str1 + " and Party = '" + findPary + "'";
                str2 = str2 + " and RelatedPerson = '" + findPary + "'";
            }
            if (!string.IsNullOrEmpty(findCreateMan))
            {
                str1 = str1 + " and StuEntering = '" + findCreateMan + "'";
                str2 = str2 + " and CreateUserName = '" + findCreateMan + "'";
            }

            if (!string.IsNullOrEmpty(findStartvalue))
            {
                str1 = str1 + " and BeanDate >= '" + findStartvalue + "'";
                str2 = str2 + " and CreateDate >= '" + findStartvalue + "'";
            }

            if (!string.IsNullOrEmpty(findTeacher) && findTeacher!="0")
            {
                str1 = str1 + " and ConsultTeacher = '" + findTeacher + "'";
                str2 = str2 + " and Inquiry = '" + findTeacher + "'";
            }

            if (!string.IsNullOrEmpty(findEndvalue))
            {
                str1 = str1 + " and BeanDate <= '" + findEndvalue + "'";
                str2 = str2 + " and CreateDate <= '" + findEndvalue + "'";
            }
            #endregion
                
             
              List<ExportStudentBeanData> list= s_Entity.Serch(str1,str2).OrderByDescending(s => s.Id).ToList();

                var data = list.Skip((page - 1) * limit).Take(limit).ToList();

                var josndata = new { code = 0, count = list.Count, data = data };

                return Json(josndata, JsonRequestBehavior.AllowGet);
             
        }

        //这是一个添加数据的页面
        public ActionResult AddorEdit(string id)
        {
            s_Entity.Stustate_Entity = new StuStateManeger();
            s_Entity.StuInfomationType_Entity = new StuInfomationTypeManeger();
            //获取信息来源的所有数据
            ViewBag.infomation = s_Entity.StuInfomationType_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
            //List<SelectListItem> ss = Stustate_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();
            //SelectListItem s1 = new SelectListItem() { Value = "-1", Text = "请选择", Selected = true };
            //ss.Add(s1);
            //ViewBag.state = ss;
            //获取所有区域
            SelectListItem s2 = new SelectListItem() { Text = "区域外", Value = "区域外" };
            var r_list = s_Entity.GetEffectiveRegionAll(true).Select(r => new SelectListItem { Text = r.RegionName, Value = r.ID.ToString() }).ToList();
            r_list.Add(s2);
            ViewBag.area = r_list;
            List<SelectListItem> infoTeacher = new List<SelectListItem>();
            infoTeacher.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            //获取咨询师              
            infoTeacher.AddRange(EmployandCounTeacherCoom.getallCountTeacher(false).Select(d => new SelectListItem() { Text = d.empname, Value = d.consultercherid.ToString() }).ToList());
            ViewBag.ConsultTeacher = infoTeacher;
            return View();
        }

        //将所有员工显示给用户选择
        public ActionResult ShowEmployeInfomation()
        {
            s_Entity.Department_Entity = new DepartmentManage();
            s_Entity.Position_Entity = new PositionManage();
            List<EmployeesInfo> list_Enploy = s_Entity.GetEffectiveEmpAll(true);//获取所有在职员工
            List<TreeClass> list_Tree = s_Entity.Department_Entity.GetList().Select(d => new TreeClass() { id = d.DeptId.ToString(), title = d.DeptName, children = new List<TreeClass>(), disable = false, @checked = false, spread = false }).ToList();
            List<Position> list_Position = s_Entity.Position_Entity.GetList().Where(s => s.IsDel == false).ToList();//获取所有岗位有用的数据
            foreach (TreeClass item1 in list_Tree)
            {
                List<TreeClass> bigTree = new List<TreeClass>();
                foreach (Position item2 in list_Position)
                {
                    if (item1.id == item2.DeptId.ToString())
                    {
                        foreach (EmployeesInfo item3 in list_Enploy)
                        {
                            if (item3.PositionId == item2.Pid)
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
            return Json(list_Tree, JsonRequestBehavior.AllowGet);
        }

        //树形图
        public ActionResult ShowTree()
        {
            return View();
        }

        //查看是否是选中员工
        public ActionResult FindEmply(string id)
        {
            EmployeesInfo finde = s_Entity.GetEffectiveEmpAll(true).Where(s => s.EmployeeId == id).FirstOrDefault();
            if (finde != null)
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
            s_Entity.Stustate_Entity = new StuStateManeger();
            AjaxResult a;
            try
            {
                //判断是否有姓名相同的备案数据                
                if (s_Entity.StudentOrride(news.StuName, news.StuPhone))
                {
                    news.StuDateTime = DateTime.Now;
                    news.BeanDate = DateTime.Now;
                    news.IsDelete = false;
                    news.StuEntering = s_Entity.Enplo_Entity.GetEntity(UserName.EmpNumber).EmpName;                   
                    AjaxResult a1 = s_Entity.Stustate_Entity.GetStu("未报名");
                    if (a1.Success == true)
                    {
                        StuStatus find_status = a1.Data as StuStatus;
                        news.StuStatus_Id = find_status.Id;
                    }                   
                    if (news.ConsultId != "0")
                    {
                        news.ConsultTeacher = EmployandCounTeacherCoom.ConsultName(Convert.ToInt32(news.ConsultId));
                    }
                    a = s_Entity.Add_data(news);
                    if (a.Success == true)
                    {
                        //通知备案人备案成功
                        string phone= s_Entity.Enplo_Entity.GetEntity(news.EmployeesInfo_Id).Phone;
                        // string number = "13204961361";//根据备案人查询电话号码
                        string smsText = "备案提示:"+ news.StuName + "学生在"+ DateTime.Now + "已备案成功";
                        string t = PhoneMsgHelper.SendMsg(phone, smsText);

                        //判断是否指派了咨询师  

                        if (news.ConsultId != "0")
                        {
                            ExportStudentBeanData find = s_Entity.StudentOrrideData(news.StuName,news.StuPhone);
                            Consult new_c = new Consult();
                            new_c.TeacherName = Convert.ToInt32(news.ConsultId);
                            new_c.StuName =Convert.ToInt32(find.Id);
                            new_c.IsDelete = false;
                            new_c.ComDate = DateTime.Now;
                            a.Success = EmployandCounTeacherCoom.AddConsult(new_c);
                        }
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
            List<ExportStudentBeanData> fins = s_Entity.StudentOrride(id);
            if (fins.Count > 0)
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
            s_Entity.Stustate_Entity = new StuStateManeger();
            s_Entity.StuInfomationType_Entity = new StuInfomationTypeManeger();
            ViewBag.id = id;
            //获取信息来源的所有数据
            ViewBag.infomation =s_Entity.StuInfomationType_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
            //List<SelectListItem> ss = Stustate_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();
            //SelectListItem s1 = new SelectListItem() { Value = "-1", Text = "请选择", Selected = true };
            //ss.Add(s1);
            //ViewBag.state = ss;
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
            AjaxResult a = s_Entity.Update_data(olds);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateGotoSchool()
        {
            List<StudentPutOnRecord> list = new List<StudentPutOnRecord>();
            Request.Form["ids"].Split(',').ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item) && item != "0")
                {
                    int id = Convert.ToInt32(item);
                    list.Add(s_Entity.GetEntity(id));
                }
            });
            DateTime date = Convert.ToDateTime(Request.Form["date"]);
            AjaxResult a = s_Entity.UpdateGotoShcool(list, date);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
      

        //根据ID找到学生信息并赋值
        public ActionResult FindStudentInfomation(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "undifind")
            {
                ExportStudentBeanData finds = s_Entity.findId(id);
                var newdata = new
                {
                    EmployeesInfo_Id =s_Entity.GetEntity(finds.Id).EmployeesInfo_Id,
                    Id = finds.Id,
                    Reak = finds.Reak,
                    StuAddress = finds.StuAddress,
                    StuBirthy = finds.StuBirthy,
                    StuDateTime =s_Entity.GetEntity(finds.Id).StuDateTime,
                    StuEducational = finds.StuEducational,
                    StuEntering = finds.StuEntering,
                    StuInfomationType_Id =s_Entity.StuInfomationType_Entity.SerchSingleData(finds.stuinfomation,false).Id,
                    Region_id =s_Entity.region_Entity.SerchRegionName(finds.RegionName,false)?.ID??null,
                    StuIsGoto = finds.StuisGoto==null?false: finds.StuisGoto,
                    StuName = finds.StuName,
                    StuPhone = finds.Stuphone,
                    StuQQ = finds.StuQQ,
                    StuSchoolName = finds.StuSchoolName,
                    StuSex = finds.StuSex==null?"男": finds.StuSex,
                    StuVisit = finds.StuVisit,
                    StuWeiXin = finds.StuWeiXin,
                    e_Name = finds.empName,
                    StuEntering_1 = finds.StuEntering,
                    InfomationTypeName = finds.stuinfomation, /*StuInfomationType_Entity.GetEntity(finds.StuInfomationType_Id) == null ? "未定义" : StuInfomationType_Entity.GetEntity(finds.StuInfomationType_Id).Name,*/
                    StatusName = finds.StatusName,//Stustate_Entity.GetEntity(finds.StuStatus_Id) == null ? "未填写" : Stustate_Entity.GetEntity(finds.StuStatus_Id).StatusName,
                    Region_Name = finds.RegionName,//finds.Region_id == null ? "区域外" : region_Entity.GetEntity(finds.Region_id).ID.ToString(),
                    Party = finds.Party,
                    reamke=finds.Reak
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
            s_Entity.Stustate_Entity = new StuStateManeger();
            s_Entity.Stustate_Entity = new StuStateManeger();
            s_Entity.StuInfomationType_Entity = new StuInfomationTypeManeger();
            //int s_id = Convert.ToInt32(id);
            ExportStudentBeanData find = s_Entity.findId(id);
            //region_Entity = new RegionManeges();
            ViewBag.id = id;
            //获取信息来源的所有数据
            ViewBag.infomation = s_Entity.StuInfomationType_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
            ViewBag.state = s_Entity.Stustate_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();

            //获取区域
            ViewBag.regin = find.RegionName == null ? "区域外" :  find.RegionName;
            return View();
        }

        //显示有疑似的数据
        public ActionResult StudentSomeData(string id)
        {
            s_Entity.StuInfomationType_Entity = new StuInfomationTypeManeger();
            s_Entity.region_Entity = new RegionManeges();
            s_Entity.Stustate_Entity = new StuStateManeger();
            List<ExportStudentBeanData> list = s_Entity.StudentOrride(id);
            var data = list.Select(s => new StudentData
            {
                stuSex = s.StuSex,
                StuName = s.StuName,
                StuPhone = s.Stuphone,
                StuSchoolName = s.StuSchoolName,
                StuAddress = s.StuAddress,
                StuInfomationType_Id = s.stuinfomation,
                StuStatus_Id = s.StatusName,
                StuIsGoto = s.StuisGoto == false ? "否" : "是",
                StuVisit = s.StuVisit,
                EmployeesInfo_Id = s.empName,
                StuDateTime = s.BeanDate,
                StuEntering = s.StuEntering,
                AreName = s.RegionName
            }).ToList();
            ViewBag.Student = data;
            return View(data);
        }


        #endregion


        #region 指派咨询师
        /// <summary>
        /// 指派咨询师
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ZhipaiConsustTacher()
        {
            int stu = Convert.ToInt32(Request.Form["stuid"]);

            int teacherid = Convert.ToInt32(Request.Form["teaid"]);

            StudentPutOnRecord findata = s_Entity.GetEntity(stu);

            findata.ConsultId = teacherid.ToString();

            findata.ConsultTeacher = EmployandCounTeacherCoom.getallCountTeacher(false).Where(g => g.consultercherid == teacherid).ToList()[0].empname;

            AjaxResult a = new AjaxResult();
            a.Success = s_Entity.ZhipaiConsultTeacher(findata);

            return Json(a, JsonRequestBehavior.AllowGet);
        }
        #endregion
 

        #region Excle文件导入
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
                string UserName = Base_UserBusiness.GetCurrentUser().UserName;
                string completefilePath = f + DateTime.Now.ToString("yyyyMMddhhmmss") + UserName + name;//将上传的文件名称转变为当前项目名称
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

        //获取已备案中的相似数据
        public List<MyExcelClass> SercherStudent(List<MyExcelClass> ex)
        {
            //List<StudentPutOnRecord> s_list = s_Entity.GetAllStudentKeepData();
            List<MyExcelClass> list = new List<MyExcelClass>();
            foreach (MyExcelClass item1 in ex)
            {
                ExportStudentBeanData find = s_Entity.StudentOrrideData(item1.StuName, item1.StuPhone);
                if (find != null)
                {
                    MyExcelClass m = new MyExcelClass();
                    m.StuSex = find.StuSex;
                    m.StuName = find.StuName;
                    m.StuPhone = find.Stuphone;
                    m.StuSchoolName = find.StuSchoolName;
                    m.StuInfomationType_Id = find.stuinfomation;
                    m.Region_id = find.RegionName;
                    m.StuEducational = find.StuEducational;
                    m.StuAddress = find.StuAddress;
                    m.Reak = find.Reak;
                    m.EmployeesInfo_Id = find.empName;
                    if (list.Where(s => s.StuName == m.StuName && s.StuPhone == m.StuPhone).FirstOrDefault() == null)
                    {
                        list.Add(m);
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
            if (namef != null)
            {
                FileInfo fi = new FileInfo(namef.ToString());
                bool ishave = fi.Exists;
                if (ishave)
                {
                    fi.Delete();
                }
            }

        }

        //获取Excle表要导入的数据(目的:有些是区域外的，会被处理)
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

        //判断是否有重复的值true--获取相同的数据false--获取没有冲突的数据
        public List<MyExcelClass> Repeatedly(bool IsRepea)
        {
            List<MyExcelClass> ExcelList = SessionHelper.Session["ExcelData"] as List<MyExcelClass>;//获取Excle文件数据     
            //List<StudentPutOnRecord> All_list = s_Entity.GetAllStudentKeepData();//获取备案数据
            List<MyExcelClass> result = new List<MyExcelClass>();
            //foreach (StudentPutOnRecord a1 in All_list)
            //{
            foreach (MyExcelClass a2 in ExcelList)
            {
                bool s = s_Entity.StudentOrride(a2.StuName, a2.StuPhone);
                if (!s)
                {
                    result.Add(a2);
                }
            }
            //}
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
                    // ExcelList.Remove(result[i]);
                    ExcelList.RemoveAll(e => e.StuName == result[i].StuName);
                }
                return ExcelList;
            }

        }

        //将Excel中的数据导入到数据库中
        public bool AddExcelToServer(List<MyExcelClass> list)
        {

            s_Entity.region_Entity = new RegionManeges();
            s_Entity.StuInfomationType_Entity = new StuInfomationTypeManeger();
            s_Entity.Stustate_Entity = new StuStateManeger();
            AjaxResult add_result = new AjaxResult();
            try
            {
                List<StudentPutOnRecord> listnew = new List<StudentPutOnRecord>();
                foreach (MyExcelClass item1 in list)
                {
                    StudentPutOnRecord s = new StudentPutOnRecord();
                    s.StuName = item1.StuName;
                    s.StuSex = item1.StuSex;
                    s.EmployeesInfo_Id = s_Entity.Enplo_Entity.FindEmpData(item1.EmployeesInfo_Id,false) == null ? null : s_Entity.Enplo_Entity.FindEmpData(item1.EmployeesInfo_Id,false).EmployeeId;
                    s.IsDelete = false;
                    s.Reak = item1.Reak;
                    Region find_region = s_Entity.region_Entity.SerchRegionName(item1.Region_id, false);
                    if (find_region != null){s.Region_id = find_region.ID;}else{s.Reak = s.Reak + ",所在区域:" + item1.Region_id;}
                    s.StatusTime = null;
                    s.StuAddress = item1.StuAddress;
                    s.StuBirthy = null;
                    s.StuDateTime = DateTime.Now;
                    s.BeanDate = DateTime.Now;
                    s.StuEducational = item1.StuEducational;
                    s.StuEntering = s_Entity.Enplo_Entity.GetEntity(UserName.EmpNumber).EmpName;
                    StuInfomationType find_sinfomation = s_Entity.StuInfomationType_Entity.SerchSingleData(item1.StuInfomationType_Id, false);
                    if (find_sinfomation != null){s.StuInfomationType_Id = find_sinfomation.Id;}
                    s.StuIsGoto = false;
                    s.StuPhone = item1.StuPhone;
                    s.StuSchoolName = item1.StuSchoolName;
                    s.StuStatus_Id = 1013;

                    listnew.Add(s);
                }
                add_result = s_Entity.Add_data(listnew);

                if (add_result.Success==true)
                {
                    //查询备案中是网络备案
                    StuInfomationType find= s_Entity.StuInfomationType_Entity.SerchSingleData("网络", false);
                    listnew= listnew.Where(l => l.StuInfomationType_Id == find.Id).ToList();
                    foreach (StudentPutOnRecord item in listnew)
                    {

                    }
                }
            }

            catch (Exception)
            {
                //BusHelper.WriteSysLog(UserName + "Excel大批量导入数据时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }


            return add_result.Success;
        }
        //将文件中的内容写入到数据库中
        public ActionResult IntoServer()
        {
           string result = "no";
           string msg = "系统错误,请重试！！！";

            Excel_Entity = new ExcelHelper();

            try
            {
                List<MyExcelClass> equally_list = Repeatedly(true); //两个数据集合之间比较取交集(挑出相同的)
                List<MyExcelClass> equally_list2 = Repeatedly(false);//将未重复的数据添加到数据库中
                if (equally_list.Count > 0)
                {                    
                    
                    if (equally_list2.Count>0)
                    {
                        bool mis = AddExcelToServer(equally_list2);  //添加数据

                        if (!mis)
                        {
                            var json = new { resut = result, msg = msg };
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                    }

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
                        //获取已备案的数据集合
                        var data = new
                        {
                            result = "okk",
                            msg = "其他数据已导入,以下是有冲突的数据",
                            old = SercherStudent(equally_list),
                            news = equally_list
                        };
                        //string number = "13204961361";
                        //string smsText = "备案提示:已备案成功，但是有重复数据,请自行去系统查看";
                        //string t = PhoneMsgHelper.SendMsg(number, smsText);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //写入出错
                        DeleteFile();
                    }
                }
                else if(equally_list2.Count>0)
                {
                    //没有重复的值
                    List<MyExcelClass> nochongfu_list = SessionHelper.Session["ExcelData"] as List<MyExcelClass>;
                    if (AddExcelToServer(nochongfu_list))
                    {
                        result = "ok";                       
                        //通知备案人备案成功
                        //string number = "13204961361";
                        //string smsText = "备案提示:已备案成功，无重复数据";
                        //string t = PhoneMsgHelper.SendMsg(number, smsText);
                    }

                }

            }
            catch (Exception ex)
            {
                // BusHelper.WriteSysLog(s_Entity.Enplo_Entity.GetEntity(UserName).EmpName +"Excel大批量导入数据时出现:"+ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                result = "no";
                msg = "系统异常，请刷新重试！！！";
            }

            var datajson = new{resut = result,msg = msg};

            return Json(datajson, JsonRequestBehavior.AllowGet);
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
            if (!string.IsNullOrEmpty(id) && id != "undefined")
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
            int count = Convert.ToInt32(Math.Ceiling(FilesName.Count / 12.0));
            List<SelectListItem> selectdata = FilesName.Skip(((Convert.ToInt32(page) - 1) * Convert.ToInt32(limit))).Take(Convert.ToInt32(limit)).ToList();

            var data = new { count = count, data = selectdata, page = page };

            return Json(data, JsonRequestBehavior.AllowGet);
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


        #region Excel数据导出

        /// <summary>
        /// 数据筛选导出页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportView()
        {
            //获取区域所有信息

            var r_list = s_Entity.GetEffectiveRegionAll(true).Select(r => new SelectListItem { Text = r.RegionName, Value = r.RegionName }).ToList();
            r_list.Add(new SelectListItem() { Text = "请选择", Value = "-1", Selected = true });
            r_list.Add(new SelectListItem() { Text = "区域外", Value = "0" });
            ViewBag.areExport = r_list;

            //获取所有咨询师
            List<SelectListItem> c_teacher = EmployandCounTeacherCoom.getallCountTeacher(true).Select(t => new SelectListItem() { Text = t.empname, Value = t.empname }).ToList();
            c_teacher.Add(new SelectListItem() { Text = "请选择", Value = "0", Selected = true });

            ViewBag.c_teacher = c_teacher;


            return View();
        }

        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportFunction()
        {
            AjaxResult a = new AjaxResult();
            string beanman = Request.Form["beanMan"];//备案人
            string Area = Request.Form["Area"];//区域
            string teacher_c = Request.Form["teacher_c"];//咨询师
            string oneTime = Request.Form["oneTime"];//开始日期
            string twoTime = Request.Form["twoTime"];//结束日期
            string str = "select * from StudentBeanView where 1=1";
            string str2 = "select * from Sch_MarketView where 1=1";
            if (!string.IsNullOrEmpty(beanman))
            {
                str = str + " and empName='" + beanman + "'";
                str2 = str2 + " and SalePerson='" + beanman + "'";
            }

            if (Area != "-1")
            {
                if (Area == "0")
                {
                    str = str + " and RegionName is null";
                    str2 = str2 + " and Area is null ";
                }
                else
                {
                    str = str + " and RegionName='" + Area + "'";

                    str2 = str2 + " and Area ='" + Area + "' ";
                }
            }

            if (teacher_c != "0")
            {
                str = str + " and ConsultTeacher='" + teacher_c + "'";

                str2 = str2 + " and Inquiry='" + teacher_c + "'";
            }

            if (!string.IsNullOrEmpty(oneTime))
            {
                str = str + " and BeanDate>='" + oneTime + "'";

                str2 = str2 + " and CreateDate>='" + oneTime + "'";
            }

            if (!string.IsNullOrEmpty(twoTime))
            {
                DateTime date = Convert.ToDateTime(twoTime);
                date.AddDays(1);
                str = str + " and BeanDate<='" + twoTime + "'";

                str2 = str2 + " and CreateDate<'" + date + "'";
            }

            DataTable data = s_Entity.GetDataTableWithSql(str);
            //去另外一张视图匹配数据

            DataTable data2 = s_Entity.GetDataTableWithSql(str2);

            if (data.Columns.Count <= 0 && data2.Columns.Count <= 0)
            {
                a.Msg = "没有符合条件的数据";
                return Json(a, JsonRequestBehavior.AllowGet);
            }

            List<Sch_MarketView> entity2 = s_Entity.GetListBySql<Sch_MarketView>(str2);


            string jsonfile = Server.MapPath("/Config/ExportStudentBean.json");//获取表头
            System.IO.StreamReader file = System.IO.File.OpenText(jsonfile);
            JsonTextReader reader = new JsonTextReader(file);
            //转化为JObject
            JObject ojb = (JObject)JToken.ReadFrom(reader);

            var jj = ojb["ExportStudentBeanData"].ToString();

            JObject jo = (JObject)JsonConvert.DeserializeObject(jj);

            //生成字段名称 
            List<string> Head = new List<string>();
            int indexss = 0;
            foreach (DataColumn col in data.Columns)
            {
                if (indexss != 0)
                {
                    Head.Add(jo[col.ColumnName].ToString());
                }
                indexss++;
            }
            Excel_Entity = new ExcelHelper();

            List<ExportStudentBeanData> entity = s_Entity.GetListBySql<ExportStudentBeanData>(str).Select(s => new ExportStudentBeanData()
            {
                StuName = s.StuName,
                StuSex = s.StuSex,
                StuBirthy = s.StuBirthy,
                Stuphone = s.Stuphone,
                StuSchoolName = s.StuSchoolName,
                StuEducational = s.StuEducational,
                StuAddress = s.StuAddress,
                StuWeiXin = s.StuWeiXin,
                StuQQ = s.StuQQ,
                stuinfomation = s.stuinfomation,
                StatusName = s.StatusName,
                StuisGoto = s.StuisGoto,
                StuVisit = s.StuVisit,
                empName = s.empName,
                Party = s.Party,
                BeanDate = s.BeanDate,
                StuEntering = s.StuEntering,
                StatusTime = s.StatusTime,
                RegionName = s.RegionName,
                Reak = s.Reak,
                ConsultTeacher = s.ConsultTeacher
            }).ToList();


            entity.AddRange(s_Entity.LongrageDataToViewmodel(entity2));


            string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
            SessionHelper.Session["filename"] = filename;
            string path = "~/uploadXLSXfile/ConsultUploadfile/ExportAll/" + filename;
            string truePath = Server.MapPath(path);

            SessionHelper.Session["truePath"] = truePath;
            bool result = Excel_Entity.DaoruExport(entity, truePath, Head);

            a.Success = false;

            if (result)
            {
                a.Success = result;
                return Json(a, JsonRequestBehavior.AllowGet);
            }
            else
            {
                a.Msg = "网络异常，请刷新重试！";
                return Json(a, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取Excel备案所有数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetExcelport()
        {
            string truePath = SessionHelper.Session["truePath"].ToString();
            FileStream stream = new FileStream(truePath, FileMode.Open);
            return File(stream, "application/octet-stream", Server.UrlEncode("Excel备案数据.xls"));
        }       
        #endregion


        #region 远程数据导入       
        public ActionResult InlocalServer()
        {
            s_Entity.InServer();
            return null;
        }
        #endregion


        #region 远程数据查询
        public ActionResult Sch_MarketIndex()
        {
            return View();
        }

        public ActionResult SchMarketTableData(int limit, int page)
        {
            return null;
        }
        #endregion

        #region 学员注册
        //班主任注册页面
        public ActionResult HandMasterRegisterds()
        {
            return View();
        }
        //注册页面
        public ActionResult Registeredstudentnumber(string id)
        {
            List<ZhuceShowData> list = new List<ZhuceShowData>();
            string[] ids = id.Split(',');
            foreach (string item in ids)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    ZhuceShowData data = new ZhuceShowData();
                    int Myid = Convert.ToInt32(item);
                    if (Myid<= 54117)
                    {
                       Sch_Market findda= s_Entity.whereMarketId(Myid);
                        data.stuName = findda.StudentName;
                        data.stuSex = findda.Sex;
                        data.stuPhone = findda.Phone;
                        data.Id = item;
                    }
                    else
                    {
                       StudentPutOnRecord finda= s_Entity.whereStudentId(Myid);
                        data.stuName = finda.StuName;
                        data.stuSex = finda.StuSex;
                        data.stuPhone = finda.StuPhone;
                        data.Id = item;
                    }
                    list.Add(data);
                }
            }

            ViewBag.list = list;
            return View();
        }
        //注册方法
        [HttpPost]
        public ActionResult RegistersFunction(ZhuceShowData data)
        {
            StudentInformationBusiness informationBusiness = new StudentInformationBusiness();
            StudentInformation student = new StudentInformation();
            student.Name = data.stuName;
            student.Sex = data.stuSex=="男"?true:false;
            AjaxResult a = new AjaxResult();
            bool m = true;
            if (data.YesHei)
            {
                bool sm= Regex.IsMatch(data.IdCare, "^\\d{8}$");
                if(!sm)
                {
                    a.Success = false;
                    a.Msg = "出生年月日不正确！！";

                    return Json(a, JsonRequestBehavior.AllowGet);
                }                 
                student.identitydocument =s_Entity.GetIdCard(data.IdCare);
                //添加到黑户表
                HeiHu hu = new HeiHu();
                hu.IdCard = student.identitydocument;
                bool s= s_Entity.heiHu.Add_SingData(hu);
            }
            else
            {
                //判断身份证
                m = s_Entity.TrueCrad(data.IdCare);
                student.identitydocument = data.IdCare;
            }
             
            student.Telephone = data.stuPhone;          
            if (m)
            {               
                //判断学号是否被注册
                int count = informationBusiness.GetList().Where(g => g.StudentPutOnRecord_Id == Convert.ToInt32(data.Id)).Count();
                if (count > 0)
                {
                    a.Success = false;
                    a.Msg = "该学生已注册过学号！";
                    return Json(a, JsonRequestBehavior.AllowGet);
                }

                a = informationBusiness.StudfentEnti(student, Convert.ToInt32(data.Class_ID), Convert.ToInt32(data.Id));
                
            }
            else
            {
                a.Success = false;
                a.Msg = "身份证格式错误！";
            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 一键转咨询师

         public ActionResult ChangTeacher()
        {
            string[] id = Request.Form["id"].Split(',') ;
            int teacherid =Convert.ToInt32(Request.Form["teacherid"]);
            List<int> list = new List<int>();
            List<StudentPutOnRecord> s_list = new List<StudentPutOnRecord>();
            List<Sch_Market> s_list2 = new List<Sch_Market>();
            foreach (string item in id)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    int number = Convert.ToInt32(item);
                    list.Add(number);

                    if (number>=54118)
                    {
                        StudentPutOnRecord studentPut= s_Entity.whereStudentId(number);
                        studentPut.ConsultTeacher = s_Entity.Enplo_Entity.GetEntity(EmployandCounTeacherCoom.Consult_entity.GetEntity(teacherid).Employees_Id).EmpName;
                        studentPut.ConsultId = teacherid.ToString();
                        s_list.Add(studentPut);
                    }
                    else
                    {
                        Sch_Market market = s_Entity.whereMarketId(number);
                        market.Inquiry= s_Entity.Enplo_Entity.GetEntity(EmployandCounTeacherCoom.Consult_entity.GetEntity(teacherid).Employees_Id).EmpName;
                        s_list2.Add(market);
                    }

                }
            }

            AjaxResult a= EmployandCounTeacherCoom.consult.ChangTeacher(list, teacherid);
            if (s_list.Count>0)
            {
               a= s_Entity.Update_data(s_list);
            }

            if (s_list2.Count>0)
            {
                a = s_Entity.s_entity.MyUpdate(s_list2);
            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取缴费情况

        #endregion

        #region 获取在校学生信息
        /// <summary>
        /// 判断某个学生是否有在校信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Ture(int id)
        {
            AjaxResult a = new AjaxResult();
            a.Success = true;
            PutStudentDataView find = s_Entity.FindStudentData(id);
            if (find==null)
            {
                a.Success = false;
            }

            return Json(a,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStudent(int id)
        {
             
            PutStudentDataView find= s_Entity.FindStudentData(id);
            ViewBag.Teacher = "";
            if (find!=null)
            {
                ViewBag.Teacher = s_Entity.GetTeacher(find.ClassID).EmpName;
            }
       
            //根据备案Id获取数据
            return View(find);
        }
        #endregion


    }
}