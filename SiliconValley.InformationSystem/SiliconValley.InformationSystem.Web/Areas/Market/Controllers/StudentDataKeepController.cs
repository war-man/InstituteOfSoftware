using System;
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

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class StudentDataKeepController : BaseMvcController
    {
        // GET: /Market/StudentDataKeep/FindStudent

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
            return View();
        }

        //往数据库中获取数据备案的信息
        public ActionResult GetStudentPutOnRecordData(int limit,int page)
        { 
             
            try
            {
                int SunLimit = s_Entity.GetList().Count;//总行数
                int SunPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(SunLimit / limit)));//总页数
                IQueryable<StudentPutOnRecord> stu_IQueryable = s_Entity.GetIQueryable().OrderByDescending(s=>s.Id);              
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
        public string GetEmployeeValue(string id)
        {
           return Enplo_Entity.GetList().Where(s => s.EmployeeId == id && s.IsDel==false).FirstOrDefault().EmpName;
        }
        #endregion

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
        public ActionResult EditView()
        {
            return View();
        }
    }
}