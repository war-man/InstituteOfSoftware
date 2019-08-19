﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.Common;//获取日志实体
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;//获取学生状态实体
using SiliconValley.InformationSystem.Business.StuInfomationType_Maneger;//获取学生信息来源实体
using SiliconValley.InformationSystem.Business.EmployeesBusiness;//获取员工信息实体
using SiliconValley.InformationSystem.Business.DepartmentBusiness; //获取岗位信息实体
using SiliconValley.InformationSystem.Entity.MyEntity;//获取树实体
using SiliconValley.InformationSystem.Business.PositionBusiness;//获取岗位实体
using SiliconValley.InformationSystem.Entity.ViewEntity;//获取员工岗位部门实体
using SiliconValley.InformationSystem.Entity.Entity;
using System.Text;
using System.IO;
using System.Data;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class StudentDataKeepController : BaseMvcController
    {
        // GET: /Market/StudentDataKeep/IntoServer

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
            return View();
        }

        //往数据库中获取数据备案的信息
        public ActionResult GetStudentPutOnRecordData(int limit,int page)
        {
            string findNamevalue = Request.QueryString["findNamevalue"];
            string findPhonevalue = Request.QueryString["findPhonevalue"];
            string findInformationvalue = Request.QueryString["findInformationvalue"];
            IQueryable<StudentPutOnRecord> stu_IQueryable = s_Entity.GetIQueryable().OrderByDescending(s => s.Id);
            if (!string.IsNullOrEmpty(findNamevalue))
            {
                stu_IQueryable = stu_IQueryable.Where(s => s.StuName.Contains(findNamevalue));
            }
            if (!string.IsNullOrEmpty(findPhonevalue))
            {
                stu_IQueryable = stu_IQueryable.Where(s => s.StuPhone.Contains(findPhonevalue));
            }
            if (!string.IsNullOrEmpty(findInformationvalue) && findInformationvalue!="请选择")
            {
                int type_id = Convert.ToInt32(findInformationvalue);
                stu_IQueryable = stu_IQueryable.Where(s => s.StuInfomationType_Id== type_id);
            }
            try
            {
                int SunLimit = s_Entity.GetList().Count;//总行数
                int SunPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(SunLimit / limit)));//总页数
              
                List<StudentPutOnRecord> PageData= s_Entity.GetPagination<StudentPutOnRecord>(stu_IQueryable, page,limit,"Id","desc",ref SunLimit, ref SunPage); //分页
                var Get_List_studentPutOnRecord = PageData.Select(s => new {
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
                    StuIsGoto =s.StuIsGoto,
                    StuVisit =s.StuVisit,
                    EmployeesInfo_Id = GetEmployeeValue(s.EmployeesInfo_Id),
                    StuDateTime =s.StuDateTime,
                    StuEntering =s.StuEntering,
                    Reak =s.Reak

                });//获取了数据库中所有数据备案信息;                                                                                          
                
                var JsonData = new {     
                    code=0, //解析接口状态,
                    msg="", //解析提示文本,
                    count= SunLimit, //解析数据长度
                    data= Get_List_studentPutOnRecord //解析数据列表
                };                       
                return Json(JsonData,JsonRequestBehavior.AllowGet);
            }                            
            catch (Exception ex)         
            {                            
                //将错误填写到日志中     
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据异常);
                return Json(Error("加载数据有误"),JsonRequestBehavior.AllowGet);
            }                            
        }                                
                                         
        //获取外键的值                   
        #region
        public string GetStuStatuValue(int? id)
        {
            List<StuStatus> Get_List_stustate = Stustate_Entity.GetList().Where(sta=>sta.IsDelete==false).ToList();//获取上门学生状态所有数据
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                return Get_List_stustate.Where(s => s.Id == id).FirstOrDefault().StatusName;
            }
 
            return "未填写";
        }

        public string GetStuInfomationTypeValue(int? id)
        {
            List<StuInfomationType> Get_List_stuInfomationtype = StuInfomationType_Entity.GetList().Where(info=>info.IsDelete==false).ToList();
           
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                return Get_List_stuInfomationtype.Where(s => s.Id == id).FirstOrDefault().Name;
            }
            return "未填写";
        }
        //这个方法是过滤的未在职员工的
        public string GetEmployeeValue(string id)
        {
            EmployeesInfo finde = Enplo_Entity.GetList().Where(s => s.EmployeeId == id && s.IsDel == false).FirstOrDefault();
            if (finde!=null)
            {
                return finde.EmpName;
            }
            else
            {
                return "无";
            }
           
        }
        //这个方法是查询所有员工，无论在职或辞职都可以查询
        public string GetEmployeeValueAll(string id)
        {
            EmployeesInfo finde = Enplo_Entity.GetList().Where(s => s.EmployeeId == id).FirstOrDefault();
            if (finde != null)
            {
                return finde.EmpName;
            }
            else
            {
                return "无";
            }
        }
        //这个方法是用于通过名字来查询信息来源Id的
        public int GetNameSearchId(string name)
        {
              StuInfomationType liststuinfomation = StuInfomationType_Entity.GetList().Where(t=>t.Name==name).FirstOrDefault();
            if (liststuinfomation !=null)
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
            EmployeesInfo e = Enplo_Entity.GetList().Where(ee => ee.EmpName == name).FirstOrDefault();
            return e.EmployeeId;
        }
        #endregion
        #region
        //这是一个添加数据的页面
        public ActionResult AddorEdit(string id)
        {
            //获取信息来源的所有数据
            ViewBag.infomation = StuInfomationType_Entity.GetList().Where(s=>s.IsDelete==false).Select(s=>new SelectListItem { Text=s.Name, Value=s.Id.ToString() }).ToList();

            //获取学生状态来源的所有数据
            ViewBag.state = Stustate_Entity.GetList().Where(s=>s.IsDelete==false).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();             
            return View();
        }

        //将所有员工显示给用户选择
        public ActionResult ShowEmployeInfomation()
        {
            List<EmployeesInfo> list_Enploy = Enplo_Entity.GetList().Where(s=>s.IsDel==false).ToList();//获取所有在职员工
            List<TreeClass> list_Tree = Department_Entity.GetList().Select(d=>new TreeClass() {id=d.DeptId.ToString(),name=d.DeptName, children=new List<TreeClass>(), disable=false, @checked=false, spread=false }).ToList();
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
                                tcc2.name = item3.EmpName;
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

        //获取录入人员
        public ActionResult FindInfoEmply()
        {
           List<Department> list_e1= Department_Entity.GetList().Where(d=>(d.DeptName=="咨询部" || d.DeptName=="网络部")&& d.IsDel==false).ToList();
           
            return View();
        }

        //加载网络部跟咨询部员工供用户选择
        public List<EmployeesInfo> loadNetSeekData1()
        {
            List<Department> d_list = Department_Entity.GetList().Where(d => d.DeptName == "咨询部" && d.IsDel==false).ToList();//获取可用的所有部门信息
            List<EmployeesInfo> list_Enploy = Enplo_Entity.GetList().Where(s=>s.IsDel==false).ToList();//获取所有在职员工信息
            List<Position> list_Position = Position_Entity.GetList().Where(p=>p.IsDel==false).ToList();//获取可用的岗位信息
            List<EmployeesInfo> ee = new List<EmployeesInfo>();
            foreach (Department item1 in d_list)
            {
                List<TreeClass> bigTree = new List<TreeClass>();
                foreach (Position item2 in list_Position)
                {
                    if (item1.DeptId == item2.DeptId)
                    {
                        foreach (EmployeesInfo item3 in list_Enploy)
                        {
                            if (item3.PositionId == item2.Pid)
                            {
                                ee.Add(item3);
                            }
                        }                        
                    }
                }
            }
            return ee;
        }
        public List<EmployeesInfo> loadNetSeekData2()
        {
            List<Department> d_list = Department_Entity.GetList().Where(d => d.DeptName == "网络部" && d.IsDel==false).ToList();//获取可用部门信息
            List<EmployeesInfo> list_Enploy = Enplo_Entity.GetList().Where(e=>e.IsDel==false).ToList();//获取在职员工信息
            List<Position> list_Position = Position_Entity.GetList().Where(p=>p.IsDel==false).ToList();//获取可用岗位信息
            List<EmployeesInfo> ee = new List<EmployeesInfo>();
            foreach (Department item1 in d_list)
            {
                List<TreeClass> bigTree = new List<TreeClass>();
                foreach (Position item2 in list_Position)
                {
                    if (item1.DeptId == item2.DeptId)
                    {
                        foreach (EmployeesInfo item3 in list_Enploy)
                        {
                            if (item3.PositionId == item2.Pid)
                            {
                                ee.Add(item3);
                            }
                        }
                    }
                }
            }
            return ee;
        }
        //将网络部的员工与咨询部的员工加载出来
        public ActionResult ShowSeekNet()
        {
            List<EmployeesInfo> ee1 = loadNetSeekData1();
            List<EmployeesInfo> ee2 = loadNetSeekData2();
            ViewBag.ee1 = ee1;
            ViewBag.ee2 = ee2;
            return View(ee1);
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
                    s_Entity.Insert(news);
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
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据异常);
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
            ViewBag.state = Stustate_Entity.GetList().Where(s => s.IsDelete == false).Select(s => new SelectListItem { Text = s.StatusName, Value = s.Id.ToString() }).ToList();
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
                fins.StuStatus_Id = olds.StuStatus_Id;
                s_Entity.Update(fins);
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //将错误填写到日志中     
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据异常);
                return Json(Error("数据编辑有误"), JsonRequestBehavior.AllowGet);
            }            
        }
        #endregion
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
                    e_Name = GetEmployeeValue(finds.EmployeesInfo_Id),
                    StuEntering_1 = GetEmployeeValueAll(finds.StuEntering),
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

        #region Excle文件导入

        //获取Excle文件中的值
        public List<StudentPutOnRecord> GetExcelFunction()
        {
            string namef = SessionHelper.Session["filename"].ToString();
            DataTable t = AsposeOfficeHelper.ReadExcel(namef, false);
            List<StudentPutOnRecord> new_listStudent = new List<StudentPutOnRecord>();
            if (t.Rows[0][0].ToString() == "姓名" && t.Rows[0][1].ToString() == "性别" && t.Rows[0][2].ToString() == "电话" && t.Rows[0][3].ToString() == "QQ" && t.Rows[0][4].ToString() == "微信" && t.Rows[0][5].ToString() == "学校" && t.Rows[0][6].ToString() == "信息来源" && t.Rows[0][7].ToString() == "学历" && t.Rows[0][8].ToString() == "备案人" && t.Rows[0][9].ToString() == "备注")
            {
                for (int i = 1; i < (t.Rows.Count); i++)
                {
                    StudentPutOnRecord create_s = new StudentPutOnRecord();
                    create_s.StuName = t.Rows[i][0].ToString();
                    create_s.StuSex = t.Rows[i][1].ToString() == "女" ? false : true;
                    create_s.StuPhone = t.Rows[i][2].ToString();
                    create_s.StuQQ = t.Rows[i][3].ToString();
                    create_s.StuWeiXin = t.Rows[i][4].ToString();
                    create_s.StuSchoolName = t.Rows[i][5].ToString();
                    create_s.StuInfomationType_Id = GetNameSearchId(t.Rows[i][6].ToString());
                    create_s.StuEducational = t.Rows[i][7].ToString() == null ? "初中" : t.Rows[i][7].ToString();
                    create_s.EmployeesInfo_Id = GetNameSreachEmploId(t.Rows[i][8].ToString());
                    create_s.StuIsGoto = false;
                    create_s.StuStatus_Id = 1;
                    create_s.Reak = t.Rows[i][9].ToString();
                    new_listStudent.Add(create_s);
                }
            }
            else
            {
                return new_listStudent;
            }
            return new_listStudent;
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
                string completefilePath = f + DateTime.Now.ToString("yyyyMMddhhmmss") + name;//将上传的文件名称转变为当前项目名称
                ProName.Append(Path.Combine(pfilename, completefilePath));//合并成一个完整的路径;
                file.SaveAs(ProName.ToString());//上传文件   
                SessionHelper.Session["filename"] = ProName.ToString();
                List<StudentPutOnRecord> studentlist = GetExcelFunction();
                if (studentlist.Count>0)
                {
                    var jsondata = new
                    {
                        code = "",
                        msg = "ok",
                        data = studentlist,
                    };
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var jsondata = new
                    {
                        code = "",
                        msg = "文件格式错误",
                        data = "",
                    };
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
                 
            }
            catch (Exception ee)
            {
                BusHelper.WriteSysLog(ee.Message, EnumType.LogType.上传文件异常);
                return Json("no", JsonRequestBehavior.AllowGet);
            }

        }

        //获取上传的Excle文件的值
        public ActionResult GetExcelValue(int page,int limit)
        {
            List<StudentPutOnRecord> new_listStudents = GetExcelFunction();
           
                var mydata = new_listStudents.Select(s => new {
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
                    EmployeesInfo_Id = GetEmployeeValue(s.EmployeesInfo_Id),
                    StuDateTime = s.StuDateTime,
                    StuEntering = s.StuEntering,
                    Reak = s.Reak
                });
                var mydata1 = mydata.Skip((page - 1) * limit).Take(limit).ToList();
                var jsondata = new
                {
                    code = 0, //解析接口状态,
                    msg = "", //解析提示文本,
                    count = new_listStudents.Count, //解析数据长度
                    data = mydata1 //解析数据列表
                };
                return Json(jsondata, JsonRequestBehavior.AllowGet);             
        }
        

        //Excle文件内容导入到数据中
        public ActionResult IntoServer()
        {
            List<StudentPutOnRecord> listStudent = GetExcelFunction();
            
                List<StudentPutOnRecord> Addstudent = new List<StudentPutOnRecord>();//用于存放已有的重复数据;
                try
                {
                    foreach (StudentPutOnRecord item in listStudent)
                    {
                        StudentPutOnRecord er = s_Entity.GetList().Where(s => s.StuName == item.StuName && s.StuPhone == item.StuPhone).FirstOrDefault();
                        if (er == null)
                        {
                            item.StuDateTime = DateTime.Now;
                            s_Entity.Insert(item);
                        }
                        else
                        {
                            Addstudent.Add(item);
                        }
                    }
                    var datas = new { data = Addstudent, msg = "ok" };
                    return Json(Addstudent, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据异常);
                    return Json(Error("数据添加有误"), JsonRequestBehavior.AllowGet);
                }            
        }
         
        #endregion
    }
}